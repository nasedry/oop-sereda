# IndependentWork13 - Polly Scenarios

Проєкт демонструє використання бібліотеки Polly для підвищення відмовостійкості .NET-застосунків.  
Реалізовані три сценарії: виклик зовнішнього API, доступ до бази даних та відправка повідомлень у чергу.


**Scenario 1: External API Call**

Проблема: Зовнішній API може тимчасово бути недоступним або повертати помилки.  
Політика Polly: Retry з експоненційною затримкою (2, 4, 8 секунд).

```csharp
var apiRetryPolicy = Policy
    .Handle<HttpRequestException>()
    .WaitAndRetry(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
        (ex, timeSpan, retryCount, context) =>
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Retry {retryCount} after {timeSpan.TotalSeconds}s due to: {ex.Message}");
        });

string apiResult = apiRetryPolicy.Execute(() => CallExternalApi("https://api.example.com/data"));

Scenario 2: Database Access

Проблема: База даних може бути недоступною або операція займати надто багато часу.
Політика Polly: Retry + Timeout

var dbPolicy = Policy
    .Handle<TimeoutException>()
    .WaitAndRetry(2, _ => TimeSpan.FromSeconds(2), 
        (ex, timeSpan, retryCount, context) =>
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] DB Retry {retryCount} after {timeSpan.TotalSeconds}s: {ex.Message}");
        })
    .Wrap(Policy.Timeout(5, Polly.Timeout.TimeoutStrategy.Pessimistic, 
        (context, timeSpan, task) =>
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Database operation timed out after {timeSpan.TotalSeconds}s");
        }));

string dbResult = dbPolicy.Execute(() => AccessDatabase());


Scenario 3: Send Message to Queue

Проблема: Черга може бути перевантажена, і повідомлення не відправляються.
Політика Polly: Retry + Circuit Breaker

var queuePolicy = Policy
    .Handle<InvalidOperationException>()
    .CircuitBreaker(2, TimeSpan.FromSeconds(10),
        (ex, breakDelay) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Circuit opened due to: {ex.Message}"),
        () => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Circuit closed, retrying operations."))
    .Wrap(Policy.Handle<InvalidOperationException>().Retry(3, (ex, retryCount) =>
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Queue Retry {retryCount} due to: {ex.Message}");
        }));

queuePolicy.Execute(() => SendMessageToQueue("Hello, World!"));


Висновки

Polly дозволяє автоматизувати повторні спроби при тимчасових помилках. Timeout контролює довгі операції та запобігає зависанню застосунку. Circuit Breaker захищає ресурси від перевантаження та помилок. Використання Polly підвищує стабільність та відмовостійкість .NET-застосунків. Код залишається чистим та легко модифікується для різних сценаріїв.