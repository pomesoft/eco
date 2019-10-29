using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;
using pome.SysGEIC.Comunes;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for TiposUsuarioHandler
    /// </summary>
    public class TiposUsuarioHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultReturn = string.Empty;
            string accion = string.Empty;

            if (context.Request["accion"] != null)
                accion = context.Request["accion"].ToString();

            if (accion == string.Empty || accion.Equals("LISTAR"))
                resultReturn = Listar(context.Request["descripcion"]);

            else if (accion.Equals("OBTENER"))
                resultReturn = Obtener(context.Request["id"]);

            else if (accion.Equals("GRABAR"))
                resultReturn = Grabar(context.Request["id"],
                                    context.Request["descripcion"],
                                    context.Request["permisos"]);

            else if (accion.Equals("ELIMINAR"))
                resultReturn = Eliminar(context.Request["id"]).ToString();

            else if (accion.Equals("LISTAR_MENUACCESOS"))
                resultReturn = ListarMenuPrincipalAccesos(context.Request["id"]).ToString();

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

        private string Listar(string descripcion)
        {
            ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();

            return servicio.TiposUsuariosObtenerVigentes(descripcion).SerializaToJson();
        }

        private string Obtener(string id)
        {
            ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();
            int _id = -1;
            if (int.TryParse(id, out _id))
                return servicio.TipoUsuarioObtener(_id).SerializaToJson();
            else
                return string.Empty;
        }
        private string Grabar(string id, string descripcion, string permisos)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();
                
                servicio.TipoUsuarioGrabar(id, descripcion, permisos);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private object Eliminar(string id)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();
                
                servicio.TipoUsuarioEliminar(id);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarMenuPrincipalAccesos(string idTipoUsuario)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();
                return servicio.ListarMenuPrincipalAccesos(idTipoUsuario).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("TiposUsuarioHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
}