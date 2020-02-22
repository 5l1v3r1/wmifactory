using System;
using wmibot.Factories;
using wmibot.Models;

namespace wmibot
{
    class Program
    {
        static void Main(string[] args)
        {
            WmiObjectFactory factory = new WmiObjectFactory("cache.db");
            PCInfo info = factory.Create<PCInfo>();
            Console.WriteLine("Processor type:\t\t{0}", info.ProcessorName);
            Console.WriteLine("BIOS Manufactorer:\t{0}", info.BIOSManufacturer);
            Console.WriteLine("Chip architecture:\t{0}", info.Architecture);
            Console.ReadLine();
        }
    }
}
