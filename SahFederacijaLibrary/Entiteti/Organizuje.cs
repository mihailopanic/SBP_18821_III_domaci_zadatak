using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Organizuje
    {
        public virtual OrganizujeId? Id { get; set; }

        public Organizuje()
        {
            Id = new OrganizujeId();
        }
    }
}
