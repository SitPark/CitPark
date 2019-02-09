using CitPark.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CitPark.Settings;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddParkTypesModal : ContentPage
	{
        private AddParkingSpotPage AddParkingSpotPage;

		public AddParkTypesModal (AddParkingSpotPage addParkingSpotPage)
		{
            AddParkingSpotPage = addParkingSpotPage;

			InitializeComponent ();

            HandicapSpotsNumber.Text = AddParkingSpotPage.ParkTypesSpots[ParkTypesEnum.Handicap].ToString();
            FamilySpotsNumber.Text = AddParkingSpotPage.ParkTypesSpots[ParkTypesEnum.Family].ToString();
            EletricSpotsNumber.Text = AddParkingSpotPage.ParkTypesSpots[ParkTypesEnum.Eletric].ToString();
            BikeSpotsNumber.Text = AddParkingSpotPage.ParkTypesSpots[ParkTypesEnum.Bike].ToString();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            AddParkingSpotPage.ParkingSpotPreview.ParkTypes = (int)ParkTypesEnum.None;

            // Bitwise OR every park type
            if (HandicapSpotsNumber.Text != "0")
            {
                AddParkingSpotPage.ParkingSpotPreview.ParkTypes |= (int)ParkTypesEnum.Handicap;
            }
            if (FamilySpotsNumber.Text != "0")
            {
                AddParkingSpotPage.ParkingSpotPreview.ParkTypes |= (int)ParkTypesEnum.Family;
            }
            if (EletricSpotsNumber.Text != "0")
            {
                AddParkingSpotPage.ParkingSpotPreview.ParkTypes |= (int)ParkTypesEnum.Eletric;
            }
            if (BikeSpotsNumber.Text != "0")
            {
                AddParkingSpotPage.ParkingSpotPreview.ParkTypes |= (int)ParkTypesEnum.Bike;
            }

            AddParkingSpotPage.ParkTypesSpots[ParkTypesEnum.Handicap] = Int32.Parse(HandicapSpotsNumber.Text);
            AddParkingSpotPage.ParkTypesSpots[ParkTypesEnum.Family] = Int32.Parse(FamilySpotsNumber.Text);
            AddParkingSpotPage.ParkTypesSpots[ParkTypesEnum.Eletric] = Int32.Parse(EletricSpotsNumber.Text);
            AddParkingSpotPage.ParkTypesSpots[ParkTypesEnum.Bike] = Int32.Parse(BikeSpotsNumber.Text);

            await Navigation.PopModalAsync();
        }
    }
}