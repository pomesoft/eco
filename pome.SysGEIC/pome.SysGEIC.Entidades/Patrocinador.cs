using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Patrocinador : EntidadBaseParametrica
    {
        public Patrocinador() { }

        public override void Validar()
        {
            if (Descripcion.Trim().Length == 0)
                throw new ApplicationException("Debe ingresar razón social.");
        }
    }
}
