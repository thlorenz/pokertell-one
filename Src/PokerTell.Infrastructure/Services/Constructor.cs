namespace PokerTell.Infrastructure.Services
{
    using System;

    using Interfaces;

    /// <summary>
    /// Acts as function wrapper which delegates to the default constructor of concrete type for the given interface
    /// </summary>
    /// <typeparam name="TInterface">The interface used to substitute for the concrete type</typeparam>
    public class Constructor<TInterface> : IConstructor<TInterface>
    {
        readonly Func<TInterface> _constructionDelegate;

       /// <summary>
       /// Receives a delegate, the points to the code which constructs the concrete type used for the interface.
       /// </summary>
       /// <param name="constructionDelegate">Points at the default constructor of the concrete type</param>
        public Constructor(Func<TInterface> constructionDelegate)
        {
            _constructionDelegate = constructionDelegate;
        }

        public TInterface New
        {
            get { return _constructionDelegate.Invoke(); }
        }
    }
}