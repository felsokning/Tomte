//-----------------------------------------------------------------------
// <copyright file="PortConnectivityActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PortConnectivityActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class PortConnectivityActivity : SwedishCodeActivity<object>
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
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            FirstArgument = FirstInArgument;
            SecondArgument = SecondInArgument;
            string hostOrIp = context.GetValue(FirstArgument);
            string port = context.GetValue(SecondArgument);

            using (TcpClient newTcpClient = new TcpClient())
            {
                // Determine if we were given an IP Address.
                if (IPAddress.TryParse(hostOrIp, out IPAddress ipAddress))
                {
                    newTcpClient.Connect(ipAddress, int.Parse(port));
                    return newTcpClient.Connected;
                }
                else
                {
                    newTcpClient.Connect(hostOrIp, int.Parse(port));
                    return newTcpClient.Connected;
                }
            }
        }
    }
}