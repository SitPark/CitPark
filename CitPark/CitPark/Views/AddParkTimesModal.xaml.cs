using CitPark.Classes;
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
	public partial class AddParkTimesModal : ContentPage
	{
        private AddParkingSpotPage AddParkingSpotPage;

		public AddParkTimesModal (AddParkingSpotPage addParkingSpotPage)
		{
            AddParkingSpotPage = addParkingSpotPage;

			InitializeComponent ();

            // Fill the correct values.
            if (AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Monday].AlwaysOpen)
            {
                MondayAllDay.IsToggled = true;
            }
            else
            {
                MondayOpenTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Monday].TimeOpen;
                MondayCloseTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Monday].TimeClose;
            }

            if (AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Tuesday].AlwaysOpen)
            {
                TuesdayAllDay.IsToggled = true;
                TuesdayOpenTimePicker.IsEnabled = false;
                TuesdayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                TuesdayOpenTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Tuesday].TimeOpen;
                TuesdayCloseTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Tuesday].TimeClose;
            }

            if (AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Wednesday].AlwaysOpen)
            {
                WednesdayAllDay.IsToggled = true;
                WednesdayOpenTimePicker.IsEnabled = false;
                WednesdayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                WednesdayOpenTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Wednesday].TimeOpen;
                WednesdayCloseTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Wednesday].TimeClose;
            }

            if (AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Thursday].AlwaysOpen)
            {
                ThursdayAllDay.IsToggled = true;
                ThursdayOpenTimePicker.IsEnabled = false;
                ThursdayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                ThursdayOpenTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Thursday].TimeOpen;
                ThursdayCloseTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Thursday].TimeClose;
            }

            if (AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Friday].AlwaysOpen)
            {
                FridayAllDay.IsToggled = true;
                FridayOpenTimePicker.IsEnabled = false;
                FridayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                FridayOpenTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Friday].TimeOpen;
                FridayCloseTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Friday].TimeClose;
            }

            if (AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Saturday].AlwaysOpen)
            {
                SaturdayAllDay.IsToggled = true;
                SaturdayOpenTimePicker.IsEnabled = false;
                SaturdayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                SaturdayOpenTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Saturday].TimeOpen;
                SaturdayCloseTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Saturday].TimeClose;
            }

            if (AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Sunday].AlwaysOpen)
            {
                SundayAllDay.IsToggled = true;
                SundayOpenTimePicker.IsEnabled = false;
                SundayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                SundayOpenTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Sunday].TimeOpen;
                SundayCloseTimePicker.Time = AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Sunday].TimeClose;
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Monday].AlwaysOpen = MondayAllDay.IsToggled;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Tuesday].AlwaysOpen = TuesdayAllDay.IsToggled;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Wednesday].AlwaysOpen = WednesdayAllDay.IsToggled;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Thursday].AlwaysOpen = ThursdayAllDay.IsToggled;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Friday].AlwaysOpen = FridayAllDay.IsToggled;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Saturday].AlwaysOpen = SaturdayAllDay.IsToggled;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Sunday].AlwaysOpen = SundayAllDay.IsToggled;

            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Monday].TimeOpen = MondayOpenTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Monday].TimeClose = MondayCloseTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Tuesday].TimeOpen = TuesdayOpenTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Tuesday].TimeClose = TuesdayCloseTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Wednesday].TimeOpen = WednesdayOpenTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Wednesday].TimeClose = WednesdayCloseTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Thursday].TimeOpen = ThursdayOpenTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Thursday].TimeClose = ThursdayCloseTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Friday].TimeOpen = FridayOpenTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Friday].TimeClose = FridayCloseTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Saturday].TimeOpen = SaturdayOpenTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Saturday].TimeClose = SaturdayCloseTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Sunday].TimeOpen = SundayOpenTimePicker.Time;
            AddParkingSpotPage.ParkingSpotPreview.Details.ParkTimes.ParkingTimes[WeekDay.Sunday].TimeClose = SundayCloseTimePicker.Time;

            await Navigation.PopModalAsync();
        }

        private void MondayAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (MondayAllDay.IsToggled)
            {
                MondayOpenTimePicker.IsEnabled = false;
                MondayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                MondayOpenTimePicker.IsEnabled = true;
                MondayCloseTimePicker.IsEnabled = true;
            }
        }

        private void TuesdayAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (TuesdayAllDay.IsToggled)
            {
                TuesdayOpenTimePicker.IsEnabled = false;
                TuesdayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                TuesdayOpenTimePicker.IsEnabled = true;
                TuesdayCloseTimePicker.IsEnabled = true;
            }
        }

        private void WednesdayAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (WednesdayAllDay.IsToggled)
            {
                WednesdayOpenTimePicker.IsEnabled = false;
                WednesdayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                WednesdayOpenTimePicker.IsEnabled = true;
                WednesdayCloseTimePicker.IsEnabled = true;
            }
        }

        private void ThursdayAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (ThursdayAllDay.IsToggled)
            {
                ThursdayOpenTimePicker.IsEnabled = false;
                ThursdayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                ThursdayOpenTimePicker.IsEnabled = true;
                ThursdayCloseTimePicker.IsEnabled = true;
            }
        }

        private void FridayAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (FridayAllDay.IsToggled)
            {
                FridayOpenTimePicker.IsEnabled = false;
                FridayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                FridayOpenTimePicker.IsEnabled = true;
                FridayCloseTimePicker.IsEnabled = true;
            }
        }

        private void SaturdayAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (SaturdayAllDay.IsToggled)
            {
                SaturdayOpenTimePicker.IsEnabled = false;
                SaturdayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                SaturdayOpenTimePicker.IsEnabled = true;
                SaturdayCloseTimePicker.IsEnabled = true;
            }
        }

        private void SundayAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (SundayAllDay.IsToggled)
            {
                SundayOpenTimePicker.IsEnabled = false;
                SundayCloseTimePicker.IsEnabled = false;
            }
            else
            {
                SundayOpenTimePicker.IsEnabled = true;
                SundayCloseTimePicker.IsEnabled = true;
            }
        }
    }
}