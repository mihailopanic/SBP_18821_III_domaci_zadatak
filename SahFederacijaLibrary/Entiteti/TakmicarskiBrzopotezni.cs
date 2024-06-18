using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class TakmicarskiBrzopotezni : Sahovski_Turnir
    {
        public virtual string? Region { get; set; }
        // Nacionalni/Regionalni/Internacionalni
        public virtual int? Max_Vreme_Trajanja { get; set; }
        // Za Brzopotezni
        public TakmicarskiBrzopotezni() { }
    }
}
