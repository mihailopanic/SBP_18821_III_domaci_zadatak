using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class TakmicarskiNormalniView : Sahovski_TurnirView
    {
        public string? Region { get; set; }

        public TakmicarskiNormalniView() { }

        internal TakmicarskiNormalniView(TakmicarskiNormalni? t) : base(t)
        {
            if (t != null)
            {
                Region = t.Region;
            }
        }
    }
}
