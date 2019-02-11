using Acr.UserDialogs;
using CitPark.Classes;
using CitPark.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CitPark.Settings;

namespace CitPark.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParkDetailsPage : ContentPage
	{
        private ParkingSpotPreview ParkingSpotPreview;
        
		public ParkDetailsPage (ParkingSpotPreview parkingSpotPreview)
		{
            ParkingSpotPreview = parkingSpotPreview;

			InitializeComponent ();
        }

        protected override async void OnAppearing()
        {
            using (IProgressDialog progress = UserDialogs.Instance.Loading("Loading park details", null, null, true, MaskType.Black))
            {
                ParkSpot[] parkSpots = await GetParkingSpot(ParkingSpotPreview.Id);

                ParkTypesEnum parkTypesEnum = ParkTypesEnum.None;

                if(ParkingSpotPreview.Details.ParkSpots.Count > 0)
                {
                    foreach (ParkSpot parkSpot in parkSpots)
                    {
                        parkTypesEnum |= (ParkTypesEnum)parkSpot.CategoryId;
                    }
                }
                else
                {
                    foreach (ParkSpot parkSpot in parkSpots)
                    {
                        parkTypesEnum |= (ParkTypesEnum)parkSpot.CategoryId;
                        ParkingSpotPreview.Details.ParkSpots.Add((ParkTypesEnum)parkSpot.CategoryId, parkSpot.NumSpots);
                    }
                }

                

                ParkingSpotPreview.ParkTypes = (int)parkTypesEnum;
                ParkingSpotPreview.Details.Image = await GetParkingSpotImage(ParkingSpotPreview.Id);

                ParkTime[] parkTimes = await GetParkingTimes(ParkingSpotPreview.Id);

                foreach(ParkTime parkTime in parkTimes)
                {
                    ParkingSpotPreview.Details.ParkTimes.ParkingTimes[parkTime.WeekDay].AlwaysOpen = parkTime.AlwaysOpen;
                    ParkingSpotPreview.Details.ParkTimes.ParkingTimes[parkTime.WeekDay].TimeOpen = parkTime.TimeOpen;
                    ParkingSpotPreview.Details.ParkTimes.ParkingTimes[parkTime.WeekDay].TimeClose = parkTime.TimeClose;
                }

                // Assign the correct values to the labels.
                ParkImage.Source = ParkingSpotPreview.Details.Image;

                ParkName.Text = ParkingSpotPreview.Name;
                ParkType.Text = new ParkTypeToStringConverter().Convert(ParkingSpotPreview.ParkTypes, null, null, CultureInfo.CurrentCulture).ToString();
                ParkUnderground.Text = new BoolToIsUndergroundString().Convert(ParkingSpotPreview.Underground, null, null, CultureInfo.CurrentCulture).ToString();
                ParkFloor.Text = "Floor: " + ParkingSpotPreview.Floor;
                ParkPaid.Text = new BoolToIsPaidStringConverter().Convert(ParkingSpotPreview.Paid, null, null, CultureInfo.CurrentCulture).ToString();

                // Check what park spot type the park has.
                if (((ParkTypesEnum)ParkingSpotPreview.ParkTypes & ParkTypesEnum.Handicap) == ParkTypesEnum.Handicap)
                {
                    ParkSpotsHandicap.IsVisible = true;
                    ParkSpotsHandicap.Text = "Handicap spots: " + ParkingSpotPreview.Details.ParkSpots[ParkTypesEnum.Handicap];
                }
                if (((ParkTypesEnum)ParkingSpotPreview.ParkTypes & ParkTypesEnum.Family) == ParkTypesEnum.Family)
                {
                    ParkSpotsFamily.IsVisible = true;
                    ParkSpotsFamily.Text = "Family spots: " + ParkingSpotPreview.Details.ParkSpots[ParkTypesEnum.Family];
                }
                if (((ParkTypesEnum)ParkingSpotPreview.ParkTypes & ParkTypesEnum.Eletric) == ParkTypesEnum.Eletric)
                {
                    ParkSpotsEletric.IsVisible = true;
                    ParkSpotsEletric.Text = "Eletric vehicle spots: " + ParkingSpotPreview.Details.ParkSpots[ParkTypesEnum.Eletric];
                }
                if (((ParkTypesEnum)ParkingSpotPreview.ParkTypes & ParkTypesEnum.Bike) == ParkTypesEnum.Bike)
                {
                    ParkSpotsBike.IsVisible = true;
                    ParkSpotsBike.Text = "Bike spots: " + ParkingSpotPreview.Details.ParkSpots[ParkTypesEnum.Bike];
                }

                // Check if the park is currently open.
                WeekDay currentDay = (WeekDay)DateTime.Today.DayOfWeek - 1;
                if ((int)currentDay == -1)
                {
                    currentDay = WeekDay.Sunday;
                }

                ParkTime currentDayParkTime = ParkingSpotPreview.Details.ParkTimes.ParkingTimes.First(time => time.Value.WeekDay == currentDay).Value;

                TimeSpan currentTime = DateTime.Now.TimeOfDay;

                if (currentDayParkTime.AlwaysOpen)
                {
                    OpenStatusLabel.Text = "Open all day";
                }
                // Check wether the park close hour is after todays open hour.
                else if(currentDayParkTime.TimeClose > currentDayParkTime.TimeOpen)
                {
                    // Check wether the park is currently open.
                    if (currentTime > currentDayParkTime.TimeOpen && currentTime < currentDayParkTime.TimeClose)
                    {
                        OpenStatusLabel.Text = string.Format("Open until {0:hh\\:mm}", currentDayParkTime.TimeClose);
                    }
                    // Check wether the park has already closed.
                    else if (currentTime > currentDayParkTime.TimeClose)
                    {
                        // Check if tomorrow will be open all day.
                        WeekDay tomorrow = currentDay + 1;
                        if ((int)tomorrow == 7)
                        {
                            tomorrow = WeekDay.Monday;
                        }

                        ParkTime tomorrowDayParkTime = ParkingSpotPreview.Details.ParkTimes.ParkingTimes.First(time => time.Value.WeekDay == tomorrow).Value;

                        if (tomorrowDayParkTime.AlwaysOpen)
                        {
                            OpenStatusLabel.Text = "Closed. Will be open tomorrow all day";
                        }
                        else
                        {
                            // Get tomorrow's opening time.
                            OpenStatusLabel.Text = string.Format("Closed. Will open tomorrow at {0:hh\\:mm}", tomorrowDayParkTime.TimeOpen);
                        }
                    }
                    // The park hasn't opened yet today.
                    else
                    {
                        OpenStatusLabel.Text = string.Format("Closed. Will open today at {0:hh\\:mm}", currentDayParkTime.TimeOpen);
                    }
                }
                // The close hour is before the open hour.
                else
                {
                    // Check wether the park is currently after it's open hour.
                    if(currentTime > currentDayParkTime.TimeOpen)
                    {
                        OpenStatusLabel.Text = "Open for the rest of the day.";
                    }
                    // Check wether the park is currently before it's close hour.
                    else if(currentTime < currentDayParkTime.TimeClose)
                    {
                        OpenStatusLabel.Text = string.Format("Open until {0:hh\\:mm}", currentDayParkTime.TimeClose);
                    }
                    // The park has already closed but will open later today.
                    else
                    {
                        OpenStatusLabel.Text = string.Format("Closed. Will open today at {0:hh\\:mm}", currentDayParkTime.TimeOpen);
                    }
                }
            }
        }

        private void DirectionsButton_Clicked(object sender, EventArgs e)
        {
            // Launch Google Maps and navigate to parking spot.
            var request = string.Format("http://maps.google.com/?daddr=" + ParkingSpotPreview.Coordinates[0] + "," + ParkingSpotPreview.Coordinates[1]);

            Device.OpenUri(new Uri(request));
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

        private async Task<ParkTime[]> GetParkingTimes(int parkId)
        {
            var request = HttpWebRequest.Create("http://citpark.tech/api/park_time/read_by_park_id.php?park_id=" + parkId);
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
                            ParkTime[] parkingTimes = JsonConvert.DeserializeObject<ParkTime[]>(content);

                            Console.Out.WriteLine("Response Body: \r\n {0}", content);

                            return parkingTimes;
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

        private void ReportButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}