using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahFederacijaLibrary.Entiteti
{
    public class Organizator
    {
        public virtual int Id { get; protected set; }
        public virtual string? Jmbg { get; set; }
        public virtual string? Adresa { get; set; }
        public virtual string? Lime { get; set; }
        public virtual char? Sslovo { get; set; }
        public virtual string? Prezime { get; set; }
        public virtual Sudija? Sudija { get; set; }
        // NULL ako ORGANIZATOR nije SUDIJA, tj. nije sudio nijedan mec
        // ako jeste, referenca na vrednost sudiju u tabeli SUDIJA 

        public virtual IList<Organizuje>? OrganizujeSahovski_Turnir { get; set; }
        // public virtual IList<Sahovski_Turnir> SahovskiTurniri { get; set; }
        // M:N ORGANIZATOR - SAHOVSKI_TURNIR (referenca na turnire koje je organizator organizovao/uje)
        
        public Organizator() 
        {
            //SahovskiTurniri = new List<Sahovski_Turnir>();
            OrganizujeSahovski_Turnir = new List<Organizuje>();
        }
    }
}
