using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using TimeZoneConverter;
using TimeZoneNames;

namespace stutor_core.Utilities
{
    public static class AvailabilityParser
    {
        public static bool IsAvailable(string availability, string expertTzName)
        {
            var result = false;
            var partitions = availability.Split(';');

            var dictionary = new Dictionary<string,string>();
            foreach (var x in partitions)
            {
                var y = x.Split("=");
                dictionary[y[0]] = y[1];
            }

            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(expertTzName);
            var expertDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(tzi.Id));

            var currentDayForExpert = expertDateTime.DayOfWeek.ToString().Substring(0,3).ToLower();
            if (dictionary["days"].Contains(currentDayForExpert))
            {
                string keyToCheck = (currentDayForExpert == "sat" || currentDayForExpert == "sun") ? "weekendHours" : "weekdayHours";
                var hours = dictionary[keyToCheck].Split('-');
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
}
