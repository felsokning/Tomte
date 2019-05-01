//-----------------------------------------------------------------------
// <copyright file="CopyRemoteFiles.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Get
{
    using System.Management.Automation;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetRemoteProcessIds"/> class.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "RemoteProcessIds")]
    public class GetRemoteProcessIds : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Process"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The Process to look for.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Process { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IsAppPool"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Whether the given process is an app pool.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public bool IsAppPool { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                object result = newWorkflowServiceClient.InvokeWorkflow($"GetProcessIdActivity&{Process}!{IsAppPool}");
                WriteObject(result);
            }
        }
    }
}