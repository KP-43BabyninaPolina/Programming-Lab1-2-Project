using Programming_Lab1.Items.Orders;
using Programming_Lab1.People;

namespace Programming_Lab1.Items.Drinks;

internal class Coffee : Drink
{
    private readonly bool _isDouble;
    private readonly bool _isWithSyrup;
    private static int _totalEspPortions;

    public static int GetTotalEspPortions(Administrator? admin) =>
        admin != null ? _totalEspPortions : -1;

    static Coffee()
    {
        _totalEspPortions = 0;
    }

    public Coffee()
    {
        _isDouble = false;
        _isWithSyrup = false;
        _totalEspPortions++;
        Console.WriteLine("Barista made a basic coffee drink.");
    }

    public Coffee(CoffeeOrder order) :
        base(order)
    {
        _isDouble = order.IsDouble;
        var espPortions = _isDouble ? 2 : 1;
        _totalEspPortions += espPortions;
        _isWithSyrup = order.IsWithSyrup;

        Console.WriteLine("Barista made a coffee drink.");
    }

    public override int CountPrice()
    {
        var price = 5 + base.CountPrice();

        if (_isDouble)
            price += 3;
        if (_isWithSyrup)
            price += 1;
        return price;
    }
}