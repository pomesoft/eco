using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
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
    /// Summary description for CartaRespuestaHandler
    /// </summary>
    public class CartaRespuestaHandler : IHttpHandler
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
                    case "OBTENER-CARTARESPUESTAMODELO":
                        resultReturn = Obtener(context.Request["idCartaRespuestaModelo"]);
                        break;

                    case "GRABAR-CARTARESPUESTAMODELO":
                        resultReturn = Grabar(context.Request["id"],
                                            context.Request["datos"]);
                        break;

                    case "ELIMINAR-CARTARESPUESTAMODELO":
                        resultReturn = Eliminar(context.Request["id"]);
                        break;

                    case "LISTAR-CARTARESPUESTAMODELO":
                        resultReturn = Listar(context.Request["descripcion"]);
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

        private string Obtener(string idCartaRespuestaModelo)
        {
            ServicioActas servicio = new ServicioActas();
            return servicio.CartaRespuestaModeloObtener(idCartaRespuestaModelo.ConvertirInt()).SerializaToJson();
        }

        private string Listar(string descripcion)
        {
            ServicioActas servicio = new ServicioActas();

            List<Entidades.CartaRespuestaModelo> modelosCartaRespuesta = servicio.CartaRespuestaModeloObtenerVigentes(string.Empty);

            return modelosCartaRespuesta.SerializaToJson();
        }

        private string Grabar(string id, string datos)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.CartaRespuestaModeloGrabar(id, datos);

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
                ServicioActas servicio = new ServicioActas();

                servicio.CartaRespuestaModeloEliminar(id);

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