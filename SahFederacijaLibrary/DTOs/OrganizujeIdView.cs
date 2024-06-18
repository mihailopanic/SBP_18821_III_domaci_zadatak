using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class OrganizujeIdView
    {
        public OrganizatorView? OrganizatorOrganizuje { get; set; }
        public Sahovski_TurnirView? OrganizujeSahovski_Turnir { get; set; }

        public OrganizujeIdView() { }

        internal OrganizujeIdView(OrganizujeId? o)
        {
            if (o != null)
            {
                OrganizatorOrganizuje = new OrganizatorView(o.OrganizatorOrganizuje);
                OrganizujeSahovski_Turnir = new Sahovski_TurnirView(o.OrganizujeSahovski_Turnir);
            }
        }
    }
}
