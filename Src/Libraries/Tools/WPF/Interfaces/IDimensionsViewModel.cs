namespace Tools.WPF.Interfaces
{
    using System.Drawing;

    public interface IDimensionsViewModel
    {
        Rectangle Rectangle { get; }

        int Left { get; set; }

        int Top { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        IDimensionsViewModel InitializeWith(Rectangle rectangle);

        IDimensionsViewModel InitializeWith(int left, int top, int width, int height);
    }
}