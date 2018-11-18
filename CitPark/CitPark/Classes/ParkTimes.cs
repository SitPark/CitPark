using System;
using System.Collections.Generic;
using System.Text;

namespace CitPark.Classes
{
    public class ParkTimes
    {
        List<ParkTime> ParkingTimes { get; set; }

        /// <summary>
        /// Default constructor for the list of ParkTime, populates with the default ParkTime constructor.
        /// </summary>
        public ParkTimes() : this(new List<ParkTime> { new ParkTime() }) { }

        /// <summary>
        /// Creates a list of ParkTime.
        /// </summary>
        /// <param name="parkingtimes">List of ParkTime, maximum of 1 for each day of the week.</param>
        public ParkTimes(List<ParkTime> parkingtimes)
        {
            // Check if we have more than 7 ParkTime
            if(parkingtimes.Count <= 7)
            {
                // Check if any day is duplicated
                bool[] WeekDays = { false, false, false, false, false, false, false };
                foreach(ParkTime ParkingTime in parkingtimes){
                    // Get the WeekDay of the ParkTime
                    WeekDay ParkWeekDay = ParkingTime.WeekDay;

                    // Check if we already passed through it
                    if(WeekDays[(int)ParkWeekDay] == true)
                    {
                        throw new ArgumentException("There is a duplicate day in the list. This is not allowed.");
                    }
                    else
                    {
                        WeekDays[(int)ParkWeekDay] = true;
                    }
                }

                this.ParkingTimes = parkingtimes;
            }
            else
            {
                throw new ArgumentException("The ParkTime list cannot have more than 7 items.");
            }
        }
    }
}
