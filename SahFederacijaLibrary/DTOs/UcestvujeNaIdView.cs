using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class UcestvujeNaIdView
    {
        public SahistaView? SahistaUcestvujeNa { get; set; }
        public Sahovski_TurnirView? UcestvujeNaSahovski_Turnir { get; set; }

        public UcestvujeNaIdView() { }

        internal UcestvujeNaIdView(UcestvujeNaId? u)
        {
            if (u != null)
            {
                SahistaUcestvujeNa = new SahistaView(u.SahistaUcestvujeNa);
                UcestvujeNaSahovski_Turnir = new Sahovski_TurnirView(u.UcestvujeNaSahovski_Turnir);
            }
        }
    }
}
