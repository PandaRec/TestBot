
using System.Collections.Generic;

namespace ParseSites
{
    class Program
    {
        static void Main(string[] args)
        {
            string apikey = "5c22c518-ebcf-4750-8524-872e96677344";
            string address = "";
            List<string> pos = Yandex.Yandex.GetPos(apikey, address);

        }
    }
}
