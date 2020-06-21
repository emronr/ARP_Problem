using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP_Problem
{
    //string parçalama
    static class StringExtensions
    {

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

    }

    class Program
    {
        // Klasördeki txt dosyaları
        static List<string> FileNames = new List<string>();

        // klasörden txt dosylarını çekme
        static void ReadFile(string klasor)
        {

            DirectoryInfo di = new DirectoryInfo(klasor);
            FileSystemInfo[] fsi = di.GetFileSystemInfos();
            foreach (FileSystemInfo File in fsi)
            {
                if (File is DirectoryInfo)
                    ReadFile(klasor + "\\" + File.ToString());

                //Eğer klasörse tekrar aynı metoda klasörü parametre olarak gönderiyorum
                else if (File is FileInfo)
                {
                    if (File.ToString().EndsWith(".txt"))
                    {
                        FileNames.Add(File.Name.ToString());
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            //Change file path of Instances
            string dir = "D:\\Github\\ARP_Problem\\Instances";
            ReadFile(dir);
            //Dosya Okuma
            foreach (var item in FileNames)
            {
                List<string> satirlar = new List<string>();
                StreamReader SW = new StreamReader(dir + "\\" + item);
                string satir;
                while ((satir = SW.ReadLine()) != null)
                {
                    satirlar.Add(satir);
                }
                SW.Close();

                foreach (var s in satirlar)
                {
                    //Txt nin aynısını ekrana yazıyor
                    Console.WriteLine(s);
                }
                Console.WriteLine("\n");
                ServiceHelper svc = new ServiceHelper();

                svc.MainReadFile(satirlar);
                svc.rotaBul();

                //var a = svc.FindDistance(5, 0);

                Console.WriteLine("{0} Bitti", item.ToString());
            }
            Console.ReadKey();

        }


    }
}
