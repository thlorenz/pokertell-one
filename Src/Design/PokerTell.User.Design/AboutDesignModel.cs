namespace PokerTell.User.Design
{
    using PokerTell.User.ViewModels;

    public class AboutDesignModel : AboutViewModel
    {
    }

    public static class AboutDesign
    {
        public static AboutViewModel Model
        {
            get { return new AboutDesignModel(); }
        }
    }
}