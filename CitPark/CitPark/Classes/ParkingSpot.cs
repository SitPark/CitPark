using System;
using System.Collections.Generic;
using System.Text;

namespace CitPark.Classes
{
    public class ParkingSpot
    {
        Coordinate Coordinate { get; set; }
        bool Paid { get; set; }
        bool Underground { get; set; }
        int Floor { get; set; }
        ParkTimes ParkTimes { get; set; }
        // TODO: add image variable
        // TODO: add comments variable

        public ParkingSpot() : this(new Coordinate(0, 0), false, false, 0, new ParkTimes() /*, image variable, comments variable */){}

        public ParkingSpot(Coordinate coordinate, bool paid, bool underground, int floor, ParkTimes parktimes /*, image variable, comments variable */)
        {
            this.Coordinate = coordinate;
            this.Paid = paid;
            this.Underground = underground;
            this.Floor = floor;
            this.ParkTimes = parktimes;
        }
    }
}
