using Programming_Lab1.Items.Orders;
using Programming_Lab1.Items.Drinks;
using Programming_Lab1.People;
using Programming_Lab1.Shops;

namespace Programming_Lab1
{
    internal class Program
    {
        private static void Main()
        {
            //КОНСТРУКТОРИ

            //Приватний конструктор. приклад - Адміністратор (Singleton).

            //1) неможливо створити екземпляр класу ззовні:
            //var admin = new Administrator("Polina", new DateTime(2006, 01, 17));

            //2) конструктор викликається через статичний метод класу:
            Administrator.Register("Polina", new DateTime(2006, 01, 17));
            Console.ReadKey();

            //3) статичне поле зберігає єдиний екземпляр класу:
            var admin = Administrator.GetAccess();
            Console.ReadKey();

            //4) повторна реєстрація не створює новий екземпляр:
            /*Administrator.Register("Alex", new DateTime(2006, 12, 17));
            var admin1 = Administrator.GetAccess();
            Console.WriteLine(ReferenceEquals(admin1, admin));
            Console.ReadKey();
            */

            //5) якщо не виконано умов доступу, повертається null:
            /*admin?.ShowInfo();
            Console.ReadKey();
            */

            //Параметризований конструктор (не можна створити екземпляр без параметрів).
            //приклад - Бариста:
            var barista1 = new Barista("Anna", new DateTime(2005, 11, 5));

            //Базовий конструктор (викликається, якщо не надано параметрів). 
            //приклад - Відвідувач:
            var customer1 = new Customer();

            //Статичний конструктор (ініціалізує статичні поля, викликається один раз при першому зверненні до класу).
            //приклад - Кава, Робітник, Відвідувач:
            Console.WriteLine($"Total customers: {Customer.GetTotalCustomers(admin)}");
            Console.WriteLine($"Total espresso portions made: {Coffee.GetTotalEspPortions(admin)}");
            Console.WriteLine($"Total workers: {Worker.GetTotalWorkers(admin)}");
            Console.ReadKey();

            //ВІРТУАЛЬНІ ТА ПЕРЕВИЗНАЧЕНІ МЕТОДИ

            //приклад 1) ShowInfo():
            admin!.ShowInfo();
            Console.ReadKey();

            barista1.ShowInfo();
            Console.ReadKey();

            customer1.ShowInfo();
            Console.ReadKey();

            //приклад 2) CountPrice():
            var coffee = new Coffee(new CoffeeOrder());
            Console.WriteLine($"Price for the basic coffee:{coffee.CountPrice()}$.");
            Console.ReadKey();

            var tea = new Tea(new TeaOrder());
            Console.WriteLine($"Price for the basic tea:{tea.CountPrice()}$.");
            Console.ReadKey();

            //приклад 3) CleanUp() з GC

            //IDISPOSABLE, ДЕСТРУКТОРИ, GC
            Console.WriteLine("============GC PRACTICE===============");

            //приклад 1) з using:
            using (var bTest = new Barista("A", default))
            {
                Console.WriteLine("\nUsing barista in a using statement.");
                bTest.ShowInfo();
            } 
            Console.ReadKey();
            

            //приклад 2) Ситуація примусового виклику очищення сміття:
           Console.WriteLine($"{GC.GetTotalMemory(false)}");

            CollectTrigger();
            Console.ReadLine();
            //Об'єкти створені в циклі втрачають посилання і стають недосяжними.
            //GC визначає їх як сміття, розподіляє по поколіннях залежно від розміру і планує виклик деструкторів.
            //Деструктори виконуються в іншому потоці при загрозі вичерпання пам'яті.
            //Якщо не використати метод очікування завершення деструкторів,
            //виконання програми продовжиться попри те, що деструктори ще не відпрацювали.
           
            //приклад 3) Адміністратор з керованими та некерованими ресурсами.
            Console.WriteLine($"Memory before 1 Collect: {GC.GetTotalMemory(false):N0} bytes");

            //збираємо сміття перед створенням нових об'єктів для наочності:
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"Memory after collecting, before Dispose: {GC.GetTotalMemory(false):N0} bytes");
            ShowColCount();

            //створюємо об'єкти:
            var shop1 = new CoffeeShop(admin, "Coffee 24/7");
            var shop2 = new CoffeeShop(admin, "CoffeeBreak");
            admin.Hire(new Barista("Test", new DateTime(2002, 2, 1)));
            admin.Hire(new Barista("Test2", new DateTime(2002, 2, 1)));

            //admin пережив збирання сміття, бо ще reachable:
            Console.WriteLine($"admin is in GC gen {GC.GetGeneration(admin)}.");
            Console.WriteLine($"Memory before Dispose: {GC.GetTotalMemory(false):N0} bytes");
            ShowColCount();

            //явно викликаємо Dispose
            admin.Dispose();
            //CleanUp() усунув посилання на складні керовані ресурси та закрив некеровані ресурси.
            //GC.SuppressFinalize потрібен, щоб деструктор не намагався знову звільнити ресурси,
            //які вже були звільнені в Dispose(). Так як він працює у паралельному потоці, 
            //може виникнути ситуація, коли деструктор і Dispose()
            //одночасно намагаються звільнити одні й ті ж ресурси.

            //Об'єкт ще існує після Dispose(), але ресурси звільнені:
            admin.ShowInfo();
            //admin.Employees[1].ShowInfo();

            Console.WriteLine($"\nMemory after Dispose: {GC.GetTotalMemory(false):N0} bytes");
            //Збирання unreachable об'єктів ще не відбулося - вони "живі", тому пам'ять купи не змінилася.
            ShowColCount();
            Console.ReadKey();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"\nMemory after 2 Collect: {GC.GetTotalMemory(false):N0} bytes");
            ShowColCount(); 
            Console.ReadKey();
            //Тут раніше звільнена пам'ять була зібрана.

            //Намагаємося спровокувати деструктор, знищуємо посилання на об'єкт:
            shop1 = null;
            shop2 = null;
            barista1 = null;
            Administrator.UnregisterInstance();

            //перереєструємо адміна для фіналізації, спробуємо зібрати як сміття:
            GC.ReRegisterForFinalize(admin);
            admin = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine($"\nMemory after 3 Collect: {GC.GetTotalMemory(false):N0} bytes");
            ShowColCount();
            Console.ReadKey();

            //admin.ShowInfo

            //Висновок: об'єкт admin вже unreachable, але деструктор не викликається (немає логу),
            //хоч і був перереєстрований для фіналізації.
            //Причина в тому, що Dispose вже звільнив усі ресурси попередньо - потреби примусово викликати
            //Collect і фіналізацію немає. 
        }

        private static void ShowColCount()
        {
            Console.WriteLine("Gens 0, 1, 2 obj were collected: ");
            Console.Write(GC.CollectionCount(0) + ", "
                                                + GC.CollectionCount(1) + ", "
                                                + GC.CollectionCount(2) + " times\n");
        }

        private static void CollectTrigger()
        {
            for (int i = 0; i < 100; i++)
            {
                ShowColCount();
                //Чекаємо завершення деструкторів перед наступною ітерацією:
                GC.WaitForPendingFinalizers();

                Object obj = new byte[850000]; // ~85 MB
                GC.WaitForPendingFinalizers();
                Console.WriteLine(GC.GetGeneration(obj));

                var newTest = new Barista("Almond", default);
                GC.WaitForPendingFinalizers();
                var newTest1 = new Barista("Gorge", default);
                GC.WaitForPendingFinalizers();

                //Стримуємо виклик деструктора - сміття лишається в купі:
                GC.SuppressFinalize(newTest1);

                Console.WriteLine($"Heap memory: {GC.GetTotalMemory(false):N0} bytes.");
            }
            ShowColCount();
        }
    }
}