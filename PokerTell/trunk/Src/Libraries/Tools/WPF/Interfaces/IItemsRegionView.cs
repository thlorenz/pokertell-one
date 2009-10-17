namespace Tools.WPF.Interfaces
{
    using Microsoft.Practices.Composite;

    public interface IItemsRegionView : IActiveAware
    {
        IItemsRegionViewModel ActiveAwareViewModel { get; }
    }
}