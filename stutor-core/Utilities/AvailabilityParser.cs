using GeoTimeZone;
using Newtonsoft.Json;
using stutor_core.Models.ViewModels;
using System;
using System.Collections.Generic;
using TimeZoneConverter;

namespace stutor_core.Utilities
{
    public static class AvailabilityParser
    {
        public static bool IsAvailable(string availability, Coordinates coords)
        {
            var result = false;

            var parsed = JsonConvert.DeserializeObject<AvailablityDisplay>(availability);

            string tzIana = TimeZoneLookup.GetTimeZone((double)coords.Lat, (double)coords.Lng).Result;
            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(tzIana);
            var expertDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(tzi.Id));

            var currentDayForExpert = expertDateTime.DayOfWeek.ToString().Substring(0,3).ToLower();
            if (parsed.Days.Contains(currentDayForExpert))
            {
                string keyToCheck = (currentDayForExpert == "sat" || currentDayForExpert == "sun") ? "weekendHours" : "weekdayHours";
                var hours = (keyToCheck == "weekdayHours") ? parsed.WeekdayHours.Split('-') : parsed.WeekendHours.Split("-");
                var start = DateTime.Parse(hours[0]);
                var end = DateTime.Parse(hours[1]);
                if(expertDateTime.TimeOfDay == start.TimeOfDay || (expertDateTime.TimeOfDay > start.TimeOfDay && expertDateTime.TimeOfDay < end.TimeOfDay))
                {
                    result = true;
                }
            }

            return result;
        }
    }

    public class AvailablityDisplay
    {
        public List<string> Days { get; set; }
        public string WeekdayHours { get; set; }
        public string WeekendHours { get; set; }
    }
}
