using NUnit.Framework;

namespace Test_Automation_Frameworks.Utilities
{
    public class FileDownloadHelper
    {
        public void VerifyFileDownload(string downloadDirectory, string fileName, TimeSpan timeout)
        {
            bool result = WaitForFileToDownload(downloadDirectory, fileName, timeout);
            Assert.That(result);
        }

        private bool WaitForFileToDownload(string downloadDirectory, string fileName, TimeSpan timeout)
        {
            string filePath = Path.Combine(downloadDirectory, fileName);
            var endTime = DateTime.Now + timeout;

            while (DateTime.Now < endTime)
            {
                if (File.Exists(filePath))
                    return true;

                Thread.Sleep(1000);
            }

            return false;
        }

        public void DeleteFileIfExists(string downloadDirectory, string fileName)
        {
            var path = Path.Combine(downloadDirectory, fileName);
            DeleteFileIfExists(path);
        }
        private void DeleteFileIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не вдалося видалити файл {filePath}: {ex.Message}");
                }
            }
        }
    }
}