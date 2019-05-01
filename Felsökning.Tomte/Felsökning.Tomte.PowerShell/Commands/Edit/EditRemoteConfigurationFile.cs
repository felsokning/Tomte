//-----------------------------------------------------------------------
// <copyright file="EditRemoteConfigurationFile.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Edit
{
    using System.Management.Automation;
    using System.Security.Permissions;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EditRemoteConfigurationFile"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [PrincipalPermission(SecurityAction.Demand, Role = "DevOps")]
    [Cmdlet(VerbsData.Edit, "RemoteConfigurationFile")]
    public class EditRemoteConfigurationFile : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ConfigurationFile"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The configuration file to be modified.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ConfigurationFile { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ConfigurationKey"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The key in the configuration file to be modified.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ConfigurationKey { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ConfigurationValue"/> value, which defines where the file exists.
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "The value to change for the supplied configuration key.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ConfigurationValue { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                string result = (string)newWorkflowServiceClient.InvokeWorkflow($"ModifyConfigurationFileActivity&{ConfigurationFile}!{ConfigurationKey}!{ConfigurationFile}");
                WriteObject(result);
            }
        }
    }
}