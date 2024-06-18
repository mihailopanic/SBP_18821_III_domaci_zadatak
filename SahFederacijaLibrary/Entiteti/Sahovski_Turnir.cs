using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public abstract class Sahovski_Turnir
    {
        public virtual int Id { get; set; }
        public virtual string? Naziv { get; set; }
        public virtual string? Zemlja { get; set; }
        public virtual string? Grad { get; set; }
        public virtual int? Godina_Odrzavanja { get; set; }
        public virtual DateTime? Datum_Od { get; set; }
        public virtual DateTime? Datum_Do { get; set; }
        public virtual string? Nacin_Odigravanja_Znacaj { get; set; }

        public virtual IList<Partija>? Partije { get; set; }
        // 1:N SAHOVSKI_TURNIR - PARTIJA (referenca na listu partija odigranih na turniru)

        public virtual IList<Sponzori>? Sponzori { get; set; }
        // visevrednosni atribut SAHOVSKI_TURNIR - SPONZORI (referenca na listu sponzora za turnir)

        public virtual IList<Organizuje>? OrganizatorOrganizuje { get; set;  }
        // public virtual IList<Organizator> Organizatori { get; set; }
        // M:N SAHOVSKI_TURNIR - ORGANIZATOR (referenca na listu organizatora na turniru)

        public virtual IList<UcestvujeNa>? SahistaUcestvujeNa { get; set; }
        // public virtual IList<Sahista> Sahisti { get; set; }
        // M:N SAHOVSKI_TURNIR - SAHISTA (referenca na listu sahista na turniru)

        public Sahovski_Turnir()
        {
            Partije = new List<Partija>();

            //Organizatori = new List<Organizator>();
            OrganizatorOrganizuje = new List<Organizuje>();

            //Sahisti = new List<Sahista>();
            SahistaUcestvujeNa = new List<UcestvujeNa>();

            Sponzori = new List<Sponzori>();
        }
    }
}
