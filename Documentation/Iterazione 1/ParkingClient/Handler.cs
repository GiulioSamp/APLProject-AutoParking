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
using System.Drawing;
using System.Runtime.ConstrainedExecution;
//add pacchetto nuget nel progetto da ide
namespace ParkingClient
{
    public class Handler
    {

        ///   /// <summary>
        /// *Singleton**
        /// </summary>
        private static Handler instance;
        private UserManager utente { get; set; }
        private VehicleManager veicolo { get; set; }
        List<VehicleManager> veicoli { get; set; }

        /// <summary>
        /// unico costruttore privato per pattern singleton
        /// </summary>
        private Handler()
        {
            // costruttore privato per evitare l'istanziazione diretta della classe, se serve solo un istanza di loro fare singleton anche li
            utente = new UserManager();
            veicolo = new VehicleManager();
        }
        public static Handler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Handler();
                }
                return instance;
            }
        }

        #region public

        /*public static HttpResponseMessage InviaDatiAlServer<T>(T oggetto, string endpoint, string email = null)
        {
           var dtoJson = JsonConvert.SerializeObject(oggetto);
           if (email != null)
            {
                dtoJson = dtoJson.Replace("}", ",");
                dtoJson = dtoJson + "\"Email\":\"" + email + "\"}";
               // Console.WriteLine(dtoJson);

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
        }*/
        public static HttpResponseMessage InviaDatiAlServer<T>(string endpoint, string email = null, T oggetto = default(T))
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
            } throw new ArgumentNullException("Errore carattere inserito");
        }

        public void EntryMenu()
        {
            //utente = new UserManager();
            //veicolo = new VehicleManager();
            while (true)
            {
                try
                {
                    bool utenteRegistrato = false;
                    Console.WriteLine("1. Registrazione");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3. Ritiro veicolo");
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
                                InviaDatiAlServer("/register",oggetto:utente);

                                Console.Write("Utente registrato, procediamo con la registrazione del veicolo\n");
                                veicolo.EnterVehicle(utente);
                                InviaDatiAlServer( "/vehicle", utente.Email,veicolo);

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
                        case 3:
                            
                            break;
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
                try
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
                            RetrieveVehicleList();
                            Console.WriteLine("Seleziona numero veicolo da parcheggiare: ");
                            //int.TryParse(Console.ReadLine(), out int i);
                            //Console.WriteLine(i);
                            //int s = i - 1;
                            //Console.WriteLine(s);
                            //veicolo = veicoli[s];
                            int.TryParse(Console.ReadLine(), out int inputI);
                            if (inputI >= 1 && inputI <= veicoli.Count)
                            {
                                int selectedI = inputI - 1;
                                veicolo = veicoli[selectedI];
                                Console.WriteLine("Hai selezionato il veicolo con targa: " + veicolo.Targa);
                                StartParking();
                            }
                            else
                            {
                                Console.WriteLine("Scelta non valido.");
                            }
                          break;
                        case "2":
                            veicolo.EnterVehicle(utente);
                            InviaDatiAlServer("/vehicle", utente.Email, veicolo);
                            break;
                        case "3":
                            veicolo.ModifyVehicle(utente);
                            InviaDatiAlServer("/vehicle", null, veicolo);
                            break;
                        case "4":
                            utente.EditUser();
                            InviaDatiAlServer("/register", null, utente);
                            break;
                        case "5":
                            //ViewParameters(utente);
                            RetrieveUser();
                            break;
                        case "6":
                            //ViewParameters(veicolo);
                            RetrieveVehicleList();
                            break;
                        case "7":
                            Console.WriteLine("Menù terminato.");
                            return;
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
        private bool StartParking()
        {
            //var Email = utente.Email;
            //var Targa = veicolo.Targa;
            var requestData = new { Targa = veicolo.Targa };
            var response = InviaDatiAlServer("/park", utente.Email, requestData);
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
            //bool accessoConcesso = false;

            utente.Email = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
            }).ToLower();

            utente.Pass = Validation.CheckInput("Inserisci la password: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });          
            var requestData = new { Email = utente.Email, Pass = utente.Pass };
            var response = InviaDatiAlServer("/login",oggetto:requestData);
            Console.WriteLine("Accesso consentito. Benvenuto!");
            Console.Write("\nEcco i tuoi dati account:\n");
            //Email = utente.Email;
             RetrieveUser();
             RetrieveVehicleList();
            return true;

        }

        private void RetrieveUser()
        {
                       
            var response = InviaDatiAlServer<object>("/ruser", utente.Email);
            string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            //Console.WriteLine(responseContent); 
            dynamic utenteDeserializzato = JsonConvert.DeserializeObject(responseContent);             
                utente.Nome = utenteDeserializzato.Nome;
                utente.Cognome = utenteDeserializzato.Cognome;
                utente.Email = utenteDeserializzato.Email;
                utente.Telefono = utenteDeserializzato.Telefono;
                Console.WriteLine($"Nome: {utente.Nome}");
                Console.WriteLine($"Cognome: {utente.Cognome}");
                Console.WriteLine($"Email: {utente.Email}");
                Console.WriteLine($"Telefono: {utente.Telefono}");          
        }
        


        private void RetrieveVehicleList() {
            
            var response = InviaDatiAlServer<object>("/rvehicle", utente.Email);
            string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            veicoli = new (JsonConvert.DeserializeObject<List<VehicleManager>>(responseContent));
            int numv = 1;
            foreach (VehicleManager veicolo in veicoli)
            {
            Console.WriteLine($"Veicolo {numv}: Targa: {veicolo.Targa}, Marca: {veicolo.Marca}, Modello: {veicolo.Modello}, Anno: {veicolo.Anno}");
                numv++;
            }
        }
    }
        #endregion
        ///ruser <summary>
        /// ruser
        /// </summary>



       /* private void RitiroV()
        {
            Console.WriteLine("Benvenuto!Grazie per aver sostato");
            bool accessoConcesso = false;

            var Email = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
            }).ToLower();

            var IDTicket = Validation.CheckInput("Inserisci IDTicket: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });

            var requestData = new { Email = Email, IDTicket= IDTicket };
            var response = InviaDatiAlServer("/nomeednpoint",null, requestData);
            Console.WriteLine("Arrivederci!");
            

        }*/

    
}

