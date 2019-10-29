using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class ContactoHP : EntidadBase
    {
        [DataMember]
        public virtual DateTime Fecha { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string Telefono { get; set; }        
        [DataMember]
        public virtual string Nombre { get; set; }
        [DataMember]
        public virtual string Comite { get; set; }
        [DataMember]
        public virtual string Mensaje { get; set; }

        public ContactoHP() { }
    }
}
