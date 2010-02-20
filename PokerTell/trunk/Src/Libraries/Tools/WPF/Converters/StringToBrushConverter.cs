namespace Tools.WPF.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    [ValueConversion(typeof(string), typeof(Brush))]
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value is string, "value should be a string");
            Debug.Assert(typeof(Brush).IsAssignableFrom(targetType), "target type should be Brush");
            return new BrushConverter().ConvertFromString((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value is Brush, "value should be a Brush");
            Debug.Assert(typeof(string).IsAssignableFrom(targetType), "target type should be string");
            return new BrushConverter().ConvertToString(value);
        }
    }
}