namespace PokerTell.LiveTracker.Views.Overlay
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Input;

    using Tools.WPF.Controls;

    public partial class PlayerOverlayViewTemplate 
    {
        AdornerLayer _adornerLayer;

        bool _selected;

        UIElement _selectedElement;
        
        void ResizableElement_MouseEnter(object sender, MouseEventArgs e)
        {
            UnSelectPreviouslySelectedElements();

            _selectedElement = e.Source as UIElement;

            if (_selectedElement != null)
            {
                _adornerLayer = AdornerLayer.GetAdornerLayer(_selectedElement);
                _adornerLayer.Add(new ResizeAdorner(_selectedElement));
                _selected = true;
            }

            e.Handled = true;
        }

        void UnSelectPreviouslySelectedElements()
        {
            if (_selected)
            {
                _selected = false;
                if (_selectedElement != null)
                {
                    // Remove the adorner from the selected element
                    _adornerLayer.Remove(_adornerLayer.GetAdorners(_selectedElement)[0]);
                    _selectedElement = null;
                }
            }
        }
    }
}