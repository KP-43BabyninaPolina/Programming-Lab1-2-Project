namespace Programming_Lab1.People;

internal abstract class Person : IDisposable
{
    private bool _disposed = false;
    protected string Name { get; init; }

    private readonly DateTime _birthDate;

    protected DateTime BirthDate
    {
        get => _birthDate;
        init
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Birth date can't be in the future.");

            if(value != default && value < new DateTime(1900, 1, 1))
                throw new ArgumentException("Birth date can't be earlier than January 1, 1900.");

            _birthDate = value;
        }
    }

    protected int Age
    {
        get
        {
            var today = DateTime.UtcNow;
            var age = today.Year - _birthDate.Year;

            if (_birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }

    static Person()
    {
        Console.WriteLine("Static person constructor called.");
    }

    protected Person()
    {
        Name = "Unknown";
        BirthDate = default;
        Console.WriteLine("Unknown person created.");
    }

    protected Person(string name, DateTime birthDate)
    {
        Name = name;
        BirthDate = birthDate;
        Console.WriteLine("Person created.");
    }

    public virtual void ShowInfo()
    {
        var message1 = Name == "Unknown" || string.IsNullOrEmpty(Name)
            ? "This person's name is unknown. "
            : $"This person's name is {Name}. ";
            Console.WriteLine(message1);

        var message2 = BirthDate == default
            ? "Their birthdate is unknown.\n "
            : $"Their age is {Age}. ";
            Console.Write(message2);

    }

    public override string ToString()
    {
        return Name;
    }

    public void Dispose()
    {
        Console.WriteLine("Dispose() is called from Person.");
        CleanUp(true);
        GC.SuppressFinalize(this);
        Console.WriteLine("GC.SuppressFinalize is called from Person.");
    }

    protected virtual void CleanUp(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //Базові абстрактні класи Person та Worker не мають ресурсів для звільнення,
                //але CleanUp у класах-предках необхідний для коректної роботи у класах-нащадках.
                Console.WriteLine("CleanUp() is called from Person");
            }
            _disposed = true;
        }
    }

    ~Person()
    {
        CleanUp(false);
        Console.WriteLine($"Destructor is called from {this}.");
        Console.WriteLine("Working in thread {0}", Environment.CurrentManagedThreadId);
        Thread.Sleep(500);
        Console.WriteLine("Person destroyed.");
    }
}