using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_Problem
{
    public class MerkezInfo
    {
        //Default Veriler
        public int count_car { get; set; }
        public int capacity { get; set; }
        public List<NoktaInfo> nokta { get; set; }

        //Hesaplama için veri seti
        public List<Distance> nokta_d { get; set; }

        //Gidilecekler listesi
        public Guzergah rota { get; set; }

        public List<CarInfo> carList { get; set; }

        public MerkezInfo()
        {
            this.nokta = new List<NoktaInfo>();
            this.nokta_d = new List<Distance>();
            this.rota = new Guzergah();
            this.carList = new List<CarInfo>();
        }
        public MerkezInfo(int count_car, int capacity, List<NoktaInfo> nokta)
        {
            this.count_car = count_car;
            this.capacity = capacity;
            this.nokta = nokta;
        }
    }
}
