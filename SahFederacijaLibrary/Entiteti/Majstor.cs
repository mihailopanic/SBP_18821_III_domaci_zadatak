using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Majstor : Sahista
    {
        public virtual DateTime? Datum_Zvanja { get; set; }
        public virtual Sudija? Sudija { get; set; }
        // NULL ako MAJSTOR nije SUDIJA, tj. nije sudio nijedan mec
        // ako jeste, referenca na sudiju u tabeli SUDIJA

        public Majstor()
        {
            Status = "Majstor";
        }
    }
}
