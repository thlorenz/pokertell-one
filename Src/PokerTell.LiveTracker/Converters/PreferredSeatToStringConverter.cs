namespace PokerTell.LiveTracker.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class PreferredSeatToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value == 0 ? "None" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}