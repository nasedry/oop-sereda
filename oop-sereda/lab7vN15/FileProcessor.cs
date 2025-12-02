using System;
using System.IO;

public class FileProcessor
{
    private int attemptCount = 0;

    public string LoadConfiguration(string path)
    {
        attemptCount++;

        if (attemptCount <= 2)
        {
            throw new IOException("Помилка читання файлу (тимчасова).");
        }

        return $"Файл {path} успішно завантажено!";
    }
}
