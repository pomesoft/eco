using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class EstudioDTO : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string Codigo { get; set; }
        [DataMember]
        public virtual string NombreCompleto { get; set; }
        [DataMember]
        public virtual EstadoEstudio Estado { get; set; }
        [DataMember]
        public virtual List<DocumentoDTO> Documentos { get; set; }
        [DataMember]
        public virtual List<NotaDTO> Notas { get; set; }
        [DataMember]
        public virtual List<Profesional> InvestigadoresPrincipalesProfesional { get; set; }
        [DataMember]
        public virtual int EstadoSemaforo { get; set; }

        [DataMember]
        public virtual string NombreEstudioListados
        {
            get { return string.Format("{0} {2} {1}", this.Codigo, this.Descripcion, (this.Descripcion.Trim().Length > 0 ? "-" : "")); }
            set { }
        }

        [DataMember]
        public virtual string EstadoDescripcion
        {
            get { return Estado != null ? Estado.Descripcion : string.Empty; }
            set { }
        }
        
        public EstudioDTO()
        {
            Documentos = new List<DocumentoDTO>();
            Notas = new List<NotaDTO>();
            InvestigadoresPrincipalesProfesional = new List<Profesional>();
        }
    }
}
