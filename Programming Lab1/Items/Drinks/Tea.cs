using Programming_Lab1.Items.Orders;

namespace Programming_Lab1.Items.Drinks;

internal class Tea : Drink
{
    private readonly bool _isWithLemon;
    private readonly bool _isFruitPuree;
    private readonly TeaType _type;

    public Tea()
    {
        _isWithLemon = false;
        _isFruitPuree = false;
        _type = TeaType.Black;

        Console.WriteLine("Barista made a basic tea drink.");
    }

    public Tea(TeaOrder order) : base(order)
    {
        _isWithLemon = order.IsWithLemon;
        _isFruitPuree = order.IsFruitPuree;
        _type = order.Type;

        Console.WriteLine("Barista made a tea drink.");
    }

    public override int CountPrice()
    {
        var price = 3 + base.CountPrice();

        if (_type == TeaType.Herbal)
            price += 1;
        if (_isWithLemon)
            price += 1;
        if (_isFruitPuree)
            price += 2;

        return price;
    }
}