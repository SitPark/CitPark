using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using static CitPark.Settings;

namespace CitPark.Converters
{
    public class ParkSpotsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int spotsCounter = 0;

            // Cycle through all elements in the dictionary, and add them to the counter.
            foreach(KeyValuePair<ParkTypesEnum, int> entry in (Dictionary<ParkTypesEnum, int>)value)
            {
                spotsCounter += entry.Value;
            }

            return spotsCounter.ToString() + " spots";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
