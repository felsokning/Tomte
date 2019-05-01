//-----------------------------------------------------------------------
// <copyright file="GetProcessIdsActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Objects;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetProcessIdsActivity" /> activity.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class GetProcessIdsActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Gets or sets the value of the <see cref="FirstArgument"/> parameter.
        /// </summary>
        public InArgument<string> FirstArgument { get; set; }

        /// <summary>
        ///     Gets or sets the value of the <see cref="SecondArgument"/> parameter.
        /// </summary>
        public InArgument<string> SecondArgument { get; set; }

        /// <summary>
        ///     The <see cref="Execute(CodeActivityContext)"/> method inherited from <see cref="CodeActivity{TResult}"/>.
        /// </summary>
        /// <param name="context">The current <see cref="CodeActivity"/> context.</param>
        /// <returns>A DateTime object.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            string stringProcessName = context.GetValue(FirstArgument);
            string appPoolBoolean = context.GetValue(SecondArgument);

            List<ReturnProcess> processIdList = new List<ReturnProcess>(0);
            if (!string.IsNullOrWhiteSpace(stringProcessName))
            {
                bool isAnAppPool = bool.Parse(appPoolBoolean);
                if (!isAnAppPool)
                {
                    Process[] processes = Process.GetProcessesByName(stringProcessName);
                    processes.ToList().ForEach(p =>
                    {
                        // Tested as working in JIT'ed C# in PowerShell.
                        processIdList.Add(new ReturnProcess { ProcessName = p.ProcessName, ProcessId = p.Id });
                    });
                }
                else
                {
                    // TODO: SCIENCE FOR APP POOLS
                    // for now, it will be an empty list. Sorry, not sorry.
                    return processIdList;
                }
            }

            return processIdList;
        }
    }
}