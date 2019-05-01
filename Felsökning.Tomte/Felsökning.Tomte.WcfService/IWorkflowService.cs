//-----------------------------------------------------------------------
// <copyright file="IWorkflowService.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService
{
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    ///     Initializes a new instance of the <see cref="IWorkflowService"/> interface.
    /// </summary>
    [ServiceContract]
    public interface IWorkflowService
    {
        /// <summary>
        ///     Contract exposed to invoke a workflow.
        /// </summary>
        /// <param name="workflowData">Data used to invoke the workflow.</param>
        /// <returns>A string back to the caller.</returns>
        [OperationContract]
        [WebGet]
        object InvokeWorkflow(string workflowData);

        /// <summary>
        ///     Contract exposed to invoke a workflow based on Events.
        /// </summary>
        /// <param name="workflowData">Data used to invoke the workflow.</param>
        /// <returns>An array of Event Log Entries.</returns>
        [OperationContract]
        [WebGet]
        EventLogEntry[] InvokeEventsWorkflow(string workflowData);

        /// <summary>
        ///     Determine if the string contains a delimiter
        /// </summary>
        /// <param name="delimiter">The delimiter we're looking for.</param>
        /// <param name="passedString">The string to evaluate.</param>
        /// <returns>A boolean signifying if the delimiter is present.</returns>
        bool EvaluateContainsDelimiter(string delimiter, string passedString);

        /// <summary>
        ///     Parses the string into the separate command and parameter.
        /// </summary>
        /// <param name="workflowData">Data used to invoke the workflow.</param>
        /// <returns>A string array with the command parameter split.</returns>
        string[] ParseCommandAndParameter(string workflowData);
    }
}