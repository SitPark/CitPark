using System;
using System.Collections.Generic;
using System.Text;

namespace CitPark.Classes
{
    public class Coordinate
    {
        double Latitude { get; set; }
        double Longitude { get; set; }

        /// <summary>
        /// Default Coordinate constructor. Creates a coordinate at 0, 0.
        /// </summary>
        public Coordinate() : this(0, 0) { }

        /// <summary>
        /// Creates a new Coordinate.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public Coordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
