using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public abstract class EntidadAcceso : EntidadBase
    {
        [DataMember]
        public virtual int IdMenuPrincipal { get; set; }
        [DataMember]
        public virtual int IdMenuSecundario { get; set; }
        [DataMember]
        public virtual NivelAcceso NivelAcceso { get; set; }

        public EntidadAcceso() { }

        public virtual void Validar()
        {
           
        }
    }
}
