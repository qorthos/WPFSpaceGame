using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace WPFSpaceGame.Views
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = (double)value;

            string p = (string)parameter;

            if (p == "kg")
            {
                if (d < 1000)
                {
                    return d.ToString("N0") + " kg";
                }
                else
                    return d.ToString("E2") + " kg";
            }
            else if (p == "seconds")
            {
                if (d < 60)
                {
                    return d.ToString("N2") + " seconds";
                }
                d /= 60.0;
                if (d < 60)
                {
                    return d.ToString("N2") + " minutes";
                }
                d /= 60;
                if (d < 24)
                {
                    return d.ToString("N2") + " hours";
                }
                d /= 24;
                if (d < 365)
                {
                    return d.ToString("N2") + " days";
                }
                d /= 365;
                return d.ToString("N2") + " years";
            }
            else if (p == "kms")
            {
                if (d < 1000)
                {
                    return d.ToString("N2") + " km/s";
                }
                else
                    return d.ToString("E2") + " km/s";
            }



            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
