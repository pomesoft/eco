using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class ActaProfesional :EntidadBase
    {
        public virtual Acta Acta { get; set; }
        [DataMember]
        public virtual Profesional Profesional { get; set; }
        [DataMember]
        public virtual RolComite RolComite { get; set; }
        [DataMember]
        public virtual int IdRolComite
        {
            get { return RolComite == null ? -1 : RolComite.Id; }
            set { }

        }

        public ActaProfesional() { }

        public void Validar()
        {
            if (Profesional == null)
                throw new ApplicationException("Debe seleccionar un profesional");
        }
    }
}
