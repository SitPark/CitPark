using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitPark.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParkTypesModal : ContentPage
	{
        Settings.ParkTypesEnum parkTypes = Settings.ParkTypesEnum.None;

		public ParkTypesModal ()
		{
			InitializeComponent ();

            // Get the park types from settings
            parkTypes = (Settings.ParkTypesEnum)Settings.ParkTypes;

            if ((parkTypes & Settings.ParkTypesEnum.Handicap) == Settings.ParkTypesEnum.Handicap)
            {
                HandicapSwitch.IsToggled = true;
            }
            if((parkTypes & Settings.ParkTypesEnum.Family) == Settings.ParkTypesEnum.Family)
            {
                FamilySwitch.IsToggled = true;
            }
            if ((parkTypes & Settings.ParkTypesEnum.Eletric) == Settings.ParkTypesEnum.Eletric)
            {
                EletricSwitch.IsToggled = true;
            }
            if ((parkTypes & Settings.ParkTypesEnum.Bike) == Settings.ParkTypesEnum.Bike)
            {
                BikeSwitch.IsToggled = true;
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            parkTypes = Settings.ParkTypesEnum.None;

            // Bitwise OR every park type
            if (HandicapSwitch.IsToggled)
            {
                parkTypes |= Settings.ParkTypesEnum.Handicap;
            }
            if (FamilySwitch.IsToggled)
            {
                parkTypes |= Settings.ParkTypesEnum.Family;
            }
            if (EletricSwitch.IsToggled)
            {
                parkTypes |= Settings.ParkTypesEnum.Eletric;
            }
            if (BikeSwitch.IsToggled)
            {
                parkTypes |= Settings.ParkTypesEnum.Bike;
            }

            Preferences.Set("park_types", (int)parkTypes);
            Settings.ParkTypes = (int)parkTypes;

            await Navigation.PopModalAsync();
        }

        private void HandicapSwitch_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private void FamilySwitch_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private void EletricSwitch_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private void BikeSwitch_Toggled(object sender, ToggledEventArgs e)
        {

        }
    }
}