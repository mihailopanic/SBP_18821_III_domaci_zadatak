using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Sponzori
    {
        public virtual string? Sponzor { get; set; }
        public virtual Sahovski_Turnir? SahovskiTurnir { get; set; }
        // visevrednosni atribut SPONZORI - SAHOVSKI_TURNIR (referenca na turnir kome pripada sponzor)

        public override bool Equals(object? obj)
        {
            var other = obj as Sponzori;
            if (other == null) return false;
            return this.SahovskiTurnir?.Id == other.SahovskiTurnir?.Id && this.Sponzor == other.Sponzor;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public Sponzori() { }
    }
}
