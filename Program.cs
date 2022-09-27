using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace AddressBook
{
   
    class addressbook
    {
        static SQLiteConnection connection;
        static SQLiteCommand command;
        static public bool Connect(string fileName)
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + fileName + ";Version=3; FailIfMissing = False");
                connection.Open();
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
                return false;
            }
        }


        public string Firstname { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }


        public addressbook() { }
        public void Writeaddressbook()
        {
            using (StreamWriter sw = File.AppendText("addressbook.txt"))
            {
                sw.WriteLine("Имя = "+Firstname);
                sw.WriteLine("Номер телефона 1 = "+Phone);
                sw.WriteLine("Номер телефона 2 = " + Phone2);
            }
        }
       

        public override string ToString()
        {
            return String.Format("{0,-20} {1,-20} {2,-20} ", Firstname, Phone, Phone2);
        }
       
        public static char otvet;
        public static string s;
        static void Main(string[] args)
        {
            if (Connect("addressbook.sqlite"))
            {
                Console.WriteLine("подключено к базе данных!\n");
            }

            string file_name = "addressbook.txt";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine("Запускаем простую записную книжку . .");
            if (System.IO.File.Exists(Convert.ToString(Path.GetFullPath(file_name))) == false)
                Console.WriteLine("Не найден файл addressbook.txt. . он будет создан автоматически . .");
            Console.WriteLine("Готово . .");
            string chislo = null;
            while (chislo != "5")
            {
                do
                {
                    Console.WriteLine("------------МЕНЮ------------");
                    Console.WriteLine("Добро пожаловать в записную книжку!");
                    Console.WriteLine("Где вы хотите сохранить данные?\n 1 --> Сохранить в текстовый файл\n" +
                        " 2 --> Сохранить в базу данных\n 3 --> Вывести на экран\n 4 --> Выход");
                    chislo = Console.ReadLine();
                    switch (chislo)
                    {
                        case "1":
                            
                            addressbook temp = new addressbook();
                            Console.WriteLine("Введите имя: ");
                            temp.Firstname = Console.ReadLine();
                            Console.WriteLine("Введите телефон: ");
                            temp.Phone = Console.ReadLine();
                            Console.WriteLine("Введите второй номер телефона: ");
                            temp.Phone2 = Console.ReadLine();
                            temp.Writeaddressbook();
                            Console.WriteLine("\n Запись добавлена!");
                            break;
                         
                        case "2":
                            Console.WriteLine("Введите имя:");
                            string name = Console.ReadLine();
                            Console.WriteLine("Введите возраст:");
                            string number1 = (Console.ReadLine());
                            Console.WriteLine("Введите возраст:");
                            string number2 = (Console.ReadLine());
                            command = new SQLiteCommand(connection)
                            {
                                CommandText = "CREATE TABLE IF NOT EXISTS [addressbook] ([id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, name VARCHAR (1, 50), number1  VARCHAR (1, 50),  number2 VARCHAR (1, 50))"
                            };
                            command.ExecuteNonQuery();
                            Console.WriteLine("\n Запись добавлена!");
                            command.CommandText = "INSERT INTO addressbook (name, number1, number2) VALUES (:name, :number1, :number2)";
                            command.Parameters.AddWithValue("name", name);
                            command.Parameters.AddWithValue("number1", number1);
                            command.Parameters.AddWithValue("number2", number2);
                            command.ExecuteNonQuery();
                            break;

                            case "3":
                            string[] lines = File.ReadAllLines("addressbook.txt");
                            foreach (string s in lines)
                            {
                                try
                                {
                                    Console.WriteLine(s);
                                }
                                catch 
                                {
                                    Console.WriteLine("Данные отсутствуют!");
                                    
                                }
                                  
                            }
                            break;

                        case "4":
                            Console.WriteLine("До встречи!");
                            Console.ReadKey(); return;
                        default:return;
                                
                    }
                    do
                    {
                        Console.WriteLine("\nПродолжаем? y/n");
                        s = Console.ReadLine();
                        try
                        {
                            otvet = char.Parse(s);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Ошибка при вводе!!! ");
                        }
                    }
                    while (otvet != 'y' && otvet != 'n'); Console.Clear();
                }
                while (otvet == 'y');
                if (otvet == 'n')
                {
                    Console.WriteLine("\n" + "До встречи!"); 
                  break;
                }
            }
            Console.ReadLine();
        }

       
    }

}