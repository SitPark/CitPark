using CitPark.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CitPark
{
    class StoredData
    {
        public static readonly StoredData _instance = new StoredData();

        private List<ParkingSpotPreview> parkingSpotPreviews = new List<ParkingSpotPreview>();

        /// <summary>
        /// Gets or sets the parkingspotpreviews list.
        /// </summary>
        public List<ParkingSpotPreview> ParkingSpotPreviews
        {
            get { return parkingSpotPreviews; }
            set { parkingSpotPreviews = value; }
        }

        public StoredData() { }
    }
}
