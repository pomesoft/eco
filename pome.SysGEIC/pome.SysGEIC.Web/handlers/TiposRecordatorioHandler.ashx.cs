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
    /// Summary description for TiposRecordatorioHandler
    /// </summary>
    public class TiposRecordatorioHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultReturn = string.Empty;
            string accion = string.Empty;

            try
            {
                if (context.Request["accion"] != null)
                    accion = context.Request["accion"].ToString();

                if (accion == string.Empty || accion.Equals("LISTAR"))
                    resultReturn = Listar(context.Request["descripcion"]);

                else if (accion.Equals("OBTENER"))
                    resultReturn = Obtener(context.Request["id"]);

                else if (accion.Equals("GRABAR"))
                    resultReturn = Grabar(context.Request["id"],
                                        context.Request["datos"]).ToString();

                else if (accion.Equals("ELIMINAR"))
                    resultReturn = Eliminar(context.Request["id"]).ToString();
                                
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType().Name, ex);
                resultReturn = serializer.Serialize(new { result = "Error", message = ex.Message });
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
        
        private string Listar(string descripcion)
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(servicio.TipoRecordatoriosObtenerVigentes(descripcion));
        }

        private string Obtener(string id)
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();

            if (id.ConvertirInt() != -1)
                return servicio.TipoRecordatorioObtener(id.ConvertirInt()).SerializaToJson();
            else
                return string.Empty;
        }
        private object Grabar(string id, string datos)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioRecordatorios servicio = new ServicioRecordatorios();

                servicio.TipoRecordatorioGrabar(id, datos);

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

                servicio.TipoRecordatorioEliminar(id);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
    
}