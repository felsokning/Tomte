//-----------------------------------------------------------------------
// <copyright file="WorkflowService.svc.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService
{
    using System;
    using System.Activities;
    using System.Activities.DurableInstancing;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.DurableInstancing;
    using System.Threading;

    using Activities;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorkflowService"/> class.
    /// </summary>
    public class WorkflowService : IWorkflowService
    {
        /// <summary>
        ///     Gets or sets the value of the <see cref="ResultObject"/> property to return to the caller.
        /// </summary>
        private object ResultObject { get; set; }

        /// <summary>
        ///     Gets or sets the value of the <see cref="ResultEvents"/> property to return to the caller.
        /// </summary>
        private EventLogEntry[] ResultEvents { get; set; }

        /// <summary>
        ///     Invokes the given workflow passed via SOAP.
        /// </summary>
        /// <param name="value">The command and parameter string.</param>
        /// <returns>A list of events.</returns>
        /// <inheritdoc cref="IWorkflowService"/>
        public EventLogEntry[] InvokeEventsWorkflow(string value)
        {
            // This should always be three parameters.
            string[] commandAndParameters = ParseCommandAndParameter(value);
            string command = commandAndParameters[0];
            string parameter = commandAndParameters[1];
            string[] splitParameters = ParseParameters(parameter);
            string firstParameter = splitParameters[0];
            string secondParameter = splitParameters[1];
            string thirdParameter = splitParameters[2];
            StartEventsWorkflowScience(command, firstParameter, secondParameter, thirdParameter);
            GC.Collect();
            return ResultEvents;
        }

        /// <summary>
        ///     Invokes the given workflow passed via SOAP.
        /// </summary>
        /// <param name="value">The command and parameter string.</param>
        /// <returns>A string value back to the caller.</returns>
        /// <inheritdoc cref="IWorkflowService"/>
        public object InvokeWorkflow(string value)
        {
            // Determine if we were given a parameter in the call and, if so, process it.
            if (EvaluateContainsDelimiter("&", value))
            {
                string[] commandAndParameters = ParseCommandAndParameter(value);
                string command = commandAndParameters[0];
                string parameter = commandAndParameters[1];
                if (!EvaluateContainsDelimiter("!", parameter))
                {
                    StartWorkflowScience(command, parameter, string.Empty, string.Empty);
                }
                else
                {
                    string[] splitParameters = ParseParameters(parameter);
                    if (splitParameters.Length.Equals((int)2))
                    {
                        string firstParameter = splitParameters[0];
                        string secondParameter = splitParameters[1];
                        StartWorkflowScience(command, firstParameter, secondParameter, string.Empty);
                    }
                    else if (splitParameters.Length.Equals((int)3))
                    {
                        string firstParameter = splitParameters[0];
                        string secondParameter = splitParameters[1];
                        string thirdParameter = splitParameters[2];
                        StartWorkflowScience(command, firstParameter, secondParameter, thirdParameter);
                    }
                    else
                    {
                        // If we got here, something is very broken.
                        throw new ArgumentException($"There was a problem processing the parameters. Parameters length was: {splitParameters.Length}");
                    }
                }
            }
            else
            {
                // We were just given a command, so let's assume best-faith effort (for now) and run it.
                StartWorkflowScience(value, string.Empty, string.Empty, string.Empty);
            }

            GC.Collect();
            return ResultObject.ToString();
        }

        /// <summary>
        ///     We determine if the string we were given contains a delimiter.
        /// </summary>
        /// <param name="delimiter">The delimiter that we're looking for.</param>
        /// <param name="workflowData">The string passed from WCF.</param>
        /// <returns>A boolean indicating if the string contains a delimiter.</returns>
        /// <inheritdoc cref="IWorkflowService"/>
        public bool EvaluateContainsDelimiter(string delimiter, string workflowData)
        {
            if (workflowData.Contains(delimiter))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Parses the string provided to return a string array containing the command and the parameter.
        /// </summary>
        /// <param name="workflowData">The string to parse.</param>
        /// <returns>The string array containing the command and parameter.</returns>
        /// <inheritdoc cref="IWorkflowService"/>
        public string[] ParseCommandAndParameter(string workflowData)
        {
            return workflowData.Split('&');
        }

        /// <summary>
        ///     Parses the string provided to return a string array containing the command and the parameter.
        /// </summary>
        /// <param name="workflowData">The string to parse.</param>
        /// <returns>The string array containing the command and parameter.</returns>
        public string[] ParseParameters(string workflowData)
        {
            return workflowData.Split('!');
        }

        /// <summary>
        ///     Starts processing the given activity via the WorkflowApplication.
        /// </summary>
        /// <param name="command">The command to be ran.</param>
        /// <param name="firstParameter">The first parameter to include (if any).</param>
        /// <param name="secondParameter">The second parameter to include (if any).</param>
        /// <param name="thirdParameter">The third parameter to include (if any).</param>
        private void StartEventsWorkflowScience(string command, string firstParameter, string secondParameter, string thirdParameter)
        {
            // Now stage the WorkflowApplication, using the SQL instance, wrapped in using to dispose all of the things when done.
            using (AutoResetEvent syncEvent = new AutoResetEvent(false))
            {
                // NOTE: If the string doesn't - explicitly - match, the .ctor() of the SwedishCodeActivity will throw,
                // since the string is the key.
                Dictionary<string, SwedishCodeActivity<EventLogEntry[]>> newDictionary = new Dictionary<string, SwedishCodeActivity<EventLogEntry[]>>
                                                                                {
                                                                                    { "GetWindowsEventsActivity", new GetWindowsEventsActivity() }
                                                                                };

                // See the 'Activities' folder for examples of Activities that you can use here.
                SwedishCodeActivity<EventLogEntry[]> newSwedishCodeActivity = (SwedishCodeActivity<EventLogEntry[]>)newDictionary[command];
                if (!string.IsNullOrWhiteSpace(firstParameter))
                {
                    newSwedishCodeActivity.FirstInArgument = firstParameter;
                }

                if (!string.IsNullOrWhiteSpace(secondParameter))
                {
                    newSwedishCodeActivity.SecondInArgument = secondParameter;
                }

                if (!string.IsNullOrWhiteSpace(thirdParameter))
                {
                    newSwedishCodeActivity.ThirdInArgument = thirdParameter;
                }

                SqlWorkflowInstanceStore newSqlWorkflowInstanceStore = new SqlWorkflowInstanceStore("Server=192.168.0.252,1433\\SQL2008EXPRESS;Initial Catalog=WorkflowInstanceStore;Integrated Security=SSPI")
                {
                    HostLockRenewalPeriod = TimeSpan.FromSeconds(1),
                    InstanceCompletionAction = InstanceCompletionAction.DeleteNothing,
                    InstanceLockedExceptionAction = InstanceLockedExceptionAction.AggressiveRetry,
                    RunnableInstancesDetectionPeriod = TimeSpan.FromSeconds(1) // Minimum allowed value.
                };

                InstanceHandle workflowInstanceStoreHandle = newSqlWorkflowInstanceStore.CreateInstanceHandle();
                CreateWorkflowOwnerCommand createWorkflowOwnerCommand = new CreateWorkflowOwnerCommand();
                InstanceView newInstanceView = newSqlWorkflowInstanceStore.Execute(workflowInstanceStoreHandle, createWorkflowOwnerCommand, TimeSpan.FromSeconds(30));
                newSqlWorkflowInstanceStore.DefaultInstanceOwner = newInstanceView.InstanceOwner;

                WorkflowApplication newWorkflowApplication = new WorkflowApplication(newSwedishCodeActivity, new WorkflowIdentity
                {
                    // The Dictionary will throw for non-found key before we ever get here, so no need to validate input.
                    Name = command,
                    Version = new Version(0, 1, 0, 0)
                })
                {
                    InstanceStore = newSqlWorkflowInstanceStore,
                    SynchronizationContext = SynchronizationContext.Current
                };

                newWorkflowApplication.Persist();
                ResultEvents = new EventLogEntry[0];
                newWorkflowApplication.Completed += delegate (WorkflowApplicationCompletedEventArgs e)
                {
                    if (e.CompletionState == ActivityInstanceState.Faulted)
                    {
                        EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has faulted.\nException: {e.TerminationException.GetType().FullName}\nMessage:{e.TerminationException.Message}");
                        syncEvent.Set();
                    }
                    else if (e.CompletionState == ActivityInstanceState.Canceled)
                    {
                        EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has been canceled.");
                        syncEvent.Set();
                    }
                    else
                    {
                        // Since the result can be *anything*, let's not treat it like a string.
                        EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} completed. Result: {e.Outputs["Result"]}");
                        ResultEvents = (EventLogEntry[])e.Outputs["Result"];
                        syncEvent.Set();
                    }
                };

                newWorkflowApplication.Aborted = delegate (WorkflowApplicationAbortedEventArgs e)
                {
                    // The workflow aborted, so let's find out why.
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has been aborted.\nException: {e.Reason.GetType().FullName}\nMessage:{e.Reason.Message}");
                    syncEvent.Set();
                };

                newWorkflowApplication.Idle = delegate (WorkflowApplicationIdleEventArgs e)
                {
                    // TODO: [FUTURE] Need to handle future persistence maintenance.
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has entered the Idle state.");
                    syncEvent.Set();
                };

                newWorkflowApplication.PersistableIdle = delegate (WorkflowApplicationIdleEventArgs e)
                {
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has entered PersistableIdle.");
                    syncEvent.Set();

                    // Runtime will persist.
                    return PersistableIdleAction.Persist;
                };

                newWorkflowApplication.Unloaded = delegate (WorkflowApplicationEventArgs e)
                {
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has been unloaded.");
                    syncEvent.Set();
                };

                newWorkflowApplication.OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs e)
                {
                    // Log the unhandled exception.
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService",
                        !string.IsNullOrWhiteSpace(e.UnhandledException.InnerException?.Message)
                            ? $"Workflow {e.InstanceId} has reached an AggregateException in OnUnhandledException.\nException Source: {e.ExceptionSource.DisplayName}\nException Instance ID: {e.ExceptionSourceInstanceId}\nException: {e.UnhandledException.InnerException.GetType().FullName}\nMessage: {e.UnhandledException.InnerException.Message}\nFirstArgument: {firstParameter}\nSecondArgument: {secondParameter}"
                            : $"Workflow {e.InstanceId} has reached OnUnhandledException.\nException Source: {e.ExceptionSource.DisplayName}\nException Instance ID: {e.ExceptionSourceInstanceId}\nException: {e.UnhandledException.GetType().FullName}\nMessage: {e.UnhandledException.Message}\nFirstArgument: {firstParameter}\nSecondArgument: {secondParameter}");

                    syncEvent.Set();

                    // Instruct the runtime to terminate the workflow.
                    // The other viable choices here are 'Abort' or 'Cancel'
                    return UnhandledExceptionAction.Terminate;
                };

                newWorkflowApplication.Run();

                // Because a new thread is spawned, we need to wait for it to complete before we can move on.
                syncEvent.WaitOne();

                // Instance MUST be unloaded to update the SQL record. One would think this would happen on the overridden delegate methods (e.g.: Completed,OnUnhandledException, etc.)
                // but testing has proven this to not be the case.
                newWorkflowApplication.Unload(TimeSpan.FromSeconds(30));

                // Now, we dump the instance owner.
                DeleteWorkflowOwnerCommand newDeleteWorkflowOwnerCommand = new DeleteWorkflowOwnerCommand();
                newSqlWorkflowInstanceStore.Execute(
                   workflowInstanceStoreHandle,
                   newDeleteWorkflowOwnerCommand,
                   TimeSpan.FromSeconds(30));
            }

            GC.Collect();
        }

        /// <summary>
        ///     Starts processing the given activity via the WorkflowApplication.
        /// </summary>
        /// <param name="command">The command to be ran.</param>
        /// <param name="firstParameter">The first parameter to include (if any).</param>
        /// <param name="secondParameter">The second parameter to include (if any).</param>
        /// <param name="thirdParameter">The third parameter to include (if any).</param>
        private void StartWorkflowScience(string command, string firstParameter, string secondParameter, string thirdParameter /* future use */)
        {
            // Now stage the WorkflowApplication, using the SQL instance, wrapped in using to dispose all of the things when done.
            using (AutoResetEvent syncEvent = new AutoResetEvent(false))
            {
                // NOTE: If the string doesn't - explicitly - match, the .ctor() of the 
                // SwedishCodeActivity will throw, since the string is the key. Also,
                // no boxing/unboxing required for Dictionary<T,T>; which saves overhead.
                Dictionary<string, SwedishCodeActivity<object>> newDictionary = new Dictionary<string, SwedishCodeActivity<object>>
                {
                    // See the 'Activities' folder for examples of Activities that you can use here.
                    { "CheckFreeDiskSpaceActivity", new CheckFreeDiskSpaceActivity() },
                    { "CopyFilesActivity", new CopyFilesActivity() },
                    { "CopyNIsAndDLLsActivity", new CopyImagesAndLibrariesActivity() },
                    { "DateTimeActivity", new UtcDateTimeActivity() },
                    { "DumpProcessThreadsActivity", new DumpProcessThreadsActivity() },
                    { "FileExistsActivity", new CheckIfFileExistsActivity() },
                    { "ForceBlueScreenActivity", new ForceBlueScreenActivity() },
                    { "GetLoggedOnUsersActivity", new GetCurrentLoggedOnUsersActivity() },
                    { "GetOsFileVersionActivity", new GetOsFileVersionActivity() },
                    { "GetProcessIdActivity", new GetProcessIdsActivity() },
                    { "GetSystemUptimeActivity", new GetSystemUptimeActivity() },
                    { "GetTimeSkewActivity", new GetSystemTimeSkewActivity() },
                    { "InstallSysInternalsActivity", new InstallSysInternalsActivity() },
                    { "ModifyConfigurationFileActivity", new EditConfigurationFileActivity() },
                    { "PingActivity", new PingResponseActivity() },
                    { "PortConnectivityActivity", new PortConnectivityActivity() },
                    { "ReadFileContentsActivity", new ReadFileContentsActivity() },
                    { "RenameMachineActivity", new RenameMachineActivity() },
                    { "RestartServiceActivity", new RestartServiceActivity() },
                    { "SetSymbolsEnvironmentPath", new SetSymbolServerEnvironmentPathActivity() },
                    { "StartSecureDeleteActivity", new StartSecureDeleteActivity() },
                    { "WindowsUpdateActivity", new WindowsUpdateActivity() },
                    { "WebStringActivity", new WebStringActivity() }
                };

                SwedishCodeActivity<object> newSwedishCodeActivity = (SwedishCodeActivity<object>)newDictionary[command];
                if (!string.IsNullOrWhiteSpace(firstParameter))
                {
                    newSwedishCodeActivity.FirstInArgument = firstParameter;
                }

                if (!string.IsNullOrWhiteSpace(secondParameter))
                {
                    newSwedishCodeActivity.SecondInArgument = secondParameter;
                }

                if (!string.IsNullOrWhiteSpace(thirdParameter))
                {
                    newSwedishCodeActivity.ThirdInArgument = thirdParameter;
                }

                SqlWorkflowInstanceStore newSqlWorkflowInstanceStore = new SqlWorkflowInstanceStore("Server=192.168.0.252,1433\\SQL2008EXPRESS;Initial Catalog=WorkflowInstanceStore;Integrated Security=SSPI")
                {
                    HostLockRenewalPeriod = TimeSpan.FromSeconds(1),
                    InstanceCompletionAction = InstanceCompletionAction.DeleteNothing,
                    InstanceLockedExceptionAction = InstanceLockedExceptionAction.AggressiveRetry,
                    RunnableInstancesDetectionPeriod = TimeSpan.FromSeconds(1) // Minimum allowed value.
                };

                InstanceHandle workflowInstanceStoreHandle = newSqlWorkflowInstanceStore.CreateInstanceHandle();
                CreateWorkflowOwnerCommand createWorkflowOwnerCommand = new CreateWorkflowOwnerCommand();
                InstanceView newInstanceView = newSqlWorkflowInstanceStore.Execute(workflowInstanceStoreHandle, createWorkflowOwnerCommand, TimeSpan.FromSeconds(30));
                newSqlWorkflowInstanceStore.DefaultInstanceOwner = newInstanceView.InstanceOwner;

                WorkflowApplication newWorkflowApplication = new WorkflowApplication(newSwedishCodeActivity, new WorkflowIdentity
                {
                    // The Dictionary will throw for non-found key before we ever get here, so no need to validate input.
                    Name = command,
                    Version = new Version(0, 1, 0, 0)
                })
                {
                    InstanceStore = newSqlWorkflowInstanceStore,
                    SynchronizationContext = SynchronizationContext.Current
                };

                newWorkflowApplication.Persist();
                ResultObject = new object();
                newWorkflowApplication.Completed += delegate (WorkflowApplicationCompletedEventArgs e)
                {
                    if (e.CompletionState == ActivityInstanceState.Faulted)
                    {
                        EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has faulted.\nException: {e.TerminationException.GetType().FullName}\nMessage:{e.TerminationException.Message}");
                        syncEvent.Set();
                    }
                    else if (e.CompletionState == ActivityInstanceState.Canceled)
                    {
                        EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has been canceled.");
                        syncEvent.Set();
                    }
                    else
                    {
                        // Since the result can be *anything*, let's not treat it like a string.
                        EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} completed. Result: {e.Outputs["Result"]}");
                        ResultObject = e.Outputs["Result"];
                        syncEvent.Set();
                    }
                };

                newWorkflowApplication.Aborted = delegate (WorkflowApplicationAbortedEventArgs e)
                {
                    // The workflow aborted, so let's find out why.
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has been aborted.\nException: {e.Reason.GetType().FullName}\nMessage:{e.Reason.Message}");
                    syncEvent.Set();
                };

                newWorkflowApplication.Idle = delegate (WorkflowApplicationIdleEventArgs e)
                {
                    // TODO: [FUTURE] Need to handle future persistence maintenance.
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has entered the Idle state.");
                    syncEvent.Set();
                };

                newWorkflowApplication.PersistableIdle = delegate (WorkflowApplicationIdleEventArgs e)
                {
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has entered PersistableIdle.");
                    syncEvent.Set();

                    // Runtime will persist.
                    return PersistableIdleAction.Persist;
                };

                newWorkflowApplication.Unloaded = delegate (WorkflowApplicationEventArgs e)
                {
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService", $"Workflow {e.InstanceId} has been unloaded.");
                    syncEvent.Set();
                };

                newWorkflowApplication.OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs e)
                {
                    // Log the unhandled exception.
                    EventLog.WriteEntry("Felsökning.Tomte.AdminService",
                        !string.IsNullOrWhiteSpace(e.UnhandledException.InnerException?.Message)
                            ? $"Workflow {e.InstanceId} has reached an AggregateException in OnUnhandledException.\nException Source: {e.ExceptionSource.DisplayName}\nException Instance ID: {e.ExceptionSourceInstanceId}\nException: {e.UnhandledException.InnerException.GetType().FullName}\nMessage: {e.UnhandledException.InnerException.Message}\nFirstArgument: {firstParameter}\nSecondArgument: {secondParameter}"
                            : $"Workflow {e.InstanceId} has reached OnUnhandledException.\nException Source: {e.ExceptionSource.DisplayName}\nException Instance ID: {e.ExceptionSourceInstanceId}\nException: {e.UnhandledException.GetType().FullName}\nMessage: {e.UnhandledException.Message}\nFirstArgument: {firstParameter}\nSecondArgument: {secondParameter}");

                    syncEvent.Set();

                    // Instruct the runtime to terminate the workflow.
                    // The other viable choices here are 'Abort' or 'Cancel'
                    return UnhandledExceptionAction.Terminate;
                };

                newWorkflowApplication.Run();

                // Because a new thread is spawned, we need to wait for it to complete before we can move on.
                syncEvent.WaitOne();

                // Instance MUST be unloaded to update the SQL record. One would think this would happen on the overridden delegate methods (e.g.: Completed,OnUnhandledException, etc.)
                // but testing has proven this to not be the case.
                newWorkflowApplication.Unload(TimeSpan.FromSeconds(30));

                // Now, we dump the instance owner.
                DeleteWorkflowOwnerCommand newDeleteWorkflowOwnerCommand = new DeleteWorkflowOwnerCommand();
                newSqlWorkflowInstanceStore.Execute(
                   workflowInstanceStoreHandle,
                   newDeleteWorkflowOwnerCommand,
                   TimeSpan.FromSeconds(30));
            }

            GC.Collect();
        }
    }
}