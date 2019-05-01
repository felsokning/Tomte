//-----------------------------------------------------------------------
// <copyright file="RequestDevOpsElevation.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.PowerShell.Commands.Request
{
    using System.DirectoryServices.ActiveDirectory;
    using System.DirectoryServices.AccountManagement;
    using System.Management.Automation;
    using System.Security.Permissions;
    using System.Security.Principal;
    using System;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RequestDevOpsElevation"/> class.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Request, "DevOpsElevation")]
    public class RequestDevOpsElevation : PSCmdlet
    {
        /// <summary>
        ///     Overrides the <see cref="ProcessRecord"/> method inherited from <see cref="Cmdlet"/>.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
                {
                    GroupPrincipal devOpsGroupPrincipal = GroupPrincipal.FindByIdentity(ctx, "DevOps");
                    SecurityIdentifier devOpsSecIdent = devOpsGroupPrincipal?.Sid;
                    WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                    if (currentIdentity.Groups != null && currentIdentity.Groups.Contains(devOpsSecIdent))
                    {
                        AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                        PrincipalPermission principalPerm = new PrincipalPermission(currentIdentity.Name, "DevOps");
                        principalPerm.Demand();
                    }
                }
            }

            // Machine is not part of a domain.
            catch (ActiveDirectoryObjectNotFoundException)
            {
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Machine))
                {
                    GroupPrincipal devOpsGroupPrincipal = GroupPrincipal.FindByIdentity(ctx, "DevOps");
                    SecurityIdentifier devOpsSecIdent = devOpsGroupPrincipal?.Sid;
                    WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                    if (currentIdentity.Groups != null && currentIdentity.Groups.Contains(devOpsSecIdent))
                    {
                        // See: EditRemoteConfigurationFile for why this permission would be desired versus open access.
                        AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                        PrincipalPermission principalPerm = new PrincipalPermission(currentIdentity.Name, "DevOps");
                        principalPerm.Demand();
                    }
                }
            }
        }
    }
}