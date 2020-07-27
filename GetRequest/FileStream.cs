using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GetRequest
{
    class FileStream
    {
        public string[] ReadFile()
        {
            return File.ReadAllLines("../../../upstreams_test.log");
        }
    }
}
