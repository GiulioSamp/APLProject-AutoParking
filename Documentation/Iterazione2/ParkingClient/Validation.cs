using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;

namespace ParkingClient
{
    internal static class Validation 
    {

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

        public static HttpResponseMessage SendDataToServer<T>(string endpoint, string email = null, T oggetto = default(T))
        {
            string dtoJson = null;

            if (oggetto != null)
            {
                dtoJson = JsonConvert.SerializeObject(oggetto);
            }

            if (email != null)
            {
                if (dtoJson == null)
                {
                    // Crea un oggetto anonimo solo con l'email
                    var emailObject = new { Email = email };
                    dtoJson = JsonConvert.SerializeObject(emailObject);
                }
                else
                {
                    dtoJson = dtoJson.Replace("}", ",");
                    dtoJson += $"\"Email\":\"{email}\"}}";
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:18080");
                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, content).GetAwaiter().GetResult();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response;
                }
                else
                {
                    throw new HttpRequestException($"Codice: {(int)response.StatusCode}, Motivo: {response.ReasonPhrase}," +
                        $" Motivo: {response.Content.ReadAsStringAsync().GetAwaiter().GetResult()}");
                }
            }
            throw new ArgumentNullException("Errore carattere inserito");
        }
    }
    
}



