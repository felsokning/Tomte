//-----------------------------------------------------------------------
// <copyright file="WindowsUpdateActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Runtime.InteropServices;
    using System.Threading;

    // Just so that we're clear: COM is the devil.
    using WUApiLib;

    using Enums;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowsUpdateActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class WindowsUpdateActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Initiates a shutdown and optional restart of the specified computer, and optionally records the reason for the shutdown.
        /// </summary>
        /// <param name="lpMachineName">The network name of the computer to be shut down. If lpMachineName is NULL or an empty string, the function shuts down the local computer.</param>
        /// <param name="lpMessage">The message to be displayed in the shutdown dialog box. This parameter can be NULL if no message is required.</param>
        /// <param name="dwTimeout">The length of time that the shutdown dialog box should be displayed, in seconds. While this dialog box is displayed, shutdown can be stopped by the AbortSystemShutdown function.
        /// If dwTimeout is not zero, InitiateSystemShutdownEx displays a dialog box on the specified computer.The dialog box displays the name of the user who called the function, displays the message specified by the lpMessage parameter, and prompts the user to log off.The dialog box beeps when it is created and remains on top of other windows in the system. The dialog box can be moved but not closed.A timer counts down the remaining time before shutdown.
        /// If dwTimeout is zero, the computer shuts down without displaying the dialog box, and the shutdown cannot be stopped by AbortSystemShutdown.</param>
        /// <param name="bForceAppsClosed">If this parameter is TRUE, applications with unsaved changes are to be forcibly closed. If this parameter is FALSE, the system displays a dialog box instructing the user to close the applications.</param>
        /// <param name="bRebootAfterShutdown">If this parameter is TRUE, the computer is to restart immediately after shutting down. If this parameter is FALSE, the system flushes all caches to disk and safely powers down the system.</param>
        /// <param name="dwReason">The reason for initiating the shutdown. This parameter must be one of the <see cref="ShutdownReason"/> codes.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InitiateSystemShutdownEx(
            string lpMachineName,
            string lpMessage,
            uint dwTimeout,
            bool bForceAppsClosed,
            bool bRebootAfterShutdown,
            ShutdownReason dwReason);

        /// <summary>
        ///     The <see cref="Execute(CodeActivityContext)"/> method inherited from <see cref="CodeActivity{TResult}"/>.
        /// </summary>
        /// <param name="context">The current <see cref="CodeActivity"/> context.</param>
        /// <returns>A DateTime object.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            UpdateSession updateSession = new UpdateSession();
            IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
            try
            {
                ISearchResult searchResult = updateSearcher.Search("IsInstalled=0 And IsHidden=0");
                foreach (IUpdate update in searchResult.Updates)
                {
                    if (update.EulaAccepted == false)
                    {
                        update.AcceptEula();
                    }
                }

                UpdateDownloader updateDownloader = updateSession.CreateUpdateDownloader();
                updateDownloader.Priority = DownloadPriority.dpHigh;
                updateDownloader.Updates = searchResult.Updates;
                IDownloadResult downloadResult = updateDownloader.Download();
                while (downloadResult.ResultCode == OperationResultCode.orcInProgress)
                {
                    Thread.Sleep(10);
                }

                // Check that the updates downloaded successfully.
                if (downloadResult.ResultCode == OperationResultCode.orcSucceeded || downloadResult.ResultCode == OperationResultCode.orcSucceededWithErrors)
                {
                    UpdateCollection updateCollection = new UpdateCollection();
                    foreach (IUpdate update in searchResult.Updates)
                    {
                        if (update.IsDownloaded)
                        {
                            updateCollection.Add(update);
                        }
                    }

                    IUpdateInstaller updateInstaller = updateSession.CreateUpdateInstaller();
                    updateInstaller.Updates = updateCollection;
                    IInstallationResult installationResult = updateInstaller.Install();
                    if (installationResult.ResultCode == OperationResultCode.orcSucceeded || installationResult.ResultCode == OperationResultCode.orcSucceededWithErrors)
                    {
                        if (installationResult.RebootRequired)
                        {
                            // Restart the system.
                            // For the reason these particular flags are used, see: https://docs.microsoft.com/en-us/windows/desktop/Shutdown/system-shutdown-reason-codes
                            InitiateSystemShutdownEx(null, null, 0, true, true, ShutdownReason.SHTDN_REASON_MAJOR_OPERATINGSYSTEM | ShutdownReason.SHTDN_REASON_MINOR_HOTFIX | ShutdownReason.SHTDN_REASON_FLAG_PLANNED);
                            return "Updates completed and machine is rebooting.";
                        }
                        else
                        {
                            return "Updates completed.";
                        }
                    }
                }
                else
                {
                    return "There was a problem downloading updates.";
                }
            }
            catch (COMException e)
            {
                if (e.HResult == -2145124316)
                {
                    return "No updates were found. Exiting...";
                }
                else
                {
                    return $"Exception {e.HResult}: {e.Message} was hit. Exiting...";
                }
            }

            return string.Empty;
        }
    }
}