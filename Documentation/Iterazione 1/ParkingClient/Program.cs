using ParkingClient;
using System;

namespace ParkingClient
{
    internal class Program
    { 
        
        /*public void IniziaParcheggio()
        {
            System.out.println("Benvenuto in AutoParkinginizio");
            //avvia menu scelte

        }*/

        static void Main(string[] args)
        {
            InserimentoDatiHandler inserimentoDati = new InserimentoDatiHandler();

            bool ripeti = true;
            while (ripeti)
            {
                Console.WriteLine("Benvenuto in AutoParking!\n");
                //inserimentoDati.StartClient();

                //  string tipo = Console.ReadLine();
                //tolto tipo da gestire come parm di risp che passavo al metod GestisciInserimentoDati(string tipo)
                //sotto  inserimentoDati.GestisciInserimentoDati(tipo);
                inserimentoDati.GestisciInserimentoDati();
               
                Console.WriteLine("Vuoi fare un'altra operazione? (s/n)");
                string scelta = Console.ReadLine();

                ripeti = (scelta.ToLower() == "s");
            }

            Console.WriteLine("Premi un tasto per uscire...");
            //
            // Chiamata al metodo di CaricaEdInviaDatiAlServer da mettere qui 

            Console.ReadKey();
        }

        }
}