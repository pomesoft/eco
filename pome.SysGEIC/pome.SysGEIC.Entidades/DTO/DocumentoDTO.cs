using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Entidades
{
    public class DocumentoDTO : EntidadBaseParametrica
    {
        [DataMember]
        public virtual int IdTipoDocumento { get; set; }
        [DataMember]
        public virtual string TipoDocumentoDescripcion { get; set; }
        [DataMember]
        public virtual string TipoDocumentoIdDescripcion { get; set; }
        [DataMember]
        public virtual bool TipoDocumentoRequiereVersion { get; set; }
        [DataMember]
        public virtual int IdEstudio { get; set; }
        [DataMember]
        public virtual string NombreEstudio { get; set; }
        [DataMember]
        public virtual int IdVersionActual { get; set; }
        [DataMember]
        public virtual string VersionActualDescripcion { get; set; }
        [DataMember]
        public virtual string VersionActualFecha { get; set; }
        [DataMember]
        public virtual string VersionActualArchivo { get; set; }
        [DataMember]
        public virtual int VersionActualActa { get; set; }
        [DataMember]
        public virtual string EstadoActual { get; set; }
        [DataMember]
        public virtual string EstadoActualFecha { get; set; }
        [DataMember]
        public virtual string EstadoFinal { get; set; }
        [DataMember]
        public virtual List<Profesional> Participantes { get; set; }
        

        public DocumentoDTO()
        {
            Participantes = new List<Profesional>();
        }
    }
}
