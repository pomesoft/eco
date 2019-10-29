using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class DocumentoVersionRecordatorio : EntidadBase
    {
        public virtual DocumentoVersion Version { get; set; }
        
        public virtual DateTime Fecha { get; set; }
        [DataMember]
        public virtual string FechaToString
        {
            get { return Fecha.ToShortDateString(); }
            set { }
        }
        [DataMember]
        public virtual Profesional ProfesionalAutor { get; set; }
        [DataMember]
        public virtual string Observaciones { get; set; }
        [DataMember]
        public virtual bool? Pendiente { get; set; }

        public DocumentoVersionRecordatorio() { }

        public virtual void Validar()
        {
            if (Fecha == null)
                throw new ApplicationException("Debe ingresar fecha.");
            if (ProfesionalAutor == null)
                throw new ApplicationException("Debe seleccionar autor.");
            if (Observaciones == null)
                throw new ApplicationException("Debe ingresar recordatorio.");
        }
    }
}
