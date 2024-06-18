using FluentNHibernate.Mapping;
using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Mapiranja
{
    class UcestvujeNaMapiranja : ClassMap<UcestvujeNa>
    {
        public UcestvujeNaMapiranja()
        {
            Table("UCESTVUJE_NA");

            CompositeId(x => x.Id)
                .KeyReference(x => x.SahistaUcestvujeNa, "RBR_SAHISTA")
                .KeyReference(x => x.UcestvujeNaSahovski_Turnir, "ID_SAHOVSKI_TURNIR");
        }
    }
}
