    using System;
    using System.IO;
    using System.Text.Json;

namespace CA2V6CP.Services
{

    public class SeasonalOpeningHours
        {
            public DateTime OpeningTime { get; private set; }
            public DateTime ClosingTime { get; private set; }

            public SeasonalOpeningHours()
            {
                // Read the opening hours from the configuration file
                var configText = File.ReadAllText("seasonalOpeningTimes.json");
                var config = JsonSerializer.Deserialize<Dictionary<string, OpeningHours>>(configText);

                // Determine the current season
                var season = DetermineSeason();

                // Set the opening and closing times based on the current season
                OpeningTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, config[season].startHour, 0, 0);
                ClosingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, config[season].endHour, 45, 0);
            }
             public DateTime GetLastTeeTime()
               {
                  var lastTeeTime = ClosingTime.AddHours(-1); // Subtract 1 hour from closing time
                  return lastTeeTime;
                }
        private string DetermineSeason()
            {
                // Determine the current season based on the current month
                var month = DateTime.Now.Month;
                if (month >= 3 && month <= 5)
                {
                    return "spring";
                }
                else if (month >= 6 && month <= 8)
                {
                    return "summer";
                }
                else if (month >= 9 && month <= 11)
                {
                    return "autumn";
                }
                else
                {
                    return "winter";
                }
            }

            private class OpeningHours
            {
                public int startHour { get; set; }
                public int endHour { get; set; }
            }
        }
    }

