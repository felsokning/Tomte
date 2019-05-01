//-----------------------------------------------------------------------
// <copyright file="GetSystemUptimeActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Diagnostics;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetSystemUptimeActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class GetSystemUptimeActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            PerformanceCounter uptimeCounter = new PerformanceCounter("System", "System Up Time");

            // Init sometimes defaults to out as zero; so, let's brace for that possibility.
            uptimeCounter.NextValue();

            return TimeSpan.FromSeconds(uptimeCounter.NextValue());
        }
    }
}