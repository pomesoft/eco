using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;
using System.Web.SessionState;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web
{
    /// <summary>
    /// Summary description for UsuariosHandler
    /// </summary>
    public class UsuariosHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultReturn = string.Empty;
            string accion = string.Empty;

            if (context.Request["accion"] != null)
                accion = context.Request["accion"].ToString();

            if (resultReturn == string.Empty)
            {
                switch (accion)
                {
                    case "OBTENER":
                        resultReturn = Obtener(context.Request["id"]);
                        break;
                    case "OBTENER_USUARIOLOGIN":
                        resultReturn = ObtenerUsuarioLogin();
                        break;
                    case "GRABAR":
                        resultReturn = Grabar(context.Request["id"],
                                        context.Request["apellido"],
                                        context.Request["nombre"],
                                        context.Request["loginUsuario"],
                                        context.Request["email"],
                                        context.Request["tipoUsuario"]).ToString();
                        break;
                    case "GRABAR_CAMBIOCLAVE":
                        resultReturn = CambiarClave(context.Request["claveActual"],
                                                    context.Request["nuevaClave1"],
                                                    context.Request["nuevaClave2"]).ToString();
                        break;
                    case "ELIMINAR":
                        resultReturn = Eliminar(context.Request["id"]).ToString();
                        break;
                    case "LISTAR_TIPOSUSUARIO":
                        resultReturn = ListarTiposUsuario();
                        break;
                    default:
                        resultReturn = Listar(context.Request["apellidoBuscar"],
                                            context.Request["nombreBuscar"]);
                        break;
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(resultReturn);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string Listar(string apellidoBuscar, string nombreBuscar)
        {            
            ServicioAccesoUsuarios servUsuarios = new ServicioAccesoUsuarios();
            return servUsuarios.ObtenerUsuarios(apellidoBuscar, nombreBuscar).SerializaToJson();
        }

        private string Obtener(string id)
        {
            ServicioAccesoUsuarios servUsuarios = new ServicioAccesoUsuarios();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            int _id = -1;
            if (int.TryParse(id, out _id))
                return servUsuarios.ObtenerUsuario(_id).SerializaToJson();
            else
                return string.Empty;
        }

        private string ObtenerUsuarioLogin()
        {
            return SessionHelper.ObtenerUsuarioLogin().SerializaToJson();
        }

        private object Grabar(string id, string apellido, string nombre, string loginUsuario, string email, string tipoUsuario)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try            
            {
                string msg = VerificarPermisos((short)Permisos.AGREGAR_EDITAR_);
                if (msg != string.Empty) return msg;

                ServicioAccesoUsuarios servUsuarios = new ServicioAccesoUsuarios();
                Usuario usr = null;

                int _id = -1;
                if (!int.TryParse(id, out _id))
                    _id = -1;

                if (_id == -1)
                    usr = new Usuario();
                else
                    usr = servUsuarios.ObtenerUsuario(_id);

                usr.Apellido = apellido == null ? string.Empty : apellido;
                usr.Nombre = nombre == null ? string.Empty : nombre;
                usr.LoginUsuario = loginUsuario == null ? string.Empty : loginUsuario;
                if (_id == -1)
                    usr.LoginClave = "1";
                usr.Mail = email == null ? string.Empty : email;                
                int _idTipoUsuario = -1;
                if (!int.TryParse(tipoUsuario, out _idTipoUsuario))
                    _idTipoUsuario = -1;
                usr.TipoUsuario = servUsuarios.TipoUsuarioObtener(_idTipoUsuario);
                
                servUsuarios.GrabarUsuario(usr);

                return serializer.Serialize(new { result = "OK" });
            }
            catch(Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }            
        }

        private object Eliminar(string id)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                string msg = VerificarPermisos((short)Permisos.ELIMINAR_);
                if (msg != string.Empty) return msg;

                ServicioAccesoUsuarios servUsuarios = new ServicioAccesoUsuarios();
                Usuario usr = null;

                int _id = -1;
                if (!int.TryParse(id, out _id))
                    throw new ApplicationException("No existe usuario que desea eliminar.");

                usr = servUsuarios.ObtenerUsuario(_id);
                if (usr == null)
                    throw new ApplicationException("No existe usuario que desea eliminar.");

                servUsuarios.EliminarUsuario(usr);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarTiposUsuario()
        {
            ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(servicio.TiposUsuariosObtenerVigentes());
        }

        private object CambiarClave(string claveActual, string nuevaClave1, string nuevaClave2)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioAccesoUsuarios servUsuarios = new ServicioAccesoUsuarios();
                Usuario userLogin = SessionHelper.ObtenerUsuarioLogin();

                Usuario user = servUsuarios.ObtenerUsuario(userLogin.Id);

                if (!user.LoginClave.Equals(claveActual))
                    throw new ApplicationException("Clave actual incorrecta.");

                if (nuevaClave1.Trim().Length == 0 || nuevaClave2.Trim().Length == 0 || !nuevaClave1.Equals(nuevaClave2))
                    throw new ApplicationException("Nueva clave incorrecta.");

                user.LoginClave = nuevaClave1;

                servUsuarios.GrabarUsuario(user);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string VerificarPermisos(short permiso)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Usuario userLoguin = SessionHelper.ObtenerUsuarioLogin();
            if (userLoguin != null && userLoguin.EsAdministrador)
                return string.Empty;
            else
                return serializer.Serialize(new { result = "Error", message = "El usuario no tiene permisos para realizar esta acción." });
        }
    }
}