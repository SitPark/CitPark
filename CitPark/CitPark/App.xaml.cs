using System;
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
