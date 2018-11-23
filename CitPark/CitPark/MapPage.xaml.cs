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

            // Show on the map current user location
            SpotsMap.MyLocationEnabled = true;

            // Show button to go to current position
            SpotsMap.UiSettings.MyLocationButtonEnabled = true;

            _pinIspgaya.IsDraggable = true;
            SpotsMap.Pins.Add(_pinIspgaya);
            SpotsMap.SelectedPin = _pinIspgaya;
            SpotsMap.MoveToRegion(MapSpan.FromCenterAndRadius(_pinIspgaya.Position, Distance.FromMeters(5000)), true);		}
	}
}