using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitPark.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimerPage : ContentPage
	{
		public TimerPage ()
		{
			InitializeComponent ();
		}

        // This method is called when the page finishes loading
        protected override async void OnAppearing()
        {
            // We'll ask for location permission if it's not given
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                // Permission has been given
                if (results.ContainsKey(Permission.Location))
                {
                    status = results[Permission.Location];

                    PositionMap.MyLocationEnabled = true;
                    PositionMap.UiSettings.MyLocationButtonEnabled = true;
                }
                else
                {
                    PositionMap.MyLocationEnabled = false;
                    PositionMap.UiSettings.MyLocationButtonEnabled = false;
                }
            }
        }

        private void StartTimerButton_Clicked(object sender, EventArgs e)
        {

        }

        private void ResetMapButton_Clicked(object sender, EventArgs e)
        {

        }

        private void SavePositionButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}