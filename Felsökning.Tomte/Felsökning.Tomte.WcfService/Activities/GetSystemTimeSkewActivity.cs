//-----------------------------------------------------------------------
// <copyright file="GetSystemTimeSkewActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetSystemTimeSkewActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class GetSystemTimeSkewActivity : SwedishCodeActivity<object>
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
            string ntpTarget = context.GetValue(FirstArgument);

            // SNTP message size is 16 bytes from the digest - https://tools.ietf.org/html/rfc2030
            byte[] ntpData = new byte[48];

            // Setting the Leap Indicator, Version Number and Mode values where
            // LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)
            ntpData[0] = 0x1B;

            // NTP uses UDP and the standard UDP port number assigned to NTP is 123
            IPAddress[] ntpServerAddresses = Dns.GetHostEntry(ntpTarget).AddressList;
            IPEndPoint ipEndPoint = new IPEndPoint(ntpServerAddresses[0], 123);
            using (Socket socket = new Socket(ntpServerAddresses[0].AddressFamily, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);

                // Stops an unresponsive hang, if NTP is blocked
                socket.ReceiveTimeout = 3000;
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            }

            // Now, we need the local server time to compare it to.
            DateTime localServerUtcTime = DateTime.UtcNow;

            // Offset to the "Transmit Timestamp" field (time at which the reply 
            // departed the server for the client, in 64-bit timestamp format.)
            byte serverReplyTime = 40;
            ulong secondsPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
            ulong secondsFractionsPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            // Convert endian
            secondsPart = SwapEndianBits(secondsPart);
            secondsFractionsPart = SwapEndianBits(secondsFractionsPart);
            ulong milliseconds = (secondsPart * 1000) + ((secondsFractionsPart * 1000) / 0x100000000L);
            DateTime networkServerUtcTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)milliseconds);
            TimeSpan offSet = networkServerUtcTime - localServerUtcTime;
            return offSet;
        }

        /// <summary>
        ///     Swaps the bits provided by the NTP server.
        /// </summary>
        /// <param name="x">The data that we received from the server.</param>
        /// <returns>An unsigned integer with the endian swapped.</returns>
        private uint SwapEndianBits(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }
    }
}