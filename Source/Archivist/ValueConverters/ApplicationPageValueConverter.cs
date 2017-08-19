using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archivist
{
public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        switch ((Pages)value)
        {
            case Pages.Projects:
                return new ProjectsPage();
            case Pages.Settings:
                return new SettingsPage();
            case Pages.Report:
                return new ReportPage();
            case Pages.Info:
                return new InfoPage();
            default:
                return new ProjectsPage();
        }
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}
