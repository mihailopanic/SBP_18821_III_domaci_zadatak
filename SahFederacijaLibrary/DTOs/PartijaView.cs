using SahFederacijaLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.DTOs
{
    public class PartijaView
    {
        public virtual int? Id { get; set; }
        public virtual DateTime? Datum_Vreme_Odigravanja { get; set; }
        public virtual string? Rezultat_Partije { get; set; }
        public virtual int? Trajanje_Partije { get; set; }
        public virtual SahistaView? Crne_Figure { get; set; }
        public virtual SahistaView? Bele_Figure { get; set; }
        public virtual Sahovski_TurnirView? SahovskiTurnir { get; set; }
        public virtual IList<PotezView>? Potezi { get; set; }
        public virtual IList<IgraView>? SahistaIgra { get; set; }
        public virtual SudijaView? Sudija { get; set; }

        public PartijaView()
        {
            Potezi = new List<PotezView>();
            SahistaIgra = new List<IgraView>();
        }

        internal PartijaView(Partija? p) : this()
        {
            if (p != null)
            {
                Id = p.Id;
                Datum_Vreme_Odigravanja = p.Datum_Vreme_Odigravanja;
                Rezultat_Partije = p.Rezultat_Partije;
                Trajanje_Partije = p.Trajanje_Partije;
            }
        }

        internal PartijaView(Partija? p, Sahista? crne, Sahista? bele, Sahovski_Turnir? t, Sudija? sud) : this (p)
        {
            Crne_Figure = new SahistaView(crne);
            Bele_Figure = new SahistaView(bele);
            SahovskiTurnir = new Sahovski_TurnirView(t);
            Sudija = new SudijaView(sud);
        }
    }
}
