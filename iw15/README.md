# Звіт з аналізу SOLID принципів (SRP, OCP) в Open-Source проєкті

## 1. Обраний проєкт
- **Назва:** ASP.NET Core
- **Посилання на GitHub:** https://github.com/dotnet/aspnetcore

## 2. Аналіз SRP (Single Responsibility Principle)

### 2.1. Приклади дотримання SRP

#### Клас: `HttpRequest`
- **Відповідальність:** представлення HTTP-запиту
- **Обґрунтування:** клас відповідає лише за зберігання та доступ до даних HTTP-запиту (headers, method, path)

```csharp
public abstract class HttpRequest
{
    public abstract string Method { get; set; }
    public abstract PathString Path { get; set; }
    public abstract IHeaderDictionary Headers { get; }
}

```

#### Клас: `HttpResponse`
- **Відповідальність:** формування HTTP-відповіді
- **Обґрунтування:** клас не виконує логіку обробки запиту, а лише описує відповідь

```csharp
public abstract class HttpResponse
{
    public abstract int StatusCode { get; set; }
    public abstract IHeaderDictionary Headers { get; }
    public abstract Stream Body { get; set; }
}
```
#### Клас: `FileLogger`
- **Відповідальність:** логування повідомлень у файл
- **Обґрунтування:** клас виконує лише одну задачу — запис логів

```csharp
public class FileLogger : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
    }
}
```
### 2.2. Приклади порушення SRP

#### Клас: `Startup`
- **Множинні відповідальності:**
    - конфігурація сервісів
    - налаштування middleware
    - логіка запуску додатку
- **Проблеми:** клас важко підтримувати та тестувати

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddAuthentication();
        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
```
Цей клас виконує декілька різних ролей, що порушує SRP.

## 3. Аналіз OCP (Open/Closed Principle)

### 3.1. Приклади дотримання OCP

#### Модуль: `ILogger`
- **Механізм розширення:** інтерфейси
- **Обґрунтування:** можна додавати нові логери без зміни існуючого коду

```csharp
public interface ILogger
{
    void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception exception, Func<TState, Exception, string> formatter);
}
```
Нові реалізації (ConsoleLogger, FileLogger, DbLogger) додаються без змін інтерфейсу.

#### Модуль: Middleware Pipeline
- **Механізм розширення:** патерн Chain of Responsibility
- **Обґрунтування:** нові middleware додаються без зміни існуючих

```csharp
app.Use(async (context, next) =>
{
    await next();
});
```
### 3.2. Приклади порушення OCP

#### Сценарій: обробка статусів через `switch`
- **Проблема:** при додаванні нового статусу потрібно змінювати існуючий код
- **Наслідки:** ризик помилок, складність підтримки

```csharp
switch (status)
{
    case Status.New:
        HandleNew();
        break;
    case Status.Processed:
        HandleProcessed();
        break;
}
```
Краще використовувати поліморфізм або Strategy.

## 4. Загальні висновки
ASP.NET Core загалом добре дотримується принципів SRP та OCP.
Більшість компонентів мають чітку відповідальність і легко розширюються через інтерфейси та middleware.
Проте деякі класи (наприклад, `Startup`) можуть порушувати SRP через поєднання кількох ролей.