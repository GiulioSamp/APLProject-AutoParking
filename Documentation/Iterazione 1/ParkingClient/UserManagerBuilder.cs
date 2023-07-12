using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    public class UserManagerBuilder
    {
        private string nome;
        private string cognome;
        private string telefono;
        private string email;
        private string pass;

        public UserManagerBuilder SetNome(string nome)
        {
            this.nome = nome;
            return this;
        }

        public UserManagerBuilder SetCognome(string cognome)
        {
            this.cognome = cognome;
            return this;
        }

        public UserManagerBuilder SetTelefono(string telefono)
        {
            this.telefono = telefono;
            return this;
        }

        public UserManagerBuilder SetEmail(string email)
        {
            this.email = email;
            return this;
        }

        public UserManagerBuilder SetPass(string pass)
        {
            this.pass = pass;
            return this;
        }

        public UserManager Build()
        {
            return new UserManager(nome, cognome, telefono, email, pass);
        }
    }


}
