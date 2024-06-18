using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Igra
    {
        public virtual IgraId? Id { get; set; }

        public Igra()
        {
            Id = new IgraId();
        }
    }
}
