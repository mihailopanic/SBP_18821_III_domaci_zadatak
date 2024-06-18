using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SahFederacijaLibrary.Entiteti;
using FluentNHibernate.Mapping;

namespace SahFederacijaLibrary.Mapiranja
{
    public class OrganizatorMapiranja : ClassMap<SahFederacijaLibrary.Entiteti.Organizator>
    {
        public OrganizatorMapiranja() 
        {
            Table("ORGANIZATOR");

            // ID se generise pomocu sekvence
            Id(x => x.Id, "ID").GeneratedBy.SequenceIdentity("ORGANIZATOR_SEQ");

            Map(x => x.Jmbg, "JMBG");
            Map(x => x.Adresa, "ADRESA");
            Map(x => x.Lime, "LIME");
            Map(x => x.Sslovo, "SSLOVO");
            Map(x => x.Prezime, "PREZIME");

            // mapiranje stranog kljuca ukoliko je ORGANIZATOR ujedno i SUDIJA na nekom mecu
            References(x => x.Sudija).Column("ID_SUDIJA").LazyLoad();

            // mapiranje veze M:N izmedju ORGANIZATOR i SAHOVSKI_TURNIR 
            /*HasManyToMany(x => x.SahovskiTurniri)
                .Table("ORGANIZUJE")
                .ParentKeyColumn("ID_ORGANIZATOR")
                .ChildKeyColumn("ID_SAHOVSKI_TURNIR")
                .Cascade.All();
            */
            HasMany(x => x.OrganizujeSahovski_Turnir).KeyColumn("ID_ORGANIZATOR").LazyLoad().Cascade.All().Inverse();
        }
    }
}
