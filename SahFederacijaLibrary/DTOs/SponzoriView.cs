using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class SponzoriView
    {
        public string? Sponzor { get; set; }
        public Sahovski_TurnirView? SahovskiTurnir { get; set; }

        public SponzoriView() { }

        internal SponzoriView(Sponzori? spo) : this()
        {
            if (spo != null)
            {
                Sponzor = spo.Sponzor;
            }
        }

        internal SponzoriView(Sponzori? spo, Sahovski_Turnir? t) : this(spo)
        {
            SahovskiTurnir = new Sahovski_TurnirView(t);
        }
    }
}
