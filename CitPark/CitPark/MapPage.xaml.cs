using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
        private bool AdBought = false;
        private int AdOffset = 50;

        readonly Pin _pinIspgaya = new Pin()
        {
            Type = PinType.Place,
            Label = "ISPGAYA",
            Address = "Avenida dos Descobrimentos, 333, 4400-103 Vila Nova de Gaia",
            Position = new Position(41.11984d, -8.6257907d)
        };

		public MapPage ()
		{
            Title = "Map";
			InitializeComponent ();

            // Called whenever the map layout changes size
            MapLayout.SizeChanged += (sender, e) =>
            {
                // Position map according to ad
                // TODO: check if ad was bought
                if (AdBought)
                {
                    AdOffset = 0;
                }

                MapLayout.Children.Add(SpotsMap, Constraint.RelativeToParent((MapLayout) =>
                {
                    return 0;
                }),
                Constraint.RelativeToParent((MapLayout) =>
                {
                    return 0;
                }),
                Constraint.Constant(MapLayout.Width), Constraint.Constant(MapLayout.Height - AdOffset));

                MapLayout.Children.Add(AdSpace, Constraint.RelativeToParent((MapLayout) =>
                {
                    return (0);

                }),
                Constraint.RelativeToParent((MapLayout) =>
                {
                    return (MapLayout.Height - AdOffset);
                }),
                Constraint.Constant(MapLayout.Width), Constraint.Constant(AdOffset));
            };

            // Show on the map current user location
            SpotsMap.MyLocationEnabled = true;

            // Show button to go to current position
            SpotsMap.UiSettings.MyLocationButtonEnabled = true;

            SpotsMap.Pins.Add(_pinIspgaya);
            SpotsMap.SelectedPin = _pinIspgaya;
            SpotsMap.MoveToRegion(MapSpan.FromCenterAndRadius(_pinIspgaya.Position, Distance.FromMeters(5000)), true);
        }
	}
}