using SahFederacijaLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class PotezView
    {
        public int? Redni_Broj { get; set; }
        public int? Broj { get; set; }
        public char? Slovo { get; set; }
        public string? Figura { get; set; }
        public int? Vreme_Odigravanja { get; set; }
        public PartijaView? Partija { get; set; }

        public PotezView() { }

        internal PotezView(Potez? p)
        {
            if (p != null)
            {
                Redni_Broj = p.Redni_Broj;
                Broj = p.Broj;
                Slovo = p.Slovo;
                Figura = p.Figura;
                Vreme_Odigravanja = p.Vreme_Odigravanja;
            }
        }

        internal PotezView(Potez? p, Partija? par) : this(p)
        {
            Partija = new PartijaView(par);
        }
    }
}
