using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_Problem
{
    public class Distance
    {
        public int cust_no1 { get; set; }
        public int cust_no2 { get; set; }
        public int distance { get; set; }

        public Distance(int cust_no1, int cust_no2, int distance)
        {
            this.cust_no1 = cust_no1;
            this.cust_no2 = cust_no2;
            this.distance = distance;
        }

    }
}
