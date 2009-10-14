/*
 * User: Thorsten Lorenz
 * Date: 8/6/2009
 * 
 */
using System;
using System.Windows;
using System.Windows.Data;

namespace Tools.WPF.Converters
{
	[ValueConversion (typeof(bool), typeof(Visibility))]
	public class VisibilityConverter : IValueConverter
	{
	    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	    {
	        if((bool) value) {
	            return Visibility.Visible;
	        }
	        else {
	            return Visibility.Collapsed;
	        }
	    }
	    
	    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	    {
	        throw new NotImplementedException();
	    }
	    
	}
	


}
