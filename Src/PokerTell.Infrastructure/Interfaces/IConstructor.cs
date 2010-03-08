namespace PokerTell.Infrastructure.Interfaces
{
    /// <summary>
    /// Acts as function wrapper which delegates to the default constructor of concrete type for the given interface
    /// </summary>
    /// <typeparam name="TInterface">The interface used to substitute for the concrete type</typeparam>
    public interface IConstructor<TInterface> 
    {
        TInterface New { get; }
    }
}