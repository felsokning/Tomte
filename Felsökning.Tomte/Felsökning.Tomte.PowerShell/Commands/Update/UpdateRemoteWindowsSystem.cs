//-----------------------------------------------------------------------
// <copyright file="CopyRemoteFiles.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Update
{
    using System;
    using System.Management.Automation;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UpdateRemoteWindowsSystem"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsData.Update, "RemoteWindowsSystem")]
    public class UpdateRemoteWindowsSystem : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding { ReceiveTimeout = TimeSpan.FromMinutes(5), SendTimeout = TimeSpan.FromMinutes(5) }, new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                string result = (string)newWorkflowServiceClient.InvokeWorkflow("WindowsUpdateActivity");
                WriteObject(result);
            }
        }
    }
}