using SahFederacijaLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class MajstorView : SahistaView
    {
        public DateTime? Datum_Zvanja { get; set; }
        public SudijaView? Sudija { get; set; }

        public MajstorView() { }

        internal MajstorView(Majstor? m) : base(m)
        {
            if (m != null)
            {
                Datum_Zvanja = m.Datum_Zvanja;
            }
        }

        internal MajstorView(Majstor? m, Sudija? sud) : this(m)
        {
            Sudija = new SudijaView(sud);
        }
    }
}
