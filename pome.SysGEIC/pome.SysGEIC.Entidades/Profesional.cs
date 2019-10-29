using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Profesional : EntidadBase
    {
        [DataMember]
        public virtual string Apellido { get; set; }
        [DataMember]
        public virtual string Nombre { get; set; }
        [DataMember]
        public virtual string Titulo { get; set; }
        [DataMember]
        public virtual bool Vigente { get; set; }
        [DataMember]
        public virtual TipoProfesional TipoProfesional { get; set; }
        [DataMember]
        public virtual RolComite RolComite { get; set; }
        [DataMember]
        public virtual int OrdenActa { get; set; }
        [DataMember]
        public virtual string MatriculaNacional { get; set; }
        [DataMember]
        public virtual string MatriculaProvincial { get; set; }
        [DataMember]
        public virtual string TelefonoParticular { get; set; }
        [DataMember]
        public virtual string TelefonoLaboral { get; set; }
        [DataMember]
        public virtual string Celular { get; set; }
        [DataMember]
        public virtual string Direccion { get; set; }
        [DataMember]
        public virtual string Email { get; set; }

        [DataMember]
        public virtual string NombreCompleto
        {
            set { }
            get { return string.Format("{0} {1} {2}", Titulo, Apellido, Nombre); }
        }
        [DataMember]
        public virtual string NombreYApellido
        {
            set { }
            get { return string.Format("{0} {1} {2}", Titulo, Nombre, Apellido); }
        }
        [DataMember]
        public virtual string DatosContacto
        {
            set { }
            get
            {
                return string.Format("{0}{1}{2}{3}", (Celular != null && Celular.Trim().Length > 0 ? " Teléfono: " : string.Empty), Celular,
                                                     (Email != null && Email.Trim().Length > 0 ? " Email: " : string.Empty), Email);
            }
        }

        public Profesional() { }

        public override string ToString()
        {
            return string.Format("{0}", this.NombreCompleto);
        }

        public virtual void Validar()
        {
            StringBuilder errores = new StringBuilder();

            if (Apellido.Trim().Length == 0)
                errores.AppendLine("Debe ingresar apellido" + Constantes.SaldoLinea);
            if (Nombre.Trim().Length == 0)
                errores.AppendLine("Debe ingresar nombre" + Constantes.SaldoLinea);
            if (TipoProfesional == null)
                errores.AppendLine("Debe seleccionar tipo de profesional" + Constantes.SaldoLinea);

            if (errores.Length != 0)
                throw new ApplicationException(errores.ToString());
        }
    }
}
