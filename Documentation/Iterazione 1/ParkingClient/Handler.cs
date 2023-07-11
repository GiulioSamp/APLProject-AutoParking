using System;
using System.Text;
using ParkingClient;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Authentication;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
//add pacchetto nuget nel progetto da ide
namespace ParkingClient
{
    public class Handler
    {

        private UserManager utente { get; set; }
        private VehicleManager veicolo { get; set; }

        #region public

        public static HttpResponseMessage InviaDatiAlServer<T>(T oggetto, string endpoint, string email = null)
        {
           var dtoJson = JsonConvert.SerializeObject(oggetto);
           if (email != null)
            {
                dtoJson = dtoJson.Replace("}", ",");
                dtoJson = dtoJson + "\"Email\":\"" + email + "\"}";
                Console.WriteLine(dtoJson);

            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:18080");
                var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, content).GetAwaiter().GetResult(); ;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response;
                }
                else
                {
                    throw new HttpRequestException($"Codice: {(int)response.StatusCode}, Motivo: {response.ReasonPhrase}");
                }
            }
        }

        public void EntryMenu()
        {
            utente = new UserManager();
            veicolo = new VehicleManager();
            while (true)
            {
                try
                {
                    bool utenteRegistrato = false;
                    Console.WriteLine("1. Registrazione");
                    Console.WriteLine("2. Login");
                    //Console.WriteLine("3. Esci");
                    Console.Write("\nDigita il numero dell'operazione desiderata tra quelle elencate sopra: ");
                    int.TryParse(Console.ReadLine(), out int sceltaMenu);
                    switch (sceltaMenu)
                    {
                        case 1:
                            if (utenteRegistrato)
                            {
                                Console.WriteLine("Utente già registrato."); 
                            }
                            else
                            {
                                utente.UserRegistration();
                                InviaDatiAlServer(utente, "/register");

                                Console.Write("Utente registrato, procediamo con la registrazione del veicolo\n");
                                veicolo.EnterVehicle(utente);
                                InviaDatiAlServer(veicolo, "/vehicle", utente.Email);

                                Console.Write("\nHai inserito i seguenti dati:\n");
                                ViewParameters(utente);
                                ViewParameters(veicolo);
                                utenteRegistrato = true; //?????????????
                                InternalMenu();
                            }
                            break;

                        case 2:
                            if (DoLogin())
                            {
                                InternalMenu();
                                utenteRegistrato = true;
                            }
                            break;

                       /* case 3:

                            Console.WriteLine("Menù terminato.");
                            return; // Terminare? 
                            */

                        default:
                            Console.WriteLine("Scelta non valida. Riprova.");
                            break;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Errore durante l'invio dei dati al server: " + ex.Message);

                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine(ex.Message + " Login negato, Riprova.");
                }
            }
        }


        #endregion

        #region private 
        private void InternalMenu()
        {
            while (true)
            {
                Console.Write($"\nBenvenuto/a!Seleziona un'opzione:");
                // Stampa tutte le opzioni del menu solo se il parcheggio non è stato avviato
                Console.WriteLine("\n1. Inizia parcheggio");
                Console.WriteLine("2. Aggiungi nuovo veicolo");
                Console.WriteLine("3. Modifica dati veicolo");
                Console.WriteLine("4. Modifica dati utente");
                Console.WriteLine("5. Visualizza dati utente");
                Console.WriteLine("6. Visualizza dati veicoli");
                Console.WriteLine("7. Logout");
                //da c# v8 switch con le espressioni 
                switch (Console.ReadLine())
                {
                    case "1":
                        StartParking();
                        break;
                    case "2":
                        //veicolo.AddNewVehicle();
                        break;
                    case "3":
                        veicolo.ModifyVehicle(utente);
                        InviaDatiAlServer(veicolo, "/vehicle");
                        break;
                    case "4":
                        utente.EditUser();
                        InviaDatiAlServer(utente, "/register");
                        break;
                    case "5":
                        ViewParameters(utente);
                        break;
                    case "6":
                        ViewParameters(veicolo);
                        break;
                    case "7":
                        Console.WriteLine("Menù terminato.");
                        //goto Ext:
                        //Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Scelta non valida. Riprova.");
                        break;
                }
            }
        }
        private void ViewParameters<T>(T oggetto)
        {
            Type tipoOggetto = typeof(T);
            PropertyInfo[] proprieta = tipoOggetto.GetProperties();

            foreach (PropertyInfo prop in proprieta)
            {
                string nomeProprieta = prop.Name;
                try
                {
                    object valoreProprietà = prop.GetValue(oggetto);
                    Console.WriteLine($"{nomeProprieta}: {valoreProprietà}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore durante la lettura della proprietà {nomeProprieta}: {ex.Message}");
                }
            }

        }
    
    #endregion

        private bool StartParking()
        {
            var Email = utente.Email;
            var Targa = veicolo.Targa;
            var requestData = new { Targa = Targa };
            var response= InviaDatiAlServer(requestData, "/park", Email);
            string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            string[] parts = responseContent.Split(':');

            if (parts.Length >= 5)
            {
               string user = parts[1].Split(' ')[1].Trim();
               string parkedCar = parts[2].Split(' ')[1].Trim();
               string floor = parts[3].Split(' ')[1].Trim();
               string spot = parts[4].Split(' ')[1].Trim();

               Console.WriteLine($"Parcheggio effettuato per: {user} Targa: {parkedCar} Piano: {floor} Posto: {spot}");

               Console.WriteLine("Parcheggio completato, grazie");
               return true;
            }
            else
            {
              return false;
              throw new Exception("Errore nel formaato della string di risposta dal server");
             }
                               
         }
        private bool DoLogin()
         {
            Console.WriteLine("Benvenuto! Effettua il login.");
            bool accessoConcesso = false;

                var Email = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
                }).ToLower();

                var Pass = Validation.CheckInput("Inserisci la password: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                var requestData = new { Email = Email, Pass = Pass };
                var response = InviaDatiAlServer(requestData, "/login");
                Console.WriteLine("Accesso consentito. Benvenuto!");
                return true;

         }
    }
}

