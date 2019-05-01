//-----------------------------------------------------------------------
// <copyright file="PingResponseActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Net.NetworkInformation;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PingActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class PingResponseActivity : SwedishCodeActivity<object>
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
            string target = context.GetValue(FirstArgument);
            bool reached = false;
            using (Ping newPing = new Ping())
            {
                try
                {
                    PingReply reply = newPing.Send(target, 1000);
                    reached = reply?.Status == IPStatus.Success;
                }
                catch (PingException)
                {
                    return reached;
                }

                return reached;
            }
        }
    }
}