using CitPark.Classes;
using CitPark.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

            // Assign the correct values to the labels.
            ParkImage.Source = ParkingSpotPreview.Details.Image;

            ParkName.Text = ParkingSpotPreview.Name;
            ParkType.Text = new ParkTypeToStringConverter().Convert(ParkingSpotPreview.ParkTypes, null, null, CultureInfo.CurrentCulture).ToString();
            ParkUnderground.Text = new BoolToIsUndergroundString().Convert(ParkingSpotPreview.Underground, null, null, CultureInfo.CurrentCulture).ToString();
            ParkFloor.Text = "Floor: " + ParkingSpotPreview.Floor;
            ParkPaid.Text = new BoolToIsPaidStringConverter().Convert(ParkingSpotPreview.Paid, null, null, CultureInfo.CurrentCulture).ToString();

            // Check what park spot type the park has.
            if(((ParkTypesEnum)ParkingSpotPreview.ParkTypes & ParkTypesEnum.Handicap) == ParkTypesEnum.Handicap){
                ParkSpotsHandicap.IsEnabled = true;
                ParkSpotsHandicap.Text = "Handicap spots: " + ParkingSpotPreview.Details.ParkSpots[ParkTypesEnum.Handicap];
            }
            if (((ParkTypesEnum)ParkingSpotPreview.ParkTypes & ParkTypesEnum.Family) == ParkTypesEnum.Family)
            {
                ParkSpotsFamily.IsEnabled = true;
                ParkSpotsFamily.Text = "Family spots: " + ParkingSpotPreview.Details.ParkSpots[ParkTypesEnum.Family];
            }
            if (((ParkTypesEnum)ParkingSpotPreview.ParkTypes & ParkTypesEnum.Eletric) == ParkTypesEnum.Eletric)
            {
                ParkSpotsEletric.IsEnabled = true;
                ParkSpotsEletric.Text = "Eletric vehicle spots: " + ParkingSpotPreview.Details.ParkSpots[ParkTypesEnum.Eletric];
            }
            if (((ParkTypesEnum)ParkingSpotPreview.ParkTypes & ParkTypesEnum.Bike) == ParkTypesEnum.Bike)
            {
                ParkSpotsBike.IsEnabled = true;
                ParkSpotsBike.Text = "Bike spots: " + ParkingSpotPreview.Details.ParkSpots[ParkTypesEnum.Bike];
            }
        }

        private void DirectionsButton_Clicked(object sender, EventArgs e)
        {
            // Launch Google Maps and navigate to parking spot.
            var request = string.Format("http://maps.google.com/?daddr=" + ParkingSpotPreview.Coordinates[0] + "," + ParkingSpotPreview.Coordinates[1]);

            Device.OpenUri(new Uri(request));
        }

        private void ReportButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}