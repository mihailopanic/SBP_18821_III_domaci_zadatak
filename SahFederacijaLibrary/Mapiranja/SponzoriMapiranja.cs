using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using SahFederacijaLibrary.Entiteti;

namespace SahFederacijaLibrary.Mapiranja
{
    public class SponzoriMapiranja : ClassMap<SahFederacijaLibrary.Entiteti.Sponzori>
    {
        public SponzoriMapiranja()
        {
            Table("SPONZORI");

            CompositeId()
                .KeyReference(x => x.SahovskiTurnir, "ID_SAHOVSKI_TURNIR")
                .KeyProperty(x => x.Sponzor, "SPONZOR");
        }
    }
}
