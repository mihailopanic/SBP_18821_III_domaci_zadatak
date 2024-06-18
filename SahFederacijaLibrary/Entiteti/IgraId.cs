using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class IgraId
    {
        public virtual Sahista? SahistaIgra { get; set; }
        public virtual Partija? IgraPartija { get; set; }

        public override bool Equals(object? obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;

            if (obj?.GetType() != typeof(IgraId))
                return false;

            IgraId recievedObject = (IgraId)obj;

            if ((SahistaIgra?.Rbr == recievedObject.SahistaIgra?.Rbr) &&
                (IgraPartija?.Id == recievedObject.IgraPartija?.Id))
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
