using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Majstorski_Kandidat : Sahista
    {
        public virtual int? Broj_Partija_Do_Zvanja { get; set; }

        public Majstorski_Kandidat()
        {
            Status = "Majstorski_Kandidat";
        }
    }
}
