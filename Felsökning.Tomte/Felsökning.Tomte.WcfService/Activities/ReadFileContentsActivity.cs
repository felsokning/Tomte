//-----------------------------------------------------------------------
// <copyright file="ReadFileContentsActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.IO;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReadFileContentsActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class ReadFileContentsActivity : SwedishCodeActivity<object>
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
            bool isValidExtension = DoesTheTargetLook2Legit2Quit(target);
            if (!isValidExtension)
            {
                // This will cause the workflow to abort.
                throw new InvalidDataException($"The file specified '{target}' does not have a valid file format.");
            }

            if (File.Exists(target))
            {
                string fileText = File.ReadAllText(target);
                return fileText;
            }
            else
            {
                // This will cause the workflow to abort.
                throw new FileNotFoundException($"The file specified '{target}' does not exist.");
            }
        }

        /// <summary>
        ///     Used to evaluate if the the file we've been given is valid for the request.
        /// </summary>
        /// <param name="targetFile">The file that we were given via the WCF call.</param>
        /// <returns>A boolean indicating if we should proceed.</returns>
        private bool DoesTheTargetLook2Legit2Quit(string targetFile)
        {
            if (targetFile.EndsWith(".log") || targetFile.EndsWith(".txt") || targetFile.EndsWith(".config"))
            {
                return true;
            }

            // Someone's been naughty...
            return false;
        }
    }
}