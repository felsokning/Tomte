//-----------------------------------------------------------------------
// <copyright file="SetRemoteComputerName.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Set
{
    using System;
    using System.Management.Automation;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SetRemoteComputerName"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsCommon.Set, "RemoteComputerName")]
    public class SetRemoteComputerName : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="RenameType"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The type of renaming to be done.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string RenameType { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="NewName"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The name to rename the server to.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string NewName { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                string result = (string)newWorkflowServiceClient.InvokeWorkflow($"RenameMachineActivity&{RenameType}!{NewName}");
                Console.WriteLine(result);
            }
        }
    }
}