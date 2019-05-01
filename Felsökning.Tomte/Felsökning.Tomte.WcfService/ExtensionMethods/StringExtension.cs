//-----------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.ExtensionMethods
{
    using System;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StringExtension"/> class.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        ///     Returns a value indicating whether a specified substring occurs within this string.
        /// </summary>
        /// <param name="str">The current string object.</param>
        /// <param name="substring">The string to seek.</param>
        /// <param name="stringComparison">The string comparison parameter.</param>
        /// <returns>true if the value parameter occurs within this string, or if value is the empty string (""); otherwise, false.</returns>
        public static bool Contains(this String str, String substring, StringComparison stringComparison)
        {
            if (string.IsNullOrWhiteSpace(substring))
            {
                throw new ArgumentNullException(nameof(substring), "substring cannot be null.");
            }

            else if (!Enum.IsDefined(typeof(StringComparison), stringComparison))
            {
                throw new ArgumentException("substring is not a member of StringComparison", nameof(substring));
            }

            return str.IndexOf(substring, stringComparison) >= 0;
        }
    }
}