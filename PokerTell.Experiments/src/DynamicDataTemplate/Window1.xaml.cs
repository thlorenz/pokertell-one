namespace DynamicDataTemplate
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        #region Constructors and Destructors

        public Window1()
        {
            InitializeComponent();
            DataContext = new ContainerViewModel();
        }

        #endregion
    }
}