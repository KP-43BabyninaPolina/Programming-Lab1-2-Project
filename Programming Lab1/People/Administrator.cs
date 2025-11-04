using Programming_Lab1.Shops;
using System.Runtime.InteropServices;

namespace Programming_Lab1.People;

internal class Administrator : Worker
{
    private bool _disposed = false;

    private static Administrator? _instance;

    private readonly string _password;

    private static bool Registered => _instance != null;
    public List<Barista>? Employees { get; private set; }
    public List<CoffeeShop>? CoffeeShops { get; private set; }

    //Приклад некерованих ресурсів:
    //виділення місця в пам'яті для зберігання логів адміністратора
    private IntPtr _nativeLogBuffer;

    private bool _hasNativeBuffer;

    static Administrator()
    {
        _instance = null;
        Console.WriteLine("Static admin instance is created.");
    }

    private Administrator(string name, DateTime birthDate, string password) : base(name, birthDate)
    {
        _password = password;
        Employees = [];
        CoffeeShops = [];

        //Виділення некерованої пам'яті (128 байтів для логів) для демонстрації.
        _nativeLogBuffer = Marshal.AllocHGlobal(128);
        _hasNativeBuffer = true;

        Console.WriteLine("Worker became administrator.");
    }

    private static string ReadPassword(bool maskInput = true)
    {
        var sb = new System.Text.StringBuilder();
        ConsoleKeyInfo key;

        while (true)
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                // finished
                Console.WriteLine();
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (sb.Length > 0)
                {
                    sb.Length--;
                    if (maskInput)
                    {
                        // erase last mask char
                        Console.Write("\b \b");
                    }
                }

                continue;
            }

            // ignore other control characters
            if (char.IsControl(key.KeyChar))
                continue;

            sb.Append(key.KeyChar);
            if (maskInput)
                Console.Write('*');
        }

        return sb.ToString();
    }

    public static void UnregisterInstance()
    {
        _instance = null;
        Console.WriteLine("Administrator logged out.");
    }

    public static void Register(string name, DateTime birthDate)
    {
        if (Registered)
        {
            Console.WriteLine("Administrator already exists. There can't be more than 1 admin.");
            return;
        }

        Console.WriteLine("To register as admin, please create an access password:");
        string? password;
        do
        {
            password = ReadPassword(maskInput: true);
            if (string.IsNullOrEmpty(password))
                Console.WriteLine("Password can't be empty. Please try again:");
        } while (string.IsNullOrEmpty(password));

        _instance = new Administrator(name, birthDate, password);
    }

    public static Administrator? GetAccess()
    {
        if (!Registered)
        {
            Console.WriteLine("Denied: no admin registered yet.");
            _instance = null;
            Thread.Sleep(2000);
            Environment.Exit(2000);
        }

        Console.WriteLine("Access code:");
        // hide the typed password during entry
        var password = ReadPassword(maskInput: true);

        if (password != _instance!._password)
        {
            Console.WriteLine("Denied: incorrect passcode.");
            _instance = null;
            Thread.Sleep(2000);
            Environment.Exit(2000);
        }

        Console.WriteLine("Administrator operations accessed.");
        return _instance;
    }

    public void Hire(Barista barista)
    {
        ArgumentNullException.ThrowIfNull(barista);

        if (Employees!.Contains(barista))
        {
            Console.WriteLine("Barista is already hired by an admin.");
            return;
        }

        Employees.Add(barista);
        barista.Boss = this;
        Console.WriteLine("Admin has hired a barista.");
    }

    public Barista? GetBarista()
    {
        var i = 0;
        while (Employees![i].IsBusy)
            i++;

        if (i != Employees.Count) return Employees[i];


        Console.WriteLine("All baristas are busy now.");
        return null;
    }

    public void ShowEmployees()
    {
        Console.WriteLine("Employees:");
        foreach (var employee in Employees!)
        {
            Console.Write(employee + ", ");
        }

        Console.WriteLine();
    }

    public void ShowCoffeeShops()
    {
        Console.WriteLine("Shops:");
        foreach (var shop in CoffeeShops!)
        {
            Console.Write(shop.Name + ", ");
        }
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.Write("They are an administrator.\n");
    }


    protected override void CleanUp(bool disposing)
    {
        if (_disposed)
        {
            //Виклик базового CleanUp() забезпечує коректне очищення ресурсів по всьому порядку спадкування:
            //Administrator.CleanUp() -> Worker.CleanUp() -> Person.CleanUp()
            base.CleanUp(disposing);
            return;
        }

        Console.WriteLine("CleanUp() is called from Administrator:");

        if (disposing)
        {
            Console.WriteLine("Releasing managed resources:");

            if (Employees != null)
            {
                Console.WriteLine($"Releasing {Employees.Count} employees:");

                foreach (var employee in Employees)
                {
                    employee.Dispose();
                    employee.Boss = null;
                }

                Employees.Clear();
                Employees = null;
                Console.WriteLine("Employees list released.");
            }

            if (CoffeeShops != null)
            {
                Console.WriteLine($"Releasing {CoffeeShops.Count} coffee shops:");
                CoffeeShops.Clear();
                CoffeeShops = null;
                Console.WriteLine("Coffee shop list released.");
            }

            Console.WriteLine("All managed resources released.");
        }
        else
        {
            Console.WriteLine("CleanUp() is called from Finalizer.");
        }

        Console.WriteLine("Releasing unmanaged resources:");
        //Відбувається в обох випадках (disposing = true/false)

        if (_hasNativeBuffer && _nativeLogBuffer != IntPtr.Zero)
        {
            Console.WriteLine($"Releasing native memory {_nativeLogBuffer}");
            Marshal.FreeHGlobal(_nativeLogBuffer);
            _nativeLogBuffer = IntPtr.Zero;
            _hasNativeBuffer = false;

            Console.WriteLine("Native memory released.");
        }

        Console.WriteLine("All unmanaged resources released.");

        //Робимо синглтон доступним для звільнення
        _instance = null;

        _disposed = true;
        Console.WriteLine("Administrator.CleanUp() completed.");
    }

    ~Administrator()
    {
    Console.WriteLine("Destructor is called from Administrator.");

    //Не викликаю Dispose(false) тут!
    //Це зробить деструктор базового класу ~Person().
    //Інші деструктори у даному прикладі несуть функцію логування.
    //Порядок: ~Administrator() -> ~Worker() -> ~Person() ->
    //-> Administrator.CleanUp(false) -> Worker.CleanUp(false) -> Person.CleanUp(false).
    }
}