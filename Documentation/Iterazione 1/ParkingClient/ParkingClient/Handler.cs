using System;
using System.Text;
using ParkingClient;
using Newtonsoft.Json;
using System.Reflection;
//add pacchetto nuget nel progetto da ide
namespace ParkingClient
{
    public class Handler
    {
        public async Task<HttpResponseMessage> InviaDatiAlServer<T>(T oggetto, string endpoint)
            {
                try
                {
                    
                    var dtoJson = JsonConvert.SerializeObject(oggetto);

                using (var client = new HttpClient())
                    {
                    client.BaseAddress = new Uri("http://localhost:18080/");

                        var content = new StringContent(dtoJson, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(endpoint, content);

                        return response;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Si è verificata un'eccezione durante l'invio dei dati al server: {ex.Message}");
                    return null;
                }
            }
        /// <summary>
        /// DA IMPLEMENTARE TUTTO sTARTpaRKIN
        /// </summary>
        public void StartParking() {
            Console.WriteLine("ok parcheggia"); ///DA IMPLEMENTARE TUTTO DI START   
        }

        public void EntryMenu()
        {
            Utente utente = new Utente();
            Veicolo veicolo = new Veicolo();
            Login login = new Login();
            bool utenteRegistrato = false;
            Console.WriteLine("1. Registrazione");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Esci");
            Console.Write("\nDigita il numero dell'operazione desiderata tra quelle elencate sopra: ");
            //potrei anche togliere if(int...) e passare direttamente l out a switch che verifica il risultato del Tryparse (vero se convert in int,
            //l'utente come input avendo lista DOVREBBE sempre mett un numero valido eusare int.Parse (considero utente distratto??)
            int.TryParse(Console.ReadLine(), out int sceltaMenu);
            switch (sceltaMenu)
            {
                case 1:
                    if (utenteRegistrato)
                    {
                        Console.WriteLine("Utente già registrato."); //ma perchè mai??? 
                    }
                    else
                    {
                        utente.UserRegistration();
                        InviaDatiAlServer(utente, "/register");

                        Console.Write("Utente registrato, procediamo con la registrazione del veicolo\n");
                        veicolo.EnterVehicle(utente);
                        InviaDatiAlServer(veicolo, "/vehicle");

                        Console.Write("\nHai inserito i seguenti dati:\n");
                        ViewParameters(utente);
                        ViewParameters(veicolo);
                        utenteRegistrato = true; //?????????????
                        InternalMenu(utente, veicolo);
                    }
                    break;

                case 2:
                    //utenteRegistrato non ne capisco il senso, se non trovo togliere todo di questo metodo
                    //if (utenteRegistrato)
                    //{
                    //    InternalMenu(utente, veicolo);
                    //}
                    //else
                    //{
                        if(login.DoLogin())
                        {
                            InternalMenu(utente, veicolo);
                            utenteRegistrato = true;
                        }
                       
                    //}
                    break;

                case 3:
                    Console.WriteLine("Menù terminato.");
                    return; // Termina
                default:
                    Console.WriteLine("Scelta non valida. Riprova.");
                    break;
            }           
    }
        public static void ViewParameters<T>(T oggetto)
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
       
        #region private 
        private void InternalMenu(Utente utente, Veicolo veicolo)
        {
            while (true)
            {
                Console.Write($"\nBenvenuto/a {utente.Nome}!");
                Console.WriteLine("\n1. Inizia parcheggio");
                Console.WriteLine("2. Aggiungi nuovo veicolo");
                Console.WriteLine("3. Modifica dati veicolo");
                Console.WriteLine("4. Modifica dati utente");
                Console.WriteLine("5. Visualizza dati utente");
                Console.WriteLine("6. Visualizza dati veicolo");
                Console.WriteLine("7. Esci");

                Console.Write("Seleziona un'opzione: ");
                //da c# v8 switch con le espressioni 
                switch (Console.ReadLine())
                {
                    case "1":
                        StartParking();
                        break;
                    case "2":
                        veicolo.AddNewVehicle();
                        break;
                    case "3":
                        veicolo.ModifyVehicle();
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
                        return;
                    default:
                        Console.WriteLine("Scelta non valida. Riprova.");
                        break;
                }
            }
        }
    }
        #endregion
    }

