namespace PokerTell.PokerHand.ViewModels
{
    using Infrastructure.Interfaces.PokerHand;

    using Tools.GenericUtilities;

    public class HandHistoriesViewModel
    {
        CompositeAction<IPokerHandCondition> _applyFilterCompositeAction;

        public CompositeAction<IPokerHandCondition> ApplyFilterCompositeAction
        {
            get
            {
                if (_applyFilterCompositeAction == null)
                {
                    _applyFilterCompositeAction = new CompositeAction<IPokerHandCondition>();
                }

                return _applyFilterCompositeAction;
            }
        }
    }
}