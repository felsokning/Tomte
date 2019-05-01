//-----------------------------------------------------------------------
// <copyright file="GetRemoteOSVersion.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Get
{
    using System.ServiceModel;
    using System.Management.Automation;
    using System;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetRemoteOSVersion"/> class.
    /// </summary>
    /// <inheritdoc cref="PSCmdlet"/>
    [Cmdlet(VerbsCommon.Get, "RemoteOSVersion")]
    public class GetRemoteOSVersion : PSCmdlet
    {
        /// <summary>
        ///     Gets or sets the <see cref="Server"/> value, which defines which server to target.
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The Server to Invoke the Workflow on.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Server { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="Kernel32"/> value, which signifies that we should target kernel32.dll
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Uses Kernel32 to determine the version.", Mandatory = false, ParameterSetName = "SourceFiles")]
        public SwitchParameter Kernel32 { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="NtosKrnl"/> value, which signifies that we should target ntoskrnl.exe
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Uses NtosKrnl to determine the version.", Mandatory = false, ParameterSetName = "SourceFiles")]
        public SwitchParameter NtosKrnl { get; set; }

        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (!Kernel32 && !NtosKrnl)
            {
                throw new InvalidOperationException("At least one parameter, either 'Kernel32' or 'NtosKrnl' must be defined.");
            }
            if (Kernel32 && NtosKrnl)
            {
                WriteWarning("The default is NtosKrnl when both source file parameters are defined.");
            }

            using (WorkflowServiceClient newWorkflowServiceClient = new WorkflowServiceClient(new WSHttpBinding(), new EndpointAddress($"http://{Server}:65534/WorkflowService/service")))
            {
                string targetFile = string.Empty;
                if (Kernel32) { targetFile = "kernel32.dll"; }
                if (NtosKrnl) { targetFile = "ntoskrnl.exe"; }
                Version result = Version.Parse((string)newWorkflowServiceClient.InvokeWorkflow($"GetOsFileVersionActivity&{targetFile}"));
                WriteObject(result);
            }
        }
    }
}