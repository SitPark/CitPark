using CitPark.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitPark
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParksListModal : ContentPage
	{
        public ObservableCollection<ParkingSpotPreview> ParksList { get; set; }

		public ParksListModal ()
		{
			InitializeComponent ();

            ParksList = new ObservableCollection<ParkingSpotPreview>();

            ParkingSpotDetails parkingSpotDetails = new ParkingSpotDetails(0, new ParkTimes(), ImageSource.FromUri(new Uri("http://citpark.tech/api/park_images/1545932271.PNG")));

            ParksList.Add(new ParkingSpotPreview(0, new double[] { 0d, 0d }, "lmao", false, false, 0, parkingSpotDetails));

            BindingContext = this;
		}

        private void parksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}