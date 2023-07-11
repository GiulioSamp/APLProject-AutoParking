using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            Handler handler = new Handler();
            bool ripeti = true;
            while (ripeti)
            {
                Console.WriteLine("Benvenuto in AutoParking!\n");
                handler.EntryMenu();
                
                string scelta = Console.ReadLine();
                ripeti = (scelta.ToLower() == "s");
            }
            Console.ReadKey();
        }
    }
}
