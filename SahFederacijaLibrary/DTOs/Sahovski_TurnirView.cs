using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class Sahovski_TurnirView
    {
        public int Id { get; set; }
        public string? Naziv { get; set; }
        public string? Zemlja { get; set; }
        public string? Grad { get; set; }
        public int? Godina_Odrzavanja { get; set; }
        public DateTime? Datum_Od { get; set; }
        public DateTime? Datum_Do { get; set; }

        public virtual IList<PartijaView>? Partije { get; set; }
        public virtual IList<SponzoriView>? Sponzori { get; set; }
        public virtual IList<OrganizujeView>? OrganizatorOrganizuje { get; set; }
        public virtual IList<UcestvujeNaView>? SahistaUcestvujeNa { get; set; }

        public Sahovski_TurnirView()
        {
            Partije = new List<PartijaView>();
            Sponzori = new List<SponzoriView>();
            OrganizatorOrganizuje = new List<OrganizujeView>();
            SahistaUcestvujeNa = new List<UcestvujeNaView>();
        }

        internal Sahovski_TurnirView(Sahovski_Turnir? t) : this()
        {
            if (t != null)
            {
                Id = t.Id;
                Naziv = t.Naziv;
                Zemlja = t.Zemlja;
                Grad = t.Grad;
                Godina_Odrzavanja = t.Godina_Odrzavanja;
                Datum_Od = t.Datum_Od;
                Datum_Do = t.Datum_Do;
            }
        }
    }
}
