
namespace WP8.Crebits.ViewModels
{
    public class AboutPageViewModel : WP8.Toolkit.ViewModels.PageViewModel
    {
        #region [ PageViewModel Overrides ]

        public override string PageTitle
        {
            get
            {
                return WP8.Crebits.Resources.AppResources.About;
            }
        }

        #endregion
    }
}