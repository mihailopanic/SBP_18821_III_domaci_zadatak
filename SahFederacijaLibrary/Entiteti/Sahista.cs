using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Sahista
    {
        public virtual int Rbr { get; protected set; }
        public virtual string? Zemlja_Porekla { get; set; }
        public virtual string? Broj_Pasosa { get; set; }
        public virtual DateTime? Datum_Uclanjenja { get; set; }
        public virtual string? Lime { get; set; }
        public virtual char? Sslovo { get; set; }
        public virtual string? Prezime { get; set; }
        public virtual string? Adresa { get; set; }
        public virtual DateTime? Datum_Rodjenja { get; set; }
        public virtual string? Status { get; set; }
        // Da li Sahista ima titulu Majstor, Majstorski_Kandidat
        // ili nema titulu, odnosno NULL

        public virtual IList<UcestvujeNa>? UcestvujeNaSahovski_Turnir {  get; set; }
        // public virtual IList<Sahovski_Turnir> SahovskiTurniri { get; set; }
        // M:N SAHISTA - SAHOVSKI_TURNIR (referenca na listu turnira na kojima je sahista ucestvovao)
        
        public virtual IList<Igra>? IgraPartija { get; set; }
        // public virtual IList<Partija> Partije { get; set; }
        // M:N SAHISTA - PARTIJA (referenca na listu odigranih partija)

        public Sahista() 
        {
            // SahovskiTurniri = new List<Sahovski_Turnir>();
            UcestvujeNaSahovski_Turnir = new List<UcestvujeNa>();

            // Partije = new List<Partija>();
            IgraPartija = new List<Igra>();
        }
    }
}
