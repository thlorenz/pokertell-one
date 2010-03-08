// ColorPicker.cs 
// An HSB (hue, saturation, brightness) based
// color picker.
// 
namespace Tools.WPF.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    #region ColorPicker

    public class ColorPicker : Control
    {
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public ColorPicker()
        {
            templateApplied = false;
            m_color = Colors.White;
            shouldFindPoint = true;
            SetValue(AProperty, m_color.A);
            SetValue(RProperty, m_color.R);
            SetValue(GProperty, m_color.G);
            SetValue(BProperty, m_color.B);
            SetValue(SelectedColorProperty, m_color);
        }

        #region Public Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_ColorDetail = GetTemplateChild(ColorDetailName) as FrameworkElement;
            m_ColorMarker = GetTemplateChild(ColorMarkerName) as Path;
            m_ColorSlider = GetTemplateChild(ColorSliderName) as SpectrumSlider;
            m_ColorSlider.ValueChanged += BaseColorChanged;

            m_ColorMarker.RenderTransform = markerTransform;
            m_ColorMarker.RenderTransformOrigin = new Point(0.5, 0.5);
            m_ColorDetail.MouseLeftButtonDown += OnMouseLeftButtonDown;
            m_ColorDetail.PreviewMouseMove += OnMouseMove;
            m_ColorDetail.SizeChanged += ColorDetailSizeChanged;

            templateApplied = true;
            shouldFindPoint = true;
            isAlphaChange = false;

            SelectedColor = m_color;
        }

        #endregion

        #region Public Properties

        // Gets or sets the selected color.
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set
            {
                SetValue(SelectedColorProperty, m_color);
                setColor(value);
            }
        }

        #region RGB Properties

        // Gets or sets the ARGB alpha value of the selected color.
        public byte A
        {
            get { return (byte)GetValue(AProperty); }
            set { SetValue(AProperty, value); }
        }

        // Gets or sets the ARGB red value of the selected color.
        public byte R
        {
            get { return (byte)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }

        // Gets or sets the ARGB green value of the selected color.
        public byte G
        {
            get { return (byte)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }

        // Gets or sets the ARGB blue value of the selected color.
        public byte B
        {
            get { return (byte)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }

        #endregion RGB Properties

        #region ScRGB Properties

        // Gets or sets the ScRGB alpha value of the selected color.
        public double ScA
        {
            get { return (double)GetValue(ScAProperty); }
            set { SetValue(ScAProperty, value); }
        }

        // Gets or sets the ScRGB red value of the selected color.
        public double ScR
        {
            get { return (double)GetValue(ScRProperty); }
            set { SetValue(RProperty, value); }
        }

        // Gets or sets the ScRGB green value of the selected color.
        public double ScG
        {
            get { return (double)GetValue(ScGProperty); }
            set { SetValue(GProperty, value); }
        }

        // Gets or sets the ScRGB blue value of the selected color.
        public double ScB
        {
            get { return (double)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }

        #endregion ScRGB Properties

        // Gets or sets the the selected color in hexadecimal notation.
        public string HexadecimalString
        {
            get { return (string)GetValue(HexadecimalStringProperty); }
            set { SetValue(HexadecimalStringProperty, value); }
        }

        #endregion

        #region Public Events

        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add { AddHandler(SelectedColorChangedEvent, value); }

            remove { RemoveHandler(SelectedColorChangedEvent, value); }
        }

        #endregion

        #region Dependency Property Fields

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register
                ("SelectedColor", 
                 typeof(Color), 
                 typeof(ColorPicker), 
                 new PropertyMetadata(Colors.Transparent, 
                                      selectedColor_changed
                     ));

        public static readonly DependencyProperty ScAProperty =
            DependencyProperty.Register
                ("ScA", 
                 typeof(float), 
                 typeof(ColorPicker), 
                 new PropertyMetadata((float)1, 
                                      ScAChanged
                     ));

        public static readonly DependencyProperty ScRProperty =
            DependencyProperty.Register
                ("ScR", 
                 typeof(float), 
                 typeof(ColorPicker), 
                 new PropertyMetadata((float)1, 
                                      ScRChanged
                     ));

        public static readonly DependencyProperty ScGProperty =
            DependencyProperty.Register
                ("ScG", 
                 typeof(float), 
                 typeof(ColorPicker), 
                 new PropertyMetadata((float)1, 
                                      ScGChanged
                     ));

        public static readonly DependencyProperty ScBProperty =
            DependencyProperty.Register
                ("ScB", 
                 typeof(float), 
                 typeof(ColorPicker), 
                 new PropertyMetadata((float)1, 
                                      ScBChanged
                     ));

        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register
                ("A", 
                 typeof(byte), 
                 typeof(ColorPicker), 
                 new PropertyMetadata((byte)255, 
                                      AChanged
                     ));

        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register
                ("R", 
                 typeof(byte), 
                 typeof(ColorPicker), 
                 new PropertyMetadata((byte)255, 
                                      RChanged
                     ));

        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register
                ("G", 
                 typeof(byte), 
                 typeof(ColorPicker), 
                 new PropertyMetadata((byte)255, 
                                      GChanged
                     ));

        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register
                ("B", 
                 typeof(byte), 
                 typeof(ColorPicker), 
                 new PropertyMetadata((byte)255, 
                                      BChanged
                     ));

        public static readonly DependencyProperty HexadecimalStringProperty =
            DependencyProperty.Register
                ("HexadecimalString", 
                 typeof(string), 
                 typeof(ColorPicker), 
                 new PropertyMetadata("#FFFFFFFF", 
                                      HexadecimalStringChanged
                     ));

        #endregion

        #region RoutedEvent Fields

        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedColorChanged", 
            RoutingStrategy.Bubble, 
            typeof(RoutedPropertyChangedEventHandler<Color>), 
            typeof(ColorPicker)
            );

        #endregion

        #region Property Changed Callbacks

        static void AChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnAChanged((byte)e.NewValue);
        }

        protected virtual void OnAChanged(byte newValue)
        {
            m_color.A = newValue;
            SetValue(ScAProperty, m_color.ScA);
            SetValue(SelectedColorProperty, m_color);
        }

        static void RChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnRChanged((byte)e.NewValue);
        }

        protected virtual void OnRChanged(byte newValue)
        {
            m_color.R = newValue;
            SetValue(ScRProperty, m_color.ScR);
            SetValue(SelectedColorProperty, m_color);
        }

        static void GChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnGChanged((byte)e.NewValue);
        }

        protected virtual void OnGChanged(byte newValue)
        {
            m_color.G = newValue;
            SetValue(ScGProperty, m_color.ScG);
            SetValue(SelectedColorProperty, m_color);
        }

        static void BChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnBChanged((byte)e.NewValue);
        }

        protected virtual void OnBChanged(byte newValue)
        {
            m_color.B = newValue;
            SetValue(ScBProperty, m_color.ScB);
            SetValue(SelectedColorProperty, m_color);
        }

        static void ScAChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnScAChanged((float)e.NewValue);
        }

        protected virtual void OnScAChanged(float newValue)
        {
            isAlphaChange = true;
            if (shouldFindPoint)
            {
                m_color.ScA = newValue;
                SetValue(AProperty, m_color.A);
                SetValue(SelectedColorProperty, m_color);
                SetValue(HexadecimalStringProperty, m_color.ToString());
            }

            isAlphaChange = false;
        }

        static void ScRChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnScRChanged((float)e.NewValue);
        }

        protected virtual void OnScRChanged(float newValue)
        {
            if (shouldFindPoint)
            {
                m_color.ScR = newValue;
                SetValue(RProperty, m_color.R);
                SetValue(SelectedColorProperty, m_color);
                SetValue(HexadecimalStringProperty, m_color.ToString());
            }
        }

        static void ScGChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnScGChanged((float)e.NewValue);
        }

        protected virtual void OnScGChanged(float newValue)
        {
            if (shouldFindPoint)
            {
                m_color.ScG = newValue;
                SetValue(GProperty, m_color.G);
                SetValue(SelectedColorProperty, m_color);
                SetValue(HexadecimalStringProperty, m_color.ToString());
            }
        }

        static void ScBChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnScBChanged((float)e.NewValue);
        }

        protected virtual void OnScBChanged(float newValue)
        {
            if (shouldFindPoint)
            {
                m_color.ScB = newValue;
                SetValue(BProperty, m_color.B);
                SetValue(SelectedColorProperty, m_color);
                SetValue(HexadecimalStringProperty, m_color.ToString());
            }
        }

        static void HexadecimalStringChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker c = (ColorPicker)d;
            c.OnHexadecimalStringChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual void OnHexadecimalStringChanged(string oldValue, string newValue)
        {
            try
            {
                if (shouldFindPoint)
                {
                    m_color = (Color)ColorConverter.ConvertFromString(newValue);
                }

                SetValue(AProperty, m_color.A);
                SetValue(RProperty, m_color.R);
                SetValue(GProperty, m_color.G);
                SetValue(BProperty, m_color.B);

                if (shouldFindPoint && !isAlphaChange && templateApplied)
                {
                    updateMarkerPosition(m_color);
                }
            }
            catch (FormatException)
            {
                SetValue(HexadecimalStringProperty, oldValue);
            }
        }

        static void selectedColor_changed(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ColorPicker cPicker = (ColorPicker)d;
            cPicker.OnSelectedColorChanged((Color)e.OldValue, (Color)e.NewValue);
        }

        protected virtual void OnSelectedColorChanged(Color oldColor, Color newColor)
        {
            RoutedPropertyChangedEventArgs<Color> newEventArgs =
                new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
            newEventArgs.RoutedEvent = SelectedColorChangedEvent;
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region Template Part Event Handlers

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            templateApplied = false;
            if (oldTemplate != null)
            {
                m_ColorSlider.ValueChanged -= BaseColorChanged;
                m_ColorDetail.MouseLeftButtonDown -= OnMouseLeftButtonDown;
                m_ColorDetail.PreviewMouseMove -= OnMouseMove;
                m_ColorDetail.SizeChanged -= ColorDetailSizeChanged;
                m_ColorDetail = null;
                m_ColorMarker = null;
                m_ColorSlider = null;
            }

            base.OnTemplateChanged(oldTemplate, newTemplate);
        }

        void BaseColorChanged(
            object sender, 
            RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_ColorPosition != null)
            {
                determineColor((Point)m_ColorPosition);
            }
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(m_ColorDetail);
            updateMarkerPosition(p);
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(m_ColorDetail);
                updateMarkerPosition(p);
                Mouse.Synchronize();
            }
        }

        void ColorDetailSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (args.PreviousSize != Size.Empty &&
                args.PreviousSize.Width != 0 && args.PreviousSize.Height != 0)
            {
                double widthDifference = args.NewSize.Width / args.PreviousSize.Width;
                double heightDifference = args.NewSize.Height / args.PreviousSize.Height;
                markerTransform.X = markerTransform.X * widthDifference;
                markerTransform.Y = markerTransform.Y * heightDifference;
            }
            else if (m_ColorPosition != null)
            {
                markerTransform.X = ((Point)m_ColorPosition).X * args.NewSize.Width;
                markerTransform.Y = ((Point)m_ColorPosition).Y * args.NewSize.Height;
            }
        }

        #endregion

        #region Color Resolution Helpers

        void setColor(Color theColor)
        {
            m_color = theColor;

            if (templateApplied)
            {
                SetValue(AProperty, m_color.A);
                SetValue(RProperty, m_color.R);
                SetValue(GProperty, m_color.G);
                SetValue(BProperty, m_color.B);
                updateMarkerPosition(theColor);
            }
        }

        void updateMarkerPosition(Point p)
        {
            markerTransform.X = p.X;
            markerTransform.Y = p.Y;

            p.X = p.X / m_ColorDetail.ActualWidth;
            p.Y = p.Y / m_ColorDetail.ActualHeight;

            m_ColorPosition = p;

            determineColor(p);

            // Bounds check to keep from getting random colors 
            // when the selection indicator triggers a move event outside of bounds. -- BKL
            if (p.X < 0)
            {
                p.X = 0;
                markerTransform.X = 0;
            }
            else if (p.X > 1)
            {
                p.X = 1;
                markerTransform.X = m_ColorDetail.ActualWidth;
            }

            if (p.Y < 0)
            {
                p.Y = 0;
                markerTransform.Y = 0;
            }
            else if (p.Y > 1)
            {
                p.Y = 1;
                markerTransform.Y = m_ColorDetail.ActualHeight;
            }

            m_ColorPosition = p;
            determineColor(p);
        }

        void updateMarkerPosition(Color theColor)
        {
            m_ColorPosition = null;

            HsvColor hsv = ColorUtilities.ConvertRgbToHsv(theColor.R, theColor.G, theColor.B);

            m_ColorSlider.Value = hsv.H;

            Point p = new Point(hsv.S, 1 - hsv.V);

            m_ColorPosition = p;
            p.X = p.X * m_ColorDetail.ActualWidth;
            p.Y = p.Y * m_ColorDetail.ActualHeight;
            markerTransform.X = p.X;
            markerTransform.Y = p.Y;
        }

        void determineColor(Point p)
        {
            HsvColor hsv = new HsvColor(360 - m_ColorSlider.Value, 1, 1);
            hsv.S = p.X;
            hsv.V = 1 - p.Y;
            m_color = ColorUtilities.ConvertHsvToRgb(hsv.H, hsv.S, hsv.V);
            shouldFindPoint = false;
            m_color.ScA = (float)GetValue(ScAProperty);
            SetValue(HexadecimalStringProperty, m_color.ToString());
            shouldFindPoint = true;
        }

        #endregion

        #region Private Fields

        SpectrumSlider m_ColorSlider;

        static readonly string ColorSliderName = "PART_ColorSlider";

        FrameworkElement m_ColorDetail;

        static readonly string ColorDetailName = "PART_ColorDetail";

        readonly TranslateTransform markerTransform = new TranslateTransform();

        Path m_ColorMarker;

        static readonly string ColorMarkerName = "PART_ColorMarker";

        Point? m_ColorPosition;

        Color m_color;

        bool shouldFindPoint;

        bool templateApplied;

        bool isAlphaChange;

        #endregion
    }

    #endregion ColorPicker
}