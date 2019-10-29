using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class ActaEstudioDTO : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string Fecha { get; set; }
        [DataMember]
        public virtual string Codigo { get; set; }
        [DataMember]
        public virtual int IdEstado { get; set; }
        [DataMember]
        public virtual string Estado { get; set; }
        [DataMember]
        public virtual string Comentario { get; set; }
        [DataMember]
        public virtual string NombreCompleto { get; set; }
        [DataMember]
        public virtual string CartaIncluirTercerPunto { get; set; }
        [DataMember]
        public virtual string CartaImprimirFirmaPresidente { get; set; }
        [DataMember]
        public virtual string CartaImprimirFirmaParticipantes { get; set; }
        [DataMember]
        public virtual int OrdenEstudio { get; set; }
        [DataMember]
        public virtual List<ActaDocumentoDTO> DocumentosTratados { get; set; }
        
        public ActaEstudioDTO() 
        {
            DocumentosTratados = new List<ActaDocumentoDTO>();
        }
    }
}
