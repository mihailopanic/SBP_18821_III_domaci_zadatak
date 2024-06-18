using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class UcestvujeNaView
    {
        public UcestvujeNaIdView? Id { get; set; }

        public UcestvujeNaView() { }

        internal UcestvujeNaView(UcestvujeNa? u)
        {
            if (u != null)
            {
                Id = new UcestvujeNaIdView(u.Id);
            }
        }
    }
}
