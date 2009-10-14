/*
 * User: Thorsten Lorenz
 * Date: 8/6/2009
 * 
 */
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Tools.WPF.Converters
{
    [ValueConversion( typeof( Color ), typeof( SolidColorBrush ) )]
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        object IValueConverter.Convert(
            object value, Type targetType, object parameter, CultureInfo culture )
        {
            Debug.Assert( value is Color, "value should be a Color" );
            Debug.Assert( typeof( Brush ).IsAssignableFrom( targetType ),
                         "targetType should be Brush or derived from Brush" );
            
            return new SolidColorBrush( (Color)value );
        }
        
        object IValueConverter.ConvertBack( object value, Type targetType,
                                           object parameter, CultureInfo culture )
        {
            Debug.Assert(value is SolidColorBrush, "value should be a SolidColorBrush");
            Debug.Assert( targetType == typeof( Color ), "targetType should be Color" );
            
            return (value as SolidColorBrush).Color;
        }
    }

}
