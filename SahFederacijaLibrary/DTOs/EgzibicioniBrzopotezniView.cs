using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class EgzibicioniBrzopotezniView : Sahovski_TurnirView
    {
        public string? Tip { get; set; }
        public string? Novac_Namenjen { get; set; }
        public int? Prikupljen_Iznos { get; set; }
        public int? Max_Vreme_Trajanja { get; set; }

        public EgzibicioniBrzopotezniView() { }

        internal EgzibicioniBrzopotezniView(EgzibicioniBrzopotezni? t) : base(t)
        {
            if (t != null)
            {
                Tip = t.Tip;
                Novac_Namenjen = t.Novac_Namenjen;
                Prikupljen_Iznos = t.Prikupljen_Iznos;
                Max_Vreme_Trajanja = t.Max_Vreme_Trajanja;
            }
        }
    }
}
