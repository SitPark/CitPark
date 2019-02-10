using Acr.UserDialogs;
using CitPark.Classes;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Toast;
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

        public bool SameTimeForAll = false;

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
            PositionMap.MapType = Settings.MapsType;

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
            // Check if everything was filled out
            if (!string.IsNullOrWhiteSpace(ParkNameEntry.Text) && !string.IsNullOrWhiteSpace(FloorPicker.Text))
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Submiting parking spot", null, null, true, MaskType.Black))
                {
                    int parkId = await InsertPark();

                    if (parkId > 0)
                    {
                        if (await InsertImage(parkId))
                        {
                            if (await InsertParkSpot(parkId))
                            {
                                if (await InsertParkTime(parkId))
                                {
                                    CrossToastPopUp.Current.ShowToastSuccess("Parking spot created successfully!");
                                    await Navigation.PopAsync();
                                }
                                else
                                {
                                    CrossToastPopUp.Current.ShowToastError("Error creating parking spot.");
                                }
                            }
                            else
                            {
                                CrossToastPopUp.Current.ShowToastError("Error creating parking spot.");
                            }
                        }
                        else
                        {
                            CrossToastPopUp.Current.ShowToastError("Error creating parking spot.");
                        }
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError("Error creating parking spot.");
                    }
                }
            }
            else
            {
                CrossToastPopUp.Current.ShowToastError("You must fill all details to submit a parking spot.");
            }
        }

        private async Task<bool> InsertImage(int parkId)
        {
            // Upload the image to the server.
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(ParkImageToUpload.GetStream()), "\"file\"", $"\"{ParkImageToUpload.Path}\"");

            var httpClient = new HttpClient();
            var url = "http://citpark.tech/api/park_image/insert_image.php";
            var responseMsg = await httpClient.PostAsync(url, content);

            var remotePath = await responseMsg.Content.ReadAsStringAsync();

            // Add the uploaded image to the database.
            var request = HttpWebRequest.Create("http://citpark.tech/api/park_image/create.php?park_id=" + parkId + "&image_file=" + Path.GetFileName(ParkImageToUpload.Path));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    // Check if server has returned success status code.
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var imageId = reader.ReadToEnd();

                        if (string.IsNullOrWhiteSpace(imageId))
                        {
                            Console.Out.WriteLine("Response contained empty body...");

                            return false;
                        }
                        else
                        {
                            if (Int32.Parse(imageId) > 0)
                            {
                                return true;
                            }

                            return false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Out.WriteLine("Error connecting to server. " + ex.Message);

                return false;
            }
        }

        private async Task<int> InsertPark()
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park/create.php?name=" + ParkNameEntry.Text + "&latitude=" + PositionMap.CameraPosition.Target.Latitude + "&longitude=" + PositionMap.CameraPosition.Target.Longitude + "&paid=" + PaidSwitch.IsToggled + "&underground=" + UndergroundSwitch.IsToggled + "&floor=" + FloorPicker.Text);
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    // Check if server has returned success status code.
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();

                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Console.Out.WriteLine("Response contained empty body...");

                            return -1;
                        }
                        else
                        {
                            return Int32.Parse(content);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.Out.WriteLine("Error connecting to server. " + ex.Message);

                return -1;
            }
        }

        private async Task<bool> InsertParkSpot(int parkId)
        {
            foreach(KeyValuePair<ParkTypesEnum, int> ParkTypes in ParkTypesSpots)
            {
                if (ParkTypes.Value > 0)
                {
                    var request = HttpWebRequest.Create("http://citpark.tech/api/park_spot/create.php?park_id=" + parkId + "&category_id=" + (int)ParkTypes.Key + "&num_spots=" + ParkTypes.Value);
                    request.ContentType = "application/json";
                    request.Method = "GET";

                    try
                    {
                        using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                        {
                            // Check if server has returned success status code.
                            if (response.StatusCode != HttpStatusCode.OK)
                                Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);

                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                var content = reader.ReadToEnd();

                                if (string.IsNullOrWhiteSpace(content))
                                {
                                    Console.Out.WriteLine("Response contained empty body...");

                                    return false;
                                }
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        Console.Out.WriteLine("Error connecting to server. " + ex.Message);

                        return false;
                    }
                }
            }

            return true;
        }

        private async Task<bool> InsertParkTime(int parkId)
        {
            foreach (KeyValuePair<WeekDay, ParkTime> DaysOfWeek in ParkingSpotPreview.Details.ParkTimes.ParkingTimes)
            {
                var request = HttpWebRequest.Create("http://citpark.tech/api/park_time/create.php?park_id=" + parkId + "&weekday=" + (int)DaysOfWeek.Key + "&open_time=" + DaysOfWeek.Value.TimeOpen + "&close_time=" + DaysOfWeek.Value.TimeClose + "&always_open=" + DaysOfWeek.Value.AlwaysOpen);
                request.ContentType = "application/json";
                request.Method = "GET";

                try
                {
                    using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                    {
                        // Check if server has returned success status code.
                        if (response.StatusCode != HttpStatusCode.OK)
                            Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);

                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            var content = reader.ReadToEnd();

                            if (string.IsNullOrWhiteSpace(content))
                            {
                                Console.Out.WriteLine("Response contained empty body...");

                                return false;
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    Console.Out.WriteLine("Error connecting to server. " + ex.Message);

                    return false;
                }
            }

            return true;
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