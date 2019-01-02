using CitPark.Classes;
using CitPark.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
			InitializeComponent ();

            BindingContext = new MainViewModel();

            RefreshMap();

            GetParkInfo();
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
            ParkingSpotPreview parkingSpot = new ParkingSpotPreview(0, new double[] { 41.118363d, -8.6235849d }, "", false, false, 0);

            Pin pin = new Pin()
            {
                Type = PinType.Place,
                Label = "GaiaShopping",
                Address = "Av. dos Descobrimentos 549, 4404-503 Vila Nova de Gaia",
                Position = new Position(parkingSpot.Coordinates[0], parkingSpot.Coordinates[1])
            };

            //SpotsMap.Pins.Add(pin);
        }
        
        private void GetParkInfo()
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park/read.php");
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Check if server has returned success status code.
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returnesd status code: {0}", response.StatusCode);

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();

                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Console.Out.WriteLine("Response contained empty body...");
                        }
                        else
                        {
                            ParkingSpotPreview[] parkingSpotPreviews = JsonConvert.DeserializeObject<ParkingSpotPreview[]>(content);

                            Console.Out.WriteLine("Response Body: \r\n {0}", content);
                        }
                    }
                }
            }
            catch(WebException ex)
            {
                Console.Out.WriteLine("Error connecting to server. " + ex.Message);
            }
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ParksListModal());
        }
    }
}