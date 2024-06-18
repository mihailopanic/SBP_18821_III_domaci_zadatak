using SahFederacijaLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class Majstorski_KandidatView : SahistaView
    {
        public int? Broj_Partija_Do_Zvanja { get; set; }

        public Majstorski_KandidatView(Majstorski_KandidatView sah) { }

        internal Majstorski_KandidatView(Majstorski_Kandidat? m) : base(m)
        {
            if (m != null)
            {
                Broj_Partija_Do_Zvanja = m.Broj_Partija_Do_Zvanja;
            }
        }
    }
}
