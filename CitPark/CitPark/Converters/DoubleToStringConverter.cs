using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace CitPark.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double newValue;

                double.TryParse((string)value, out newValue);

                return newValue;
            }
            catch (Exception)
            {
                Console.WriteLine("Error converting from string to double.");

                return null;
            }
        }
    }
}
