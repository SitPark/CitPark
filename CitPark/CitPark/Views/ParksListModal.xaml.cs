using CitPark.Classes;
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
        public ObservableCollection<ParkingSpotDetails> parkingSpotDetails = new ObservableCollection<ParkingSpotDetails>();
        public ObservableCollection<ParkingSpotPreview> ParksList = new ObservableCollection<ParkingSpotPreview>();
        public Location locationToLoad;

		public ParksListModal (Location location)
		{
            locationToLoad = location;

			InitializeComponent ();

            /*ParkingSpotDetails parkingSpotDetails = new ParkingSpotDetails(0, new ParkTimes(), new Dictionary<ParkTypesEnum, int> { { ParkTypesEnum.Family, 3 } }, ImageSource.FromUri(new Uri("http://citpark.tech/api/park_images/1545932271.PNG")));

            ParksList.Add(new ParkingSpotPreview(0, new double[] { 0d, 0d }, "lmao", false, false, 0, (int)ParkTypesEnum.Family, 1.2f, parkingSpotDetails));
            ParksList.Add(new ParkingSpotPreview(0, new double[] { 0d, 0d }, "borghini", false, false, 0, (int)(ParkTypesEnum.Bike | ParkTypesEnum.Family), 4.3f, parkingSpotDetails));*/
		}

        protected override async void OnAppearing()
        {
            await GetParksByLocation(locationToLoad);

            foreach(ParkingSpotPreview parkingSpotPreview in ParksList)
            {
                parkingSpotPreview.Details = new ParkingSpotDetails();

                ParkSpot[] parkSpots = await GetParkingSpot(parkingSpotPreview.Id);

                ParkTypesEnum parkTypesEnum = ParkTypesEnum.None;

                foreach(ParkSpot parkSpot in parkSpots)
                {
                    parkTypesEnum |= (ParkTypesEnum)parkSpot.CategoryId;
                    parkingSpotPreview.Details.ParkSpots.Add((ParkTypesEnum)parkSpot.CategoryId, parkSpot.NumSpots);
                }

                parkingSpotPreview.ParkTypes = (int)parkTypesEnum;
                parkingSpotPreview.Details.Image = await GetParkingSpotImage(parkingSpotPreview.Id);

                parkingSpotPreview.Distance = Location.CalculateDistance(locationToLoad, new Location(parkingSpotPreview.Coordinates[0], parkingSpotPreview.Coordinates[1]), DistanceUnits.Kilometers);
            }

            parksListView.ItemsSource = ParksList;
        }

        private void parksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async Task GetParksByLocation(Location location)
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park/read_by_location.php?latitude=" + location.Latitude.ToString() + "&longitude=" + location.Longitude.ToString() + "&radius=" + Settings.SearchRadius * 1000);
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