//-----------------------------------------------------------------------
// <copyright file="ComputerNameFormat.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Enums
{
    /// <summary>
    ///     Specifies a type of computer name.
    /// </summary>
    public enum ComputerNameFormat
    {
        /// <summary>
        ///     The NetBIOS name of the local computer or the cluster associated with the local computer.
        ///     This name is limited to MAX_COMPUTERNAME_LENGTH + 1 characters and may be a truncated version of the DNS host name.
        ///     For example, if the DNS host name is "corporate-mail-server", the NetBIOS name would be "corporate-mail-"
        /// </summary>
        ComputerNameNetBIOS,

        /// <summary>
        ///     The DNS name of the local computer or the cluster associated with the local computer.
        /// </summary>
        ComputerNameDnsHostname,

        /// <summary>
        ///     The name of the DNS domain assigned to the local computer or the cluster associated with the local computer.
        /// </summary>
        ComputerNameDnsDomain,

        /// <summary>
        ///     The fully qualified DNS name that uniquely identifies the local computer or the cluster associated with the local computer.
        ///     This name is a combination of the DNS host name and the DNS domain name, using the form HostName.DomainName.
        ///     For example, if the DNS host name is "corporate-mail-server" and the DNS domain name is "microsoft.com", the fully qualified DNS name is "corporate-mail-server.microsoft.com".
        /// </summary>
        ComputerNameDnsFullyQualified,

        /// <summary>
        ///     The NetBIOS name of the local computer. On a cluster, this is the NetBIOS name of the local node on the cluster.
        /// </summary>
        ComputerNamePhysicalNetBIOS,

        /// <summary>
        ///     The DNS host name of the local computer. On a cluster, this is the DNS host name of the local node on the cluster.
        /// </summary>
        ComputerNamePhysicalDnsHostname,

        /// <summary>
        ///     The name of the DNS domain assigned to the local computer. On a cluster, this is the DNS domain of the local node on the cluster.
        /// </summary>
        ComputerNamePhysicalDnsDomain,

        /// <summary>
        ///     The fully qualified DNS name that uniquely identifies the computer.
        ///     On a cluster, this is the fully qualified DNS name of the local node on the cluster.
        ///     The fully qualified DNS name is a combination of the DNS host name and the DNS domain name, using the form HostName.DomainName.
        /// </summary>
        ComputerNamePhysicalDnsFullyQualified,

        /// <summary>
        ///     Not used.
        /// </summary>
        ComputerNameMax
    }
}