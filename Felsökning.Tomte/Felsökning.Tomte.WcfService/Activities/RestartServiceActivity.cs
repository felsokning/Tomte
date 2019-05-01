//-----------------------------------------------------------------------
// <copyright file="RestartServiceActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Diagnostics;
    using System.ServiceProcess;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RestartServiceActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class RestartServiceActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Gets or sets the value of the <see cref="FirstArgument"/> parameter.
        /// </summary>
        public InArgument<string> FirstArgument { get; set; }

        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            FirstArgument = FirstInArgument;
            string serviceNameValue = context.GetValue(FirstArgument);

            try
            {
                using (ServiceController newServiceController = new ServiceController(serviceNameValue))
                {
                    if (newServiceController.Status.Equals(ServiceControllerStatus.Running)
                        || newServiceController.Status.Equals(ServiceControllerStatus.StartPending)
                        || newServiceController.Status.Equals(ServiceControllerStatus.Paused))
                    {
                        newServiceController.Stop();
                        newServiceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        newServiceController.Start();
                        newServiceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        return $"Successfully restarted {serviceNameValue} service.";
                    }
                    else
                    {
                        return $"The service {serviceNameValue} is not in a running state.";
                    }
                }
            }
            catch (ArgumentException e)
            {
                string message = $"Encountered an error with restarting '{serviceNameValue}': {e.Message}";
                EventLog.WriteEntry("Service1", message);
                return message;
            }
        }
    }
}