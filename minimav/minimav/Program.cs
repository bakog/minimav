//utolsó módosító: Tóth Csaba
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace minimav
{
enum Colors { szurke, feher, fekete };
    class csucs //csúcs = állomás
    {
        int _id; //csucs azonosito 
        string _nev; //az csucs neve
        public List<csucs> szomszedok = new List<csucs>(); //szomszédos csúcsok listája
        public Colors szin; //a bejáráskori szín tárolása, kezdetben fehér, ha bejárható a gráf szürke lesz
        public csucs os; //az csucs ose

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string nev
        {
            get { return _nev; }
            set
            {
                if (value.Length > 0)
                {
                    _nev = value;
                }
                else
                {
                    throw new Exception("Az állomás neve nem lehet üres...");
                }
            }
        }
        public csucs(int idje, string neve)
        {
            this.id = idje;
            this.nev = neve;
        }
    }
    class utvonal
    {
        public List<csucs> veremcsucs = new List<csucs>();
        public void kiurit()
        {
            veremcsucs.Clear();
        }
        public bool ures()
        {
            if (veremcsucs.Count>0)
            { return false; }
            else { return true; }
        }
        public void berak(csucs C)
        {
            veremcsucs.Add(C);
        }
        public  csucs kivesz ()
        {
            int utolso = veremcsucs.Count()-1;
            csucs x=veremcsucs[utolso];
            veremcsucs.Remove(veremcsucs[utolso]);
            return x;
        }
    }

}

    class el
    {
        public csucs k; 
        public csucs v; 
        public int suly; 
        public el(csucs elso, csucs masodik, int s)
        {
            this.k = elso;
            this.v = masodik;
            this.suly = s;
        }
    }
    class minimav 
    {
        public List<csucs> allomasok = new List<csucs>();
        public List<csucs> szomszed_allomas = new List<csucs>();
        public List<el> utvonalak = new List<el>();

        public List<csucs> csucsok()
        {
            return allomasok;
        }
        public List<csucs> szomszedos_csucsok(csucs x)
        {
            return x.szomszedok;
        }
        public List<el> osszesel()
        {
            return utvonalak;
        }
        public int allomast_felvesz(csucs x)
        {
            if (x != null && !allomasok.Contains(x))
            {
                allomasok.Add(x);
            }
            return allomasok.IndexOf(x);
        }
        public void szomszedot_felvesz(csucs x, csucs y)
        {
            x.szomszedok.Add(y);
        }
        public void utvonalat_felvesz(csucs x, csucs y, int ertek)
        {
            utvonalak.Add(new el(x, y, ertek));
            utvonalak.Add(new el(y, x, ertek));
        }
    }
    class Program
    {
        static minimav G = new minimav();
        static utvonal utvonal_adat = new utvonal();
        static int idje1;
        static int idje2;
        static int a1;
        static int a2;
        static void Main(string[] args)
        {
            string rname = "adatok.txt";
            string[] sor;
            Random rnd = new Random();
            int kezdoindex;
            int vegindex;
            List<string> mh = new List<string>();
            //ide kerülnek az állomások nevei és ebből generálom az állomás id-jét az IndexOf segítségével, ellenőrizve, hogy van-e már ilyen állomás a listában
            //lehetne másképp is, pl. a G állomásainak ellenőrzésével is és egy számláló bevezetésével  <- ez takarékosabb megoldás lenne, mert nincs szükség egy +listára... 

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
                    a1 = G.allomast_felvesz(new csucs(idje1, mezo1));
                }
                else
                {
                    idje1 = mh.IndexOf(mezo1);
                    a1 = idje1;
                }
                if (!mh.Contains(mezo2))
                {
                    mh.Add(mezo2);
                    idje2 = mh.IndexOf(mezo2);
                    a2 = G.allomast_felvesz(new csucs(idje2, mezo2));
                    G.utvonalat_felvesz(new csucs(idje1, mezo1), new csucs(idje2, mezo2), mezo3);
                }
                else
                {
                    idje2 = mh.IndexOf(mezo2);
                    a2 = idje2;
                    G.utvonalat_felvesz(new csucs(idje1, mezo1), new csucs(idje2, mezo2), mezo3);
                }
                Console.WriteLine("FELVESZ: {0} - {1}", G.allomasok[a1].nev, G.allomasok[a2].nev);
                G.szomszedot_felvesz(G.allomasok[a1], G.allomasok[a2]);
                Console.WriteLine("FELVESZ: {0} - {1}", G.allomasok[a2].nev, G.allomasok[a1].nev);
                G.szomszedot_felvesz(G.allomasok[a2], G.allomasok[a1]);
            }
            Console.ForegroundColor = ConsoleColor.White;
            while (!r.EndOfStream)
            {
                sor = r.ReadLine().Split(';');
                string mezo1 = sor[0];
                string mezo2 = sor[1];
                int mezo3 = Int32.Parse(sor[2]);
                int index1 = mh.IndexOf(mezo1);
                int index2 = mh.IndexOf(mezo2);
            }
            r.Close();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nÁllomások:\n");
            foreach (csucs m in G.allomasok)
            {
                Console.WriteLine("{0}   - {1}", m.id, m.nev);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\nÉlek:\n");
            List<el> osszes = G.osszesel();
            for (int i = 0; i < osszes.Count(); i++)
            {
                Console.WriteLine("{0} - {1} ({2} km.)", osszes[i].k.nev, osszes[i].v.nev, osszes[i].suly);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nVéletlenszerűen generálok két indexet az útvonal kiválasztásához és a gráf bejáráshoz!\n");
            kezdoindex = rnd.Next(0, G.allomasok.Count - 1);
            do
            {
                vegindex = rnd.Next(0, G.allomasok.Count - 1);
            } while (kezdoindex == vegindex);
            Console.WriteLine("\nA kezdő index {0}, a végpont indexe {1}, \nígy a következő két állomást sorsoltam ki: {2} - {3}\n", kezdoindex, vegindex, G.allomasok[kezdoindex].nev, G.allomasok[vegindex].nev);

            bejaras.melysegi_bejaras_os_kezd(G); //a bejárás előtti inicializálás
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n\nA mélységi bejárás kezdő csúcspontja {0}\n\n", G.allomasok[kezdoindex].nev);
            bejaras.melysegi_bejaras(G, G.allomasok[kezdoindex]); //mélységi bejárás a véletlenszerűen kiválasztott csúcsból
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nMélységi bejárás után a csúcsok színei...\n");
            foreach (csucs m in G.allomasok)
            {
                Console.WriteLine("{0}   - {1}  - {2}", m.id, m.nev, m.szin);
            }
            bejaras.ut_kiiras(G, G.allomasok[kezdoindex], G.allomasok[vegindex], utvonal_adat); //útvonal kiírása

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nA kilépéshez nyomjon le egy billentyűt!");
            Console.ReadKey();
        }
    }
}

    
