//-----------------------------------------------------------------------
// <copyright file="DumpProcessThreadsActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System;
    using System.Activities;
    using System.Linq;
    using System.Text;

    using Microsoft.Diagnostics.Runtime;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DumpProcessThreadsActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class DumpProcessThreadsActivity : SwedishCodeActivity<object>
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
            string stringProcessId = context.GetValue(FirstArgument);
            int processId = int.Parse(stringProcessId);
            StringBuilder newStringBuilder = new StringBuilder(0);
            using (DataTarget target = DataTarget.AttachToProcess(processId, (uint)TimeSpan.FromSeconds(5).TotalMilliseconds))
            {
                if (target.ClrVersions.Count > 0)
                {
                    // Use the first CLR Runtime available due to SxS.
                    ClrRuntime clrRuntime = target.ClrVersions.SingleOrDefault()?.CreateRuntime();

                    // Set the symbol file path, so we can debug the dump.
                    target.SymbolLocator.SymbolPath = "SRV*https://msdl.microsoft.com/download/symbols";

                    if (clrRuntime != null)
                    {
                        foreach (ClrThread thread in clrRuntime.Threads)
                        {
                            if (!thread.IsAlive)
                            {
                                continue;
                            }

                            // If the thread's single frame is WaitForSingleObject, we probably don't care.
                            if (thread.StackTrace.Count > 1)
                            {
                                newStringBuilder.AppendLine($"{thread.OSThreadId:X}");
                                foreach (ClrStackFrame frame in thread.StackTrace)
                                {
                                    newStringBuilder.AppendLine($"    {frame.StackPointer,12:x} {frame.InstructionPointer,12:x} {frame.ModuleName} {frame}");
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("The process by ID, {processId}, appears to be a native process and no CLR was found for it.");
                    }
                }
                else
                {
                    throw new ArgumentException($"The process by ID, {processId}, appears to be a native process and no CLR was found for it.");
                }
            }

            return newStringBuilder.ToString();
        }
    }
}