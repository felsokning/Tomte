//-----------------------------------------------------------------------
// <copyright file="CopyRemoteFiles.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Copy
{
    using System.Management.Automation;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CopyRemoteFiles"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsCommon.Copy, "RemoteFiles")]
    public class CopyRemoteFiles : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="SourcePath"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The source to copy the files from.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string SourcePath { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="TargetPath"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The destination to copy the files to.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string TargetPath { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                bool result = bool.Parse((string)newWorkflowServiceClient.InvokeWorkflow($"CopyFilesActivity&{SourcePath}!{TargetPath}"));
                WriteObject(result);
            }
        }
    }
}