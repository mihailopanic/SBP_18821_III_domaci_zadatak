using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class SahistaView
    {
        public int Rbr { get; set; }
        public string? Zemlja_Porekla { get; set; }
        public string? Broj_Pasosa { get; set; }
        public DateTime? Datum_Uclanjenja { get; set; }
        public string? Lime { get; set; }
        public char? Sslovo { get; set; }
        public string? Prezime { get; set; }
        public string? Adresa { get; set; }
        public DateTime? Datum_Rodjenja { get; set; }
        public string? Status { get; set; }

        public IList<UcestvujeNaView>? UcestvujeNaSahovski_Turnir { get; set; }
        public IList<IgraView>? IgraPartija { get; set; }

        public SahistaView()
        {
            UcestvujeNaSahovski_Turnir = new List<UcestvujeNaView>();
            IgraPartija = new List<IgraView>();
        }

        internal SahistaView(Sahista? sah) : this()
        {
            if (sah != null)
            {
                Rbr = sah.Rbr;
                Zemlja_Porekla = sah.Zemlja_Porekla;
                Broj_Pasosa = sah.Broj_Pasosa;
                Datum_Uclanjenja = sah.Datum_Uclanjenja;
                Lime = sah.Lime;
                Sslovo = sah.Sslovo;
                Prezime = sah.Prezime;
                Adresa = sah.Adresa;
                Datum_Rodjenja = sah.Datum_Rodjenja;
                Status = sah.GetType().Name;
            }
        }
    }
}
