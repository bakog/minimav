using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
