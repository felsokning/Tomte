//-----------------------------------------------------------------------
// <copyright file="ReturnProcess.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Objects
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ReturnProcess"/> class, which
    ///     is used to simplify return data from the process query.
    /// </summary>
    public class ReturnProcess
    {
        /// <summary>
        ///     Gets or sets the value for <see cref="ProcessName"/>.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        ///     Gets or sets the value for <see cref="ProcessId"/>.
        /// </summary>
        public int ProcessId { get; set; }
    }
}