namespace Tools.WPF.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(int), typeof(double))]
    public class CountToMaximumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value is int, "value should be an interger");
            Debug.Assert(typeof(double).IsAssignableFrom(targetType), "target type should be double");
            return (double)((int)value - 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}