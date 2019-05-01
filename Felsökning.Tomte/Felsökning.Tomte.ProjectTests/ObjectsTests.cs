//-----------------------------------------------------------------------
// <copyright file="ObjectsTests.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.ProjectTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Felsökning.Tomte.WcfService.Objects;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ObjectsTests"/> class.
    /// </summary>
    [TestClass]
    public class ObjectsTests
    {
        [TestMethod]
        public void TestReturnProcess()
        {
            ReturnProcess newReturnProcess = new ReturnProcess { ProcessName = "Something", ProcessId = 0 };

            // Getters
            Assert.IsInstanceOfType(newReturnProcess, typeof(ReturnProcess));
            Assert.IsFalse(string.IsNullOrWhiteSpace(newReturnProcess.ProcessName));
            Assert.IsFalse(newReturnProcess.ProcessId == 1);
            Assert.IsFalse(newReturnProcess.ProcessName.Equals("Nothing"));

            // Setters
            newReturnProcess.ProcessName = "SomethingElse";
            newReturnProcess.ProcessId = 58222;
            Assert.IsTrue(newReturnProcess.ProcessName.Equals("SomethingElse"));
            Assert.IsTrue(newReturnProcess.ProcessId.Equals(58222));
        }
    }
}