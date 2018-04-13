
namespace WP8.Toolkit.Helpers
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Windows.Storage;

    public static class FileHelper
    {
        public static bool IsFileExisting(string fileName)
        {
            return File.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName));
        }

        public static async Task<StorageFile> WriteFileAsync(string fileName, string content)
        {
            byte[] data = Encoding.Unicode.GetBytes(content);

            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(data, 0, data.Length);
            }

            return file;
        }

        public static async Task<string> ReadFileAsync(string fileName)
        {
            var folder = ApplicationData.Current.LocalFolder;

            try
            {
                var file = await folder.OpenStreamForReadAsync(fileName);

                using (var streamReader = new StreamReader(file))
                {
                    return streamReader.ReadToEnd();
                }
            }

            catch
            {
                return string.Empty;
            }
        }
    }
}