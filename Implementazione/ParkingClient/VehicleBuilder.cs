using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingClient
{
    public abstract class VehicleBuilder
    {
        protected VehicleManager vehicle;

        public VehicleManager GetVehicle()
        { //restituirmi istanza veico creat

            return vehicle;
        }

        public void CreateNewVehicle()
        {
            vehicle = new VehicleManager();
        }

        public abstract void SetTarga();
        public abstract void SetMarca();
        public abstract void SetModello();
        public abstract void SetAnno();
        public abstract void SetTipo();
    }

}
