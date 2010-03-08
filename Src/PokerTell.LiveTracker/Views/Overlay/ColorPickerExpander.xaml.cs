namespace PokerTell.LiveTracker.Views.Overlay
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Tools.WPF.ViewModels;

    /// <summary>
    /// Interaction logic for ColorPickerExpander.xaml
    /// This control only seems to work with a <see cref="ColorViewModel"/> as DataContext
    /// </summary>
    public partial class ColorPickerExpander : UserControl
    {
        #region Constructors and Destructors

        public ColorPickerExpander()
        {
            InitializeComponent();
            ColorPickerControl.SelectedColorChanged += ColorPickerControl_SelectedColorChanged;
        }

        #endregion

        #region Methods

        void ColorPickerControl_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            // If [Undo changes] is used and the ColorPicker is showing, we need to close and re-expand it
            // in order to make it refresh itself
            if (expColorPickerArea.IsExpanded && ! IsMouseOver)
            {
                expColorPickerArea.IsExpanded = false;
                expColorPickerArea.IsExpanded = true;
            }
        }

        void ColorPickerExpander_Expanded(object sender, RoutedEventArgs e)
        {
            // Workaround to ensure that StartingColor is equal previously to selected Color
            // on first time expand 
            ColorPickerControl.SelectedColor = ((SolidColorBrush)ColorSampleEllipse.Fill).Color;
        }

        #endregion
    }
}