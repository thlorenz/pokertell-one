namespace DragAndResize
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;

    public partial class WindowWithCanvasAndAdorner : Window
    {
        AdornerLayer _adornerLayer;

        bool _isDown;

        bool _isDragging;

        bool _selected;

        UIElement _selectedElement;

        Point _startPoint;

        double _originalLeft;

        double _originalTop;

        public WindowWithCanvasAndAdorner()
        {
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myCanvas.MouseLeftButtonUp += DragFinishedMouseHandler;
            myCanvas.MouseMove += Window1_MouseMove;
            myCanvas.MouseLeave += Window1_MouseLeave;
        }

        // Handler for drag stopping on leaving the window
        void Window1_MouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // Handler for drag stopping on user choice
        void DragFinishedMouseHandler(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // Method to stop dragging
        void StopDragging()
        {
            if (_isDown)
            {
                _isDown = false;
                _isDragging = false;
            }
        }

        // Hanler for providing drag operation with selected element (Turn on dragging)
        void Window1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(myCanvas).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                     (Math.Abs(e.GetPosition(myCanvas).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    _isDragging = true;
                }

                if (_isDragging)
                {
                    Point position = Mouse.GetPosition(myCanvas);
                    Canvas.SetTop(_selectedElement, position.Y - (_startPoint.Y - _originalTop));
                    Canvas.SetLeft(_selectedElement, position.X - (_startPoint.X - _originalLeft));
                }
            }
        }

        void ResizableElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(_selectedElement);
            _adornerLayer.Add(new ResizeAdorner(_selectedElement));
        }

        // Handler for clearing element selection, adorner removal
        void DragableElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UnSelectPreviouslySelectedElements();

            _isDown = true;
            _startPoint = e.GetPosition(myCanvas);

            _selectedElement = e.Source as UIElement;

            if (_selectedElement != null)
            {
                _originalLeft = Canvas.GetLeft(_selectedElement);
                _originalTop = Canvas.GetTop(_selectedElement);

                // Bring to Front
                UpdateZOrder(_selectedElement, true);

                    _adornerLayer = AdornerLayer.GetAdornerLayer(_selectedElement);
                    _adornerLayer.Add(new ResizeAdorner(_selectedElement));
            }

            _selected = true;
            e.Handled = true;
        }

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

        /// <summary>
        /// Helper method used by the BringToFront and SendToBack methods.
        /// </summary>
        /// <param name="element">
        /// The element to bring to the front or send to the back.
        /// </param>
        /// <param name="bringToFront">
        /// Pass true if calling from BringToFront, else false.
        /// </param>
        void UpdateZOrder(UIElement element, bool bringToFront)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (!myCanvas.Children.Contains(element))
            {
                throw new ArgumentException("Must be a child element of the Canvas.", "element");
            }

            // Determine the Z-Index for the target UIElement.
            int elementNewZIndex = -1;
            if (bringToFront)
            {
                foreach (UIElement elem in myCanvas.Children)
                {
                    if (elem.Visibility != Visibility.Collapsed)
                    {
                        ++elementNewZIndex;
                    }
                }
            }
            else
            {
                elementNewZIndex = 0;
            }

            // Determine if the other UIElements' Z-Index 
            // should be raised or lowered by one. 
            int offset = (elementNewZIndex == 0) ? +1 : -1;

            int elementCurrentZIndex = Panel.GetZIndex(element);

            // Update the Z-Index of every UIElement in the Canvas.
            foreach (UIElement childElement in myCanvas.Children)
            {
                if (childElement == element)
                {
                    Panel.SetZIndex(element, elementNewZIndex);
                }
                else
                {
                    int zIndex = Panel.GetZIndex(childElement);

                    // Only modify the z-index of an element if it is  
                    // in between the target element's old and new z-index.
                    if ((bringToFront && (elementCurrentZIndex < zIndex)) || (!bringToFront && (zIndex < elementCurrentZIndex)))
                    {
                        Panel.SetZIndex(childElement, zIndex + offset);
                    }
                }
            }
        }
    }
}