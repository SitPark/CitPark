using CitPark.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        // Convert enumerator to list
        List<DistanceUnits> distanceUnits = Enum.GetValues(typeof(DistanceUnits)).Cast<DistanceUnits>().ToList();

		public SettingsPage ()
		{
            Title = "Settings";
			InitializeComponent ();

            // Assign the distance units list to the picker
            DistanceUnitPicker.ItemsSource = distanceUnits;

            DarkModeSwitch.IsToggled = Settings.DarkMode;
            TimerNotificationSwitch.IsToggled = Settings.TimerNotification;
            TimerUpDown.Value = Settings.DefaultTimer;
            DistanceUnitPicker.SelectedItem = Settings.SelectedDistanceUnit;
        }

        private void TimerNotificationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("timer_notification", e.Value);
            Settings.TimerNotification = e.Value;
        }

        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("dark_mode", e.Value);
            Settings.DarkMode = e.Value;
        }

        private void TimerUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            Preferences.Set("default_timer", TimerUpDown.Value);
            Settings.DefaultTimer = (int)TimerUpDown.Value;
        }

        private void TimerUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            Preferences.Set("default_timer", TimerUpDown.Value);
            Settings.DefaultTimer = (int)TimerUpDown.Value;
        }

        private void TimerUpDown_EntryUnfocused(object sender, EventArgs e)
        {
            Preferences.Set("default_timer", TimerUpDown.Value);
            Settings.DefaultTimer = (int)TimerUpDown.Value;
        }

        private void DistanceUnitPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Preferences.Set("distance_unit", (int)DistanceUnitPicker.SelectedItem);
            Settings.SelectedDistanceUnit = (DistanceUnits)DistanceUnitPicker.SelectedItem;
        }

        private void TimeWarningUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            Preferences.Set("warn_time", TimeWarningUpDown.Value);
            Settings.WarnTime = (int)TimeWarningUpDown.Value;
        }

        private void TimeWarningUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            Preferences.Set("warn_time", TimeWarningUpDown.Value);
            Settings.WarnTime = (int)TimeWarningUpDown.Value;
        }

        private void TimeWarningUpDown_EntryUnfocused(object sender, EventArgs e)
        {
            Preferences.Set("warn_time", TimeWarningUpDown.Value);
            Settings.WarnTime = (int)TimeWarningUpDown.Value;
        }

        private void RadiusUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            Preferences.Set("search_radius", RadiusUpDown.Value);
            Settings.SearchRadius = (int)RadiusUpDown.Value;
        }

        private void RadiusUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            Preferences.Set("search_radius", RadiusUpDown.Value);
            Settings.SearchRadius = (int)RadiusUpDown.Value;
        }

        private void RadiusUpDown_EntryUnfocused(object sender, EventArgs e)
        {
            Preferences.Set("search_radius", RadiusUpDown.Value);
            Settings.SearchRadius = (int)RadiusUpDown.Value;
        }

        async void ParkTypesButton_Clicked(object sender, EventArgs e)
        {
            var parkTypesPage = new ParkTypesModal();
            await Navigation.PushModalAsync(parkTypesPage);
        }
    }
}