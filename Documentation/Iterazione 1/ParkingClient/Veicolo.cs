using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Text.Json;

namespace ParkingUser
{
    internal class Veicolo
    {
        public string Targa { get; set; }
        public bool DueRuote { get; set; }
        public string TipoVeicolo { get; set; }
        public string Marca { get; set; }
        public string Modello { get; set; }

        public Veicolo(string targa, string tipo, bool dueRuote, string marca, string modello)
        {
            Targa = targa;
            TipoVeicolo = tipo;
            DueRuote = dueRuote;
            Marca = marca;  
            Modello = modello;
        }

        public Veicolo() { }

        public void InserisciVeicolo()
        {
            try
            {
                Console.WriteLine("Inserisci la targa del veicolo:");
                Targa = Console.ReadLine();
                Console.WriteLine("Inserisci tipo del veicolo:(ad esempio, \"auto\", \"moto\", \"camion\", ecc.)");
                TipoVeicolo = Console.ReadLine();
                Console.WriteLine("Il veicolo è a due ruote? (s/n):");
                string risposta = Console.ReadLine();
                DueRuote = (risposta.ToLower() == "s");
                Console.WriteLine("Inserisci la marca del veicolo:");
                Marca = Console.ReadLine();
                Console.WriteLine("Inserisci il modello del veicolo:");
                Modello = Console.ReadLine();
                //booleano true alla variabile DueRuote se la risposta dell'utente
                //s" ( case-insensitive), altrimenti le assegna il valore booleano false.
            }
            catch (Exception ex)
            {
                Console.WriteLine("Si è verificato un errore durante l'inserimento dei dati del veicolo:");
                Console.WriteLine(ex.Message);
            }

        }

        public void ModificaDati()
        {
            try
            {
                if (Targa == null)
                {
                    Console.WriteLine("Nessun veicolo presente.\nInserisci prima un veicolo\n");
                    return;
                }
                Console.WriteLine("Vuoi modificare la targa del veicolo? (s/n):");
                string risposta = Console.ReadLine();

                if (risposta.ToLower() == "s")
                {
                    Console.WriteLine("Targa attuale {0} Inserisci la nuova targa:",Targa);
                    Targa = Console.ReadLine();
                }//
                Console.WriteLine("Vuoi modificare la marca di veicolo? (s/n):");
                risposta = Console.ReadLine();
                if (risposta.ToLower() == "s")
                {
                    Console.WriteLine("Inserisci marca:");
                    Marca = Console.ReadLine();

                }
                Console.WriteLine("Vuoi modificare il modello del veicolo? (s/n):");
                risposta = Console.ReadLine();
                if (risposta.ToLower() == "s")
                {
                    Console.WriteLine("Inserisci marca:");
                    Modello = Console.ReadLine();

                }

                Console.WriteLine("Vuoi modificare il tipo di veicolo? (s/n):");
                risposta = Console.ReadLine();

                if (risposta.ToLower() == "s")
                {
                    Console.WriteLine("Il veicolo è a due ruote? (s/n):");
                    risposta = Console.ReadLine();
                    DueRuote = (risposta.ToLower() == "s");
                    Console.WriteLine("Inserisci tipo veicolo:");
                    TipoVeicolo = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Si è verificato un errore durante la modifica dei dati del veicolo:");
                Console.WriteLine(ex.Message);
            }
        }

        public void VisualizzaDati()
        {
            Console.WriteLine("Dati del veicolo:");
            Console.WriteLine("Targa: " + Targa);
            Console.WriteLine("Veicolo a due ruote: " + DueRuote);
        }
     
        public void InviaDatiAlServer(string url)
        {//da approfondire doc
            using (HttpClient client = new HttpClient())
            {
                try {
                    //fa i dati del veicolo in formato JSON
                    string jsonData = JsonSerializer.Serialize(this);

                    // creo il contenuto della richiesta HTTP
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Invia la richiesta POST al server
                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                    // Verifica lo stato della risposta
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Dati del veicolo inviati con successo al server.");
                    }
                    else
                    {
                        Console.WriteLine("Errore durante l'invio dei dati del veicolo al server. Codice di stato: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Si è verificato un errore durante l'invio dei dati del veicolo al server:");
                    Console.WriteLine(ex.Message);
                }
                /*I dati del veicolo vengono serializzati in formato JSON utilizzando la classe JsonSerializer. 
              * Successivamente, viene creato un oggetto StringContent che rappresenta il contenuto della richiesta HTTP,
              * specificando che il tipo di contenuto è JSON. 
              *  la richiesta viene inviata al server tramite PostAsync(), 
              *  e viene verificato lo stato della risposta per gestire eventuali errori.*/
            }

        }



    }
}
