using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    internal interface IUserManagerBuilder
    {
        IUserManagerBuilder SetNome(string Nome);
        IUserManagerBuilder SetCognome(string Cognome);
        IUserManagerBuilder SetTelefono(string Telefono);
        IUserManagerBuilder SetEmail(string Email);
        IUserManagerBuilder SetPass(string Pass);
        UserManager Build();
    }
}
