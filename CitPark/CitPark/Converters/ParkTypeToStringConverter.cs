using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using static CitPark.Settings;

namespace CitPark.Converters
{
    public class ParkTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";

            if (((ParkTypesEnum)value & ParkTypesEnum.Handicap) == ParkTypesEnum.Handicap)
            {
                result = "Handicap";
            }
            if(((ParkTypesEnum)value & ParkTypesEnum.Family) == ParkTypesEnum.Family)
            {
                if (result != "")
                    result = result + " | Family";
                else
                    result = "Family";
            }
            if (((ParkTypesEnum)value & ParkTypesEnum.Eletric) == ParkTypesEnum.Eletric)
            {
                if (result != "")
                    result = result + " | Eletric";
                else
                    result = "Eletric";
            }
            if (((ParkTypesEnum)value & ParkTypesEnum.Bike) == ParkTypesEnum.Bike)
            {
                if (result != "")
                    result = result + " | Bike";
                else
                    result = "Bike";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
