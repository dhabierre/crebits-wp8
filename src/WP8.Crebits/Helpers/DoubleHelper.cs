
namespace WP8.Crebits.Helpers
{
    using System;
    using System.Globalization;

    public static class DoubleHelper
    {
        public static string ToString(double value)
        {
            string str = value.ToString("F", new CultureInfo("en-US"));
            return str.EndsWith(".00") ? Convert.ToInt32(value).ToString() : str;
        }

        public static double? ToDouble(string value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                string[] cutter = value.ToString().Split(new char[] { '.', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (cutter.Length < 1 && cutter.Length > 2)
                {
                    return null;
                }

                double integer = Convert.ToDouble(cutter[0]);

                if (cutter.Length == 2)
                {
                    double decimals = Convert.ToDouble(cutter[1]);

                    while (decimals >= 1) decimals /= 10;

                    foreach (char c in cutter[1])
                    {
                        if (c != '0') break;
                        decimals = decimals / (double)10;
                    }

                    integer += decimals;
                }

                return Math.Round(integer, 2);
            }
            catch
            {
                try
                {
                    return Convert.ToInt32(value);
                }
                catch //(Exception x)
                {
                    //throw new Exception(
                    //	string.Format(
                    //		"Cannot convert '{0}' string to decimal (culture = {1})",
                    //		value,
                    //		CultureInfo.CurrentCulture.Name),
                    //	x);

                    return null;
                }
            }
        }
    }
}
