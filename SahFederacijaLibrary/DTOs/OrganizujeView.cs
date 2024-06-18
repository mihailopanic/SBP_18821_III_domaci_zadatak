using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class OrganizujeView
    {
        public OrganizujeIdView? Id { get; set; }

        public OrganizujeView() { }

        internal OrganizujeView(Organizuje? o)
        {
            if (o != null)
            {
                Id = new OrganizujeIdView(o.Id);
            }
        }
    }
}
