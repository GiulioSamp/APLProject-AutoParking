using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    internal class ParkingManager
    {
        public string Email { get; private set; }
        public string Targa { get; private set; }

        public async Task<string> SendDataToServer()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var requestData = new { Email = Email, Targa = Targa };
                    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("http://localhost:18080/park", content);
                    //Console.WriteLine(response);
                    if (response.IsSuccessStatusCode)
                    {
                        // Ricevi la risposta dal server???
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        //Console.WriteLine("----Risposta JSON ricevuta dal server:-----------");
                        //Console.WriteLine(jsonResponse);
                  
                        return jsonResponse;
                    }
                    else
                    {
                        return "Failure: Error during data sending to server.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendDataToServer errore durante l'invio dei dati al server: " + ex.Message);
                return "Fail";
            }
        }

        public async Task<string> StartParking(UserManager utente, VehicleManager veicolo)
        {
            Email = utente.Email;
            Targa = veicolo.Targa;

            try
            {
                var response = await SendDataToServer();
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("StartParking errore durante l'invio dei dati al server: " + ex.Message);
                return "Fail2 su start.";
            }
        }
    }
}