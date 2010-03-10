namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using Interfaces;

    using Microsoft.Practices.Unity;

    using Overlay;

    public static class AutoWirerForTableAttacher
    {
        public static IUnityContainer ConfigureTableAttacherDependencies()
        {
            return new UnityContainer()
            .RegisterType<IWindowFinder, WindowFinder>()
            .RegisterType<IWindowManipulator, WindowManipulator>()
            .RegisterType<IOverlayToTableAttacher, OverlayToTableAttacher>();
        }
    }
}