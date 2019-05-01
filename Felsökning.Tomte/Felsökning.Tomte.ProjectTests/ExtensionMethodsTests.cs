//-----------------------------------------------------------------------
// <copyright file="ExtensionMethodsTests.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.ProjectTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Felsökning.Tomte.WcfService.ExtensionMethods;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ExtensionMethodsTests"/> class.
    /// </summary>
    [TestClass]
    public class ExtensionMethodsTests
    {
        /// <summary>
        ///     Tests the Extension Method to return the percentage of free space on a drive.
        /// </summary>
        [TestMethod]
        public void TestDriveInfoExtension()
        {
            DriveInfo[] newDriveInfo = DriveInfo.GetDrives();
            if (newDriveInfo.Length > 0)
            {
                newDriveInfo.ToList().ForEach(d =>
                {
                    if (d.IsReady)
                    {
                        double freeSpace = d.PercentageFreeSpace();
                        Assert.IsFalse(
                            freeSpace <
                            0.00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001);
                    }
                });
            }
        }

        /// <summary>
        ///     Tests the Extension Method to verify that a string contains a substring.
        /// </summary>
        [TestMethod]
        public void TestStringExtension()
        {
            string testString = "This is a test string for SCIENCE!";
            Assert.IsTrue(testString.Contains("SCIENCE", StringComparison.InvariantCulture));
            Assert.IsFalse(testString.Contains("MATHS", StringComparison.InvariantCulture));
        }
    }
}