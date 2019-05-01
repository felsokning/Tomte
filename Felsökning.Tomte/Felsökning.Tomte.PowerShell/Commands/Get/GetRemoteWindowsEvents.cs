//-----------------------------------------------------------------------
// <copyright file="GetRemoteWindowsEvents.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Get
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Management.Automation;
    using System.ServiceModel;
    using System.Xml;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetRemoteWindowsEvents"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsCommon.Get, "RemoteWindowsEvents")]
    public class GetRemoteWindowsEvents : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Log"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The log to target.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Log { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Source"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The Source to target.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Source { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="EventId"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "The Event ID to target.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string EventId { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Filter"/> value, which defines if we should filter the events returned.
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Whether we should filter the results.", Mandatory = false, ParameterSetName = "Filtering")]
        public SwitchParameter Filter { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Filter"/> value, which defines the time span that we should filter the results for.
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "The time span filter to apply.", Mandatory = false, ParameterSetName = "Filtering")]
        public TimeSpan TimeSpan { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            WSHttpBinding newWsHttpBinding = new WSHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,

                // Required, since we're returning such a large array of objects.
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = 4096,
                    MaxDepth = 128,
                    MaxNameTableCharCount = 16384,
                    MaxStringContentLength = int.MaxValue
                }
            };

            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(newWsHttpBinding, new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                EventLogEntry[] result = newWorkflowServiceClient.InvokeEventsWorkflow($"GetWindowsEventsActivity&{Log}!{Source}!{EventId}");
                if (Filter)
                {
                    // Do the work on this side, as it's easier.
                    DateTime targetDateTime = DateTime.UtcNow - TimeSpan;
                    EventLogEntry[] filteredResults = result.ToList().Where(e => e.TimeGenerated > targetDateTime).ToArray();
                    WriteObject(filteredResults);
                }
                else
                {
                    WriteObject(result);
                }
            }
        }
    }
}