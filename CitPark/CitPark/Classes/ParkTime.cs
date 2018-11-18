using System;
using System.Collections.Generic;
using System.Text;

namespace CitPark.Classes
{
    public enum WeekDay
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class ParkTime
    {
        public WeekDay WeekDay { get; set; }
        public bool AlwaysOpen { get; set; }
        public TimeSpan TimeOpen { get; set; }
        public TimeSpan TimeClose { get; set; }

        /// <summary>
        /// Default ParkTime constructor. Creates a ParkTime at Monday from 7h to 19h.
        /// </summary>
        public ParkTime() : this(WeekDay.Monday, true, new TimeSpan(7,0,0), new TimeSpan(19,0,0)) { }

        /// <summary>
        /// Creates a new ParkTime object.
        /// </summary>
        /// <param name="weekday">The day of the week assigned to this time.</param>
        /// <param name="alwaysopen">Wether or not the park is open 24 hours in that day.</param>
        /// <param name="timeopen">The time the park opens.</param>
        /// <param name="timeclose">The time the park closes.</param>
        /// <example>For Thursday opening at 07h and closing at 21h30
        /// <code>
        /// ParkTime(WeekDay.Thursday, false, new TimeSpan(7, 0, 0), new TimeSpan(21, 30, 0);
        /// </code>
        /// </example>
        public ParkTime(WeekDay weekday, bool alwaysopen, TimeSpan timeopen, TimeSpan timeclose)
        {
            this.WeekDay = weekday;
            this.AlwaysOpen = alwaysopen;
            this.TimeOpen = timeopen;
            this.TimeClose = timeclose;
        }
    }
}
