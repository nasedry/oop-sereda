using System;
using lab5v15;
using lab5v15.Models;
using lab5v15.Generics;
using lab5v15.Exceptions;

class Program
{
    static void Main()
    {
        Utils.PrintHeader("Lab 5: Post Office & Shipments");

        var office = new PostOffice("Rivne Central");

        try
        {
            office.AddShipment(new Shipment("S1", "Kyiv", DateTime.Now.AddDays(-5), DateTime.Now));
            office.AddShipment(new Shipment("S2", "Lviv", DateTime.Now.AddDays(-3), DateTime.Now.AddDays(1)));
            office.AddShipment(new Shipment("S3", "Odesa", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1))); // буде виняток
        }
        catch (InvalidShipmentDatesException ex)
        {
            Console.WriteLine($"Помилка створення відправлення: {ex.Message}");
        }

        Console.WriteLine($"Середній термін доставки: {office.AverageDeliveryDays()} днів");
        Console.WriteLine($"Відсоток втрачених: {office.LostPercentage()}%");
        Console.WriteLine($"Відсоток пошкоджених: {office.DamagedPercentage()}%");

        Console.WriteLine("Топ напрямки:");
        foreach (var dest in office.TopDestinations(2))
        {
            Console.WriteLine(dest);
        }
    }
}
