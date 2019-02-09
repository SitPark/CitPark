using CitPark.Classes;
using CitPark.ViewModels;
using CitPark.Views;
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
using static CitPark.Settings;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        private int AdOffset = 50;

        private bool FirstTimeLoaded = true;

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
            if (FirstTimeLoaded)
            {
                // We'll ask for location permission if it's not given
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
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

                await RefreshMap(location);

                FirstTimeLoaded = false;
            }
        }

        public async Task RefreshMap(Location location)
        {
            // Verify if we have permission to get current location.
            if (Settings.GeolocationPermissionGranted)
            {
                // Get current location.
                try
                {
                    // Pass current location and retrieves parks nearby.
                    await GetParksByLocation(location);

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

        public void OpenParkSpotDetails(object sender, EventHandler e)
        {

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
        
        private async Task GetParksByLocation(Location location)
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park/read_by_location.php?latitude=" + location.Latitude.ToString() + "&longitude=" + location.Longitude.ToString() + "&radius=" + Settings.SearchRadius * 1000 + "&categories=" + Settings.ParkTypes);
            request.ContentType = "application/json";
            request.Method = "GET";

            StoredData.ParkingSpotPreviews = new ObservableCollection<ParkingSpotPreview>();

            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
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

                await RefreshMap(new Location(place.Latitude, place.Longitude));

                await Navigation.PushModalAsync(new ParksListModal(new Location(place.Latitude, place.Longitude)));
            }
        }

        private async void SpotsMap_CameraChanged(object sender, CameraChangedEventArgs e)
        {
            await RefreshMap(new Location(e.Position.Target.Latitude, e.Position.Target.Longitude));
        }

        private async void SpotsMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            string ParkName = e.Pin.Label;

            // Find the park with the provided name.
            ParkingSpotPreview parkingSpotPreview = StoredData.ParkingSpotPreviews.First(park => park.Name == ParkName);

            // Get park details.
            parkingSpotPreview.Details = new ParkingSpotDetails();

            ParkSpot[] parkSpots = await GetParkingSpot(parkingSpotPreview.Id);

            ParkTypesEnum parkTypesEnum = ParkTypesEnum.None;

            foreach (ParkSpot parkSpot in parkSpots)
            {
                parkTypesEnum |= (ParkTypesEnum)parkSpot.CategoryId;
                parkingSpotPreview.Details.ParkSpots.Add((ParkTypesEnum)parkSpot.CategoryId, parkSpot.NumSpots);
            }

            parkingSpotPreview.ParkTypes = (int)parkTypesEnum;
            parkingSpotPreview.Details.Image = await GetParkingSpotImage(parkingSpotPreview.Id);

            await Navigation.PushModalAsync(new ParkDetailsPage(parkingSpotPreview));
        }

        private async Task<ImageSource> GetParkingSpotImage(int parkId)
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park_image/read_by_park_id.php?park_id=" + parkId);
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
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

                            return null;
                        }
                        else
                        {
                            ParkImage[] parkingImageName = JsonConvert.DeserializeObject<ParkImage[]>(content);

                            Console.Out.WriteLine("Response Body: \r\n {0}", content);

                            return ImageSource.FromUri(new Uri("http://citpark.tech/api/park_images/" + parkingImageName[0].Image));
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.Out.WriteLine("Error connecting to server. " + ex.Message);

                return null;
            }
        }

        private async Task<ParkSpot[]> GetParkingSpot(int parkId)
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park_spot/read_by_park_id.php?park_id=" + parkId);
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
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

                            return null;
                        }
                        else
                        {
                            ParkSpot[] parkingSpot = JsonConvert.DeserializeObject<ParkSpot[]>(content);

                            Console.Out.WriteLine("Response Body: \r\n {0}", content);

                            return parkingSpot;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.Out.WriteLine("Error connecting to server. " + ex.Message);

                return null;
            }
        }

        private async void NearbyFAB_Clicked(object sender, EventArgs e)
        {
            Location location = await GetLastKnownDeviceLocation();

            MoveMap(location);

            await RefreshMap(location);

            await Navigation.PushModalAsync(new ParksListModal(location));
        }
    }
}