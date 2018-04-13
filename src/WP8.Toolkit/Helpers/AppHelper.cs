
namespace WP8.Toolkit.Helpers
{
    using System.Xml.Linq;

    public static class AppHelper
    {
        #region [ Members ]

        private static readonly XElement _appContainer = null;

        #endregion

        #region [ Constructors ]

        static AppHelper()
        {
            _appContainer = XDocument.Load("WMAppManifest.xml").Root.Element("App");
        }

        #endregion

        #region [ Methods ]

        public static string ApplicationTitle
        {
            get { return GetAppValue("Title"); }
        }

        public static string ApplicationDescription
        {
            get { return GetAppValue("Description"); }
        }

        public static string ApplicationVersion
        {
            get { return GetAppValue("Version"); }
        }

        public static string ApplicationAuthor
        {
            get { return GetAppValue("Author"); }
        }

        private static string GetAppValue(string keyName)
        {
            return _appContainer.Attribute(keyName).Value;
        }

        #endregion
    }
}
