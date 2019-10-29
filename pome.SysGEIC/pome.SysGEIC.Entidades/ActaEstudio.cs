using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class ActaEstudio : EntidadBase
    {
        public virtual Acta Acta { get; set; }
        public virtual Estudio Estudio { get; set; }
        [DataMember]
        public virtual int OrdenEstudio { get; set; }
        [DataMember]
        public virtual EstadoEstudio EstadoEstudio { get; set; }
        [DataMember]
        public virtual string ComentarioAntesDocumentos { get; set; }
        [DataMember]
        public virtual string ComentarioDespuesDocumentos { get; set; }
        [DataMember]
        public virtual CartaRespuestaModelo CartaRespuestaModelo { get; set; }
        [DataMember]
        public virtual string TextoLibreCartaRespuesta { get; set; }

        [DataMember]
        public virtual int IdEstudio
        {
            get { return Estudio == null ? -1 : Estudio.Id; }
            set { } 
        }
        [DataMember]
        public virtual string NombreEstudioListados 
        {
            get { return Estudio == null ? string.Empty : Estudio.NombreEstudioListados; }
            set { } 
        }
        [DataMember]
        public virtual string NombreCompleto
        {
            get { return Estudio == null ? string.Empty : Estudio.NombreCompleto; }
            set { }
        }
        [DataMember]
        public virtual string EstadoDescripcion
        {
            get { return EstadoEstudio == null ? string.Empty : EstadoEstudio.Descripcion; }
            set { }
        }
        [DataMember]
        public virtual string CartaRespuestaModeloDescripcion
        {
            get { return CartaRespuestaModelo == null ? string.Empty : CartaRespuestaModelo.Descripcion; }
            set { }
        }

        public ActaEstudio() { }
    }
}
