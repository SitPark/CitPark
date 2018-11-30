using CitPark.Classes;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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

            RefreshMap();
        }

        // This method is called when the page finishes loading
        protected override async void OnAppearing()
        {
            // We'll ask for location permission if it's not given
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if(status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                // Permission has been given
                if (results.ContainsKey(Permission.Location))
                {
                    status = results[Permission.Location];

                    SpotsMap.MyLocationEnabled = true;
                    SpotsMap.UiSettings.MyLocationButtonEnabled = true;
                }
                else
                {
                    SpotsMap.MyLocationEnabled = false;
                    SpotsMap.UiSettings.MyLocationButtonEnabled = false;
                }
            }
        }

        public void RefreshMap()
        {
            // TODO: actually get spots from server
            List<ParkTime> parkTimes = new List<ParkTime>();
            ParkTime parkTime = new ParkTime(WeekDay.Monday, true, new TimeSpan(), new TimeSpan());
            parkTimes.Add(parkTime);
            ParkTimes parkTimesClass = new ParkTimes(parkTimes);
            ParkingSpotPreview parkingSpot = new ParkingSpotPreview(new Position(41.118363d, -8.6235849d), "");

            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Label = "GaiaShopping",
                Address = "Av. dos Descobrimentos 549, 4404-503 Vila Nova de Gaia",
                Position = new Position(parkingSpot.Position.Latitude, parkingSpot.Position.Longitude)
            };

            //SpotsMap.Pins.Add(pin);
        }
    }
}