using System;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ParkingClient;
using System.Security.Authentication;

namespace ParkingClient
{
    
    public class UserManager
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
       
        
        public UserManager(string nome, string cognome, string telefono, string email, string pass)
        {
            Nome = nome;
            Cognome = cognome;
            Telefono = telefono;
            Email = email;
            Pass = pass;
        }
        public UserManager() { }

        public void UserRegistration()
        {
            Console.WriteLine("\nInserisci i dati utente per procedere con la registrazione");
              
            UserManagerBuilder builder = new UserManagerBuilder();

                Nome = Validation.CheckInput("Inserisci il nome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                Cognome = Validation.CheckInput("Inserisci il cognome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                Telefono = Validation.CheckInput("Inserisci il nuovo numero di telefono: ", input =>
                {
                    return input.All(char.IsDigit) && input.Length >= 3;
                });

                Email = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
                }).ToLower();

                Pass = Validation.CheckInput("Inserisci la pass: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                UserManager utente = builder.SetNome(Nome)
                                          .SetCognome(Cognome)
                                          .SetTelefono(Telefono)
                                          .SetEmail(Email)
                                          .SetPass(Pass)
                                          .Build();
           
        }
        public bool IsUtenteInserito()
        {
            return !string.IsNullOrWhiteSpace(Nome)
                && !string.IsNullOrWhiteSpace(Cognome)
                && !string.IsNullOrWhiteSpace(Telefono)
                && !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Pass);
        }
 
        public void EditUser()
        {
            try
            {
                if (Nome == null)
                {
                    Console.WriteLine("Nessun utente presente. Inserisci una persona prima di effettuare una modifica.");
                    return;
                }
                Console.WriteLine("\nModifica i dati inseriti precedentemente:");
               
                Console.Write("Nome attuale: {0}", Nome);
                string nuovoNome = Validation.CheckInput("Inserisci il nuovo Nome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });
                Console.Write("Cognome attuale: {0}", Cognome);
                string nuovoCognome = Validation.CheckInput("Inserisci il nuovo Cognome: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                string nuovaPassword = Validation.CheckInput("Inserisci la nuova Pass: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input);
                });

                string nuovoNumeroTelefono = Validation.CheckInput("Inserisci il nuovo numero di Telefono: ", input =>
                {
                    return input.All(char.IsDigit) && input.Length >= 3;
                });

                Console.Write("Indirizzo e-mail attuale {0}", Email);
                string nuovaEmail = Validation.CheckInput("Inserisci indirizzo e-mail: ", input =>
                {
                    return !string.IsNullOrWhiteSpace(input) && (input.Contains("@"));
                }).ToLower();
                // Aggiornamento dei dati della persona
                Nome = nuovoNome;
                Cognome = nuovoCognome;
                Pass = nuovaPassword;
                Telefono = nuovoNumeroTelefono;
                Email = nuovaEmail;

                Console.WriteLine("\nDati del utente modificati con successo.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }
      

    }
}








