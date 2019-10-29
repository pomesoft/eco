using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class EstudioRecordatoriosDTO : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string NombreDocumento { get; set; }        
        [DataMember]
        public virtual string VersionDocumento { get; set; }
        [DataMember]
        public virtual string Fecha { get; set; }
        [DataMember]
        public virtual string Autor { get; set; }
        [DataMember]
        public virtual string Pendiente { get; set; }
        [DataMember]
        public virtual string Observaciones { get; set; }

        public EstudioRecordatoriosDTO() { }
    }
}
