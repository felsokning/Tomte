//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.AdminService
{
    using System.ServiceProcess;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Program"/> class.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AdminService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}