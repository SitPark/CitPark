using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.GoogleMaps;
using static CitPark.Settings;

namespace CitPark.Classes
{
    public class ParkingSpotPreview : INotifyPropertyChanged
    {
        [JsonProperty("park_id")]
        public int Id { get; set; }
        [JsonProperty("coordinates")]
        public double[] Coordinates { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("paid")]
        public bool Paid { get; set; }
        [JsonProperty("underground")]
        public bool Underground { get; set; }
        [JsonProperty("floor")]
        public int Floor { get; set; }
        [JsonProperty("park_types")]
        public int ParkTypes { get; set; }
        public double Distance { get; set; }
        public ParkingSpotDetails Details { get; set; }

        /// <summary>
        /// Default constructor for ParkingSpotPreview.
        /// Initializes a ParkingSpotPreview at 0,0, with an empty name and
        /// null ParkingSpotDetails.
        /// </summary>
        public ParkingSpotPreview() : this(0, new double[] { 0d, 0d }, "", false, false, 0, (int)ParkTypesEnum.None, 0.0f, null) { }

        /// <summary>
        /// Preview constructor for ParkingSpotPreview.
        /// Initializes a ParkingSpotPreview with the given Position and
        /// name, and null ParkingSpotDetails.
        /// </summary>
        /// <param name="id">The ParkingSpotPreview id.</param>
        /// <param name="coordinates">The ParkingSpotPreview position.</param>
        /// <param name="position">The ParkingSpotPreview position.</param>
        /// <param name="name">The ParkingSpotPreview name.</param>
        /// <param name="paid">If the ParkingSpotPreview is paid.</param>
        /// <param name="underground">If the ParkingSpotPreview is located underground.</param>
        /// <param name="floor">The ParkingSpotPreview's floor.</param>
        /// <param name="distance">The ParkingSpotPreview's distance to the wanted point.</param>
        public ParkingSpotPreview(int id, double[] coordinates, string name, bool paid, bool underground, int floor, float distance) : this(id, coordinates, name, paid, underground, floor, (int)ParkTypesEnum.None, distance, null) { }

        /// <summary>
        /// Full constructor for ParkingSpotPreview.
        /// Initialize a ParkingSpotPreview with the given Position and
        /// name, and ParkingSpotDetails.
        /// </summary>
        /// <param name="id">The ParkingSpotPreview id.</param>
        /// <param name="coordinates">The ParkingSpotPreview position.</param>
        /// <param name="name">The ParkingSpotPreview name.</param>
        /// <param name="paid">If the ParkingSpotPreview is paid.</param>
        /// <param name="underground">If the ParkingSpotPreview is located underground.</param>
        /// <param name="floor">The ParkingSpotPreview's floor.</param>
        /// <param name="parkTypes">The ParkingSpotPreview's parking types.</param>
        /// <param name="distance">The ParkingSpotPreview's distance to the wanted point.</param>
        /// <param name="details">The ParkingSpotPreview details.</param>
        public ParkingSpotPreview(int id, double[] coordinates, string name, bool paid, bool underground, int floor, int parkTypes, float distance, ParkingSpotDetails details)
        {
            Id = id;
            Coordinates = coordinates;
            Name = name;
            Paid = paid;
            Underground = underground;
            Floor = floor;
            ParkTypes = parkTypes;
            Distance = distance;
            Details = details;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool GetDetailsFromDB()
        {
            return false;
        }
    }
}
