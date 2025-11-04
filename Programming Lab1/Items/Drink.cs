using Programming_Lab1.People;

namespace Programming_Lab1.Items;

public enum DrinkSize
{
    Small,
    Medium,
    Large
}

public enum TeaType
{
    Black,
    Green,
    Herbal
}

internal abstract class Drink 
{
    private readonly bool _isWithMilk;

    private readonly bool _isIced;

    private readonly bool _isWithSugar;

    private readonly DrinkSize _volume; 
    
    protected Drink()
    {
        _isIced = false;
        _isIced = false;
        _isWithSugar = false;
        _volume = DrinkSize.Medium;

        Console.WriteLine("Barista started making a basic drink.");
    }

    protected Drink(Order order)
    {
        _isWithMilk = order.IsWithMilk;
        _isIced = order.IsIced;
        _isWithSugar = order.IsWithSugar;
        _volume = order.Volume;

        Console.WriteLine("Barista started making a drink.");
    }

    public virtual int CountPrice()
    {
        var price = 0;

        price += _volume switch
        {
            DrinkSize.Small => 2,
            DrinkSize.Medium => 3,
            DrinkSize.Large => 4,
            _ => throw new ArgumentOutOfRangeException(),
        };

        if (_isIced)
            price += 1;
        if (_isWithMilk)
            price += 1;
        return price;
    }
}
