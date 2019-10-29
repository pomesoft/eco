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
    /// Summary description for RecordatoriosHandler
    /// </summary>
    public class RecordatoriosHandler : IHttpHandler
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
                    case "OBTENER-RECORDATORIO":
                        resultReturn = Obtener(context.Request["idRecordatorio"]);
                        break;

                    case "GRABAR-RECORDATORIO":
                        resultReturn = Grabar(context.Request["id"],
                                            context.Request["datos"],
                                            context.Request["fechaAlta"],
                                            context.Request["fechaActivacion"], 
                                            context.Request["idEstudio"], 
                                            context.Request["documentos"]);
                        break;

                    case "LISTAR-RECORDATORIOS":
                        resultReturn = Listar();
                        break;

                    case "BUSCAR-RECORDATORIOS":
                        resultReturn = BuscarRecordatorios(context.Request["tipoRecordatorio"],
                                                        context.Request["descripcion"],
                                                        context.Request["codigoEstudio"],
                                                        context.Request["estado"]);
                        break;

                    case "LISTAR-RECORDATORIOS-ACTIVOS-POPUP":
                        resultReturn = ListarActivosPopup();
                        break;

                    case "LISTAR-TIPOSRECORDATORIO":
                        resultReturn = ListarTiposRecordatorio();
                        break;

                    case "LISTAR-ESTADOSRECORDATORIO":
                        resultReturn = ListarEstadosRecordatorio();
                        break;

                    case "CAMBIAR-ESTADO-RECORDATORIO":
                        resultReturn = CambiarEstado(context.Request["id"],
                                              context.Request["idEstado"]);
                        break;

                    case "ENVIAR-EMAILRECORDATORIO":
                        resultReturn = EnviarMailRecordatorio(context.Request["idRecordatorio"]);
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

        private string Obtener(string idRecordatorio)
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();
            return servicio.Obtener(idRecordatorio).SerializaToJson();
        }

        private string Listar()
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();
            
            List<Recordatorio> recordatoriosActivos = servicio.Listar();
            
            return recordatoriosActivos.SerializaToJson();
        }

        private string ListarActivosPopup()
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();
            return servicio.ListarActivosPopup().SerializaToJson();
        }

        private string Grabar(string id, string datos, string fechaAlta, string fechaActivacion, string idEstudio, string documentos)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioRecordatorios servicio = new ServicioRecordatorios();

                servicio.Grabar(id, datos, fechaAlta, fechaActivacion, idEstudio, documentos);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType().Name, ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarTiposRecordatorio()
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();            
            return servicio.TipoRecordatoriosObtenerVigentes(string.Empty).SerializaToJson();
        }

        private string ListarEstadosRecordatorio()
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();
            return servicio.EstadosRecordatoriosObtenerVigentes(string.Empty).SerializaToJson();
        }

        private string BuscarRecordatorios(string tipoRecordatorio, string descripcion, string codigoEstudio, string estado)
        {
            ServicioRecordatorios servicio = new ServicioRecordatorios();
            return servicio.BuscarRecordatorios(tipoRecordatorio, descripcion, codigoEstudio, estado).SerializaToJson();
        }

        private string CambiarEstado(string id, string idEstado)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioRecordatorios servicio = new ServicioRecordatorios();

                servicio.CambiarEstado(id, idEstado);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType().Name, ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EnviarMailRecordatorio(string idRecordatorio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioRecordatorios servicio = new ServicioRecordatorios();
                servicio.EnviarMail(idRecordatorio);

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