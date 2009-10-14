namespace Tools.WPF
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;

    // From http://karlshifflett.wordpress.com/2008/10/11/viewing-design-time-data-in-visual-studio-2008-cider-designer-in-wpf-and-silverlight-projects/
    public class DesignAndRunTimeDataContext : DataSourceProvider
    {
        #region Constants and Fields

        private readonly bool _bolIsInDesignMode = false;

        private object _objDesignTimeDataContext;

        private object _objRuntimeDataContext;

        #endregion

        #region Constructors and Destructors

        public DesignAndRunTimeDataContext()
        {
            _bolIsInDesignMode =
                (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
        }

        #endregion

        #region Properties

        public object DesignTimeDataContext
        {
            get { return _objDesignTimeDataContext; }

            set
            {
                _objDesignTimeDataContext = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DesignTimeDataContext"));
            }
        }

        public object RuntimeDataContext
        {
            get { return _objRuntimeDataContext; }

            set
            {
                _objRuntimeDataContext = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RuntimeDataContext"));
            }
        }

        #endregion

        #region Methods

        protected override void BeginQuery()
        {
            if (_bolIsInDesignMode)
            {
                OnQueryFinished(DesignTimeDataContext);
            }
            else
            {
                OnQueryFinished(RuntimeDataContext);
            }
        }

        #endregion
    }
}