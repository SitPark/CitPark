using CitPark.ViewModels;
using Plugin.LocalNotifications;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace CitPark.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimerPage : ContentPage
	{
        private TimeSpan selectedTime = new TimeSpan(0, 0, 0);
        Pin pin;

        public TimerPage ()
		{
			InitializeComponent ();

            // Call function every second to update timer
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                OnTimerTick();

                return true;
            });
		}

        // This method is called when the page finishes loading
        protected override async void OnAppearing()
        {
            PositionMap.MapType = Settings.MapsType;

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
                    PositionMap.UiSettings.MyLocationButtonEnabled = false;
                }
            }
            else
            {
                PositionMap.MyLocationEnabled = true;
                PositionMap.UiSettings.MyLocationButtonEnabled = true;
            }

            pin = new Pin()
            {
                Type = PinType.Place,
                Label = "Your car position"
            };

            // Check if the car position has been saved.
            if (Settings.CarPositionSaved)
            {
                double[] carPosition = new double[2];
                carPosition[0] = Settings.CarPositionLatitude;
                carPosition[1] = Settings.CarPositionLongitude;
                MoveMap(new Location(carPosition[0], carPosition[1]));

                pin.Position = new Position(Settings.CarPositionLatitude, Settings.CarPositionLongitude);
            }
            else
            {
                var location = GetLastKnownDeviceLocation().Result;

                MoveMap(location);

                pin.Position = new Position(location.Latitude, location.Longitude);
            }

            PositionMap.Pins.Add(pin);
        }

        public void MoveMap(Location location)
        {
            PositionMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(Settings.SearchRadius)), true);
        }

        private async Task<Location> GetLastKnownDeviceLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                return location;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

                return null;
            }
        }

        private void StartTimerButton_Clicked(object sender, EventArgs e)
        {
            TimerHandler();
        }

        private void SavePositionButton_Clicked(object sender, EventArgs e)
        {
            Settings.CarPositionLatitude = PositionMap.CameraPosition.Target.Latitude;
            Settings.CarPositionLongitude = PositionMap.CameraPosition.Target.Longitude;
            Settings.CarPositionSaved = true;
        }

        private void TimerHandler()
        {
            // Check if timer is running
            if (!Settings.TimerActivated)
            {
                CrossLocalNotifications.Current.Show("Warning", "Remember to pick up your car!", 101, DateTime.Now.Add(selectedTime));

                // Check if time input is less than time with warning
                if (selectedTime > Settings.WarnTime)
                {
                    // Create a notification for when the warn time comes
                    CrossLocalNotifications.Current.Show("Warning", "In " + Settings.WarnTime + " minutes you'll have to pick up your car!", 102, DateTime.Now.Add(selectedTime - Settings.WarnTime));
                }
                
                StartTimerButton.Text = "Stop timer";
                RemindLabel.Text = "You have the following minutes to pick up your car";
                WarnTimePicker.IsVisible = false;
                Settings.TimerLimit = DateTime.Now.Add(selectedTime);
                Preferences.Set("timer_limit", Settings.TimerLimit);
                Settings.TimerActivated = true;
                Preferences.Set("timer_activated", true);
            }
            else
            {
                // Cancel all notifications
                CrossLocalNotifications.Current.Cancel(101);
                CrossLocalNotifications.Current.Cancel(102);
                
                StartTimerButton.Text = "Start timer";
                RemindLabel.Text = "How long until you want us to remind of your car?";
                WarnTimePicker.IsVisible = true;
                Settings.TimerLimit = DateTime.MinValue;
                Preferences.Set("timer_limit", DateTime.Today);
                Settings.TimerActivated = false;
                Preferences.Set("timer_activated", false);
            }
        }

        private void WarnTimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            selectedTime = WarnTimePicker.Time;
        }

        private void OnTimerTick()
        {
            // Check if timer is running
            if (Settings.TimerActivated)
            {
                // Check if timer has already reached limit
                if (DateTime.Now >= Settings.TimerLimit)
                {
                    // Stop timer
                    StartTimerButton.Text = "Start timer";
                    RemindLabel.Text = "How long until you want us to remind of your car?";
                    WarnTimePicker.IsVisible = true;
                    Settings.TimerLimit = DateTime.MinValue;
                    Preferences.Set("timer_limit", DateTime.Today);
                    Settings.TimerActivated = false;
                    Preferences.Set("timer_activated", false);
                }
                else
                {
                    // Calculate remaining time
                    long timeLeft = (Settings.TimerLimit - DateTime.Now.TimeOfDay).Minute;

                    // Check if there's less than a minute remaining
                    if (timeLeft <= 0)
                    {
                        RemindLabel.Text = "You'll be reminded in less than a minute to pick up your car";
                    }
                    else
                    {
                        RemindLabel.Text = "You'll be reminded in " + timeLeft + " minutes to pick up your car";
                    }
                }
            }
        }

        private void ClearPositionButton_Clicked(object sender, EventArgs e)
        {
            Settings.CarPositionSaved = false;
        }

        private void PositionMap_CameraChanged(object sender, CameraChangedEventArgs e)
        {
            // If the car position has been saved, don't move the pin
            // otherwise, center it on the camera target.
            if (!Settings.CarPositionSaved)
            {
                pin.Position = new Position(PositionMap.CameraPosition.Target.Latitude, PositionMap.CameraPosition.Target.Longitude);
            }
        }
    }
}