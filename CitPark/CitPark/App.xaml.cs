using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CitPark
{
    public partial class App : Application
    {
        public static NavigationPage NavigationPage { get; private set; }

        public App()
        {
            // Get settings from preferences
            Settings.DefaultTimer = Preferences.Get("default_timer", 30);
            Settings.TimerNotification = Preferences.Get("timer_notification", true);
            Settings.DarkMode = Preferences.Get("dark_mode", false);
            Settings.SelectedDistanceUnit = (DistanceUnits)Preferences.Get("distance_unit", (int)DistanceUnits.Kilometers);
            Settings.WarnTime = Preferences.Get("warn_time", 10);
            Settings.SearchRadius = Preferences.Get("search_radius", 1);
            Settings.ParkTypes = Preferences.Get("park_types", 0x1111);

            var menuPage = new MenuPage();

            NavigationPage = new NavigationPage(new MapPage());

            var rootPage = new RootPage();
            rootPage.Master = menuPage;
            rootPage.Detail = NavigationPage;
            MainPage = rootPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
