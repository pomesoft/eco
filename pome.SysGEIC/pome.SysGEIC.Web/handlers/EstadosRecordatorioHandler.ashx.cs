using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;
using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for EstadosRecordatorioHandler
    /// </summary>
    public class EstadosRecordatorioHandler : IHttpHandler
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
                                    context.Request["descripcion"]).ToString();

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
            ServicioRecordatorios servicio = new ServicioRecordatorios();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(servicio.EstadosRecordatoriosObtenerVigentes(descripcion));
        }

        private string Obtener(string id)
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();

            if (id.ConvertirInt() != -1)
                return servicio.EstadoRecordatorioObtener(id.ConvertirInt()).SerializaToJson();
            else
                return string.Empty;
        }
        private object Grabar(string id, string descripcion)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioRecordatorios servicio = new ServicioRecordatorios();

                servicio.EstadoRecordatorioGrabar(id, descripcion);

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
                ServicioRecordatorios servicio = new ServicioRecordatorios();

                servicio.EstadoRecordatorioEliminar(id);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
}