using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class UcestvujeNaId
    {
        public virtual Sahista? SahistaUcestvujeNa { get; set; }
        public virtual Sahovski_Turnir? UcestvujeNaSahovski_Turnir { get; set; }

        public override bool Equals(object? obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;

            if (obj?.GetType() != typeof(UcestvujeNaId))
                return false;

            UcestvujeNaId recievedObject = (UcestvujeNaId)obj;

            if ((SahistaUcestvujeNa?.Rbr == recievedObject.SahistaUcestvujeNa?.Rbr) &&
                (UcestvujeNaSahovski_Turnir?.Id == recievedObject.UcestvujeNaSahovski_Turnir?.Id))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
