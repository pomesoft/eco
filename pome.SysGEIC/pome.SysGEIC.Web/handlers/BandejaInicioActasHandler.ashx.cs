using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.SessionState;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;
using pome.SysGEIC.Comunes;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for BandejaInicioActasHandler
    /// </summary>
    public class BandejaInicioActasHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultReturn = string.Empty;
            string accion = string.Empty;

            try
            {
                if (context.Request["accion"] != null)
                    accion = context.Request["accion"].ToString();

                switch (accion)
                {
                    case "OBTENER-ACTA":
                        resultReturn = ObtenerActa(context.Request["idActa"]);
                        break;
                    case "LISTAR":
                        resultReturn = Listar();
                        break;
                    case "BUSCAR-ACTAS":
                        resultReturn = BuscarActas(context.Request["cerrada"],
                                                context.Request["descripcion"]);                      
                        break;                    
                    case "LISTAR-ACTAESTUDIOSDOCUMENTOS":
                        resultReturn = ListarActaEstudiosDocumentos(context.Request["idActa"],
                                                                context.Request["idEstudio"]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioHandler", ex);
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

        private string ObtenerActa(string idActa)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioActas servicio = new ServicioActas();

                Acta acta = servicio.Obtener(idActa);
                estudiosReturn = acta.SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioActaHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }

            return estudiosReturn;
        }

        private string Listar()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string datosReturn = string.Empty;
            try
            {
                ServicioActas servicio = new ServicioActas();

                datosReturn = servicio.ListarActasDTO(string.Empty, string.Empty, "2").SerializaToJson();

            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioActaHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
            return datosReturn;
        }

        private string BuscarActas(string cerrada, string descripcion)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string datosReturn = string.Empty;
            try
            {
                ServicioActas servicio = new ServicioActas();
                
                datosReturn = servicio.ListarActasDTO(cerrada, descripcion, "2").SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioActaHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
            return datosReturn;
        }


        private string ListarActaEstudiosDocumentos(string idActa, string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioActas servicio = new ServicioActas();

                estudiosReturn = servicio.ListarDocumentosDelEstudiosDelActa(idActa, idEstudio).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioActaHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
            return estudiosReturn;
        }
    }
}