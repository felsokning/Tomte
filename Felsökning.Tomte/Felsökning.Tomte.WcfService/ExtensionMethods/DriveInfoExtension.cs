//-----------------------------------------------------------------------
// <copyright file="DriveInfoExtension.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.ExtensionMethods
{
    using System.IO;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DriveInfoExtension"/> class.
    /// </summary>
    public static class DriveInfoExtension
    {
        /// <summary>
        ///     Returns the free space percentage on a given drive.
        /// </summary>
        /// <param name="driveIn">The drive to analyze.</param>
        /// <returns>A long representing the free space percentage.</returns>
        /// <inheritdoc cref="DriveInfo"/>
        public static double PercentageFreeSpace(this DriveInfo driveIn)
        {
            return (driveIn.AvailableFreeSpace / (double)driveIn.TotalSize) * 100;
        }
    }
}