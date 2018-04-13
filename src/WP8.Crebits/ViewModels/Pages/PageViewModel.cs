
namespace WP8.Crebits.ViewModels
{
    using WP8.Toolkit.Helpers;

    public abstract class PageViewModel : WP8.Toolkit.ViewModels.ViewModel
    {
        #region [ Properties ]

        public string ApplicationTitle
        {
            get
            {
                return AppHelper.ApplicationTitle;
            }
        }

        public string ApplicationDescription
        {
            get { return AppHelper.ApplicationDescription; }
        }

        public string ApplicationAuthor
        {
            get { return AppHelper.ApplicationAuthor; }
        }

        public string ApplicationVersion
        {
            get { return string.Format("v{0}", AppHelper.ApplicationVersion); }
        }

        #endregion
    }
}
