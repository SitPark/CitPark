using CitPark.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        private bool AdBought = false;
        private int AdOffset = 50;

        readonly Pin _pinIspgaya = new Pin()
        {
            Type = PinType.Place,
            Label = "ISPGAYA",
            Address = "Avenida dos Descobrimentos, 333, 4400-103 Vila Nova de Gaia",
            Position = new Position(41.11984d, -8.6257907d)
        };

		public MapPage ()
		{
            Title = "Map";
			InitializeComponent ();

            // Show on the map current user location
            SpotsMap.MyLocationEnabled = true;

            // Show button to go to current position
            SpotsMap.UiSettings.MyLocationButtonEnabled = true;

            RefreshMap();
        }

        public void RefreshMap()
        {
            // TODO: actually get spots from server
            List<ParkTime> parkTimes = new List<ParkTime>();
            ParkTime parkTime = new ParkTime(WeekDay.Monday, true, new TimeSpan(), new TimeSpan());
            parkTimes.Add(parkTime);
            ParkTimes parkTimesClass = new ParkTimes(parkTimes);
            ParkingSpot parkingSpot = new ParkingSpot(new Position(41.118363d, -8.6235849d), false, false, 0, parkTimesClass);

            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Label = "GaiaShopping",
                Address = "Av. dos Descobrimentos 549, 4404-503 Vila Nova de Gaia",
                Position = new Position(parkingSpot.Coordinate.Latitude, parkingSpot.Coordinate.Longitude)
            };

            //SpotsMap.Pins.Add(pin);
        }
    }
}