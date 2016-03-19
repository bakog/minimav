// Készítette: Bakó Gábor, Tóth Csaba, Vásárhelyi Mátyás
//commit teszt from VS Csaba
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
        int _id; //csucs azonositó   
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

interface sor{//soradatszerkezet
	void kiurit();
	bool ures();
	void berak (csucs C);
	csucs kivesz();
}

    class el
    {//a gráf egy éléhez tartozó információk
        public csucs k; //graf−el egyik vege
        public csucs v; //graf−el masik vege
        public int suly; //az el−hez rendelt
        public el(csucs elso, csucs masodik, int s)
        {
            this.k = elso;
            this.v = masodik;
            this.suly = s;
        }
    }
    class minimav //az állomások gráfja
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

    /// <summary>
    /// Bejárással kapcsolatos metódusokat tartalmazó osztály.
    /// </summary>
    class Bejaras
    {
        /// <summary>
        /// Útvonal hosszának kiszámolása.
        /// </summary>
        /// <param name="utak"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int utvonal_hossz(List<el> utak, csucs x, csucs y)
        {
            int hossz = 0;
            foreach (el elek in utak)
            {
                if (elek.k.id == x.id && elek.v.id == y.id)
                {
                    hossz = elek.suly;
                    break;
                }
            }
            return hossz;
        }

        /// <summary>
        /// Bejárás előtt minden álloms fehérre színezése, ős törlése.
        /// </summary>
        /// <param name="G"></param>
        public static void melysegi_bejaras_os_kezd(minimav G) 
        {
            List<csucs> csucsok = G.csucsok();
            foreach (csucs a in csucsok)
            {
                a.szin = Colors.feher;
                a.os = null;
            }
        }

        /// <summary>
        /// Mélységi bejárás.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="x"></param>
        public static void melysegi_bejaras(minimav G, csucs x)
        {
            x.szin = Colors.szurke;
            foreach (csucs sz in G.szomszedos_csucsok(x))
            {
                Console.WriteLine("Ezt a csúcsot vizsgálom... {0}", sz.nev);
                if (sz.szin == Colors.feher)
                {
                    sz.os = x;
                    Console.WriteLine("{0} őse {1}", sz.nev, x.nev);
                    melysegi_bejaras(G, sz);
                }
            }
        }

        /// <summary>
        /// Útvonal kiírása.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="kezdet"></param>
        /// <param name="veg"></param>
        /// <param name="U"></param>
        public static void ut_kiiras(minimav G, csucs kezdet, csucs veg, utvonal U)
        {
            int hossza = 0;
            U.kiurit(); //az utvonalat érintő csúcsok listájának kiürítése (itt tároljuk majd az útvonalat visszafelé)
           //útvonal osszeallitasa
            if (veg.szin != Colors.feher && kezdet.szin != Colors.feher)
            {
                csucs u = veg;
                while (u != kezdet)
                {
                    csucs elozo = u;
                    U.berak(u);
                    u = u.os;
                    if (u != elozo)
                    {
                        hossza += utvonal_hossz(G.osszesel(), elozo, u);
                    }
                }
            }
            if (U.ures())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nem vezet útvonal {0} állomástól {1} állomásig.", kezdet.nev, veg.nev);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Egy lehetséges útvonal {0} állomástól {1} állomásig.\n", kezdet.nev, veg.nev);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0} -> ", kezdet.nev);
                while (U.ures() == false)  //van még állomás
                {
                    csucs x = U.kivesz();
                    Console.Write("{0} -> ", x.nev);
                }
                Console.WriteLine("{0} km.", hossza);
            }
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

        static void about()
        {
            string rname = "about.txt";
            string sor;
            Console.Clear();
            Console.Write("A program készítői:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Bakó Gábor, Tóth Csaba, Vásárhelyi Mátyás");
            Console.CursorTop = 5;
            Console.WriteLine("A program leírása:\n");
            Console.ForegroundColor = ConsoleColor.Gray;

            StreamReader r = new StreamReader(rname, Encoding.Default);
            while (!r.EndOfStream)
            {
                sor = r.ReadLine();
                Console.WriteLine(sor);

            }
            Console.ReadKey();
        }
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
            about();
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
            kezdoindex = rnd.Next(0,G.allomasok.Count-1);
            do
            {
                vegindex = rnd.Next(0, G.allomasok.Count - 1);
            } while (kezdoindex == vegindex);
            Console.WriteLine("\nA kezdő index {0}, a végpont indexe {1}, \nígy a következő két állomást sorsoltam ki: {2} - {3}\n",kezdoindex,vegindex ,G.allomasok[kezdoindex].nev, G.allomasok[vegindex].nev);
	        Bejaras.melysegi_bejaras_os_kezd(G); //a bejárás előtti inicializálás
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n\nA mélységi bejárás kezdő csúcspontja {0}\n\n", G.allomasok[kezdoindex].nev);
            Bejaras.melysegi_bejaras(G, G.allomasok[kezdoindex]); //mélységi bejárás a véletlenszerűen kiválasztott csúcsból
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nMélységi bejárás után a csúcsok színei...\n");
            foreach (csucs m in G.allomasok)
            {
                Console.WriteLine("{0}   - {1}  - {2}", m.id, m.nev, m.szin);
            }
            Bejaras.ut_kiiras(G, G.allomasok[kezdoindex], G.allomasok[vegindex], utvonal_adat); //útvonal kiírása

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nA kilépéshez nyomjon le egy billentyűt!");
            Console.ReadKey();
        }
    }
}
