using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    /// <summary>
    /// Converts <see cref="KeyboardShortcut"/> to string soo user will know that he is required to press some shortcut
    /// </summary>
    public class ToStringValueConverter : BaseValueConverter<ToStringValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is KeyboardShortcut shortcut)
            {
                if (shortcut.Key == System.Windows.Input.Key.None)
                {
                    return "Press some keys...";
                }

                return value.ToString();
            }
            else
            {
                return String.Empty; 
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
