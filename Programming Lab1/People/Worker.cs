namespace Programming_Lab1.People;

internal abstract class Worker : Person
{
    private bool _disposed = false; 

    private static int _totalWorkers;
    public static int GetTotalWorkers(Administrator? administrator) => administrator == null ? -1 : _totalWorkers;

    static Worker()
    {
        _totalWorkers = 0;
        Console.WriteLine("Static workers counter is created.");
    }

    protected Worker(string name, DateTime birthDate) : base(name, birthDate)
    {
        if (Age < 18)
            throw new ArgumentException("Person must be at least 18 years old to work.");

        Console.Write("Person is a worker. ");
        _totalWorkers++;
    }

    protected override void CleanUp(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Console.WriteLine("CleanUp() is called from Worker.");
            }
            _disposed = true;
        }
        base.CleanUp(disposing);
    }

    ~Worker()
    {
        Console.WriteLine("Destructor is called from Worker.");
    }
}
