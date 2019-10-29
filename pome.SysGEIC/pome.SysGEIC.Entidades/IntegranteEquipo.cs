using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class IntegranteEquipo : EntidadBase
    {
        [DataMember]
        public virtual int IdEquipo { get; set; }
        [DataMember]
        public virtual Profesional Profesional { get; set; }
        [DataMember]
        public virtual Rol Rol { get; set; }
        [DataMember]
        public virtual bool Vigente { get; set; }

        public IntegranteEquipo()
            : base() 
        {
            this.Id = -1;
            this.IdEquipo = -1;                
        }

        public override string ToString()
        {
            return string.Format("[{2}] {0} - {1}", Rol.ToString(), Profesional.ToString(), this.GetType().Name);
        }

        public virtual string Validar() 
        {
            StringBuilder errores = new StringBuilder();

            if (Profesional == null)
                errores.AppendLine("Debe seleccionar profesional" + Constantes.SaldoLinea);

            if (Rol == null)
                errores.AppendLine("Debe seleccionar rol" + Constantes.SaldoLinea);

            return errores.ToString();
        }
    }
}
