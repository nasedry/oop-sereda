# Звіт з аналізу SOLID принципів (SRP, OCP) в Open-Source проєкті

## 1. Обраний проєкт

- **Назва:** ASP.NET Core
- **Посилання на GitHub:** https://github.com/dotnet/aspnetcore

ASP.NET Core — це open-source фреймворк на C#, який широко використовується для створення веб-додатків. Проєкт має велику кодову базу, чітку архітектуру та активно застосовує принципи SOLID.

---

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

---

#### Клас: `HttpResponse`
- **Відповідальність:**  формування HTTP-відповіді
- **Обґрунтування:**  клас не виконує логіку обробки запиту, а лише описує відповідь


```csharp
public abstract class HttpResponse
{
    public abstract int StatusCode { get; set; }
    public abstract IHeaderDictionary Headers { get; }
    public abstract Stream Body { get; set; }
}
