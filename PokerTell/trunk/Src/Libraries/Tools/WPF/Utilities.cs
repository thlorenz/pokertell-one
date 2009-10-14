namespace Tools.WPF
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    public static class Utilities
    {
        public static Image ImageFromResource(string resourcePath)
        {
            return new Image { Source = new BitmapImage(new Uri(resourcePath, UriKind.Relative)) };
        }
    }
}
