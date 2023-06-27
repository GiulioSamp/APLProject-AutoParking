using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//per rest api HTTP POST
using System.Net.Http;
using System.Text.Json;
using Microsoft.VisualBasic;
using ParkingClient;
using System.Net.Sockets;

namespace ParkingClient
{
    
    public class Utente
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string NumeroTelefono { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
       
        
        public Utente(string nome, string cognome, string numeroTelefono, string email, string password)
        {
            Nome = nome;
            Cognome = cognome;
            NumeroTelefono = numeroTelefono;
            Email = email;
            Password = password;
        }
        public Utente() { } //serve   

        /// <summary>
        /// RegistrazioneUtente
        /// </summary>

         #region public
        public void RegistrazioneUtente()
        {
            Console.WriteLine("\nInserisci i dati utente per procedere con la registrazione");
            try
            {
               Nome = CheckInput("Inserisci il nome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
 
               Cognome= CheckInput("Inserisci il cognome: ", input =>
               {
                   return !string.IsNullOrWhiteSpace(input);
               });
               
                Password = CheckInput("Inserisci la password: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                NumeroTelefono = CheckInput("Inserisci il nuovo numero di telefono: ", input =>
               {
                    return int.TryParse(input, out _);
               });

               Email = CheckInput("Inserisci l'indirizzo email: ", EmailCheck);

            }
            catch (Exception ex)
            {
                    Console.WriteLine($"Errore: {ex.Message}");
            }

        }
        
        public bool IsUtenteInserito()
        {
            return !string.IsNullOrWhiteSpace(Nome)
                && !string.IsNullOrWhiteSpace(Cognome)
                && !string.IsNullOrWhiteSpace(NumeroTelefono)
                && !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Password);
        }


        
        public void ModificaUtente()
        {
            //
            try
            {
                if (Nome == null)
                {
                    Console.WriteLine("Nessun utente presente. Inserisci una persona prima di effettuare una modifica.");
                    return;
                }
                Console.WriteLine("\nModifica i dati inseriti precedentemente:");
                //Console.WriteLine($"Modifica dei dati di {persona.Nome} {persona.Cognome}:");
                Console.Write("Nome attuale: {0}", Nome);
                string nuovoNome = CheckInput("Inserisci il nuovo nome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
                Console.Write("Cognome attuale: {0}", Cognome);
                string nuovoCognome = CheckInput("Inserisci il nuovo cognome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                string nuovaPassword = CheckInput("Inserisci la nuova password: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                string nuovoNumeroTelefono = CheckInput("Inserisci il nuovo numero di telefono: ", input =>
                {
                    return int.TryParse(input, out _);
                });

                NumeroTelefono = nuovoNumeroTelefono;
                Console.WriteLine("Numero di telefono modificato con successo.");
                Console.Write("Indirizzo e-mail attuale {0}", Email);
                string nuovaEmail = CheckInput("Inserisci l'indirizzo email: ", EmailCheck);

                // Aggiornamento dei dati della persona
                Nome = nuovoNome;
                Cognome = nuovoCognome;
                Password = nuovaPassword;
                //NumeroTelefono = nuovoNumeroTelefono;
                Email = nuovaEmail;

                Console.WriteLine("\nDati del utente modificati con successo.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }

        public void VisualizzaUtente()
        {
            try
            {
                if (Nome == null)
                {
                    Console.WriteLine("Nessun utente presente.Inserisci un utente prima di visualizzare i dati.");
                    return;
                }
                Console.WriteLine("\nVisualizza dati utente inseriti:");
                Console.WriteLine($"Nome: {Nome}");
                Console.WriteLine($"Cognome: {Cognome}");
                Console.WriteLine($"Numero di telefono: {NumeroTelefono}");
                Console.WriteLine($"E-mail: {Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }

        }

        public void EffettuaLogin()
        {
            Console.WriteLine("Effettua il login");

            Console.Write("Nome utente: ");
            string nomeUtente = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            // Aggiungi qui la logica per verificare le credenziali e restituire l'oggetto Utente corrispondente
            // In caso di credenziali valide, restituisci l'oggetto Utente
            // In caso di credenziali non valide, restituisci null

            //if (nomeUtente == vedi come fare && password ==  idem un check serve)
            //{
              //  return new Utente(nomeUtente, password);
            //}

//            return null;
        }
        //bozza uguale nelle due classi BOZZA 
      
        public void InviaDati()
        { 
            string ServerAddress = "127.0.0.1";
          int ServerPort = 12345;
            try
            {
                TcpClient client = new TcpClient(ServerAddress, ServerPort);
                NetworkStream stream = client.GetStream();

                // Creazione e invio dell'oggetto Utente
               
                byte[] data = Encoding.ASCII.GetBytes("Nome:"+Nome + ";" + "Cognome:" + Cognome + ";" + "NumeroTelefono" + NumeroTelefono + ";" + Email + ";" + Password);
                stream.Write(data, 0, data.Length);

                // Chiusura della connessione
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la connessione al server: " + ex.Message);
            }
        
        }





        /// <summary>
        /// può essere utilizzato per richiedere input valido per diversi tipi di dat,
        /// passando il messaggio appropriato e la funzione di validazione
        /// Check numero di telefono, viene utilizzata la funzione int.TryParse
        /// </summary>

        public string CheckInput(string messaggio, Func<string, bool> validazione)
        {
            while (true)
            {
                Console.Write(messaggio);
                string input = Console.ReadLine();

                if (validazione(input))
                {
                    return input;
                }

                Console.WriteLine("Input non valido. Riprova.");
            }

        }
        #endregion
        #region private
        /// <summary>
        /// Metodo per verificare se un indirizzo email è valido
        /// 
        /// </summary>
        private static bool EmailCheck(string email)
        {
            return email.Contains("@");
        }

        /* private bool ContieneNumeri(string input)
         {
             return input.Any(c => char.IsDigit(c));
         }*/
        #endregion




    }
}








