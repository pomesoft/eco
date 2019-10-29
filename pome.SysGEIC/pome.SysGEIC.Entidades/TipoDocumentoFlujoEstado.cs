using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class TipoDocumentoFlujoEstado : EntidadBase
    {
        public virtual TipoDocumentoFlujo TipoDocumentoFlujo { get; set; }
        [DataMember]
        public virtual EstadoDocumento Estado { get; set; }
        [DataMember]
        public virtual EstadoDocumento EstadoPadre { get; set; }
        [DataMember]
        public virtual bool Final { get; set; }

        public TipoDocumentoFlujoEstado() { }

        public virtual void Validar() 
        {
            if(Estado==null)
                throw new ApplicationException("Debe ingresar estado.");
        }
    }
}
