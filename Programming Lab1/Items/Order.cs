//заготовка лаба 2
using Programming_Lab1.People;

namespace Programming_Lab1.Items;

internal abstract class Order 
{
    public Customer Orderer { get; init; }

    public DrinkSize Volume { get; init; }

    public bool IsIced { get; init; }

    public bool IsWithMilk { get; init; }

    public bool IsWithSugar { get; init; }

    protected Order()
    {
        Orderer = new Customer();
        Volume = DrinkSize.Medium;
        IsIced = false;
        IsWithMilk = false;
        IsWithSugar = false;
        Console.WriteLine("Basic order is created.");
    }

    protected Order(Order other)
    {
        Orderer = other.Orderer;
        Volume = other.Volume;
        IsIced = other.IsIced;
        IsWithMilk = other.IsWithMilk;
        IsWithSugar = other.IsWithSugar;
    }

    protected Order(Customer c)
    {
        Orderer = c;
        Console.WriteLine("Choose your drink options:");
        Console.WriteLine("What volume? (Small - 1, Medium - 2, Large - 3");
        var sizeInput = Console.ReadLine();
        Volume = sizeInput switch
        {
            "1" => DrinkSize.Small,
            "2" => DrinkSize.Medium,
            "3" => DrinkSize.Large,
            _ => DrinkSize.Medium
        };

        Console.WriteLine("Do you want it iced? (yes/no)");
        var icedInput = Console.ReadLine();
        IsIced = icedInput?.ToLower() == "yes";

        Console.WriteLine("Do you want milk? (yes/no)");
        var milkInput = Console.ReadLine();
        IsWithMilk = milkInput?.ToLower() == "yes";

        Console.WriteLine("Do you want sugar? (yes/no)");
        var sugarInput = Console.ReadLine();
        IsWithSugar = sugarInput?.ToLower() == "yes";
    }
}