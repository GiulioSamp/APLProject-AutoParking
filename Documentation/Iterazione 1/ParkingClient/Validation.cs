using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace ParkingClient
{
    internal class Validation 
    {
        /// <summary>
        /// mi conviene fare una class ServerCommunication ????.-.
        /// </summary>
        #region private 

       private const string ServerUrl = "http://localhost:18080"; //opp mettere direttamete 
        #endregion

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

        /*public static void SendUserDataToServer(string email, string endpoint)
        {
            // struttura dati fornita dal framework .NET per crearmi ogg key val
            var data = new Dictionary<string, string>
            {
                 { "email", email }
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ServerUrl);

                //Invia i dati al server
                var response = client.PostAsync(endpoint, new FormUrlEncodedContent(data)).Result;

                //printo la risposta del server
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("OKI dati utente inviati al server con succ");
                }
                else
                {
                    Console.WriteLine("AIU errore durante l'invio dei dati emailpass al server.");
                }
            }
        }*/ 

       

    }
    #endregion
}



