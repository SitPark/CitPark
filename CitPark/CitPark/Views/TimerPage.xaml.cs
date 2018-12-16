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
using Xamarin.Forms.Xaml;

namespace CitPark.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimerPage : ContentPage
	{
        private TimeSpan selectedTime = new TimeSpan(0, 0, 0);

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
            TimerHandler();
        }

        private void ResetMapButton_Clicked(object sender, EventArgs e)
        {

        }

        private void SavePositionButton_Clicked(object sender, EventArgs e)
        {

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
    }
}