namespace DragAndResize
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ResizeAdorner : Adorner
    {
        readonly Thumb _bottomRight;

        // To store and manage the adorner's visual children.
        readonly VisualCollection _visualChildren;

        // Initialize the ResizeAdorner.
        public ResizeAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            _visualChildren = new VisualCollection(this);

            BuildAdornerCorner(ref _bottomRight, Cursors.SizeNWSE);
            _bottomRight.DragDelta += HandleBottomRight;
        }

        // Handler for resizing from the bottom-right.
        void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            var adornedElement = AdornedElement as FrameworkElement;
            var hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null)
            {
                return;
            }

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizeAdorner at the corners of the adorned element.  
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;

            // adornerWidth & adornerHeight are used for placement as well.
            double adornerWidth = DesiredSize.Width;
            double adornerHeight = DesiredSize.Height;

            _bottomRight.Arrange(new Rect(desiredWidth - (adornerWidth / 2) - 5, desiredHeight - (adornerHeight / 2) - 5, adornerWidth, adornerHeight));

            // Return the final size.
            return finalSize;
        }

        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        void BuildAdornerCorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            if (cornerThumb != null)
            {
                return;
            }

            cornerThumb = new Thumb { Cursor = customizedCursor };

            // Set some arbitrary visual characteristics.
            cornerThumb.Height = cornerThumb.Width = 5;
            cornerThumb.Opacity = 0.40;
            cornerThumb.Background = new SolidColorBrush(Colors.Transparent);

            _visualChildren.Add(cornerThumb);
        }

        // This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        // Width and Height values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        // need to be set first.  It also sets the maximum size of the adorned element.
        static void EnforceSize(FrameworkElement adornedElement)
        {
            if (adornedElement.Width.Equals(Double.NaN))
            {
                adornedElement.Width = adornedElement.DesiredSize.Width;
            }

            if (adornedElement.Height.Equals(Double.NaN))
            {
                adornedElement.Height = adornedElement.DesiredSize.Height;
            }

            var parent = adornedElement.Parent as FrameworkElement;
            if (parent != null)
            {
                adornedElement.MaxHeight = parent.ActualHeight;
                adornedElement.MaxWidth = parent.ActualWidth;
            }
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount
        {
            get { return _visualChildren.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }
    }
}