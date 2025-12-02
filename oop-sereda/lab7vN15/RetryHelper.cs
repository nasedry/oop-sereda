using System;
using System.Threading;

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool> shouldRetry = null)
    {
        if (initialDelay == default) initialDelay = TimeSpan.FromSeconds(1);

        for (int attempt = 1; attempt <= retryCount; attempt++)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    throw;
                }

                Console.WriteLine($"Спроба {attempt} не вдалася: {ex.Message}");

                if (attempt == retryCount)
                {
                    throw;
                }

                var delay = TimeSpan.FromMilliseconds(initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                Console.WriteLine($"Очікування {delay.TotalSeconds} секунд перед повторною спробою...");
                Thread.Sleep(delay);
            }
        }

        throw new InvalidOperationException("Неможливо виконати операцію.");
    }
}
