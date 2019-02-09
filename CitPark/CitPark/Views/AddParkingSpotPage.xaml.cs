using CitPark.Classes;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using static CitPark.Settings;

namespace CitPark.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddParkingSpotPage : ContentPage
	{
        public ParkingSpotPreview ParkingSpotPreview = new ParkingSpotPreview()
        {
            Details = new ParkingSpotDetails()
        };

        public Dictionary<ParkTypesEnum, int> ParkTypesSpots = new Dictionary<ParkTypesEnum, int>()
        {
            {ParkTypesEnum.Handicap, 0 },
            {ParkTypesEnum.Family, 0 },
            {ParkTypesEnum.Eletric, 0 },
            {ParkTypesEnum.Bike, 0 }
        };

        private bool FirstTimeLoad = true;
        private string ImagePath = "";
        private MediaFile ParkImageToUpload;

        Pin pin;

        public AddParkingSpotPage ()
		{
			InitializeComponent ();
        }

        protected override async void OnAppearing()
        {
            if (FirstTimeLoad)
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

                        PositionMap.MyLocationEnabled = true;
                        PositionMap.UiSettings.MyLocationButtonEnabled = true;
                    }
                    else
                    {
                        Settings.GeolocationPermissionGranted = false;

                        PositionMap.MyLocationEnabled = false;
                        PositionMap.UiSettings.MyLocationButtonEnabled = false;
                    }
                }
                else
                {
                    Settings.GeolocationPermissionGranted = true;

                    PositionMap.MyLocationEnabled = true;
                    PositionMap.UiSettings.MyLocationButtonEnabled = true;
                }

                var location = GetLastKnownDeviceLocation().Result;

                MoveMap(location);

                pin = new Pin()
                {
                    Type = PinType.Place,
                    Label = "Your car position"
                };

                pin.Position = new Position(location.Latitude, location.Longitude);

                PositionMap.Pins.Add(pin);

                FirstTimeLoad = false;
            }
        }

        public void MoveMap(Xamarin.Essentials.Location location)
        {
            PositionMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(Settings.SearchRadius)), true);
        }

        private async Task<Xamarin.Essentials.Location> GetLastKnownDeviceLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                return location;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

                return null;
            }
        }

        private async void ParkTypeButton_Clicked(object sender, EventArgs e)
        {
            var addParkTypesPage = new AddParkTypesModal(this);
            await Navigation.PushModalAsync(addParkTypesPage);
        }

        private async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            await InsertImage();
        }

        private async Task InsertImage()
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(ParkImageToUpload.GetStream()), "\"file\"", $"\"{ParkImageToUpload.Path}\"");

            var httpClient = new HttpClient();
            var url = "http://citpark.tech/api/park_image/insert_image.php";
            var responseMsg = await httpClient.PostAsync(url, content);

            var remotePath = await responseMsg.Content.ReadAsStringAsync();
        }

        private async Task GetParksByLocation(Xamarin.Essentials.Location location)
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
            catch (WebException ex)
            {
                Console.Out.WriteLine("Error connecting to server. " + ex.Message);
            }
        }

        private async void AddParkImage_Clicked(object sender, EventArgs e)
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }

            if (storageStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Large
                });

                if (file == null)
                {
                    return;
                }

                ParkImageToUpload = file;

                ImagePath = file.Path;

                ParkingSpotPreview.Details.Image = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                ParkImage.Source = ParkingSpotPreview.Details.Image;
            }
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to take photo.", "OK");
            }
        }

        private async void TakeParkImage_Clicked(object sender, EventArgs e)
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "CitPark",
                    Name = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString()
                });

                if(file == null)
                {
                    return;
                }

                ParkImageToUpload = file;

                ImagePath = file.Path;

                ParkingSpotPreview.Details.Image = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                ParkImage.Source = ParkingSpotPreview.Details.Image;
            }
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to take photo.", "OK");
            }
        }

        private void PositionMap_CameraChanged(object sender, CameraChangedEventArgs e)
        {
            pin.Position = new Position(PositionMap.CameraPosition.Target.Latitude, PositionMap.CameraPosition.Target.Longitude);
        }

        private async void TimesButton_Clicked(object sender, EventArgs e)
        {
            var addParkTimesPage = new AddParkTimesModal(this);
            await Navigation.PushModalAsync(addParkTimesPage);
        }
    }
}