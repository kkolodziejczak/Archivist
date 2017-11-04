using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    /// <summary>
    /// Converts selected page to color, soo color will stay even if mouse is not there
    /// </summary>
    public class PageToActiveButtonValueConverter : BaseValueConverter<PageToActiveButtonValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            if((Pages)parameter == (Pages)value)
            {
                return ResourceHelper.GetStaticFieldValue("ActiveDarkPrimaryColorBrush");
            }

            return ResourceHelper.GetStaticFieldValue("DarkPrimaryColorBrush"); ;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
