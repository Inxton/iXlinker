using System;
using System.Globalization;
using System.Windows.Data;

namespace iXlinkerExt.WPF.Converters
{
    class PlcProjectCountToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    if (Int32.TryParse(value.ToString(), out int plcProjCount))
                    {
                        if (plcProjCount > 0)
                            return 1.0;
                        else
                            return 0.5;
                    }
                    else return 0.5;
                }
                else
                {
                    return 0.5;
                }
            }
            catch (Exception)
            {
                return 0.5;
                throw;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
