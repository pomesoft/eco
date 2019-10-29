using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class EstudioParticipante : EntidadBase
    {
        public virtual Estudio Estudio { get; set; }
        [DataMember]
        public virtual Profesional Profesional { get; set; }
        [DataMember]
        public virtual Rol Rol { get; set; }

        public virtual DateTime? Desde { get; set; }
        [DataMember]
        public virtual string DesdeToString
        {
            get { return Desde.HasValue ? Desde.Value.ToShortDateString() : string.Empty; }
            set { }
        }
        
        public virtual DateTime? Hasta { get; set; }
        [DataMember]
        public virtual string HastaToString
        {
            get { return Hasta.HasValue ? Hasta.Value.ToShortDateString() : string.Empty; }
            set { }
        }

        public virtual bool EsInvestigadorPrincipal
        {
            get { return (this.Rol.Id == 1); }
        }
        public virtual bool EsAuxiliar
        {
            get { return (this.Rol.Id == 2); }
        }

        public EstudioParticipante() : base() { }

        public void Validar()
        {
            if (Profesional == null)
                throw new ApplicationException("Debe seleccionar un profesional");
            if (Rol == null)
                throw new ApplicationException("Debe seleccionar un rol");
        }
    }
}
