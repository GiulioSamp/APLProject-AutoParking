using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    internal class Menu
    {
        Handler handler = Handler.Instance;
        public void EntryMenu()
        {
            
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
                             handler.Register();
                               
                                InternalMenu();

                            break;

                        case 2:
                            if (handler.DoLogin())
                            {
                                InternalMenu();                             
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
                            handler.ChoiceVehicleListForPark();
                            break;
                        case "2":
                            handler.RegisterVehicle();

                            break;
                        case "3":
                            handler.ModdVehicle();
                            break;
                        case "4":
                           // utente.EditUser();
                            //SendDataToServer("/register", null, utente);
                            break;
                        case "5":
                            
                            handler.RetrieveUser();
                            break;
                        case "6":
                            
                            handler.RetrieveVehicleList();
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

    }
}
