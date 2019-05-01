//-----------------------------------------------------------------------
// <copyright file="RenameMachineActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Enums;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RenameMachineActivity"/> class.
    /// </summary>
    public class RenameMachineActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     The target type passed by the caller that we translate.
        /// </summary>
        private ComputerNameFormat _resultType;

        /// <summary>
        ///     Gets or sets the value of the <see cref="FirstArgument"/> parameter.
        /// </summary>
        public InArgument<string> FirstArgument { get; set; }

        /// <summary>
        ///     Gets or sets the value of the <see cref="SecondArgument"/> parameter.
        /// </summary>
        public InArgument<string> SecondArgument { get; set; }

        /// <summary>
        ///     Sets a new NetBIOS or DNS name for the local computer. Name changes made by SetComputerNameEx do not take effect until the user restarts the computer.
        /// </summary>
        /// <param name="nameType">The type of name to be set. This parameter can be one of the following values from the <see cref="COMPUTER_NAME_FORMAT"/> enumeration type.</param>
        /// <param name="computerName">The new name. The name cannot include control characters, leading or trailing spaces, or any special characters.</param>
        /// <returns>If the function succeeds, the return value is a nonzero value. If the function fails, the return value is zero.</returns>
        [DllImport("kernel32.dll")]
        internal static extern bool SetComputerNameExA(ComputerNameFormat nameType, string computerName);

        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            FirstArgument = FirstInArgument;
            string targetName = context.GetValue(FirstArgument);
            SecondArgument = SecondInArgument;
            string nameType = context.GetValue(SecondArgument);
            Enum.TryParse(nameType, true, out _resultType);

            if (SetComputerNameExA(_resultType, targetName))
            {
                Thread.Sleep(TimeSpan.FromSeconds(30));
                Process lsassProcess = Process.GetProcessesByName("lsass").FirstOrDefault();
                lsassProcess?.Kill();
                return $"Renaming system to {targetName} was successful. Restarting the machine for this take effect.";
            }

            return $"Renaming system to {targetName} was unsuccessful.";
        }
    }
}