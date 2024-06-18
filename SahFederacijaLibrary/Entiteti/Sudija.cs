using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Sudija
    {
        public virtual int? Id { get; protected set; }

        public virtual IList<Partija>? SudjenePartije { get; set; }
        // 1:N SUDIJA - PARTIJA (referenca na listu sudjenih partija)

        public Sudija() { }
    }
}
