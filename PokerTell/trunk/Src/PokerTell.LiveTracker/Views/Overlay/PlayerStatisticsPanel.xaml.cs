namespace PokerTell.LiveTracker.Views.Overlay
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;

    using Tools.WPF.Controls;

    /// <summary>
    /// Interaction logic for PlayerStatisticsPanel.xaml
    /// </summary>
    public partial class PlayerStatisticsPanel : UserControl
    {
        public PlayerStatisticsPanel()
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

        public void RemoveResizeAdorner()
        {
            if (_selectedElement != null)
            {
                // Remove the adorner from the selected element
                _adornerLayer.Remove(_adornerLayer.GetAdorners(_selectedElement)[0]);
                _selectedElement = null;
            }
        }
    }
}