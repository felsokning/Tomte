//-----------------------------------------------------------------------
// <copyright file="GetRemoteProcessThreads.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Get
{
    using System;
    using System.Management.Automation;
    using System.ServiceModel;
    using System.Xml;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetRemoteProcessThreads"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsCommon.Get, "RemoteProcessThreads")]
    public class GetRemoteProcessThreads : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ProcessId"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The Process ID to target for the workflow.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ProcessId { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            WSHttpBinding newWsHttpBinding = new WSHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,

                // Required, since we're returning such a large string.
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxDepth = 128,
                    MaxNameTableCharCount = 16384,
                    MaxStringContentLength = int.MaxValue
                }
            };

            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(newWsHttpBinding, new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                string result = (string)newWorkflowServiceClient.InvokeWorkflow($"DumpProcessThreadsActivity&{ProcessId}");
                Console.WriteLine(result);
            }
        }
    }
}