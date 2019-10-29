using System.Collections.Generic; 
using System.Text; 
using System;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades 
{
    [DataContract]
    public class UsuarioAcceso : EntidadAcceso
    {
        public virtual Usuario Usuario { get; set; }
        
        public UsuarioAcceso() { }

        public override void Validar()
        {
            base.Validar();

            if (Usuario == null)
                throw new ApplicationException("Debe ingresar seleccionar un usuario.");
        }
    }
}
