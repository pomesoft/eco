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

namespace pome.SysGEIC.Web
{
    /// <summary>
    /// Summary description for BandejaInicioHandler
    /// </summary>
    public class BandejaInicioHandler : IHttpHandler, IRequiresSessionState
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
                    case "OBTENER-ESTUDIO":
                        resultReturn = ObtenerEstudio(context.Request["idEstudio"],
                                                    context.Request["idActa"],
                                                    context.Request["soloPendientes"]);
                        break;
                    case "LISTAR":
                        resultReturn = Listar();
                        break;

                    case "LISTAR-NOFINALIZADOS":
                        resultReturn = ListarSinFinalizar();
                        Logger.LogInfo("BandejaInicioHandler", "accion:" + accion + " - " + resultReturn);
                        break;

                    case "BUSCAR-ESTUDIOS":
                        resultReturn = BuscarEstudios(context.Request["codigo"],
                                                    context.Request["abreviado"],
                                                    context.Request["nombreCompleto"]);
                        break;
                    case "BUSCAR-DOCUMENTOS-DEL-ESTUDIO":
                        resultReturn = ObtenerDocumentosDelEstudio(context.Request["idEstudio"],
                                                    context.Request["tipoDocumento"],
                                                    context.Request["nombreDocumento"]);
                        break;
                    case "BUSCAR-RECORDATORIOS-DEL-ESTUDIO":
                        resultReturn = ObtenerRecordatoriosEstudio(context.Request["idEstudio"],
                                                    context.Request["pendiente"]);
                        break;

                    case "PROCESAR-SEMAFOROS":
                        resultReturn = ProcesarSemaforos();
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

        private string ObtenerEstudio(string idEstudio, string idActa, string soloPendientes)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                Estudio estudio = servicio.Obtener(idEstudio);
                SessionHelper.GuardarEnSession(string.Format("Estudio_{0}", estudio.Id.ToString()), estudio);

                bool _soloPendientes = soloPendientes.ConvertirBool();

                EstudioDTO estudioDTO = servicio.ObtenerDTO(estudio, idActa, _soloPendientes);
                estudiosReturn = estudioDTO.SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }

            return estudiosReturn;
        }

        private string Listar()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                estudiosReturn = servicio.ObtenerVigentes().SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }

            return estudiosReturn;
        }

        private string ProcesarSemaforos()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();
                servicio.ProcesarSemaforoEstudios();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }

            return serializer.Serialize(new { result = "OK", message = "Se procesaron todos los estudios OK." });
        }


        private string ListarSinFinalizar()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                estudiosReturn = servicio.ObtenerNoFinalizados().SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }

            return estudiosReturn;
        }

        private string BuscarEstudios(string codigo, string abreviado, string nombreCompleto)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                estudiosReturn = servicio.BuscarEstudios(codigo, abreviado, nombreCompleto)
                                        .SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }

            return estudiosReturn;
        }


        private string ObtenerRecordatoriosEstudio(string idEstudio, string pendiente)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();
                Estudio estudio = SessionHelper.ObtenerDeSession(string.Format("Estudio_{0}", idEstudio)) as Estudio;

                estudiosReturn = servicio.ObtenerRecordatorios(estudio, pendiente).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }

            return estudiosReturn;
        }

        private string ObtenerDocumentosDelEstudio(string idEstudio, string tipoDocumento, string nombreDocumento)
        {
            string estudiosReturn = string.Empty;
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                Estudio estudio = servicio.BuscarDocumentosDelEstudio(idEstudio, tipoDocumento, nombreDocumento);


                EstudioDTO estudioDTO = servicio.ObtenerDTO(estudio, false);
                estudiosReturn = estudioDTO.SerializaToJson();
            }
            catch { throw; }

            return estudiosReturn;
        }   
    }
}