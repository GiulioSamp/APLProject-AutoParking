using ParkingClient;
using System;

namespace ParkingClient
{
    internal class Program
    { 
        

        static void Main(string[] args)
        {
            Handler inserimentoDati = new Handler();

            bool ripeti = true;
            while (ripeti)
            {
                Console.WriteLine("Benvenuto in AutoParking!\n");
                
                inserimentoDati.EntryMenu();
               
                //Console.WriteLine("Vuoi fare un'altra operazione? (s/n)");
                string scelta = Console.ReadLine();

                ripeti = (scelta.ToLower() == "s");
            }
            Console.ReadKey();
        }

        }
}