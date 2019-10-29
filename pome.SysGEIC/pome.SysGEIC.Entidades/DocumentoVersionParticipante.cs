using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class DocumentoVersionParticipante : EntidadBase
    {        
        public virtual DocumentoVersion DocumentoVersion { get; set; }
        [DataMember]
        public virtual Profesional Profesional { get; set; }

        public DocumentoVersionParticipante() { }

        public void Validar()
        {
            if (DocumentoVersion == null)
                throw new ApplicationException("Debe seleccionar un versión del documento");
            if (Profesional == null)
                throw new ApplicationException("Debe seleccionar un profesional");            
        }
    }
}
