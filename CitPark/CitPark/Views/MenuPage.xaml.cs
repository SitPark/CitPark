using CitPark.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
			InitializeComponent ();
		}

        private void MapButton_Clicked(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new MapPage());
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new SettingsPage());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new TimerPage());
        }

        private void AddParkingButton_Clicked(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new AddParkingSpotPage());
        }
    }
}