using System;
using System.Globalization;
using System.Windows.Data;

namespace iXlinkerExt.WPF.Converters
{
    class PlcProjectCountToEnabledConverter : IValueConverter
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
                            return true;
                        else
                            return false;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
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
