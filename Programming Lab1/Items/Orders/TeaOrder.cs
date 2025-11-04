using Programming_Lab1.People;

namespace Programming_Lab1.Items.Orders;

internal class TeaOrder : Order
{
    public bool IsWithLemon { get; init; }

    public bool IsFruitPuree { get; init; }

    public TeaType Type { get; init; }

    public TeaOrder()
    {
        IsWithLemon = false;
        IsFruitPuree = false;
        Type = TeaType.Black;
        Console.WriteLine("Basic tea order is created.");
    }

    public TeaOrder(TeaOrder other) : base(other)
    {
        IsWithLemon = other.IsWithLemon;
        IsFruitPuree = other.IsFruitPuree;
        Type = other.Type;

        Console.WriteLine("Tea order is created.");
    }

    public TeaOrder(Customer c) : base(c)
    {
        Console.WriteLine("What type of tea? (Black - 1, Green - 2, Herbal - 3)");
        var typeInput = Console.ReadLine();
        Type = typeInput switch
        {
            "1" => TeaType.Black,
            "2" => TeaType.Green,
            "3" => TeaType.Herbal,
            _ => TeaType.Black
        };

        Console.WriteLine("Do you want lemon? (yes/no)");
        var lemonInput = Console.ReadLine();
        IsWithLemon = lemonInput?.ToLower() == "yes";

        Console.WriteLine("Do you want fruit puree? (yes/no)");
        var pureeInput = Console.ReadLine();
        IsFruitPuree = pureeInput?.ToLower() == "yes";

        Console.WriteLine("Tea order created.");
    }
}