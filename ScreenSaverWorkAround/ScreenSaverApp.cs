namespace ScreenSaverWorkAround
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    public static class ScreenSaverApp
    {
        public static void App()
        {
            var nimCommandLocation = Properties.Settings.Default.NirCmd;

            while (true)
            {
                if (CanRunToday())
                {
                    var now = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);

                    if (Properties.Settings.Default.ScheduleTimes.IndexOf(now.ToString()) > 0)
                    {
                        Process.Start(nimCommandLocation, "screensaver");
                        Thread.Sleep(now.Add(new TimeSpan(0, 0, 1, 0)));
                    }

                    Thread.Sleep(10000);
                }
            }
        }

        private static bool CanRunToday()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return Properties.Settings.Default.chkSunday;
                    break;
                case DayOfWeek.Monday:
                    return Properties.Settings.Default.chkMonday;
                    break;
                case DayOfWeek.Tuesday:
                    return Properties.Settings.Default.chkTuesday;
                    break;
                case DayOfWeek.Wednesday:
                    return Properties.Settings.Default.chkWednesday;
                    break;
                case DayOfWeek.Thursday:
                    return Properties.Settings.Default.chkThursday;
                    break;
                case DayOfWeek.Friday:
                    return Properties.Settings.Default.chkFriday;
                    break;
                case DayOfWeek.Saturday:
                    return Properties.Settings.Default.chkSaturday;
                    break;
            }

            return false;
        }
    }
}
