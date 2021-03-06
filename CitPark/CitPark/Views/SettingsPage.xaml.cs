﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        // Convert enumerator to list
        List<DistanceUnits> distanceUnits = Enum.GetValues(typeof(DistanceUnits)).Cast<DistanceUnits>().ToList();
        List<MapType> mapTypes = Enum.GetValues(typeof(MapType)).Cast<MapType>().ToList();

		public SettingsPage ()
		{
            // Remove the last element from the map types list.
            mapTypes.RemoveAt(mapTypes.Count - 1);

			InitializeComponent ();

            // Assign the distance units list to the picker
            DistanceUnitPicker.ItemsSource = distanceUnits;
            MapTypePicker.ItemsSource = mapTypes;

            DarkModeSwitch.IsToggled = Settings.DarkMode;
            TimerNotificationSwitch.IsToggled = Settings.TimerNotification;
            TimerUpDown.Value = Settings.DefaultTimer;
            DistanceUnitPicker.SelectedItem = Settings.SelectedDistanceUnit;
            MapTypePicker.SelectedItem = Settings.MapsType;
            RadiusUpDown.Value = Settings.SearchRadius;
        }

        private void TimerNotificationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Settings.TimerNotification = e.Value;
        }

        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Settings.DarkMode = e.Value;
        }

        private void TimerUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            Settings.DefaultTimer = (int)TimerUpDown.Value;
        }

        private void TimerUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            Settings.DefaultTimer = (int)TimerUpDown.Value;
        }

        private void TimerUpDown_EntryUnfocused(object sender, EventArgs e)
        {
            Settings.DefaultTimer = (int)TimerUpDown.Value;
        }

        private void DistanceUnitPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.SelectedDistanceUnit = (DistanceUnits)DistanceUnitPicker.SelectedItem;
        }

        private void TimeWarningUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            Settings.WarnTime = new TimeSpan(0, (int)TimeWarningUpDown.Value, 0);
        }

        private void TimeWarningUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            Settings.WarnTime = new TimeSpan(0, (int)TimeWarningUpDown.Value, 0);
        }

        private void TimeWarningUpDown_EntryUnfocused(object sender, EventArgs e)
        {
            Settings.WarnTime = new TimeSpan(0, (int)TimeWarningUpDown.Value, 0);
        }

        private void RadiusUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            Settings.SearchRadius = (int)RadiusUpDown.Value;
        }

        private void RadiusUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            Settings.SearchRadius = (int)RadiusUpDown.Value;
        }

        private void RadiusUpDown_EntryUnfocused(object sender, EventArgs e)
        {
            Settings.SearchRadius = (int)RadiusUpDown.Value;
        }

        async void ParkTypesButton_Clicked(object sender, EventArgs e)
        {
            var parkTypesPage = new ParkTypesModal();
            await Navigation.PushModalAsync(parkTypesPage);
        }

        private void MapTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.MapsType = (MapType)MapTypePicker.SelectedItem;
        }
    }
}