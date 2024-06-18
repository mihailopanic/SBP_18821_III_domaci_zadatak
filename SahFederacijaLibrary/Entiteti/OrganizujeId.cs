using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class OrganizujeId
    {
        public virtual Organizator? OrganizatorOrganizuje { get; set; }
        public virtual Sahovski_Turnir? OrganizujeSahovski_Turnir { get; set; }

        public override bool Equals(object? obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;

            if (obj?.GetType() != typeof(OrganizujeId))
                return false;

            OrganizujeId recievedObject = (OrganizujeId)obj;

            if ((OrganizatorOrganizuje?.Id == recievedObject.OrganizatorOrganizuje?.Id) &&
                (OrganizujeSahovski_Turnir?.Id == recievedObject.OrganizujeSahovski_Turnir?.Id))
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
