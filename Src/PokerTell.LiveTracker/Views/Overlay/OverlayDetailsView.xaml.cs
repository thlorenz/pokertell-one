namespace PokerTell.LiveTracker.Views.Overlay
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;

    using Tools.WPF.Controls;

    /// <summary>
    /// Interaction logic for OverlayDetailsView.xaml
    /// </summary>
    public partial class OverlayDetailsView 
    {
        public OverlayDetailsView()
        {
            InitializeComponent();
        }

        AdornerLayer _adornerLayer;

        UIElement _selectedElement;

        public void ResizableElement_MouseEnter(object sender, MouseEventArgs e)
        {
            _selectedElement = e.Source as UIElement;
            AddResizeAdorner();

            e.Handled = true;
        }

        void AddResizeAdorner()
        {
            if (_selectedElement != null)
            {
                _adornerLayer = AdornerLayer.GetAdornerLayer(_selectedElement);
                _adornerLayer.Add(new ResizeAdorner(_selectedElement));
            }
        }
    }
}