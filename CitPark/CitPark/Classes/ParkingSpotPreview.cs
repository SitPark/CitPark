using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace CitPark.Classes
{
    class ParkingSpotPreview
    {
        public Position Position { get; set; }
        public string Name { get; set; }
        public ParkingSpotDetails Details { get; set; }

        /// <summary>
        /// Default constructor for ParkingSpotPreview.
        /// Initializes a ParkingSpotPreview at 0,0, with an empty name and
        /// null ParkingSpotDetails.
        /// </summary>
        public ParkingSpotPreview() : this(new Position(), "", null) { }

        /// <summary>
        /// Preview constructor for ParkingSpotPreview.
        /// Initializes a ParkingSpotPreview with the given Position and
        /// name, and null ParkingSpotDetails.
        /// </summary>
        /// <param name="position">The ParkingSpotPreview position.</param>
        /// <param name="name">The ParkingSpotPreview name.</param>
        public ParkingSpotPreview(Position position, string name) : this(position, name, null) { }

        /// <summary>
        /// Full constructor for ParkingSpotPreview.
        /// Initialize a ParkingSpotPreview with the given Position and
        /// name, and ParkingSpotDetails.
        /// </summary>
        /// <param name="position">The ParkingSpotPreview position.</param>
        /// <param name="name">The ParkingSpotPreview name.</param>
        /// <param name="details">The ParkingSpotPreview details.</param>
        public ParkingSpotPreview(Position position, string name, ParkingSpotDetails details)
        {
            this.Position = position;
            this.Name = name;
            this.Details = details;
        }
    }
}
