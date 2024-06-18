using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Partija
    {
        public virtual int Id { get; set; }
        public virtual DateTime? Datum_Vreme_Odigravanja { get; set; }
        public virtual string? Rezultat_Partije { get; set; }
        // moguce vrednosti: "MatC", "MatB", "Remi" i "Pat"
        public virtual int? Trajanje_Partije { get; set; }
        public virtual Sahista? Crne_Figure { get; set; }
        // ID igraca koji je igrao crnim figurama
        public virtual Sahista? Bele_Figure { get; set; }
        // ID igraca koji je igrao belim figurama

        public virtual Sahovski_Turnir? SahovskiTurnir { get; set; }
        // N:1 PARTIJA - SAHOVSKI_TURNIR (referenca na turnir na koji je partija odigrana)

        public virtual IList<Potez>? Potezi { get; set; }
        // 1:N PARTIJA - POTEZ (referenca na listu poteza odigranih u mecu)

        public virtual IList<Igra>? SahistaIgra { get; set; }
        //public virtual IList<Sahista> Sahiste { get; set; }
        // M:N PARTIJA - SAHISTA (referenca na listu od dvoje sahista koji su odigrali partiju)

        public virtual Sudija? Sudija { get; set; }
        // referenca na SUDIJA koji je sudio na mecu

        public Partija()
        {
            Potezi = new List<Potez>();

            // Sahiste = new List<Sahista>();
            SahistaIgra = new List<Igra>();
        }
    }
}
