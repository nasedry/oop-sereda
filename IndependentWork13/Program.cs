using System;
using System.Net.Http;
using System.Threading;
using Polly;

namespace IndependentWork13
{
    public class Program
    {
        private static int _apiCallAttempts = 0;
        private static int _dbCallAttempts = 0;
        private static int _queueSendAttempts = 0;

        public static string CallExternalApi(string url)
        {
            _apiCallAttempts++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Attempt {_apiCallAttempts}: Calling API {url}...");
            if (_apiCallAttempts <= 2)
            {
                throw new HttpRequestException($"API call failed for {url}");
            }
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] API call to {url} successful!");
            return "Data from API";
        }

        public static string AccessDatabase()
        {
            _dbCallAttempts++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Attempt {_dbCallAttempts}: Accessing database...");
            if (_dbCallAttempts <= 1)
            {
                throw new TimeoutException("Database timeout occurred.");
            }
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Database access successful!");
            return "Data from DB";
        }

        public static void SendMessageToQueue(string message)
        {
            _queueSendAttempts++;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Attempt {_queueSendAttempts}: Sending message '{message}' to queue...");
            if (_queueSendAttempts <= 3)
            {
                throw new InvalidOperationException("Queue is full");
            }
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Message '{message}' sent successfully!");
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("--- Scenario 1: External API Call with Retry ---");
            var apiRetryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetry(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), 
                (ex, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Retry {retryCount} after {timeSpan.TotalSeconds}s due to: {ex.Message}");
                });

            try
            {
                string apiResult = apiRetryPolicy.Execute(() => CallExternalApi("https://api.example.com/data"));
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Final Result: {apiResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] API operation failed: {ex.Message}");
            }

            Console.WriteLine("\n--- Scenario 2: Database Access with Retry + Timeout ---");
            var dbPolicy = Policy
                .Handle<TimeoutException>()
                .WaitAndRetry(2, _ => TimeSpan.FromSeconds(2), 
                (ex, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Retry {retryCount} after {timeSpan.TotalSeconds}s: {ex.Message}");
                })
                .Wrap(Policy.Timeout(5, Polly.Timeout.TimeoutStrategy.Pessimistic, (context, timeSpan, task) =>
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Database operation timed out after {timeSpan.TotalSeconds}s");
                }));

            try
            {
                string dbResult = dbPolicy.Execute(() => AccessDatabase());
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Final DB Result: {dbResult}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB operation failed: {ex.Message}");
            }

            Console.WriteLine("\n--- Scenario 3: Send Message to Queue with Retry + Circuit Breaker ---");
            var queuePolicy = Policy
                .Handle<InvalidOperationException>()
                .CircuitBreaker(2, TimeSpan.FromSeconds(10),
                (ex, breakDelay) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Circuit opened due to: {ex.Message}"),
                () => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Circuit closed, retrying operations."))
                .Wrap(Policy.Handle<InvalidOperationException>().Retry(3, (ex, retryCount) =>
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Queue Retry {retryCount} due to: {ex.Message}");
                }));

            try
            {
                queuePolicy.Execute(() => SendMessageToQueue("Hello, World!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Queue operation failed: {ex.Message}");
            }

            Console.WriteLine("\n--- End of Scenarios ---");
        }
    }
}
