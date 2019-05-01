//-----------------------------------------------------------------------
// <copyright file="StartRemoteSecureDelete.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Start
{
    using System.Management.Automation;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StartRemoteSecureDelete"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsLifecycle.Start, "RemoteSecureDelete")]
    public class StartRemoteSecureDelete : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="FilePath"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The path to the file to be deleted.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string FilePath { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                string result = (string)newWorkflowServiceClient.InvokeWorkflow($"StartSecureDeleteActivity&{FilePath}");
                if (result.Contains("Bad Science"))
                {
                    WriteWarning("SDelete does not exist on the system. Run 'Install-RemoteSysInternals' to install the SysInternal tools on the server.");
                }

                else
                {
                    WriteObject(result);
                }
            }
        }
    }
}