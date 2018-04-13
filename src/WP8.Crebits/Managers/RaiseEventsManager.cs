
namespace WP8.Crebits.Managers
{
    using WP8.Crebits.ViewModels;

    public static class RaiseEventsManager
    {
        private static MainPageViewModel _mainPageViewModel = null;

        static RaiseEventsManager()
        {
        }

        public static void SetMainViewModel(MainPageViewModel mainPageViewModel)
        {
            _mainPageViewModel = mainPageViewModel;
        }

        /// <summary>
        /// Refresh the main screen with data from the local database.
        /// </summary>
        /// 
        /// <param name="withForceLoad">
        /// Value indicating whether the data is reloaded even if it has been already loaded.
        /// </param>
        public static void RaiseRefreshMainScreen(bool withForceLoad = true)
        {
            if (_mainPageViewModel != null)
            {
                if (withForceLoad || !_mainPageViewModel.IsDataLoaded)
                {
                    _mainPageViewModel.LoadData();
                }
            }
        }
    }
}
