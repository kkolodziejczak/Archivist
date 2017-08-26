using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
    public class PageToActiveButtonValueConverter : BaseValueConverter<PageToActiveButtonValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            if((Pages)parameter == (Pages)value)
            {
                return ResourceHelper.GetColor("ActiveDarkPrimaryColorBrush");
            }

            return ResourceHelper.GetColor("DarkPrimaryColorBrush"); ;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
