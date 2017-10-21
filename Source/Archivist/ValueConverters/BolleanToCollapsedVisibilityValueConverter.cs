using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Archivist
{
    /// <summary>
    /// Convert bool value to <see cref="Visibility"/> Collapsed value
    ///     True => Collapsed
    ///     False => Visible
    /// 
    /// Note:
    ///     Optional parametr will switch logic
    ///     True => Visible
    ///     False => Collapsed
    /// 
    /// </summary>
    public class BolleanToCollapsedVisibilityValueConverter : BaseValueConverter<BolleanToCollapsedVisibilityValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool type))
                return null;

            bool ConverterLogic = true;

            if (parameter != null)
            {
                // negate converter logic if parameter is provided
                ConverterLogic ^= ConverterLogic;
            }

            if(type == ConverterLogic)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;

            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
