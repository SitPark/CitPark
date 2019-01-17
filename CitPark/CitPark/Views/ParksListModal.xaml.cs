using CitPark.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CitPark.Settings;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParksListModal : ContentPage
	{
        public ObservableCollection<ParkingSpotDetails> parkingSpotDetails = new ObservableCollection<ParkingSpotDetails>();

		public ParksListModal ()
		{
			InitializeComponent ();

            /*ParkingSpotDetails parkingSpotDetails = new ParkingSpotDetails(0, new ParkTimes(), new Dictionary<ParkTypesEnum, int> { { ParkTypesEnum.Family, 3 } }, ImageSource.FromUri(new Uri("http://citpark.tech/api/park_images/1545932271.PNG")));

            ParksList.Add(new ParkingSpotPreview(0, new double[] { 0d, 0d }, "lmao", false, false, 0, (int)ParkTypesEnum.Family, 1.2f, parkingSpotDetails));
            ParksList.Add(new ParkingSpotPreview(0, new double[] { 0d, 0d }, "borghini", false, false, 0, (int)(ParkTypesEnum.Bike | ParkTypesEnum.Family), 4.3f, parkingSpotDetails));*/

            BindingContext = this;
		}

        private void parksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}