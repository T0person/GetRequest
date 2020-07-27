using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GetRequest
{
    public class Info
    {
        public string[] stream;
        // Сегодняшнее начальное время, само начальное время и просто время
        public DateTime NowFirstTime, FirstTime, Time;
        // Запрос и путь
        public Match matchRequest, matchOut, matchPath;
        // Количество запросов
        public uint amout, SecondAmount;
        // Прописанный IP
        public string UserPath;
    }
}
