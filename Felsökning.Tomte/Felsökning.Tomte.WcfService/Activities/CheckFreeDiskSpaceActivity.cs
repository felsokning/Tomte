//-----------------------------------------------------------------------
// <copyright file="CheckFreeDiskSpaceActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.IO;
    using System.Linq;
    using System.Text;

    using ExtensionMethods;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CheckFreeDiskSpaceActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class CheckFreeDiskSpaceActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            StringBuilder newStringBuilder = new StringBuilder();
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            driveInfos.ToList().ForEach(
                d =>
                {
                    // To prevent System.IO.IOException: The device is not ready.
                    // This can occur for CD-ROM drives, which have no disks in them.
                    if (d.IsReady)
                    {
                        newStringBuilder.AppendLine($"Drive {d.Name} has {Math.Round(d.PercentageFreeSpace(), 2)}% free");
                    }
                });

            return newStringBuilder.ToString();
        }
    }
}