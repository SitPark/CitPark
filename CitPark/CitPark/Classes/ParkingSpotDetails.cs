using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace CitPark.Classes
{
    public class ParkingSpotDetails : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public ParkTimes ParkTimes { get; set; }
        public ImageSource Image { get; set; }
        // TODO: add comments variable

        public ParkingSpotDetails() : this(0, new ParkTimes(), "" /*, comments variable */){}

        public ParkingSpotDetails(int id, ParkTimes parktimes, ImageSource image /*, comments variable */)
        {
            Id = Id;
            ParkTimes = parktimes;
            Image = image;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
