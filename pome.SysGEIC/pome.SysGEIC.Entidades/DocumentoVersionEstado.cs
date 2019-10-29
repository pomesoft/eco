using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class DocumentoVersionEstado : EntidadBase
    {
        public virtual DocumentoVersion Version { get; set; }
        [DataMember]
        public virtual EstadoDocumento Estado { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual Profesional ProfesionalAutor { get; set; }
        [DataMember]
        public virtual Profesional ProfesionalPresenta { get; set; }
        [DataMember]
        public virtual Profesional ProfesionalResponsable { get; set; }
        
        public virtual DateTime Fecha { get; set; }
        [DataMember]
        public virtual string FechaToString
        {
            get { return Fecha.ToShortDateString(); }
            set { }
        }
        [DataMember]
        public virtual string Observaciones { get; set; }
        [DataMember]
        public virtual bool? EstadoFinal { get; set; }

        public DocumentoVersionEstado() { }

        public virtual void Validar() 
        {
            if (Fecha == DateTime.MinValue)
                throw new ApplicationException("Debe ingresar fecha.");
            if (Estado==null)
                throw new ApplicationException("Debe seleccionar estado.");
        }
    }
}
