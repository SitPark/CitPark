using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

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

        public static bool TimerNotification { get; set; }
        public static bool DarkMode { get; set; }
        public static DistanceUnits SelectedDistanceUnit { get; set; }
        public static int DefaultTimer { get; set; }
        public static TimeSpan WarnTime { get; set; }
        public static int SearchRadius { get; set; }

        // This setting will be in binary, so each 1 corresponds to a parking type
        public static int ParkTypes { get; set; }

        public static bool TimerActivated { get; set; }
        public static DateTime TimerLimit { get; set; }
    }
}
