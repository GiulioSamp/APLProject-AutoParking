﻿using System;
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
           Menu menu= new Menu();
            
            while (true)
            {
                Console.WriteLine("Benvenuto in AutoParking!\n");
                menu.EntryMenu();
                
            }
            Console.ReadKey();
        }
    }
}
