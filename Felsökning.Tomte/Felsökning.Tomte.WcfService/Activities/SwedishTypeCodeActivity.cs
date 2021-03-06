﻿//-----------------------------------------------------------------------
// <copyright file="SwedishTypeCodeActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SwedishCodeActivity{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    public abstract class SwedishCodeActivity<T> : CodeActivity<T>
    {
        /// <summary>
        ///     Gets or sets the <see cref="FirstInArgument"/> value.
        /// </summary>
        public InArgument<string> FirstInArgument { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="SecondInArgument"/> value.
        /// </summary>
        public InArgument<string> SecondInArgument { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ThirdInArgument"/> value.
        /// </summary>
        public InArgument<string> ThirdInArgument { get; set; }

        /// <summary>
        ///     The abstract method <see cref="Execute"/> is exposed via <see cref="Activity{TResult}"/>
        /// </summary>
        /// <param name="context">The execution context when the activity is ran.</param>
        /// <returns>The type requested.</returns>
        /// <inheritdoc cref="CodeActivity{TResult}"/>
        protected abstract override T Execute(CodeActivityContext context);
    }
}