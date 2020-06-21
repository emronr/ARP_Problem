using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_Problem
{
    public class CarInfo
    {
        public int carID { get; set; }
        public Guzergah rota { get; set; }

        public CarInfo()
        {
            this.rota = new Guzergah();
        }

        public CarInfo(int carID, Guzergah rota)
        {
            this.carID = carID;
            this.rota = rota;
        }
    }
}
