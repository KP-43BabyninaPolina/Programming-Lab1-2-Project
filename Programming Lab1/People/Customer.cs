using Programming_Lab1.Items;
using Programming_Lab1.Items.Orders;
using Programming_Lab1.Shops;

namespace Programming_Lab1.People;

internal class Customer : Person
{
    private static int _totalCustomers;
    public static int GetTotalCustomers(Administrator? admin) => admin == null ? -1 : _totalCustomers;
    public CoffeeShop? CurrentShop { get; private set; }
    public Order? Preference { get; private set; }
    public Drink? Drink { get; set; }
    public int Money { get; private set; }

    static Customer()
    {
        _totalCustomers = 0;
        Console.WriteLine("Static customers counter is created.");
    }

    public Customer() : base()
    {
        Money = 100;
        Console.WriteLine("Person is a customer.");
        _totalCustomers++;
    }

    public Customer(string name = "Unknown", DateTime birthDate = default, int money = 100) : base(name, birthDate)
    {
        Money = money < 0 ? 0 : money;
        Console.WriteLine("Person is a customer.");
        _totalCustomers++;
    }

    public void EnterShop(CoffeeShop? shop)
    {
        if (shop == null)
        {
            Console.WriteLine("Customer can't enter a null shop.");
            return;
        }

        if (!shop.IsOpen)
        {
            Console.WriteLine("Customer can't enter a closed shop.");
            return;
        }

        if (CurrentShop == shop)
        {
            Console.WriteLine("Customer is already in this shop.");
            return;
        }

        CurrentShop = shop;
        Console.WriteLine("Customer has entered the shop.");
    }

    public void LeaveShop()
    {
        if (CurrentShop == null)
        {
            Console.WriteLine("Customer is not in any shop.");
            return;
        }

        CurrentShop = null;
        Console.WriteLine("Customer has left the shop.");
    }

    public Order MakeOrder()
    {
        if (CurrentShop == null)
        {
            Console.WriteLine("To make an order, Customer needs to enter the shop first!");
        }


        if (Preference != null) return Preference;

        Console.WriteLine("Choose your drink type (Coffee - 1, Tea - 2)");
        var typeInput = Console.ReadLine();
        Order dType = typeInput switch
        {
            "1" => new CoffeeOrder(this),
            "2" => new TeaOrder(this),
            _ => new CoffeeOrder(this)
        };

        Preference = dType;
        return Preference;
    }

    public int Pay(int v)
    {
        Money -= v;
        return v;
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        if (BirthDate != default)
            Console.WriteLine("Person`s birthday is at {0}.\n", BirthDate.ToShortDateString());
    }
}