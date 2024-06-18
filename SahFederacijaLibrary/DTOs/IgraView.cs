using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class IgraView
    {
        public IgraIdView? Id;

        public IgraView() { }

        internal IgraView(Igra? i)
        {
            if (i != null)
            {
                Id = new IgraIdView(i.Id);
            }
        }
    }
}
