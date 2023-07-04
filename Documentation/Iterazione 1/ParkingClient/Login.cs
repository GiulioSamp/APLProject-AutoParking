using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ParkingClient
{
    public class Login
    {
        public string Email { get; set; }
        public string Pass { get; set; }

        public Login(string email, string password)
        {
            Email = email;
            Pass = password;
        }

        public Login()
        {
        }


        public bool DoLogin()
        {
            Console.WriteLine("Benvenuto! Effettua il login.");
            bool accessoConcesso = false;
            do
            {
                  Email = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
                });  
                
                Pass= Validation.CheckInput("Inserisci la password: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
                //get consigliati d angio vedere meglio che fann
                accessoConcesso = CheckLogin(Email, Pass).GetAwaiter().GetResult();

                if (accessoConcesso)
                {
                    Console.WriteLine("Accesso consentito. Benvenuto!");
                        return true;
                }
                else
                {
                    Console.WriteLine("Accesso negato. Riprova.");              
                }
            } while (!accessoConcesso);
            return false;
        }

        public async Task<bool> CheckLogin(string email, string password)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                var requestData = new { Email = email, Pass = password };

                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:18080/login", content);
                //response.StatusCode.ToString().Contains("200")
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
            {
                Console.WriteLine("Si è verificato un errore durante la verifica dell'accesso: " + ex.Message);
                return false;
            }
        }
    }
}





