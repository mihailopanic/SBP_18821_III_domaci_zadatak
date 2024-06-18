using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class TakmicarskiBrzopotezniView : Sahovski_TurnirView
    {
        public string? Region { get; set; }
        public int? Max_Vreme_Trajanja { get; set; }

        public TakmicarskiBrzopotezniView() { }

        internal TakmicarskiBrzopotezniView(TakmicarskiBrzopotezni? t) : base(t)
        {
            if (t != null)
            {
                Region = t.Region;
                Max_Vreme_Trajanja = t.Max_Vreme_Trajanja;
            }
        }
    }
}
