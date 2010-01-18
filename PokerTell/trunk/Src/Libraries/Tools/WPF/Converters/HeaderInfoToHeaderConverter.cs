namespace Tools.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(string), typeof(string))]
    public class HeaderInfoToHeaderConverter : IValueConverter
    {
        #region Implemented Interfaces

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            const int defaultMaxLength = 20;

            int maxLength = defaultMaxLength;
            if (parameter != null)
            {
                int.TryParse(parameter.ToString(), out maxLength);
            }
           
            var header = (string)value;
            if (header.Length > maxLength)
                {
                    return header.Substring(0, maxLength) + "...";
                }

                return header;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}