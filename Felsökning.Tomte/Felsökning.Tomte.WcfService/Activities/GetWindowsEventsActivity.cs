//-----------------------------------------------------------------------
// <copyright file="GetWindowsEventsActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetWindowsEventsActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class GetWindowsEventsActivity : SwedishCodeActivity<EventLogEntry[]>
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
        ///     Gets or sets the value of the <see cref="ThirdArgument"/> parameter.
        /// </summary>
        public InArgument<string> ThirdArgument { get; set; }

        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override EventLogEntry[] Execute(CodeActivityContext context)
        {
            FirstArgument = FirstInArgument;
            SecondArgument = SecondInArgument;
            ThirdArgument = ThirdInArgument;

            string logName = context.GetValue(FirstArgument);
            string source = context.GetValue(SecondArgument);
            string eventId = context.GetValue(ThirdArgument);

            int id = int.Parse(eventId);

            // Wrap for disposal when we're done.
            using (EventLog eventLog = new EventLog(logName))
            {
                EventLogEntry[] eventsList = eventLog.Entries.Cast<EventLogEntry>().Where(x => x.Source == source)
                    .Where(y => y.InstanceId == id).ToArray();
                return eventsList;
            }
        }
    }
}