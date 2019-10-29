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
    /// Summary description for ListasEmailsHandler
    /// </summary>
    public class ListasEmailsHandler : IHttpHandler
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

                switch (accion)
                {
                    case "OBTENER-LISTASEMAILS":
                        resultReturn = Obtener(context.Request["id"]);
                        break;

                    case "LISTAR-LISTASEMAILS":
                        resultReturn = Listar();
                        break;

                    case "GRABAR-LISTASEMAILS":
                        resultReturn = Grabar(context.Request["id"],
                                            context.Request["descripcion"],
                                            context.Request["emails"]);
                        break;

                    case "ELIMINAR-LISTASEMAILS":
                        resultReturn = Eliminar(context.Request["id"]);
                        break;
                }

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

        private string Obtener(string id)
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();
            return servicio.ListaEmailsObtener(id.ConvertirInt()).SerializaToJson();
        }

        private string Listar()
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();
            return servicio.ListaEmailsObtenerVigentes().SerializaToJson();
        }

        private string Grabar(string id, string descripcion, string emails)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioRecordatorios servicio = new ServicioRecordatorios();

                servicio.ListaEmailsGrabar(id, descripcion, emails);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType().Name, ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string Eliminar(string id)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioRecordatorios servicio = new ServicioRecordatorios();

                servicio.ListaEmailsEliminar(id);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType().Name, ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
}