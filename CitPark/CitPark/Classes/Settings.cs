using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace CitPark
{
    public static class Settings
    {
        [Flags]
        public enum ParkTypesEnum
        {
            None = 0,
            Handicap = 1,
            Family = 2,
            Eletric = 4,
            Bike = 8
        }

        public static bool TimerNotification
        {
            get
            {
                return Preferences.Get("timer_notification", true);
            }
            set
            {
                Preferences.Set("timer_notification", value);
            }
        }
        public static bool DarkMode
        {
            get
            {
                return Preferences.Get("dark_mode", false);
            }
            set
            {
                Preferences.Set("dark_mode", value);
            }
        }
        public static DistanceUnits SelectedDistanceUnit
        {
            get
            {
                return (DistanceUnits)Preferences.Get("distance_unit", (int)DistanceUnits.Kilometers);
            }
            set
            {
                Preferences.Set("distance_unit", (int)value);
            }
        }
        public static int DefaultTimer
        {
            get
            {
                try
                {
                    return Preferences.Get("default_timer", 30);
                }
                catch(Exception ex)
                {
                    return 30;
                }
            }
            set
            {
                Preferences.Set("default_timer", value);
            }
        }
        public static TimeSpan WarnTime
        {
            get
            {
                return new TimeSpan(0, Preferences.Get("warn_time", 10), 0);
            }
            set
            {
                Preferences.Set("warn_time", value.Minutes);
            }
        }
        public static int SearchRadius
        {
            get
            {
                return Preferences.Get("search_radius", 1);
            }
            set
            {
                Preferences.Set("search_radius", value);
            }
        }

        // This setting will be in binary, so each 1 corresponds to a parking type
        public static int ParkTypes
        {
            get
            {
                return Preferences.Get("park_types", 0b1111);
            }
            set
            {
                Preferences.Set("park_types", value);
            }
        }

        public static bool CarPositionSaved
        {
            get
            {
                return Preferences.Get("car_position_saved", false);
            }
            set
            {
                Preferences.Set("car_position_saved", value);
            }
        }

        public static double CarPositionLatitude
        {
            get
            {
                return double.Parse(Preferences.Get("car_position_latitude", "0"));
            }
            set
            {
                Preferences.Set("car_position_latitude", value);
            }
        }

        public static double CarPositionLongitude
        {
            get
            {
                return double.Parse(Preferences.Get("car_position_longitude", "0"));
            }
            set
            {
                Preferences.Set("car_position_longitude", value);
            }
        }

        public static MapType MapsType
        {
            get
            {
                return (MapType)Preferences.Get("maps_type", 0);
            }
            set
            {
                Preferences.Set("maps_type", (int)value);
            }
        }

        public static bool TimerActivated { get; set; }
        public static DateTime TimerLimit { get; set; }
        public static bool GeolocationPermissionGranted { get; set; }
    }
}
