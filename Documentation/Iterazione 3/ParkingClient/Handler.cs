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

        #region private
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
            var requestData = new { Targa = veicolo.Targa };
            var response = Validation.SendDataToServer("/park", utente.Email, requestData);
            string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            
            if (!string.IsNullOrEmpty(responseContent))
            {
                Console.WriteLine(responseContent);
                Console.WriteLine("Parcheggio completato, grazie");
                return true;
            }
            else
            {
                return false;
                throw new Exception("Errore nel formaato della string di risposta dal server");
            }

        }
        #endregion

        #region public
        public void Register()
        {
            utente.UserRegistration();

            Console.Write("Utente registrato, procediamo con la registrazione del veicolo\n");
            veicolo=veicolo.EnterVehicle(utente);
            //Console.WriteLine(veicolo.ToString());
            Validation.SendDataToServer("/register", oggetto: utente);
            Validation.SendDataToServer("/vehicle", utente.Email, veicolo);

            Console.Write("\nHai inserito i seguenti dati:\n");
            ViewParameters(utente);
            ViewParameters(veicolo);
        }
        public bool DoLogin()
        {
            Console.WriteLine("Benvenuto! Effettua il login.");
  
            utente.Email = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
            }).ToLower();

            utente.Pass = Validation.CheckInput("Inserisci la password: ", input =>
            {
                return !string.IsNullOrWhiteSpace(input);
            });
            var requestData = new { Email = utente.Email, Pass = utente.Pass };
            var response = Validation.SendDataToServer("/login", oggetto: requestData);
            Console.WriteLine("Accesso consentito. Benvenuto!");
            Console.Write("\nEcco i tuoi dati account:\n");
            //Email = utente.Email;
            RetrieveUser();
            RetrieveVehicleList();
            return true;

        }

        public void RetrieveUser()
        {

            var response = Validation.SendDataToServer<object>("/ruser", utente.Email);
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

        public void RetrieveVehicleList() {

            var response = Validation.SendDataToServer<object>("/rvehicle", utente.Email);
            string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            veicoli = new(JsonConvert.DeserializeObject<List<VehicleManager>>(responseContent));
            int numv = 1;
            foreach (VehicleManager veicolo in veicoli)
            {
                Console.WriteLine($"Veicolo {numv}: Targa: {veicolo.Targa}, Marca: {veicolo.Marca}, Modello: {veicolo.Modello}, Anno: {veicolo.Anno}");
                numv++;
            }
        }

        public void ChoiceVehicleListForPark() {
            RetrieveVehicleList();
            Console.WriteLine("Seleziona numero veicolo da parcheggiare: ");
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
        }
        public void RegisterVehicle(){
            veicolo = veicolo.EnterVehicle(utente);
            Validation.SendDataToServer("/vehicle", utente.Email, veicolo);
    }
        public void ModdVehicle()
        {
            veicolo.ModifyVehicle(utente);
            Validation.SendDataToServer("/vehicle", null, veicolo);
        }
        public void ModUser() 
        {
            utente.EditUser();
            Validation.SendDataToServer("/register", null, utente);
        }
        public void Takevehicle()
        {
            Console.WriteLine("Benvenuto! Grazie per aver sostato");
            string Id = Validation.CheckInput("Inserisci IDTicket: ", input =>
            {
                return input.All(char.IsDigit);
            });

            var requestData = new { Id = Id };
            var response = Validation.SendDataToServer("/pay", oggetto: requestData);            
            string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            dynamic pay = JsonConvert.DeserializeObject(responseContent);
            if (pay != null && pay.costo != null)
            {
                double costo = Convert.ToDouble(pay.costo);

                string costoFormatted = costo.ToString("0.00");
                Console.WriteLine($"Costo del parcheggio: {costoFormatted}");
                Pay(requestData);
            }
        }

        public void Pay(object requestData)
        {
            var responseFin = Validation.SendDataToServer("/endpark", oggetto: requestData);
            string responseCFin = responseFin.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Console.WriteLine("Grazie per aver sostato qui");
            dynamic datiDes = JsonConvert.DeserializeObject(responseCFin);

            Console.WriteLine($"Id: {datiDes.id}");
            Console.WriteLine($"Piano: {datiDes.piano}");
            Console.WriteLine($"Posto: {datiDes.posto}");
            Task.Delay(10000).GetAwaiter().GetResult(); // 10sec
            Console.WriteLine("\nPagamento ricevuto arrivederci!\n\n");
        }

        }
    }
        #endregion
    


