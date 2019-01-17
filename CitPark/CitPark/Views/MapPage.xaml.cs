using CitPark.Classes;
using CitPark.ViewModels;
using DurianCode.PlacesSearchBar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        private int AdOffset = 50;

		public MapPage ()
		{
			InitializeComponent ();

            BindingContext = new MainViewModel();

            SearchBar.ApiKey = StoredData.GoogleApiKey;
            SearchBar.Type = PlaceType.All;
            SearchBar.PlacesRetrieved += SearchBar_PlacesRetrieved;
            SearchBar.TextChanged += SearchBar_TextChanged;
            ResultsList.ItemSelected += ResultsList_ItemSelected;
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
                    Settings.GeolocationPermissionGranted = true;

                    status = results[Permission.Location];

                    SpotsMap.MyLocationEnabled = true;
                    SpotsMap.UiSettings.MyLocationButtonEnabled = true;
                }
                else
                {
                    Settings.GeolocationPermissionGranted = false;

                    SpotsMap.MyLocationEnabled = false;
                    SpotsMap.UiSettings.MyLocationButtonEnabled = false;
                }
            }
            else
            {
                Settings.GeolocationPermissionGranted = true;

                SpotsMap.MyLocationEnabled = true;
                SpotsMap.UiSettings.MyLocationButtonEnabled = true;
            }

            var location = GetLastKnownDeviceLocation().Result;

            MoveMap(location);

            RefreshMap(location);
        }

        public void RefreshMap(Location location)
        {
            // Verify if we have permission to get current location.
            if (Settings.GeolocationPermissionGranted)
            {
                // Get current location.
                try
                {
                    // Pass current location and retrieves parks nearby.
                    GetParksByLocation(location);

                    SpotsMap.Pins.Clear();

                    foreach (ParkingSpotPreview parkingSpotPreview in StoredData.ParkingSpotPreviews)
                    {
                        Pin pin = new Pin()
                        {
                            Type = PinType.Place,
                            Label = parkingSpotPreview.Name,
                            Position = new Position(parkingSpotPreview.Coordinates[0], parkingSpotPreview.Coordinates[1])
                        };

                        SpotsMap.Pins.Add(pin);
                    }
                }
                catch(PermissionException pEx)
                {

                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        public void MoveMap(Location location)
        {
            SpotsMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(Settings.SearchRadius)), true);
        }

        private async Task<Location> GetLastKnownDeviceLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                return location;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

                return null;
            }
        }

        private async Task<Location> GetCurrentDeviceLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);

                var location = await Geolocation.GetLocationAsync(request);

                return location;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

                return null;
            }
        }
        
        private void GetParksByLocation(Location location)
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park/read_by_location.php?latitude=" + location.Latitude.ToString() + "&longitude=" + location.Longitude.ToString() + "&radius=" + Settings.SearchRadius * 1000);
            request.ContentType = "application/json";
            request.Method = "GET";

            StoredData.ParkingSpotPreviews = new ObservableCollection<ParkingSpotPreview>();

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

                            foreach (ParkingSpotPreview parkingSpotPreview in parkingSpotPreviews)
                            {
                                StoredData.ParkingSpotPreviews.Add(parkingSpotPreview);
                            }

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

        private void SearchBar_PlacesRetrieved(object sender, AutoCompleteResult result)
        {
            ResultsList.ItemsSource = result.AutoCompletePlaces;

            if (result.AutoCompletePlaces != null && result.AutoCompletePlaces.Count > 0)
                ResultsList.IsVisible = true;
            else
                ResultsList.IsVisible = false;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                ResultsList.IsVisible = false;
            }
            else
            {
                ResultsList.IsVisible = true;
            }
        }

        private async void ResultsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var prediction = (AutoCompletePrediction)e.SelectedItem;
            ResultsList.SelectedItem = null;

            var place = await Places.GetPlace(prediction.Place_ID, StoredData.GoogleApiKey);

            if (place != null)
            {
                ResultsList.IsVisible = false;

                MoveMap(new Location(place.Latitude, place.Longitude));

                RefreshMap(new Location(place.Latitude, place.Longitude));

                await Navigation.PushModalAsync(new ParksListModal());
            }
        }

        private void SpotsMap_CameraChanged(object sender, CameraChangedEventArgs e)
        {
            RefreshMap(new Location(e.Position.Target.Latitude, e.Position.Target.Longitude));
        }
    }
}