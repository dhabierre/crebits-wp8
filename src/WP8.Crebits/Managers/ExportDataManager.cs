
namespace WP8.Crebits.Managers
{
    using Microsoft.Phone.Tasks;

    using System;
    using System.Linq;
    using System.Text;

    using WP8.Crebits.DataServices;
    using WP8.Crebits.Entities;
    using WP8.Crebits.Helpers;
    using WP8.Crebits.Resources;
    using WP8.Crebits.ViewModels;
    using WP8.Toolkit.Helpers;

    public static class ExportDataManager
    {
        public static void ShowExportDataEmailComposeTask()
        {
            var email = new EmailComposeTask();

            email.To = SettingsHelper.GetExportDataEmail();

            email.Subject = string.Format("{0} - {1}",
                AppHelper.ApplicationTitle,
                DateTime.Today.ToString("dd MMMM yyyy"));

            email.Body = GetTextData();

            email.Show();
        }

        private static string GetTextData()
        {
            using (var dataService = new DataService())
            {
                #region [ Load Data & ViewModels ]

                var credits = dataService.GetAllCredits(withEnabledOnly: true);
                var creditsViewModel = new CreditsViewModel();

                var debits = dataService.GetAllDebits(withEnabledOnly: true);
                var debitsViewModel = new DebitsViewModel();

                var categories = dataService.GetAllCategories();

                var summaryViewModel = new SummaryViewModel();
                var statsViewModel = new StatsViewModel();

                debitsViewModel.Update(debits);
                creditsViewModel.Update(credits);
                summaryViewModel.Update(credits, debits);
                statsViewModel.Update(categories, debits);

                #endregion

                var sbOutput = new StringBuilder(10 * 1024);
                const string separator = "------------------\r\n\r\n";

                sbOutput.AppendFormat("{0}: {1}\r\n",
                    AppResources.Credit,
                    DoubleHelper.ToString(summaryViewModel.TotalCredits));

                sbOutput.AppendFormat("{0}: {1}\r\n",
                    AppResources.Debit,
                    DoubleHelper.ToString(summaryViewModel.TotalDebits));

                sbOutput.AppendFormat("{0}: {1}\r\n",
                    AppResources.Cash,
                    DoubleHelper.ToString(summaryViewModel.Cash));

                sbOutput.AppendFormat("\r\n{0}\r\n", AppResources.Credits);
                sbOutput.AppendFormat(separator);

                foreach (var item in creditsViewModel.Credits)
                {
                    sbOutput.AppendFormat("{0} -> {1} {2}\r\n",
                        DoubleHelper.ToString(item.CurrentValue),
                        item.Caption,
                        item.IsMonthly ? string.Empty : string.Format("({0})", item.Date.ToString("yyyy-MM-dd")));
                }

                sbOutput.AppendFormat("\r\n{0}\r\n", AppResources.Debits);
                sbOutput.AppendFormat(separator);

                foreach (var item in debitsViewModel.Debits)
                {
                    sbOutput.AppendFormat("{0} -> {1} {2}\r\n",
                        DoubleHelper.ToString(item.CurrentValue),
                        item.Caption,
                        item.IsMonthly ? string.Empty : string.Format("({0})", item.Date.ToString("yyyy-MM-dd")));
                }

                sbOutput.AppendFormat("\r\n{0}\r\n", AppResources.Categories);
                sbOutput.AppendFormat(separator);

                double debitsSum = debitsViewModel.Debits.Sum(j => j.CurrentValue);

                foreach (var category in statsViewModel.Categories.Where(i => i.TotalValue > 0))
                {
                    sbOutput.AppendFormat("{0} -> {1} ({2}%)\r\n",
                        category.Caption,
                        category.TotalValue,
                        category.GetPercentageDistribution(debitsSum));
                }

                return sbOutput.ToString();
            }
        }
    }
}
