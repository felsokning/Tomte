//-----------------------------------------------------------------------
// <copyright file="ForceBlueScreenActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ForceBlueScreenActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class ForceBlueScreenActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity"/>
        protected override object Execute(CodeActivityContext context)
        {
            using (Process lsassProcess = Process.GetProcessesByName("lsass").FirstOrDefault())
            {
                lsassProcess?.Kill();
                return "Successfully killed the LSASS process.";
            }
        }
    }
}