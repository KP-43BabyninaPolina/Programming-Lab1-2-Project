using Programming_Lab1.People;

namespace Programming_Lab1.Items.Orders;

internal class CoffeeOrder : Order
{
    public bool IsDouble { get; init; }

    public bool IsWithSyrup { get; init; }

    public CoffeeOrder()
    {
        IsDouble = false;
        IsWithSyrup = false;
        Console.WriteLine("Basic coffee order is created.");
    }

    public CoffeeOrder(CoffeeOrder other) : base(other)
    {
        IsDouble = other.IsDouble;
        IsWithSyrup = other.IsWithSyrup;

        Console.WriteLine("Coffee order is created.");
    }

    public CoffeeOrder(Customer c) : base(c)
    {
        Console.WriteLine("Do you want a double espresso? (yes/no)");
        var doubleInput = Console.ReadLine();
        IsDouble = doubleInput?.ToLower() == "yes";

        Console.WriteLine("Do you want syrup? (yes/no)");
        var syrupInput = Console.ReadLine();
        IsWithSyrup = syrupInput?.ToLower() == "yes";

        Console.WriteLine("Coffee order created.");
    }
}