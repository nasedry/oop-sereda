using System;
using System.IO;
using System.Net.Http;

class Program
{
    static void Main()
    {
        var fileProcessor = new FileProcessor();
        var networkClient = new NetworkClient();

        Func<Exception, bool> shouldRetry = ex =>
            ex is IOException || ex is HttpRequestException;

        try
        {
            string fileResult = RetryHelper.ExecuteWithRetry(
                () => fileProcessor.LoadConfiguration("config.json"),
                retryCount: 3,
                initialDelay: TimeSpan.FromSeconds(1),
                shouldRetry: shouldRetry
            );
            Console.WriteLine(fileResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не вдалося завантажити файл: {ex.Message}");
        }

        try
        {
            string networkResult = RetryHelper.ExecuteWithRetry(
                () => networkClient.DownloadConfiguration("http://example.com/config"),
                retryCount: 4,
                initialDelay: TimeSpan.FromSeconds(1),
                shouldRetry: shouldRetry
            );
            Console.WriteLine(networkResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не вдалося завантажити конфігурацію з мережі: {ex.Message}");
        }
    }
}
