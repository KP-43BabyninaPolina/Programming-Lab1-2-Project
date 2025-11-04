using Programming_Lab1.Items;
using Programming_Lab1.Items.Drinks;
using Programming_Lab1.Items.Orders;
using Programming_Lab1.Shops;

namespace Programming_Lab1.People;

internal class Barista : Worker
{
    private bool _disposed = false;

    public bool IsBusy { get; set; } = false;

    private Administrator? _boss;

    public Administrator? Boss
    {
        set
        {
            if (value == null)
            {
                Console.WriteLine("Barista is unemployed.");
                _boss = null;
                return;
            }

            if (_boss == value)
            {
                Console.WriteLine("Barista is already hired by the admin.");
                return;
            }

            _boss = value;
            Console.WriteLine("Barista is hired by an admin.");
        }
    }

    public Barista(string name, DateTime birthDate) : base(name, birthDate)
    {
        Console.WriteLine("Worker became a barista.");
        Boss = null;
    }

    public void OpenShop(CoffeeShop shop)
    {
        shop.IsOpen = true;
        Console.WriteLine("Barista has opened the shop.");
    }

    public void CloseShop(CoffeeShop shop)
    {
        shop.IsOpen = false;
        Console.WriteLine("Barista has closed the shop.");
    }

    public Drink TakeOrder(Order order, out Customer c)
    {
        Console.WriteLine("Barista is taking an order.");

        var drinkType = order.GetType().Name;
        c = order.Orderer;
        return drinkType switch
        {
            "CoffeeOrder" => new Coffee((CoffeeOrder)order),
            "TeaOrder" => new Tea((TeaOrder)order),
            _ => new Coffee((CoffeeOrder)order)
        };
    }

    public int ServeDrink(Drink d, Customer c)
    {
        c.Drink = d;
        Console.WriteLine($"Barista has served the drink. It costed {d.CountPrice()}$.");
        return c.Pay(d.CountPrice());
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.Write("They are a barista.");

        if (_boss == null)
            Console.WriteLine("They are not hired yet.");
        else
        {
            Console.WriteLine("They are hired by the admin.\n");
            if (IsBusy)
                Console.WriteLine("They are at work now.\n");
        }
    }

    protected override void CleanUp(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Console.WriteLine("CleanUp is called from Barista.");
            }
            _disposed = true;
        }
        base.CleanUp(disposing);
    }

    ~Barista()
    {
        Console.WriteLine("Destructor is called from Barista.");
    }

}