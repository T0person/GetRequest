using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace GetRequest
{
    class Realisation
    {
        static AutoResetEvent waitHandler = new AutoResetEvent(true);
        public static void Count(object obj)
        {
            Info file = (Info)obj;

            foreach (var str in file.stream)
            {
                waitHandler.WaitOne();

                // Получаем данные из фаила
                if (str == null || file.SecondAmount == file.amout)
                {
                    waitHandler.Set();
                    break;
                }

                file = GetRegex(str, file, file.UserPath);

                if (file.matchRequest.Success)
                {
                    // Считаю разницу во времени
                    var millesec = (file.Time - file.FirstTime).TotalMilliseconds - (DateTime.Now - file.NowFirstTime).TotalMilliseconds;

                    // Если нужно подождать
                    if (millesec > 0)
                    {
                        Console.WriteLine("\t\tЖду: " + Convert.ToInt32(millesec) + " миллисекунд!!!");
                        Thread.Sleep(Convert.ToInt32(millesec));
                    }

                    // Отправляю запрос
                    try
                    {
                        WebRequest webRequest = WebRequest.Create("https://" + file.matchOut + file.matchPath);

                        // Получание ответа. Если это включено, нужно запустить сервер
                        //WebResponse response = webRequest.GetResponse();
                        Console.WriteLine("Запрос отправлен" /*webRequest*/ + /*"\tОтветка:" + response +*/ "\tВремя: " + DateTime.Now + "\tПуть: " + "https://" + file.matchOut + file.matchPath);
                        file.SecondAmount++;
                    }
                    catch (Exception) { waitHandler.Set(); }

                    waitHandler.Set();
                }
                else
                    waitHandler.Set();
            }
        }

        public static List<Thread> CreateThreads()
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(Count));
                thread.Name = $"{i}. Поток";
                threads.Add(thread);
            }
            return threads;
        }

        public static Info GetRegex(string FileString, Info file, string UserPath)
        {
            // Выборка времени в фаиле и получаем результат
            Regex regexTime = new Regex(@"\d{2}/\w{3}/\d{4}:\d{2}:\d{2}:\d{2}\b");
            Match matchTime = regexTime.Match(FileString);
            file.Time = GetTime(matchTime);
            if (file.FirstTime == DateTime.MinValue)
                file.FirstTime = file.Time;

            // Получаем наше начальное время
            if (file.NowFirstTime == DateTime.MinValue)
                file.NowFirstTime = DateTime.Now;

            // Убираем излишки строки (чтобы "Путь" корректно нашелся)
            FileString = FileString.Replace(matchTime.Value, "");

            // Выборка localhost и получение результата
            //Regex regexOut = new Regex(@"\b(\d+\.\d+\.\d+\.\d+:\d+)\b");
            Regex regexOut = new Regex(UserPath);
            file.matchOut = regexOut.Match(FileString);

            // Выборка типа запроса и получаем результат
            Regex regexRequest = new Regex(@"\b(GET)\b");
            file.matchRequest = regexRequest.Match(FileString);

            // Выборка пути и получаение результата
            Regex regexPath = new Regex(@"(/\S+)+\b");
            file.matchPath = regexPath.Match(FileString);

            return file;
        }

        public static DateTime GetTime(Match match)
        {
            DateTime Time;

            // Ищем первое ":" 
            int Index = match.Value.IndexOf(":");

            // Разбиваем на массив char-ов
            char[] arr = match.Value.ToCharArray();

            // Замена ":"
            arr[Index] = ' ';

            // Обратно в строку
            string str = new string(arr);

            // Перобразуем в дату
            DateTime.TryParse(str, out Time);

            return Time;
        }
    }
}
