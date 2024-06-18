using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class EgzibicioniNormalni : Sahovski_Turnir
    {
        public virtual string? Tip { get; set; }
        // Humanitarni/Promotivni
        public virtual string? Novac_Namenjen { get; set; }
        // Za Humanitarni, NULL ako je Promotivni
        public virtual int? Prikupljen_Iznos { get; set; }
        // Za Humanitarni, NULL ako je Promotivni
        public EgzibicioniNormalni() { }
    }
}
