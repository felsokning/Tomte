//-----------------------------------------------------------------------
// <copyright file="RestartRemoteService.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Restart
{
    using System;
    using System.Management.Automation;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RestartRemoteService"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsLifecycle.Restart, "RemoteService")]
    public class RestartRemoteService : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ServiceName"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The name of the service to restart.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ServiceName { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                string result = (string)newWorkflowServiceClient.InvokeWorkflow($"RestartServiceActivity&{ServiceName}");
                Console.WriteLine(result);
            }
        }
    }
}