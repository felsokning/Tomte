//-----------------------------------------------------------------------
// <copyright file="GetOsFileVersionActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Diagnostics;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetOsFileVersionActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class GetOsFileVersionActivity : SwedishCodeActivity<object>
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
            string fileTarget = context.GetValue(FirstArgument);
            string windowsDirectory = Environment.GetEnvironmentVariable("windir");
            return FileVersionInfo.GetVersionInfo($"{windowsDirectory}\\System32\\{fileTarget}").ProductVersion;
        }
    }
}