using Acr.UserDialogs;
using CitPark.Classes;
using CitPark.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CitPark.Settings;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParksListModal : ContentPage
	{
        public ObservableCollection<ParkingSpotPreview> ParksList = new ObservableCollection<ParkingSpotPreview>();
        public Location locationToLoad;

		public ParksListModal (Location location)
		{
            locationToLoad = location;

			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            if (ParksList.Count == 0)
            {
                using (IProgressDialog progress = UserDialogs.Instance.Loading("Fetching parks...", null, null, true, MaskType.Black))
                {
                    await GetParksByLocation(locationToLoad);

                    foreach (ParkingSpotPreview parkingSpotPreview in ParksList)
                    {
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

                        parkingSpotPreview.Distance = Location.CalculateDistance(locationToLoad, new Location(parkingSpotPreview.Coordinates[0], parkingSpotPreview.Coordinates[1]), DistanceUnits.Kilometers);
                    }

                    // Order parks by distance.
                    ParksList = new ObservableCollection<ParkingSpotPreview>(ParksList.OrderBy(park => park.Distance).ToList());

                    parksListView.ItemsSource = ParksList;
                }
            }
        }

        private async void parksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Get selected parking spot.
            ParkingSpotPreview parkingSpotPreview = (ParkingSpotPreview)e.SelectedItem;

            await Navigation.PushModalAsync(new ParkDetailsPage(parkingSpotPreview));
        }

        private async Task GetParksByLocation(Location location)
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park/read_by_location.php?latitude=" + location.Latitude.ToString() + "&longitude=" + location.Longitude.ToString() + "&radius=" + Settings.SearchRadius * 1000 + "&categories=" + Settings.ParkTypes);
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
                        }
                        else
                        {
                            ParkingSpotPreview[] parkingSpotPreviews = JsonConvert.DeserializeObject<ParkingSpotPreview[]>(content);

                            foreach (ParkingSpotPreview parkingSpotPreview in parkingSpotPreviews)
                            {
                                ParksList.Add(parkingSpotPreview);
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Get selected parking spot.
            var button = sender as Button;
            ParkingSpotPreview parkingSpotPreview = button.BindingContext as ParkingSpotPreview;

            // Launch Google Maps and navigate to parking spot.
            var request = string.Format("http://maps.google.com/?daddr=" + parkingSpotPreview.Coordinates[0] + "," + parkingSpotPreview.Coordinates[1]);

            Device.OpenUri(new Uri(request));
        }
    }

    class ParkImage
    {
        [JsonProperty("park_id")]
        public int ParkId { get; set; }

        [JsonProperty("image_id")]
        public int ImageId { get; set; }

        [JsonProperty("image_file")]
        public string Image { get; set; }
    }

    class ParkSpot
    {
        [JsonProperty("spot_id")]
        public int SpotId { get; set; }

        [JsonProperty("park_id")]
        public int ParkId { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        [JsonProperty("num_spots")]
        public int NumSpots { get; set; }
    }
}