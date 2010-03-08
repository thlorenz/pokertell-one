namespace PokerTell.Infrastructure.Events
{
    using Interfaces.PokerHand;

    using Microsoft.Practices.Composite.Presentation.Events;

    public class NewHandEvent : CompositePresentationEvent<NewHandEventArgs>
    {
    }

    public class NewHandEventArgs
    {
        readonly string _foundInFullPath;

        readonly IConvertedPokerHand _convertedPokerHand;

        public NewHandEventArgs(string foundInFullPath, IConvertedPokerHand convertedPokerHand)
        {
            _foundInFullPath = foundInFullPath;
            _convertedPokerHand = convertedPokerHand;
        }

        public string FoundInFullPath
        {
            get { return _foundInFullPath; }
        }

        public IConvertedPokerHand ConvertedPokerHand
        {
            get { return _convertedPokerHand; }
        }
    }
}