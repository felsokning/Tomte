//-----------------------------------------------------------------------
// <copyright file="AdminService.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.AdminService
{
    using System;
    using System.Activities.DurableInstancing;
    using System.ServiceModel;
    using System.ServiceModel.Activities.Description;
    using System.ServiceProcess;

    using WcfService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AdminService"/> class.
    ///     The Windows Service wraps the <see cref="WcfService"/> and makes it available.
    /// </summary>
    /// <inheritdoc cref="ServiceBase" />
    public partial class AdminService : ServiceBase
    {
        /// <summary>
        ///     The ServiceHost used to host the WCF service.
        /// </summary>
        private ServiceHost _internalServiceHost;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdminService"/> class.
        /// </summary>
        public AdminService()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Overrides the <see cref="OnStart(string[])"/> method inherited from <see cref="ServiceBase"/>.
        /// </summary>
        /// <param name="args">Arguments passed on the start of the service.</param>
        protected override void OnStart(string[] args)
        {
            _internalServiceHost = new ServiceHost(typeof(WorkflowService));
            SqlWorkflowInstanceStoreBehavior instanceStoreBehavior = new SqlWorkflowInstanceStoreBehavior("Server=192.168.0.252,1433\\SQL2008EXPRESS;Initial Catalog=WorkflowInstanceStore;Integrated Security=SSPI")
            {
                HostLockRenewalPeriod = TimeSpan.FromSeconds(1),
                InstanceCompletionAction = InstanceCompletionAction.DeleteNothing,
                InstanceLockedExceptionAction = InstanceLockedExceptionAction.AggressiveRetry,
                RunnableInstancesDetectionPeriod = TimeSpan.FromSeconds(1) // Minimum allowed value.
            };

            _internalServiceHost.Description.Behaviors.Add(instanceStoreBehavior);
            _internalServiceHost.Open();
        }

        /// <summary>
        ///     Overrides the <see cref="OnStop"/> method inherited from <see cref="ServiceBase"/>.
        /// </summary>
        protected override void OnStop()
        {
            _internalServiceHost.Close();
        }
    }
}