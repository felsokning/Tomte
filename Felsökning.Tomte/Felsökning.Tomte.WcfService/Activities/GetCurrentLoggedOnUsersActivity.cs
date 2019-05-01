//-----------------------------------------------------------------------
// <copyright file="GetCurrentLoggedOnUsersActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Cassia;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetCurrentLoggedOnUsersActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class GetCurrentLoggedOnUsersActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            ITerminalServicesManager iTerminalServicesManager = new TerminalServicesManager();
            StringBuilder newStringBuilder = new StringBuilder();
            using (ITerminalServer server = iTerminalServicesManager.GetRemoteServer(Environment.MachineName))
            {
                server.Open();
                IList<ITerminalServicesSession> newSessionsList = server.GetSessions();
                Parallel.ForEach(
                    newSessionsList,
                    s =>
                    {
                        if (!string.IsNullOrWhiteSpace(s.UserName))
                        {
                            newStringBuilder.AppendLine($"Session: {s.SessionId} UserName: {s.UserName} LogonTime: {s.LoginTime} IdleTime: {s.IdleTime}");
                        }
                    });
            }

            return newStringBuilder.ToString();
        }
    }
}