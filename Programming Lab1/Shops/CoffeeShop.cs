
using Programming_Lab1.People;

namespace Programming_Lab1.Shops;

    internal class CoffeeShop
    {
        public string Name { get; init; }
        private readonly Administrator _owner;
        private int _income;
        public bool IsOpen = false;


        public CoffeeShop(Administrator owner, string name)
        {
            _owner = owner;
            _income = 0;
            Name = name;
            owner.CoffeeShops!.Add(this);
            Console.WriteLine("Coffee shop is created.");
        }

        public void Run()
        {
            Barista? barista = _owner.GetBarista();
            if (barista == null)
            {
                Console.WriteLine("All hired baristas are busy at other shops. Hire more to run this shop.");
                return;
            }

            barista.OpenShop(this);

            var count = 0;
            do
            {
                var customer = new Customer();
                customer.EnterShop(this);
                var order = customer.MakeOrder();
                var drink = barista.TakeOrder(order, out Customer reciever);
                var money = barista.ServeDrink(drink, reciever);
                _income += money;
                count++;
            } 
            while (count < 3);

            barista.CloseShop(this);
            Console.WriteLine($"Barista ended the shift. Income from {Name} for today: {_income}$.");
        }
    }

