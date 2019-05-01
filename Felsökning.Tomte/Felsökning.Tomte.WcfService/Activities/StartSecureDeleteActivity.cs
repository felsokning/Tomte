//-----------------------------------------------------------------------
// <copyright file="StartSecureDeleteActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StartSecureDeleteActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class StartSecureDeleteActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Gets or sets the value of the <see cref="FirstArgument"/> parameter.
        /// </summary>
        public InArgument<string> FirstArgument { get; set; }

        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            FirstArgument = FirstInArgument;
            string targetFile = context.GetValue(FirstArgument);
            if (!File.Exists(targetFile))
            {
                return "File wasn't found, so nothing to delete!";
            }

            if (!File.Exists(@"C:\SysInternals\sdelete64.exe"))
            {
                return "Bad Science";
            }

            using (Process newProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = $"/accepteula -p 3 {targetFile}",
                    CreateNoWindow = true,
                    FileName = @"C:\SysInternals\sdelete64.exe",
                    RedirectStandardError = true,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,

                }
            })
            {
                newProcess.PriorityClass = ProcessPriorityClass.High;
                newProcess.Start();
                newProcess.WaitForExit();

                return "Completed!";
            }
        }
    }
}