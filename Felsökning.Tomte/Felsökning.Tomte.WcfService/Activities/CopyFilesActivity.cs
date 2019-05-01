//-----------------------------------------------------------------------
// <copyright file="CopyFilesActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CopyFilesActivity"/> class.
    /// </summary>
    public class CopyFilesActivity : SwedishCodeActivity<object>
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
            // TODO: MOVE THIS TO NON-RETURN ACTIVITY
            FirstArgument = FirstInArgument;
            SecondArgument = SecondInArgument;
            string sourceDirectory = context.GetValue(FirstArgument);
            string targetDirectory = context.GetValue(SecondArgument);
            string[] filesFound = Directory.GetFileSystemEntries(sourceDirectory);

            Parallel.ForEach(
                filesFound,
                f =>
                {
                    string[] fileStrings = f.Split('\\');
                    int lastIndex = fileStrings.Length - 1;
                    string fileName = fileStrings[lastIndex];
                    File.Copy(f, targetDirectory + '\\' + fileName);
                });
            return string.Empty;
        }
    }
}