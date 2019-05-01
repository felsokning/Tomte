//-----------------------------------------------------------------------
// <copyright file="InterfaceTests.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.ProjectTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Felsökning.Tomte.WcfService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="InterfaceTests"/> class.
    /// </summary>
    [TestClass]
    public class InterfaceTests
    {
        [TestMethod]
        public void InterfaceWorkflowServiceTests()
        {
            IWorkflowService newWorkflowService = new WorkflowService();
            Assert.IsInstanceOfType(newWorkflowService, typeof(IWorkflowService));
            Assert.IsInstanceOfType(newWorkflowService, typeof(WorkflowService));
            string[] commandAndParameterStrings = newWorkflowService.ParseCommandAndParameter("DoSomeScienceStuff&Parameter1");
            Assert.IsFalse(string.IsNullOrWhiteSpace(commandAndParameterStrings[0]));
            Assert.IsFalse(string.IsNullOrWhiteSpace(commandAndParameterStrings[1]));
            Assert.AreNotEqual(commandAndParameterStrings[0], commandAndParameterStrings[1]);
            Assert.IsTrue(newWorkflowService.EvaluateContainsDelimiter("!", "Testing!Delimiter"));
            Assert.IsTrue(newWorkflowService.EvaluateContainsDelimiter("?", "Testing?Delimiter"));
            Assert.IsTrue(newWorkflowService.EvaluateContainsDelimiter("^", "Top^Hat"));
            Assert.IsFalse(newWorkflowService.EvaluateContainsDelimiter("!", "Testing%Delimiter"));
            Assert.IsFalse(newWorkflowService.EvaluateContainsDelimiter("$", "Testing.Delimiter"));
        }
    }
}