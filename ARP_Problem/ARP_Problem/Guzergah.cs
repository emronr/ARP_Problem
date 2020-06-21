using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_Problem
{
    public class Guzergah
    {
        public List<int> target { get; set; }
        public int t_time { get; set; }
        public int yuk { get; set; }

        public Guzergah()
        {
            this.t_time = 0;
            this.yuk = 0;
            this.target = new List<int>();
        }

        public Guzergah(List<int> target, int t_time, int yuk)
        {
            this.target = target;
            this.t_time = t_time;
            this.yuk = yuk;
        }

    }
}
