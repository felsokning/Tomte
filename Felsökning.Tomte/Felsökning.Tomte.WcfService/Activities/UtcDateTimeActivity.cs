//-----------------------------------------------------------------------
// <copyright file="UtcDateTimeActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UtcDateTimeActivity" /> activity.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class UtcDateTimeActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     The <see cref="Execute(CodeActivityContext)"/> method inherited from <see cref="CodeActivity{TResult}"/>.
        /// </summary>
        /// <param name="context">The current <see cref="CodeActivity"/> context.</param>
        /// <returns>A DateTime object.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            // Return a simple <T> for demonstration.
            return DateTime.UtcNow;
        }
    }
}