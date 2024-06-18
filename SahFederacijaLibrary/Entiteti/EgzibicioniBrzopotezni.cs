using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class EgzibicioniBrzopotezni : Sahovski_Turnir
    {
        public virtual string? Tip { get; set; }
        // Humanitarni/Promotivni
        public virtual string? Novac_Namenjen { get; set; }
        // Za Humanitarni, NULL ako je Promotivni
        public virtual int? Prikupljen_Iznos { get; set; }
        // Za Humanitarni, NULL ako je Promotivni
        public virtual int? Max_Vreme_Trajanja { get; set; }
        // Za Brzopotezni
        public EgzibicioniBrzopotezni() { }
    }
}
