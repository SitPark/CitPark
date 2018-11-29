using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace CitPark.Classes
{
    public class ParkingSpot
    {
        public Position Coordinate { get; set; }
        public bool Paid { get; set; }
        public bool Underground { get; set; }
        public int Floor { get; set; }
        public ParkTimes ParkTimes { get; set; }
        // TODO: add image variable
        // TODO: add comments variable

        public ParkingSpot() : this(new Position(0, 0), false, false, 0, new ParkTimes() /*, image variable, comments variable */){}

        public ParkingSpot(Position coordinate, bool paid, bool underground, int floor, ParkTimes parktimes /*, image variable, comments variable */)
        {
            this.Coordinate = coordinate;
            this.Paid = paid;
            this.Underground = underground;
            this.Floor = floor;
            this.ParkTimes = parktimes;
        }
    }
}
