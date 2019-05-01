//-----------------------------------------------------------------------
// <copyright file="WebStringActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Net;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WebStringActivity" /> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class WebStringActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Gets or sets the value of the <see cref="FirstArgument"/> parameter.
        /// </summary>
        public InArgument<string> FirstArgument { get; set; }

        /// <summary>
        ///     The <see cref="Execute(CodeActivityContext)"/> method inherited from <see cref="CodeActivity{TResult}"/>.
        /// </summary>
        /// <param name="context">The current <see cref="CodeActivity"/> context.</param>
        /// <returns>A string response from the request.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            FirstArgument = FirstInArgument;
            string target = context.GetValue(FirstArgument);
            using (WebClient newWebClient = new WebClient())
            {
                return newWebClient.DownloadString(target);
            }
        }
    }
}