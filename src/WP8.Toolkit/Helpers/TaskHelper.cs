
namespace WP8.Toolkit.Helpers
{
    using Microsoft.Phone.Tasks;

    public static class TaskHelper
    {
        public static void OpenMarketplace()
        {
            var marketPlace = new MarketplaceDetailTask();

            marketPlace.Show();
        }
    }
}
