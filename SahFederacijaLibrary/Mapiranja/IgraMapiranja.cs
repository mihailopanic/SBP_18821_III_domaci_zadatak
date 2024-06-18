using FluentNHibernate.Mapping;
using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Mapiranja
{
    class IgraMapiranja : ClassMap<Igra>
    {
        public IgraMapiranja()
        {
            Table("IGRA");

            CompositeId(x => x.Id)
                .KeyReference(x => x.SahistaIgra, "RBR_SAHISTA")
                .KeyReference(x => x.IgraPartija, "ID_PARTIJA");
        }
    }
}
