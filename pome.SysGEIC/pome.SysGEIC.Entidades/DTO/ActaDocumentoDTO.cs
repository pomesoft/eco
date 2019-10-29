using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class ActaDocumentoDTO : EntidadBaseParametrica
    {
        [DataMember]
        public virtual int IdActaDocumento { get; set; }
        [DataMember]
        public virtual int IdDocumento { get; set; }
        [DataMember]
        public virtual string Fecha { get; set; }
        [DataMember]
        public virtual bool Cerrada { get; set; }
        [DataMember]
        public virtual string Comentario { get; set; }
        [DataMember]
        public virtual string TipoDocumentoDescripcion { get; set; }
        [DataMember]
        public virtual string Documento { get; set; }
        [DataMember]
        public virtual string DocumentoVersion { get; set; }
        [DataMember]
        public virtual string DocumentoVersionFecha { get; set; }
        [DataMember]
        public virtual string DocumentoVersionEstado { get; set; }
        [DataMember]
        public virtual int OrdenEstudio { get; set; }
        [DataMember]
        public virtual int OrdenDocumento { get; set; }

        public ActaDocumentoDTO() { }
    }
}
