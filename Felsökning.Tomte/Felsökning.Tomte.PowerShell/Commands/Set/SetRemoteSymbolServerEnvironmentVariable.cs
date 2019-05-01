//-----------------------------------------------------------------------
// <copyright file="SetRemoteSymbolServerEnvironmentVariable.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Set
{
    using System.Management.Automation;
    using System.ServiceModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SetRemoteSymbolsEnvironmentVariable"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsCommon.Set, "RemoteSymbolServerEnvironmentVariable")]
    public class SetRemoteSymbolServerEnvironmentVariable : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="SymbolFilePath"/> value, which defines where the symbols files should be located.
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The path to store for the symbols environment variable.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string SymbolFilePath { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteWarning("The environment variable will be for the user context that the service runs under.");

            // Wrap in using to prevent leaks from non-disposed calls.
            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                string result = (string)newWorkflowServiceClient.InvokeWorkflow($"SetSymbolsEnvironmentPath&{SymbolFilePath}");
                WriteObject(result);
            }
        }
    }
}