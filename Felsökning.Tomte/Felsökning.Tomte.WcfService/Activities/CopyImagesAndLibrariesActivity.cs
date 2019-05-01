//-----------------------------------------------------------------------
// <copyright file="CopyImagesAndLibrariesActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CopyImagesAndLibrariesActivity"/>
    /// </summary>
    public class CopyImagesAndLibrariesActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Gets or sets the value of the <see cref="FirstArgument"/> parameter.
        /// </summary>
        public InArgument<string> FirstArgument { get; set; }

        /// <summary>
        ///     Gets or sets the value of the <see cref="SecondArgument"/> parameter.
        /// </summary>
        public InArgument<string> SecondArgument { get; set; }

        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            // TODO: Move to SwedishCodeActivity (with void signature)
            FirstArgument = FirstInArgument;
            SecondArgument = SecondInArgument;

            string stringProcessId = context.GetValue(FirstArgument);
            string targetPath = context.GetValue(SecondArgument);

            // Sanity check to help the caller out.
            if (!targetPath.EndsWith("\\"))
            {
                targetPath += "\\";
            }

            // We want this to hard fail.
            int processId = int.Parse(stringProcessId);

            using (Process process = Process.GetProcessById(processId))
            {
                // Process contains a list of loaded modules, which also contains the
                // Native Images loaded, as well; which /can/ be important for debugging
                // purposes (thus, the need to copy them out).
                ProcessModuleCollection modules = process.Modules;

                // Should always be the case, since we're running in the LSA context,
                // but better safe to check than to be sorry and null-reference.
                if (modules.Count > 0)
                {
                    // Added overhead but necessary for parallel operations.
                    List<ProcessModule> modulesList = modules.Cast<ProcessModule>().ToList();
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("Copied: ");
                    Parallel.ForEach(
                        modulesList,
                        module =>
                        {
                            string targetPathQualified = targetPath + module.ModuleName;

                            // TODO: Requires investigation. This is a workaround to prevent duplication
                            if (!File.Exists(targetPathQualified))
                            {
                                File.Copy(module.FileName, targetPathQualified);
                                stringBuilder.AppendLine(module.ModuleName);
                            }
                        });

                    return stringBuilder.ToString();
                }
                else
                {
                    return "No modules were found to copy...";
                }
            }
        }
    }
}