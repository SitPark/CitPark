using CitPark.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CitPark
{
    public static class StoredData
    {
        public static string GoogleApiKey = "AIzaSyD6NTzXfehYZQ5v02zaWb6nQt9-z_5QwtQ";
        public static bool AdBought;
        public static ObservableCollection<ParkingSpotPreview> ParkingSpotPreviews { get; set; } = new ObservableCollection<ParkingSpotPreview>();
    }
}
