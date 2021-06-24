using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Planzy.Models.Util
{
    class MyConverter : IValueConverter
    {
        public const string RED_COLOR = "#FFE05050";
        public const string WHITE_COLOR = "#FFFFFF";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString().ToLower())
            {
                case "false":
                    return WHITE_COLOR;
                default:
                    return RED_COLOR;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == RED_COLOR)
                return true;
            else
                return false;
        }
    }
}
