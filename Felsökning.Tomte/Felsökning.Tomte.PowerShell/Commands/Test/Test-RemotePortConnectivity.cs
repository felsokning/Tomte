//-----------------------------------------------------------------------
// <copyright file="TestRemotePortConnectivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Test
{
    using System.Management.Automation;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestRemotePortConnectivity"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsDiagnostic.Test, "RemotePortConnectivity")]
    public class TestRemotePortConnectivity : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="TargetHost"/> value, which defines which hostname to target.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The host to test connectivity against.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string TargetHost { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Port"/> value, which defines which hostname to target.
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The port to test connectivity against.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Port { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                bool result = bool.Parse((string)newWorkflowServiceClient.InvokeWorkflow($"PortConnectivityActivity&{TargetHost}!{Port}"));
                WriteObject(result);
            }
        }
    }
}