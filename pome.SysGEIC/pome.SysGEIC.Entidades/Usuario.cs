using System.Collections.Generic; 
using System.Text; 
using System;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades 
{
    [Serializable]
    [DataContract]
    public class Usuario : EntidadBase 
    {        
        [DataMember]
        public virtual string Apellido { get; set; }
        [DataMember]
        public virtual string Nombre { get; set; }
        [DataMember]
        public virtual string LoginUsuario { get; set; }
        [DataMember]
        public virtual string LoginClave { get; set; }
        [DataMember]
        public virtual bool Vigente { get; set; }
        [DataMember]
        public virtual byte[] TimesStamp { get; set; }
        [DataMember]
        public virtual string Mail { get; set; }
        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual IList<UsuarioAcceso> Permisos { get; set; }

        public virtual bool EsAdministrador
        {
            get
            {
                if (TipoUsuario == null)
                    return false;
                else
                    return TipoUsuario.Id == 1;
            }
        }

        public virtual string NombreCompleto 
        {
            get { return string.Format("{0} {1}", Apellido, Nombre); } 
        }

        public Usuario() 
        {
            Permisos = new List<UsuarioAcceso>();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} {2}", Id, Apellido, Nombre); 
        }

        public virtual void Validar()
        {
            StringBuilder errores = new StringBuilder();

            if (Apellido.Trim().Length == 0)
                errores.AppendLine("Debe ingresar apellido" + Constantes.SaldoLinea);
            if (Nombre.Trim().Length == 0)
                errores.AppendLine("Debe ingresar nombre" + Constantes.SaldoLinea);
            if (LoginUsuario.Trim().Length == 0)
                errores.AppendLine("Debe ingresar login" + Constantes.SaldoLinea);
            if(TipoUsuario==null)
                errores.AppendLine("Debe seleccionar tipo de usuario" + Constantes.SaldoLinea);

            if (errores.Length != 0)
                throw new ApplicationException(errores.ToString());
        }

    }
}
