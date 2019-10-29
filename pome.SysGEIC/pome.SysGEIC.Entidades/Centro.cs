using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Centro : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string Direccion { get; set; }
        [DataMember]
        public virtual string Telefono { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual int IdCentroVinculado { get; set; }

        [DataMember]
        public virtual string DatosContacto
        {
            set { }
            get { return string.Format("{0} {1} {2}", Direccion, Telefono, Email); }
        }
        public Centro() { }

        public override void Validar()
        {
            if (Descripcion.Trim().Length == 0)
                throw new ApplicationException("Debe ingresar razón social.");
        }
    }
}
