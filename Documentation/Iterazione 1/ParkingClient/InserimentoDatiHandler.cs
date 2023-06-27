
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ParkingClient;

namespace ParkingClient
{
    public class InserimentoDatiHandler
    {
        //classi softwar non corrisponde a nessuna classe concettuale non c'è nel modello di dominio!!va solo diagram class
        /// <summary>
        /// è una definizione di tipo in C# che rappresenta le opzioni del menu nella gestione dei dati.
        /// Registrazione è assegnato al valore intero 1.
        ///Login è assegnato al valore succ 2 
        ////Esci è assegnato al valore successivo a Login ecc
        /// </summary>
        enum SceltaMenu
        {
            Registrazione = 1,
            Login,
            Esci
        }
        public void IniziaParcheggio() {
            Console.WriteLine("ok parcheggia");
        }

        public void StartClient()
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 12345); //indirizzo IP o il nome del server, ma server con any

                Console.WriteLine("Connessione al server riuscita.");

                NetworkStream stream = client.GetStream();

                // Invia dati al server
                string message = "Ciao server!";
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // Ricevi dati dal server
                data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string response = Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine("Risposta dal server: " + response);

                // Chiudi la connessione
                //stream.Close();
               // client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la connessione al server: " + ex.Message);
            }
        }



        public void GestisciInserimentoDati()
        {
            Utente utente = new Utente();
            Veicolo veicolo = new Veicolo();

            bool utenteRegistrato = false;
            bool continua = true;
            while (continua)
            {
                Console.WriteLine("1. Registrazione");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Esci");

                Console.Write("\nDigita il numero operazione desiderata tra quelle elencate sopra: ");
                if (Enum.TryParse(Console.ReadLine(), out SceltaMenu scelta))
                {
                    switch (scelta)
                    {
                        case SceltaMenu.Registrazione:
                            if (utenteRegistrato)
                            {
                                Console.WriteLine("Utente già registrato.");
                            }
                            else
                            {
                                //StartClient();
                                utente.RegistrazioneUtente();
                                utente.InviaDati();
                                Console.Write("Utente registrato, procediamo con la registrazione veicolo\n ");
                                veicolo.InserisciVeicolo(utente);
                                Console.Write("Hai inserito i seguenti dati: ");
                                utente.VisualizzaUtente();
                                veicolo.VisualizzaVeicolo();
                                Console.Write("Per iniziare la sosta procedi con il login! (o modificare i dati)\n");
                                utenteRegistrato = true;
                            }
                            break;
                        case SceltaMenu.Login:
                            if (utenteRegistrato)
                            {   //chiamata metodo loggin
                                Console.WriteLine("Utente già loggato. ipo");
                                Console.WriteLine("1. Modifica dati veicolo");
                                Console.WriteLine("2. Modifica dati utente");
                                Console.WriteLine("3. visualizza dati utente");
                                Console.WriteLine("4. Visualizza dati veicolo");
                                Console.WriteLine("5. Inizia parcheggio");

                                Console.Write("Seleziona un'opzione: ");
                                if (int.TryParse(Console.ReadLine(), out int sceltaModifica))
                                {
                                    switch (sceltaModifica)
                                    {
                                        case 1:
                                            veicolo.ModificaVeicolo();
                                            break;
                                        case 2:
                                            utente.ModificaUtente();
                                            break;
                                        case 3:
                                            utente.VisualizzaUtente();
                                            break;
                                        case 4:
                                            veicolo.VisualizzaVeicolo();
                                            break;
                                        case 5:
                                            IniziaParcheggio();
                                            break;
                                        default:
                                            Console.WriteLine("Scelta non valida. Riprova.");
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Scelta non valida. Riprova.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Effettua il login");
                                // Aggiungi qui la logica per il login
                                utente.EffettuaLogin();
                                if (utente != null)
                                {
                                    utenteRegistrato = true;
                                }
                            }
                            break;
                        case SceltaMenu.Esci:
                            continua = false;
                            break;
                        default:
                            Console.WriteLine("Scelta non valida. Riprova.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Scelta non valida. Riprova.");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Programma terminato.");
        }





    
    }
}
