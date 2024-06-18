using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class UcestvujeNa
    {
        public virtual UcestvujeNaId? Id { get; set; }

        public UcestvujeNa()
        {
            Id = new UcestvujeNaId();
        }
    }
}
