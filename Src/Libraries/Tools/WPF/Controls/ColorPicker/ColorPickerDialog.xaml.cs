namespace Tools.WPF.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        void okButtonClicked(object sender, RoutedEventArgs e)
        {
            m_color = cPicker.SelectedColor;
            DialogResult = true;
            Hide();
        }

        void cancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        Color m_color;

        public Color SelectedColor
        {
            get { return m_color; }
        }

        public Color StartingColor
        {
            get { return cPicker.SelectedColor; }
            set { cPicker.SelectedColor = value; }
        }
    }
}