using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades 
{
    [Serializable]
    [DataContract]    
    public class TipoUsuario : EntidadBaseParametrica 
    {
        public virtual IList<TipoUsuarioAcceso> Permisos { get; set; }

        public TipoUsuario() 
        {
            Permisos = new List<TipoUsuarioAcceso>();
        }

        public void AsignarPermiso(TipoUsuarioAcceso permiso)
        {
            TipoUsuarioAcceso tipoUsuarioAcceso = null;

            tipoUsuarioAcceso = Permisos.ToList<TipoUsuarioAcceso>().Find(delegate(TipoUsuarioAcceso acceso) 
            {
                return acceso.IdMenuPrincipal == permiso.IdMenuPrincipal && acceso.IdMenuSecundario == permiso.IdMenuSecundario;
            });

            if (tipoUsuarioAcceso == null)
            {
                tipoUsuarioAcceso = new TipoUsuarioAcceso();
                tipoUsuarioAcceso.Id = -1;
                tipoUsuarioAcceso.IdMenuPrincipal = permiso.IdMenuPrincipal;
                tipoUsuarioAcceso.IdMenuSecundario = permiso.IdMenuSecundario;
            }
            
            tipoUsuarioAcceso.NivelAcceso = permiso.NivelAcceso;
            tipoUsuarioAcceso.TipoUsuario = this;

            if (tipoUsuarioAcceso.Id == -1)
                Permisos.Add(tipoUsuarioAcceso);
        }
    }
}
