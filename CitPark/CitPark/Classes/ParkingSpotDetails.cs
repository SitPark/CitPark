using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace CitPark.Classes
{
    public class ParkingSpotDetails
    {
        public bool Paid { get; set; }
        public bool Underground { get; set; }
        public int Floor { get; set; }
        public ParkTimes ParkTimes { get; set; }
        // TODO: add image variable
        // TODO: add comments variable

        public ParkingSpotDetails() : this(false, false, 0, new ParkTimes() /*, image variable, comments variable */){}

        public ParkingSpotDetails(bool paid, bool underground, int floor, ParkTimes parktimes /*, image variable, comments variable */)
        {
            this.Paid = paid;
            this.Underground = underground;
            this.Floor = floor;
            this.ParkTimes = parktimes;
        }
    }
}
