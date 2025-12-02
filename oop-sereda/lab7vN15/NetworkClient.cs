using System;
using System.Net.Http;

public class NetworkClient
{
    private int attemptCount = 0;

    public string DownloadConfiguration(string url)
    {
        attemptCount++;

        if (attemptCount <= 3)
        {
            throw new HttpRequestException("Помилка завантаження з мережі (тимчасова).");
        }

        return $"Конфігурація з {url} успішно завантажена!";
    }
}
