using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_Problem
{
    public class ServiceHelper
    {

        //center Info for select txt
        MerkezInfo center = new MerkezInfo();

        //Bütün dosya değerleri matematiksel işlem için class yapısına aktarıldı.
        public void MainReadFile(List<string> satirlar)
        {
            //Capacity and count car
            int[] temp_info = new int[2];
            int count_info = 0;

            // all points
            int[] temp = new int[7];
            int count;


            //read info
            var info = satirlar[4].SplitInParts(8);
            foreach (var i in info)
            {
                if (count_info == 2) break;
                temp_info[count_info] = Convert.ToInt32(i.ToString());
                count_info++;
            }
            center.count_car = temp_info[0];
            center.capacity = temp_info[1];

            //read point 
            for (int i = 9; i < satirlar.Count; i++)
            {
                count = 0;
                var parts = satirlar[i].SplitInParts(10);
                foreach (var a in parts)
                {
                    if (count == 7) break;
                    temp[count] = Convert.ToInt32(a.ToString());
                    count++;
                }
                center.nokta.Add(new NoktaInfo(
                    temp[0],
                    temp[1],
                    temp[2],
                    temp[3],
                    temp[4],
                    temp[5],
                    temp[6]
                    ));
                temp = new int[7];
            }

            //butun noktalar arası uzaklık kaydedildi
            for (int i = 0; i < center.nokta.Count; i++)
            {
                for (int j = 0; j < center.nokta.Count; j++)
                {
                    if (i != j)
                    {
                        int d = SaveDistance(center.nokta[i].x, center.nokta[i].y, center.nokta[j].x, center.nokta[j].y);
                        center.nokta_d.Add(new Distance(
                            center.nokta[i].cust_no,
                            center.nokta[j].cust_no,
                            d
                            ));
                    }

                }
            }
            //Araba listesi için ilk araba oluşturuldu
            center.carList.Add(new CarInfo(1, new Guzergah()));

            foreach (var c in center.nokta)
            {
                //Console.WriteLine(c.cust_no + "\t" + c.x + "\t" + c.y + "\t" + c.yuk + "\t" + c.r_time + "\t" + c.d_time + "\t" + c.s_time);
            }

            //Console.WriteLine("\t\tBütün Uzaklıklar\n\n");
            foreach (var c in center.nokta_d)
            {
                //Console.WriteLine(c.cust_no1 + "\t" + c.cust_no2 + "\t" + c.distance);
            }
        }

        //Uzaklık Hesaplama
        public int SaveDistance(int x1, int y1, int x2, int y2)
        {
            int xfark = Math.Abs(x1 - x2);
            int yfark = Math.Abs(y1 - y2);

            double yfark_2 = Math.Pow(yfark, 2);
            double xfark_2 = Math.Pow(xfark, 2);

            double distance = Math.Sqrt(xfark_2 + yfark_2);

            return Convert.ToInt32(distance);
        }

        //Verilen iki nokta arası uzaklık bulma
        public Distance FindDistance(int cust1, int cust2)
        {
            Distance d = center.nokta_d.Where(x => x.cust_no1 == cust1 && x.cust_no2 == cust2).FirstOrDefault();

            return d;
        }

        //verilen noktaya en yakın diğer noktayı bulma
        public List<Distance> ShortestDistance(int c)
        {
            List<Distance> dlist = new List<Distance>();
            if (c == 0)
            {
                dlist = center.nokta_d.Where(x => x.cust_no1 == c).OrderBy(x => x.distance).ToList();
            }
            else
            {
                dlist = center.nokta_d.Where(x => x.cust_no1 == c && x.cust_no2 != 0).OrderBy(x => x.distance).ToList();
            }
            return dlist;
        }

        //c = here, c hariç due_date sıralaması
        public List<NoktaInfo> DueDateSort(int c)
        {
            List<NoktaInfo> dlist = new List<NoktaInfo>();
            dlist = center.nokta.Where(x => x.cust_no != c && x.cust_no != 0).OrderBy(x => x.d_time).ToList();
            return dlist;
        }

        public void rotaBul()
        {
            int state = 0;
            while (state != -2)
            {
                if (center.rota.target == null)
                {
                    state = rotaAdd(0);
                    Console.WriteLine("\t\tCUST_NO: 0 noktasından harekete geçtim");
                }
                else if (state != -1)
                {
                    state = rotaAdd(state);
                }
                if (state == -2)
                {
                    Console.WriteLine("\n\n------------Bütün teslimatlar yapıldı------------");
                }
                if (state == -1)
                {
                    Console.WriteLine("\t\tKapasite doldu." +
                        "Dönüyom");
                    state = -2;
                }
            }
        }

        public int rotaAdd(int here)
        {
            //var car = center.carList.LastOrDefault();
            CarInfo car = new CarInfo();
            var carL = center.carList.ToList();

            //hangi arabayı kontrol ettiğimizi buluyoruz
            foreach (var item in carL)
            {
                if (item.rota.target.LastOrDefault() == here)
                {
                    car = item;
                }
            }

            //var r1 = ShortestDistance(here);

            // son  teslim saatine göre sıralama
            var r1 = DueDateSort(here);

            int temp_distance_time = 0;
            int temp_t_time;

            NoktaInfo temp_hedef;               // bütün koşulların uyduğu gitmeye karar verdiğim yer 
            NoktaInfo temp_capacityP = null;    // üsteki koşulun sorunsuz sağlanmadığı durumlarda kapasite yüzünden gidilmeyen yer
            NoktaInfo temp_timeP = null;        // üsteki koşulun sorunsuz sağlanmadığı durumlarda teslim saati  yüzünden gidilmeyen yer
            bool tukendi_mi = false;

            //aracın kapasitesi yeterli mi
            if (car.rota.yuk != center.capacity)
            {
                // aracın gidebileceği noktalar
                foreach (var r in r1)
                {
                    temp_hedef = center.nokta.Where(x => x.cust_no == r.cust_no).FirstOrDefault();

                    temp_distance_time = FindDistance(here, r.cust_no).distance;
                    temp_t_time = car.rota.t_time + temp_distance_time;

                    //Daha önce gidilmiş mi
                    if (!center.rota.target.Contains(r.cust_no))
                    {
                        //Açılış-Kapanış süreleri arasında mı
                        if (temp_hedef.r_time <= temp_t_time && temp_t_time <= temp_hedef.d_time)
                        {
                            //Kapasitem yeterli mi
                            if (car.rota.yuk + temp_hedef.yuk <= center.capacity)
                            {
                                center.rota.target.Add(r.cust_no);

                                car.rota.target.Add(r.cust_no);
                                car.rota.t_time = temp_t_time + temp_hedef.s_time;
                                car.rota.yuk += temp_hedef.yuk;
                                Console.WriteLine("\n\n\t\tCUST_NO : " + r.cust_no + " noktasındayım.");
                                Console.WriteLine("\t\t--Araç Bilgileri--");
                                Console.WriteLine("\t\tCarID       : " + car.carID);
                                Console.WriteLine("\t\tYük         : " + car.rota.yuk);
                                Console.WriteLine("\t\tGeldiğim Zaman : " + (car.rota.t_time - temp_hedef.s_time));
                                Console.WriteLine("\t\tÇıktığım Zaman : " + car.rota.t_time);


                                if (center.nokta.Count == center.rota.target.Count + 1)
                                {
                                    return -2;
                                }
                                #region istisna
                                ///<summary>
                                ///İstisna durumu şunu sorgulamaktır :
                                /// 3 koşulu da sağlamasına rağmen o noktaya gidilirse,
                                /// Due_date saatini kaçıracağı başka bir nokta var mı
                                /// Varsa;
                                ///    Önce boşta kullanmadığımız araba var mı
                                ///         => Varsa return et;
                                ///         => Yoksa Merkez noktasından yeni araba kaldır.
                                /// Yoksa;
                                /// Devam et
                                ///
                                /// </summary>

                                // En son gittiğim hariç gittiklerimi listeden çıkarıyor
                                foreach (var b in center.rota.target)
                                {
                                    if (b != r.cust_no)
                                    {
                                        r1.RemoveAll(x => x.cust_no == b);
                                    }
                                }
                                //Hiç gitmediklerim arasında dönüyor
                                foreach (var a in r1)
                                {
                                    // a = ideal noka, r = bulunduğum nokta
                                    // ideal nokta bulunduğum nokta olmamalı
                                    if (a != r)
                                    {
                                        NoktaInfo temp_hedef2 = center.nokta.Where(x => x.cust_no == a.cust_no).FirstOrDefault();

                                        temp_t_time = car.rota.t_time + FindDistance(r.cust_no, temp_hedef2.cust_no).distance;
                                        CarInfo bCar;

                                        //due_date saatini kaçırdığım var mı
                                        if (temp_hedef2.d_time < temp_t_time)
                                        {
                                            //boşta araç varmı
                                            bCar = carL.Where(x => x.rota.yuk < center.capacity && x.carID != car.carID).FirstOrDefault();

                                            //  VARSA
                                            if (bCar != null)
                                            {
                                                if (temp_hedef2.r_time - bCar.rota.t_time < 150)
                                                {
                                                    return bCar.rota.target.LastOrDefault();

                                                }
                                                else
                                                {
                                                    bCar.rota.t_time = temp_hedef2.r_time - 50;

                                                    return bCar.rota.target.LastOrDefault();
                                                }
                                            }
                                            //  YOKSA
                                            else
                                            {// Yeni araç oluştur

                                                Guzergah newcarG = new Guzergah();
                                                newcarG.t_time = temp_hedef2.d_time - 40;

                                                center.carList.Add(new CarInfo(car.carID + 1, newcarG));
                                                Console.WriteLine("\n\n\t\tMerkezden yeni araç kalktı");

                                                return 0;

                                            }
                                        }

                                    }


                                }
                                #endregion

                                return r.cust_no;
                            }
                            else
                            {
                                temp_capacityP = temp_hedef;
                            }
                        }
                        else if (temp_hedef.r_time - temp_t_time < 150 && temp_t_time <= temp_hedef.d_time)
                        {
                            foreach (var item in carL)
                            {
                                if (item.rota.yuk < center.capacity && item.rota.t_time < temp_hedef.d_time)
                                {
                                    item.rota.t_time = temp_hedef.r_time;
                                    return item.rota.target.LastOrDefault();
                                }

                            }
                            if (car.rota.yuk + temp_hedef.yuk <= center.capacity)
                            {
                                temp_timeP = temp_hedef;
                            }
                            else
                            {
                                tukendi_mi = true;
                            }
                        }
                    }
                }

                ///<summary>
                ///Tüm noktalar arasında tüm koşulları sağlayan bir durum çıkmadığında;
                ///Hem zaman hem capasity sorunu olan iki nokta varsa
                ///zaman sorunu olana git
                ///kapasite sorunu olan için merkezden yeni araç kaldır
                /// </summary>
                if (temp_timeP != null && temp_capacityP != null)
                {
                    center.rota.target.Add(temp_timeP.cust_no);

                    car.rota.target.Add(temp_timeP.cust_no);
                    car.rota.yuk += temp_timeP.yuk;

                    Guzergah newcarG = new Guzergah();
                    newcarG.t_time = car.rota.t_time;

                    center.carList.Add(new CarInfo(car.carID + 1, newcarG));

                    car.rota.t_time = temp_timeP.r_time + temp_timeP.s_time;
                    Console.WriteLine("\n\n\t\tCUST_NO : " + temp_timeP.cust_no + " noktasındayım.");
                    Console.WriteLine("\t\t--Araç Bilgileri--");
                    Console.WriteLine("\t\tCarID       : " + car.carID);
                    Console.WriteLine("\t\tYük         : " + car.rota.yuk);
                    Console.WriteLine("\t\tGeldiğim Zaman : " + (car.rota.t_time - temp_timeP.s_time));
                    Console.WriteLine("\t\tÇıktığım Zaman : " + car.rota.t_time);



                    Console.WriteLine("\n\n\t\tMerkezden yeni araç kalktı");
                    return 0;
                }
                //Sadece zaman için sorun varsa git
                if (temp_timeP != null)
                {

                    center.rota.target.Add(temp_timeP.cust_no);

                    car.rota.target.Add(temp_timeP.cust_no);
                    car.rota.t_time = temp_timeP.r_time + temp_timeP.s_time;
                    car.rota.yuk += temp_timeP.yuk;
                    Console.WriteLine("\n\n\t\tCUST_NO : " + temp_timeP.cust_no + " noktasındayım.");
                    Console.WriteLine("\t\t--Araç Bilgileri--");
                    Console.WriteLine("\t\tCarID       : " + car.carID);
                    Console.WriteLine("\t\tYük         : " + car.rota.yuk);
                    Console.WriteLine("\t\tGeldiğim Zaman : " + (car.rota.t_time - temp_timeP.s_time));
                    Console.WriteLine("\t\tÇıktığım Zaman : " + car.rota.t_time);
                    return temp_timeP.cust_no;
                }
            }
            else
            {
                if (center.nokta.Count == center.rota.target.Count + 1)
                {
                    return -2;
                }
                foreach (var item in carL)
                {
                    if (item.rota.yuk < center.capacity)
                    {
                        return item.rota.target.LastOrDefault();
                    }

                }
                Guzergah newcarG = new Guzergah();
                newcarG.t_time = car.rota.t_time;

                center.carList.Add(new CarInfo(car.carID + 1, newcarG));
                Console.WriteLine("\n\n\t\tMerkezden yeni araç kalktı");


                return 0;
            }

            temp_capacityP = null;
            temp_timeP = null;
            return -1;
        }
    }
}
