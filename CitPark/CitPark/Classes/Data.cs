using CitPark.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CitPark
{
    class Data
    {
        public readonly Data _instance = new Data();

        public List<ParkingSpotPreview> parkingSpotPreviews { get; set; }

        public Data() { }
    }
}
