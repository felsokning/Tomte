//-----------------------------------------------------------------------
// <copyright file="SetSymbolServerEnvironmentPathActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Security;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SetSymbolServerEnvironmentPathActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class SetSymbolServerEnvironmentPathActivity : SwedishCodeActivity<object>
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
            string symbolFilePath = context.GetValue(FirstArgument);
            try
            {
                Environment.SetEnvironmentVariable("_NT_SYMBOL_PATH", symbolFilePath);
                return "Completed";
            }
            catch (SecurityException e)
            {
                return $"Exception was encountered. Message: {e.Message}";
            }

        }
    }
}