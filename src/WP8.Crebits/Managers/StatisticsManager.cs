
namespace WP8.Crebits.Managers
{
    using Newtonsoft.Json;

    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;

    using WP8.Crebits.Helpers;
    using WP8.Toolkit.Helpers;

    public static class StatisticsManager
    {
        public static void SubmitVersion()
        {
            try
            {
                var token = string.Empty;
                var exportDataEmail = SettingsHelper.GetExportDataEmail();

                if (!string.IsNullOrEmpty(exportDataEmail))
                {
                    token = exportDataEmail
                        .Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries)
                        .FirstOrDefault();
                }
                
                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    Token = token,
                    Version = AppHelper.ApplicationVersion,
                    ExecutionCounter = SettingsHelper.GetExecutionCounter()
                }));

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var client = new HttpClient(); // do not use Dispose otherwise PostAsync freezes...

                client.PostAsync(AppConstants.ApiUrl + "Statistics/Version", content);
            }
            catch
            {
                // nothing.
            }
        }
    }
}