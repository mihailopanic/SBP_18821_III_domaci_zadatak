using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class IgraIdView
    {
        public SahistaView? SahistaIgra { get; set; }
        public PartijaView? IgraPartija { get; set; }

        public IgraIdView() { }

        internal IgraIdView(IgraId? i)
        {
            if (i != null)
            {
                SahistaIgra = new SahistaView(i.SahistaIgra);
                IgraPartija = new PartijaView(i.IgraPartija);
            }
        }
    }
}
