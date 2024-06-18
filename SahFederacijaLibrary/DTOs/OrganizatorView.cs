using SahFederacijaLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class OrganizatorView
    {
        public int Id { get; set; }
        public string? Jmbg { get; set; }
        public string? Adresa { get; set; }
        public string? Lime { get; set; }
        public char? Sslovo { get; set; }
        public string? Prezime { get; set; }
        public SudijaView? Sudija { get; set; }
        public IList<OrganizujeView>? OrganizujeSahovski_Turnir { get; set; }

        public OrganizatorView()
        {
            OrganizujeSahovski_Turnir = new List<OrganizujeView>();
        }

        internal OrganizatorView(Organizator? o) : this()
        {
            if (o != null)
            {
                Id = o.Id;
                Jmbg = o.Jmbg;
                Adresa = o.Adresa;
                Lime = o.Lime;
                Sslovo = o.Sslovo;
                Prezime = o.Prezime;
            }
        }

        internal OrganizatorView(Organizator? o, Sudija? sud) : this(o)
        {
            Sudija = new SudijaView(sud);
        }
    }
}
