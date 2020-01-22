using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASmirnov
{
    public static class ASmirnovCustoms
    {
        public const string SaveGamesPath = "saves";

        public static double SecondsBetween(this DateTime dt0, DateTime dt1)
        {
            return Math.Abs((dt1.ToOADate() - dt0.ToOADate()) * 86400);
        }

        public static string DateTimeNowString()
        {
            DateTime dt = DateTime.Now;
            var sb = new StringBuilder();
            sb.Append(dt.Year + "." + dt.Month + "." + dt.Day + "_" + dt.Hour + "-" + dt.Minute);
            return sb.ToString();
        }
    }
}
