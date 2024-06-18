using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class TakmicarskiNormalni : Sahovski_Turnir
    {
        public virtual string? Region { get; set; }
        // Nacionalni/Regionalni/Internacionalni
        public TakmicarskiNormalni() { }
    }
}
