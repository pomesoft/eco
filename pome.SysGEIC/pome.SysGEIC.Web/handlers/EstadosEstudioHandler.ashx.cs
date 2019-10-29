using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for EstadosEstudioHandler
    /// </summary>
    public class EstadosEstudioHandler : IHttpHandler
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
                                    context.Request["final"]).ToString();

            else if (accion.Equals("ELIMINAR"))
                resultReturn = Eliminar(context.Request["id"]).ToString();

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
            ServicioParametricas servicio = new ServicioParametricas();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(servicio.EstadoEstudioObtenerVigentes(descripcion));
        }

        private string Obtener(string id)
        {
            ServicioParametricas servicio = new ServicioParametricas();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            int _id = -1;
            if (int.TryParse(id, out _id))
                return serializer.Serialize(servicio.EstadoEstudioObtener(_id));
            else
                return string.Empty;
        }
        private object Grabar(string id, string descripcion, string final)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioParametricas servicio = new ServicioParametricas();

                servicio.EstadoEstudioGrabar(id, descripcion, final);

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
                ServicioParametricas servicio = new ServicioParametricas();

                servicio.EstadoEstudioEliminar(id);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
}