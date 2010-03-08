namespace Tools.WPF.Interfaces
{
    using Microsoft.Practices.Composite;

    public interface IItemsRegionViewModel : IActiveAware
    {
        string HeaderInfo { get; }
    }
}