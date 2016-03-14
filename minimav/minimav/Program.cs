using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minimav
{
    class Program
    {
        static void Main(string[] args)
        {
            string rname = "adatok.txt";
            string[] sor;
            Random rnd = new Random();
            int kezdoindex;
            int vegindex;
            List<string> mh = new List<string>(); 
            //ide kerülnek az állomások nevei és ebből generálom az állomás id-jét az IndexOf segítségével, ellenőrizve, hogy van-e már ilyen állomás a listában

            StreamReader r = new StreamReader(rname, Encoding.Default);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Adatok beolvasása fájlból... \n");
            while (!r.EndOfStream)
            {
                sor = r.ReadLine().Split(';');
                string mezo1 = sor[0];
                string mezo2 = sor[1];
                int mezo3 = Int32.Parse(sor[2]);
                Console.WriteLine("{0} - {1} ({2} km.)", mezo1, mezo2, mezo3);

                if (!mh.Contains(mezo1))
                {
                    mh.Add(mezo1);
                    idje1 = mh.IndexOf(mezo1);
                }
                else
                {
                    idje1 = mh.IndexOf(mezo1);

                }
                if (!mh.Contains(mezo2))
                {
                    mh.Add(mezo2);
                    idje2 = mh.IndexOf(mezo2);
                }
                else
                {
                    idje2 = mh.IndexOf(mezo2);
                }
            }
        }
    }
}
