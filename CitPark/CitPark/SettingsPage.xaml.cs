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
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
            Title = "Settings";
			InitializeComponent ();
		}

        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {

        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {

        }

        private void TimerNotificationSwitch_Toggled(object sender, ToggledEventArgs e)
        {

        }
    }
}