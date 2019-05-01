//-----------------------------------------------------------------------
// <copyright file="ActivitiesTests.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.ProjectTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Felsökning.Tomte.WcfService.Activities;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ActivitiesTests"/> class.
    /// </summary>
    [TestClass]
    public class ActivitiesTests
    {
        /// <summary>
        ///     Tests all of the <see cref="SwedishCodeActivity{T}"/> class invocations.
        /// </summary>
        [TestMethod]
        public void TestSwedishCodeActivitiesCreation()
        {
            // CheckFreeDiskSpaceActivity
            CheckFreeDiskSpaceActivity newCheckFreeDiskSpaceActivity = new CheckFreeDiskSpaceActivity();
            Assert.IsInstanceOfType(newCheckFreeDiskSpaceActivity, typeof(CheckFreeDiskSpaceActivity));

            // CopyFilesActivity
            CopyFilesActivity newCopyFilesActivity = new CopyFilesActivity();
            Assert.IsInstanceOfType(newCopyFilesActivity, typeof(CopyFilesActivity));

            // CopyImagesAndLibrariesActivity
            CopyImagesAndLibrariesActivity newCopyImagesAndLibrariesActivity = new CopyImagesAndLibrariesActivity();
            Assert.IsInstanceOfType(newCopyImagesAndLibrariesActivity, typeof(CopyImagesAndLibrariesActivity));

            // UtcDateTimeActivity
            UtcDateTimeActivity newDateTimeActivity = new UtcDateTimeActivity();
            Assert.IsInstanceOfType(newDateTimeActivity, typeof(UtcDateTimeActivity));

            // DumpProcessThreadsActivity
            DumpProcessThreadsActivity newDumpProcessThreadsActivity = new DumpProcessThreadsActivity();
            Assert.IsInstanceOfType(newDumpProcessThreadsActivity, typeof(DumpProcessThreadsActivity));

            // FileExistsActivity
            CheckIfFileExistsActivity newFileExistsActivity = new CheckIfFileExistsActivity();
            Assert.IsInstanceOfType(newFileExistsActivity, typeof(CheckIfFileExistsActivity));

            // ForceBlueScreenActivity
            ForceBlueScreenActivity newForceBlueScreenActivity = new ForceBlueScreenActivity();
            Assert.IsInstanceOfType(newForceBlueScreenActivity, typeof(ForceBlueScreenActivity));

            // GetCurrentLoggedOnUsersActivity
            GetCurrentLoggedOnUsersActivity newGetLoggedOnUsersActivity = new GetCurrentLoggedOnUsersActivity();
            Assert.IsInstanceOfType(newGetLoggedOnUsersActivity, typeof(GetCurrentLoggedOnUsersActivity));

            // GetOsFileVersionActivity
            GetOsFileVersionActivity newGetOsFileVersionActivity = new GetOsFileVersionActivity();
            Assert.IsInstanceOfType(newGetOsFileVersionActivity, typeof(GetOsFileVersionActivity));

            // GetProcessIdsActivity
            GetProcessIdsActivity newGetProcessIdActivity = new GetProcessIdsActivity();
            Assert.IsInstanceOfType(newGetProcessIdActivity, typeof(GetProcessIdsActivity));

            // GetSystemUptimeActivity
            GetSystemUptimeActivity newGetSystemUptimeActivity = new GetSystemUptimeActivity();
            Assert.IsInstanceOfType(newGetSystemUptimeActivity, typeof(GetSystemUptimeActivity));

            // GetSystemTimeSkewActivity
            GetSystemTimeSkewActivity newGetTimeSkewActivity = new GetSystemTimeSkewActivity();
            Assert.IsInstanceOfType(newGetTimeSkewActivity, typeof(GetSystemTimeSkewActivity));

            // GetWindowsEventsActivity
            GetWindowsEventsActivity newGetWindowsEventsActivity = new GetWindowsEventsActivity();
            Assert.IsInstanceOfType(newGetWindowsEventsActivity, typeof(GetWindowsEventsActivity));

            // InstallSysInternalsActivity
            InstallSysInternalsActivity newInstallSysInternalsActivity = new InstallSysInternalsActivity();
            Assert.IsInstanceOfType(newInstallSysInternalsActivity, typeof(InstallSysInternalsActivity));

            // EditConfigurationFileActivity
            EditConfigurationFileActivity newModifyConfigurationFileActivity = new EditConfigurationFileActivity();
            Assert.IsInstanceOfType(newModifyConfigurationFileActivity, typeof(EditConfigurationFileActivity));

            // PingResponseActivity
            PingResponseActivity newPingActivity = new PingResponseActivity();
            Assert.IsInstanceOfType(newPingActivity, typeof(PingResponseActivity));

            // PortConnectivityActivity
            PortConnectivityActivity newPortConnectivityActivity = new PortConnectivityActivity();
            Assert.IsInstanceOfType(newPortConnectivityActivity, typeof(PortConnectivityActivity));

            // ReadFileContentsActivity
            ReadFileContentsActivity newReadFileContentsActivity = new ReadFileContentsActivity();
            Assert.IsInstanceOfType(newReadFileContentsActivity, typeof(ReadFileContentsActivity));

            // RenameMachineActivity
            RenameMachineActivity newRenameMachineActivity = new RenameMachineActivity();
            Assert.IsInstanceOfType(newRenameMachineActivity, typeof(RenameMachineActivity));

            // RestartServiceActivity
            RestartServiceActivity newRestartServiceActivity = new RestartServiceActivity();
            Assert.IsInstanceOfType(newRestartServiceActivity, typeof(RestartServiceActivity));

            // SetSymbolServerEnvironmentPathActivity
            SetSymbolServerEnvironmentPathActivity newSetSymbolsEnvironmentPath = new SetSymbolServerEnvironmentPathActivity();
            Assert.IsInstanceOfType(newSetSymbolsEnvironmentPath, typeof(SetSymbolServerEnvironmentPathActivity));

            // StartSecureDeleteActivity
            StartSecureDeleteActivity newStartSecureDeleteActivity = new StartSecureDeleteActivity();
            Assert.IsInstanceOfType(newStartSecureDeleteActivity, typeof(StartSecureDeleteActivity));

            // WebStringActivity
            WebStringActivity neWebStringActivity = new WebStringActivity();
            Assert.IsInstanceOfType(neWebStringActivity, typeof(WebStringActivity));

            // WindowsUpdateActivity
            WindowsUpdateActivity newWindowsUpdateActivity = new WindowsUpdateActivity();
            Assert.IsInstanceOfType(newWindowsUpdateActivity, typeof(WindowsUpdateActivity));
        }
    }
}