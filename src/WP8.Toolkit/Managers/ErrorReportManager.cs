
namespace WP8.Toolkit.Managers
{
    using Microsoft.Phone.Tasks;

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Windows;

    using WP8.Toolkit.Helpers;

    public static class ErrorReportManager
    {
        #region [ Members ]

        private static readonly string _filename = "ErrorReport.txt";

        #endregion

        #region [ Methods ]

        public static void CheckForPreviousException(string message, string caption, string toEmail)
        {
            try
            {
                string contents = null;
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(_filename))
                    {
                        using (var reader = new StreamReader(store.OpenFile(_filename, FileMode.Open, FileAccess.Read, FileShare.None)))
                        {
                            contents = reader.ReadToEnd();
                        }

                        SafeDeleteFile(store);
                    }
                }

                if (contents != null)
                {
                    if (MessageBox.Show(message, caption, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        var email = new EmailComposeTask();

                        email.To = toEmail;

                        email.Subject = string.Format("{0} v{1} (WP8) - Problem Report",
                            AppHelper.ApplicationTitle,
                            AppHelper.ApplicationVersion);

                        email.Body = contents;

                        SafeDeleteFile(IsolatedStorageFile.GetUserStoreForApplication());

                        if (Debugger.IsAttached)
                        {
                            MessageBox.Show(email.Body, email.Subject, MessageBoxButton.OK);
                        }

                        email.Show();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                SafeDeleteFile(IsolatedStorageFile.GetUserStoreForApplication());
            }
        }

        public static void StoreException(string message, Exception exception)
        {
            if (exception != null)
            {
                try
                {
                    using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        SafeDeleteFile(store);

                        using (var output = new StreamWriter(store.CreateFile(_filename)))
                        {
                            output.WriteLine(message);
                            output.WriteLine();

                            output.WriteLine(exception.ToString());
                        }
                    }

                    if (Debugger.IsAttached)
                    {
                        ShowException(message, exception);
                    }
                }
                catch
                {
                }
            }
        }

        private static void ShowException(string message, Exception exception)
        {
            if (exception != null)
            {
                string error = exception.Message;
                string stackTrace = exception.StackTrace;

                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                    error = string.Format("{0} -> {1}", error, exception.Message);
                }

                MessageBox.Show(string.Format("{0}\r\n{1}", error, stackTrace), message, MessageBoxButton.OK);
            }
        }

        private static void SafeDeleteFile(IsolatedStorageFile store)
        {
            try
            {
                store.DeleteFile(_filename);
            }
            catch
            {
            }
        }

        #endregion
    }
}
