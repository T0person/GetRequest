using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading;

namespace GetRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Вводим IP
            Console.Write("Введие IP: ");
            string UserPath = Console.ReadLine();
            uint UserAmout;

            // Вводим количество запросов
            while (true)
            {
                try
                {
                    Console.Write("Введите количество запросов: ");
                    UserAmout = UInt32.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Не правильный ввод!!!");
                }
            }

            // Отркрываю фаил на чтение
            FileStream f = new FileStream();
            string[] lines = f.ReadFile();

            // Создаю потоки
            List<Thread> threads = Realisation.CreateThreads();

            Info file = new Info();

            // Заполняю класс настоящее время, старое и фаил
            file.NowFirstTime = DateTime.MinValue;
            file.FirstTime = DateTime.MinValue;
            file.stream = lines;
            file.UserPath = UserPath;
            file.amout = UserAmout;
            file.SecondAmount = 0;

            // Запускаю потоки
            for (int i = 0; i < 5; i++)
                threads[i].Start(file);
        }
    }
}
