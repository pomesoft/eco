using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class TipoUsuarioAcceso : EntidadAcceso
    {
        public TipoUsuario TipoUsuario { get; set; }
      
        public TipoUsuarioAcceso() { }

        public override void Validar()
        {
            base.Validar();

            if (TipoUsuario == null)
                throw new ApplicationException("Debe ingresar seleccionar un tipo de usuario.");
        }
    }
}
