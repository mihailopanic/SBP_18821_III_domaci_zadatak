using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Potez
    {
        public virtual int? Redni_Broj { get; set; }
        public virtual int? Broj { get; set; }
        public virtual char? Slovo { get; set; }
        public virtual string? Figura { get; set; }
        public virtual int? Vreme_Odigravanja { get; set; }
        public virtual Partija? Partija { get; set; }
        // vlasnik identifikacije, referenca na partiju na kojoj je odigran potez

        public override bool Equals(object? obj)
        {
            var other = obj as Potez;
            if (other == null) return false;
            return this.Partija?.Id == other.Partija?.Id && this.Redni_Broj == other.Redni_Broj;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Potez() { }
    }
}
