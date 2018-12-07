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
            Title = "Menu";
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
    }
}