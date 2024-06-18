using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class EgzibicioniNormalniView : Sahovski_TurnirView
    {
        public string? Tip { get; set; }
        public string? Novac_Namenjen { get; set; }
        public int? Prikupljen_Iznos { get; set; }

        public EgzibicioniNormalniView() { }

        internal EgzibicioniNormalniView(EgzibicioniNormalni? t) : base(t)
        {
            if (t != null)
            {
                Tip = t.Tip;
                Novac_Namenjen = t.Novac_Namenjen;
                Prikupljen_Iznos = t.Prikupljen_Iznos;
            }
        }
    }
}
