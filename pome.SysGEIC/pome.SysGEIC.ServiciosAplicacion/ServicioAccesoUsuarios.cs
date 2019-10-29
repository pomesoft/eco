using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Repositorios;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public class ServicioAccesoUsuarios
    {

        public ServicioAccesoUsuarios() { }

        public List<MenuPrincipal> ObtenerMenuPrincipal()
        {
            RepositoryGenerico<MenuPrincipal> menuRep = new RepositoryGenerico<MenuPrincipal>();
                        
            List<MenuPrincipal> listMenu = menuRep.ObtenerTodos().ToList<MenuPrincipal>()
                                                                .OrderBy(item => item.Orden)
                                                                .ToList<MenuPrincipal>();

            return listMenu;
        }

        public List<MenuDTO> ListarMenuPrincipalAccesos(string idTipoUsuario)
        {
            List<MenuDTO> menuReturn = new List<MenuDTO>();
            List<MenuPrincipal> menu = this.ObtenerMenuPrincipal();

            TipoUsuario tipoUsuario = this.TipoUsuarioObtener(idTipoUsuario.ConvertirInt());

            TipoUsuarioAcceso tipoUsuarioAcceso = null;
            NivelAcceso permiso = null;

            menu.ForEach(delegate(MenuPrincipal menuPrincipal) 
            {
                List<MenuSecundario> items = menuPrincipal.Items.ToList<MenuSecundario>()
                                                              .OrderBy(item => item.Orden)
                                                              .ToList<MenuSecundario>();

                if (items == null || items.Count == 0)
                {
                    tipoUsuarioAcceso = tipoUsuario.Permisos.ToList<TipoUsuarioAcceso>().Find(delegate(TipoUsuarioAcceso acceso) { return acceso.IdMenuPrincipal == menuPrincipal.Id; });
                    if (tipoUsuarioAcceso != null)
                        permiso = tipoUsuarioAcceso.NivelAcceso;
                    else
                        permiso = new NivelAcceso((int)NIVEL_ACCESO.SIN_ACCESO, "SIN ACCESO");
                }
                else
                    permiso = new NivelAcceso(0, string.Empty);

                menuReturn.Add(new MenuDTO(menuPrincipal.Id, menuPrincipal.Descripcion, -1, string.Empty, permiso.Id, permiso.Descripcion));

                items.ForEach(delegate(MenuSecundario menuSecundario) 
                {
                    tipoUsuarioAcceso = tipoUsuario.Permisos.ToList<TipoUsuarioAcceso>().Find(delegate(TipoUsuarioAcceso acceso) { return acceso.IdMenuSecundario == menuSecundario.Id; });
                    if (tipoUsuarioAcceso != null)
                        permiso = tipoUsuarioAcceso.NivelAcceso;
                    else
                        permiso = new NivelAcceso((int)NIVEL_ACCESO.SIN_ACCESO, "SIN ACCESO");

                    menuReturn.Add(new MenuDTO(menuPrincipal.Id, string.Empty, menuSecundario.Id, menuSecundario.Descripcion, permiso.Id, permiso.Descripcion));
                });
            });

            return menuReturn;
        }


        public Usuario LoginUsuario(string usuario, string clave)
        {
            UsuariosRepository usuariosRep = new UsuariosRepository();
            return usuariosRep.ObtenerUsuario(usuario, clave);
        }
        public Usuario ObtenerUsuario(int id)
        {
            UsuariosRepository usuariosRep = new UsuariosRepository();
            return usuariosRep.ObtenerUsuario(id);
        }
        public List<Usuario> ObtenerUsuarios(string apellidoBuscar, string nombreBuscar)
        {
            UsuariosRepository usuariosRep = new UsuariosRepository();
            Usuario usrBuscar = new Usuario();

            usrBuscar.Apellido = apellidoBuscar == null ? string.Empty : apellidoBuscar.Trim();
            usrBuscar.Nombre = nombreBuscar == null ? string.Empty : nombreBuscar.Trim();

            return usuariosRep.ObtenerUsuarios(usrBuscar);
        }
        public void GrabarUsuario(Usuario usr)
        {
            usr.Validar();
            UsuariosRepository usuariosRep = new UsuariosRepository();
            usuariosRep.ActualizarUsuario(usr);
        }
        public void EliminarUsuario(Usuario usr)
        {
            UsuariosRepository usuariosRep = new UsuariosRepository();
            usuariosRep.EliminarUsuario(usr);
        }



        #region TipoUsuario
        public List<TipoUsuario> TiposUsuariosObtenerVigentes()
        {
            RepositoryGenerico<TipoUsuario> repository = new RepositoryGenerico<TipoUsuario>();
            return repository.ObtenerTodosVigentes().ToList<TipoUsuario>();
        }
        public List<TipoUsuario> TiposUsuariosObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<TipoUsuario> repository = new RepositoryGenerico<TipoUsuario>();

            return repository.ObtenerTodosVigentes(descripcion).ToList<TipoUsuario>();
        }
        public TipoUsuario TipoUsuarioObtener(int id)
        {
            RepositoryGenerico<TipoUsuario> repository = new RepositoryGenerico<TipoUsuario>();
            return repository.Obtener(id);
        }
        public void TipoUsuarioGrabar(string id, string descripcion, string permisos)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            TipoUsuario tipoUSR = null;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                tipoUSR = new TipoUsuario();
            else
                tipoUSR = this.TipoUsuarioObtener(_id);

            tipoUSR.Descripcion = descripcion == null ? string.Empty : descripcion;
            tipoUSR.Vigente = true;

            if (permisos.Trim().Length > 0)
            {
                TipoUsuarioAcceso tipoUsuarioAcceso = null;
                string[] permisosSeleccionados = permisos.Substring(1).Split(';');

                foreach (string permiso in permisosSeleccionados)
                {
                    tipoUsuarioAcceso = new TipoUsuarioAcceso();
                    string[] menuAccesos = permiso.Split(',');

                    tipoUsuarioAcceso.IdMenuPrincipal = menuAccesos[0].ConvertirInt();
                    tipoUsuarioAcceso.IdMenuSecundario = menuAccesos[1].ConvertirInt();
                    tipoUsuarioAcceso.NivelAcceso = servParametricas.ObtenerObjeto<NivelAcceso>(menuAccesos[2].ConvertirInt());

                    tipoUSR.AsignarPermiso(tipoUsuarioAcceso);
                }
            }
            
            RepositoryGenerico<TipoUsuario> repository = new RepositoryGenerico<TipoUsuario>();
            repository.Actualizar(tipoUSR);
        }
        public void TipoUsuarioEliminar(string id)
        {
            TipoUsuario tipoUSR = null;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe tipo de usuario que desea eliminar.");

            tipoUSR = this.TipoUsuarioObtener(_id);
            if (tipoUSR == null)
                throw new ApplicationException("No existe tipo de usuario que desea eliminar.");

            tipoUSR.Vigente = false;

            RepositoryGenerico<TipoUsuario> repository = new RepositoryGenerico<TipoUsuario>();
            repository.Actualizar(tipoUSR);
        }
        #endregion

    }
}
