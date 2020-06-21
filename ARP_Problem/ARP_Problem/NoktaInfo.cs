using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_Problem
{
    public class NoktaInfo
    {
        public int cust_no { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int yuk { get; set; }
        public int r_time { get; set; }
        public int d_time { get; set; }
        public int s_time { get; set; }


        public NoktaInfo(int cust_no, int x, int y, int yuk, int r_time, int d_time, int s_time)
        {
            this.cust_no = cust_no;
            this.x = x;
            this.y = y;
            this.yuk = yuk;
            this.r_time = r_time;
            this.d_time = d_time;
            this.s_time = s_time;
        }
    }
}
