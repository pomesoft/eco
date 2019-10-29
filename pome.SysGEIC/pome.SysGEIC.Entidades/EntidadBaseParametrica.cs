using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class EntidadBaseParametrica : EntidadBase
    {
        [DataMember]
        public virtual string Descripcion { get; set; }
        [DataMember]
        public virtual bool Vigente { get; set; }

        public EntidadBaseParametrica() 
        {
            this.Descripcion = string.Empty;
        }

        public override string ToString()
        {
            return string.Format("{0}", Descripcion);
        }

        public virtual void Validar()
        {
            if (Descripcion.Trim().Length == 0)
                throw new ApplicationException("Debe ingresar descripción.");
        }
    }
}
