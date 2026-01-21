using System;
using System.Collections.Generic;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Order(int id, string customerName, decimal totalAmount)
    {
        Id = id;
        CustomerName = customerName;
        TotalAmount = totalAmount;
        Status = OrderStatus.New;
    }
}

public enum OrderStatus
{
    New,
    PendingValidation,
    Processed,
    Shipped,
    Delivered,
    Cancelled
}

public interface IOrderValidator
{
    bool IsValid(Order order);
}

public interface IOrderRepository
{
    void Save(Order order);
    Order GetById(int id);
}

public interface IEmailService
{
    void SendOrderConfirmation(Order order);
}

public class OrderValidator : IOrderValidator
{
    public bool IsValid(Order order)
    {
        return order.TotalAmount > 0;
    }
}

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<int, Order> _orders = new();

    public void Save(Order order)
    {
        _orders[order.Id] = order;
        Console.WriteLine($"Замовлення {order.Id} збережено");
    }

    public Order GetById(int id)
    {
        return _orders.ContainsKey(id) ? _orders[id] : null;
    }
}

public class ConsoleEmailService : IEmailService
{
    public void SendOrderConfirmation(Order order)
    {
        Console.WriteLine($"Email надіслано клієнту {order.CustomerName}");
    }
}

public class OrderService
{
    private readonly IOrderValidator _validator;
    private readonly IOrderRepository _repository;
    private readonly IEmailService _emailService;

    public OrderService(
        IOrderValidator validator,
        IOrderRepository repository,
        IEmailService emailService)
    {
        _validator = validator;
        _repository = repository;
        _emailService = emailService;
    }

    public void ProcessOrder(Order order)
    {
        order.Status = OrderStatus.PendingValidation;

        if (!_validator.IsValid(order))
        {
            Console.WriteLine("Замовлення невалідне");
            order.Status = OrderStatus.Cancelled;
            return;
        }

        _repository.Save(order);
        _emailService.SendOrderConfirmation(order);
        order.Status = OrderStatus.Processed;

        Console.WriteLine($"Замовлення {order.Id} оброблено");
    }
}

class Program
{
    static void Main()
    {
        IOrderValidator validator = new OrderValidator();
        IOrderRepository repository = new InMemoryOrderRepository();
        IEmailService emailService = new ConsoleEmailService();

        OrderService orderService = new OrderService(
            validator,
            repository,
            emailService
        );

        Order validOrder = new Order(1, "Анна", 1000);
        orderService.ProcessOrder(validOrder);

        Console.WriteLine();

        Order invalidOrder = new Order(2, "Олег", -300);
        orderService.ProcessOrder(invalidOrder);
    }
}
