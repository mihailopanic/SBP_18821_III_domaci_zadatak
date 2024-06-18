using SahFederacijaLibrary.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class SudijaView
    {
        public int Id { get; set; }
        public IList<PartijaView>? SudjenePartije { get; set; }

        public SudijaView()
        {
            SudjenePartije = new List<PartijaView>();
        }

        internal SudijaView(Sudija? sud) : this()
        {
            if (sud != null)
            {
                Id = sud.Id ?? -1;
            }
        }
    }
}
