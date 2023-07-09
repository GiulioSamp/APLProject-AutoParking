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
        #region public
    
        public static async Task<HttpResponseMessage> InviaDatiAlServer<T>(T oggetto, string endpoint)
         {
             try
             {

                 var dtoJson = JsonConvert.SerializeObject(oggetto);

                 using (var client = new HttpClient())
                 {
                     client.BaseAddress = new Uri("http://localhost:18080");

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

        public void EntryMenu()
        {
            UserManager utente = new UserManager();
            VehicleManager veicolo = new VehicleManager();
            LoginManager login = new LoginManager();
            ParkingManager park = new ParkingManager();
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
                        InternalMenu(utente, veicolo, park);
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
                            InternalMenu(utente, veicolo, park );
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
        #endregion

        #region private 
        private async Task InternalMenu(UserManager utente, VehicleManager veicolo, ParkingManager park)
        {
            while (true)
            {
                

                //if (!parcheggioAvviato)
                //{
                    Console.Write($"\nBenvenuto/a {utente.Nome}!Seleziona un'opzione:"); 
                    // Stampa tutte le opzioni del menu solo se il parcheggio non è stato avviato
                    Console.WriteLine("\n1. Inizia parcheggio");
                    Console.WriteLine("2. Aggiungi nuovo veicolo");
                    Console.WriteLine("3. Modifica dati veicolo");
                    Console.WriteLine("4. Modifica dati utente");
                    Console.WriteLine("5. Visualizza dati utente");
                    Console.WriteLine("6. Visualizza dati veicolo");
                    
               // }

                Console.WriteLine("7. Esci");
                

                //da c# v8 switch con le espressioni 
                switch (Console.ReadLine())
                {
                    case "1":
                        var response = await park.StartParking(utente, veicolo);
                        Console.WriteLine(response);
                        Console.WriteLine("fine park");
                       // parcheggioAvviato = true; // Imposta il flag su true dopo l'avvio del parcheggio
                        break;
                    case "2":
                        veicolo.AddNewVehicle();
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

