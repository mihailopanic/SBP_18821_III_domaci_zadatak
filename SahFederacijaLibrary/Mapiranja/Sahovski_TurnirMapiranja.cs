using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using SahFederacijaLibrary.Entiteti;

namespace SahFederacijaLibrary.Mapiranja
{
    public class Sahovski_TurnirMapiranja : ClassMap<SahFederacijaLibrary.Entiteti.Sahovski_Turnir>
    {
        public Sahovski_TurnirMapiranja()
        {
            Table("SAHOVSKI_TURNIR");

            // Vrsta sahovskog turnira odredjuje se na osnovu kolone "NACIN_ODIGRAVANJA_ZNACAJ"
            DiscriminateSubClassesOnColumn("NACIN_ODIGRAVANJA_ZNACAJ");

            // ID se generise pomocu sekvence
            Id(x => x.Id, "ID").GeneratedBy.SequenceIdentity("SAHOVSKI_TURNIR_SEQ");
            
            Map(x => x.Naziv, "NAZIV");
            Map(x => x.Zemlja, "ZEMLJA");
            Map(x => x.Grad, "GRAD");
            Map(x => x.Godina_Odrzavanja, "GODINA_ODRZAVANJA");
            Map(x => x.Datum_Od, "DATUM_OD");
            Map(x => x.Datum_Do, "DATUM_DO");

            // mapiranje veze 1:N izmedju SAHOVSKI_TURNIR i PARTIJA
            HasMany(x => x.Partije).KeyColumn("ID_SAHOVSKI_TURNIR").Cascade.All().Inverse();

            // mapiranje visevrednosnog atributa SAHOVSKI_TURNIR i SPONZORI (fakticki veza 1:N)
            HasMany(x => x.Sponzori).KeyColumn("ID_SAHOVSKI_TURNIR").Cascade.All().Inverse();

            // mapiranje veze M:N izmedju SAHOVSKI_TURNIR i ORGANIZATOR
            /*HasManyToMany(x => x.Organizatori)
                .Table("ORGANIZUJE")
                .ParentKeyColumn("ID_SAHOVSKI_TURNIR")
                .ChildKeyColumn("ID_ORGANIZATOR")
                .Cascade.All()
                .Inverse();
            */
            HasMany(x => x.OrganizatorOrganizuje).KeyColumn("ID_SAHOVSKI_TURNIR").LazyLoad().Cascade.All().Inverse();

            // mapiranje veze M:N izmedju SAHOVSKI_TURNIR i SAHISTA
            /*HasManyToMany(x => x.Sahisti)
                .Table("UCESTVUJE_NA")
                .ParentKeyColumn("ID_SAHOVSKI_TURNIR")
                .ChildKeyColumn("RBR_SAHISTA")
                .Cascade.All()
                .Inverse();
            */
            HasMany(x => x.SahistaUcestvujeNa).KeyColumn("ID_SAHOVSKI_TURNIR").LazyLoad().Cascade.All().Inverse();
        }
    }

    public class TakmicarskiNormalniMapiranja : SubclassMap<TakmicarskiNormalni>
    {
        public TakmicarskiNormalniMapiranja()
        {
            Map(x => x.Region, "REGION");

            DiscriminatorValue("TakmicarskiNormalni");
        }
    }

    public class TakmicarskiBrzopotezniMapiranja : SubclassMap<TakmicarskiBrzopotezni>
    {
        public TakmicarskiBrzopotezniMapiranja()
        {
            Map(x => x.Region, "REGION");
            Map(x => x.Max_Vreme_Trajanja, "MAX_VREME_TRAJANJA");

            DiscriminatorValue("TakmicarskiBrzopotezni");
        }
    }

    public class EgzibicioniNormalniMapiranja : SubclassMap<EgzibicioniNormalni>
    {
        public EgzibicioniNormalniMapiranja()
        {
            Map(x => x.Tip, "TIP");
            Map(x => x.Novac_Namenjen, "NOVAC_NAMENJEN");
            Map(x => x.Prikupljen_Iznos, "PRIKUPLJEN_IZNOS");

            DiscriminatorValue("EgzibicioniNormalni");
        }
    }

    public class EgzibicioniBrzopotezniMapiranja : SubclassMap<EgzibicioniBrzopotezni>
    {
        public EgzibicioniBrzopotezniMapiranja()
        {
            Map(x => x.Tip, "TIP");
            Map(x => x.Novac_Namenjen, "NOVAC_NAMENJEN");
            Map(x => x.Prikupljen_Iznos, "PRIKUPLJEN_IZNOS");
            Map(x => x.Max_Vreme_Trajanja, "MAX_VREME_TRAJANJA");

            DiscriminatorValue("EgzibicioniBrzopotezni");
        }
    }
}
