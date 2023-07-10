using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace ParkingClient
{
    internal class Validation 
    {


        #region public

        public static string CheckInput(string messaggio, Func<string, bool> validazione)
        {
            while (true)
            {
                Console.Write(messaggio);
                string input = Console.ReadLine();

                if (validazione(input))
                {
                    return input;
                }

                Console.WriteLine("Input non valido. Riprova.");
            }

        }

    }
    #endregion
}



