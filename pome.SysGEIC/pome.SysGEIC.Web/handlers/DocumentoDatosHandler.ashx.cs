using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using System.IO;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;
using pome.SysGEIC.Comunes;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for DocumentoDatosHandler
    /// </summary>
    public class DocumentoDatosHandler : IHttpHandler, IRequiresSessionState
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
                    case "OBTENER":
                        resultReturn = Obtener(context.Request["idEstudio"],
                                            context.Request["idDocumento"]);
                        break;

                    case "GRABAR":
                        resultReturn = Grabar(context.Request["idEstudio"],
                                            context.Request["idDocumento"],
                                            context.Request["descripcion"],
                                            context.Request["idTipoDocumento"],
                                            context.Request["limitante"],
                                            context.Request["requiereAlertaInactividad"], 
                                            context.Request["mesesAlertaInactividad"], 
                                            context.Request["requiereAlertaInformeAvance"], 
                                            context.Request["mesesAlertaInformeAvance"]);
                        break;

                    case "ELIMINAR-DOCUMENTO":
                        resultReturn = Eliminar(context.Request["idEstudio"],
                                            context.Request["idDocumento"]);
                        break;

                    case "ANULAR-DOCUMENTO":
                        resultReturn = Anular(context.Request["idEstudio"],
                                            context.Request["idDocumento"]);
                        break;

                    case "REACTIVAR-DOCUMENTO":
                        resultReturn = Reactivar(context.Request["idDocumento"]);
                        break;

                    case "GRABAR-VERSION":
                        resultReturn = GrabarVersion(context.Request["idEstudio"],
                                                context.Request["idDocumento"],
                                                context.Request["idVersion"],
                                                context.Request["descripcion"],
                                                context.Request["fecha"],
                                                context.Request["aprobadoANMAT"],
                                                context.Request["fechaAprobadoANMAT"],
                                                context.Request["fechaEstado"],
                                                context.Request["estado"],
                                                context.Request["participantes"]);
                        break;

                    case "GRABAR-DOCUMENTO-Y-VERSION":
                        resultReturn = GrabarDocumentoYVersion(context.Request["idEstudio"],
                                                            context.Request["idDocumento"],
                                                            context.Request["descripcion"],
                                                            context.Request["idTipoDocumento"],
                                                            context.Request["limitante"],
                                                            context.Request["idVersion"],
                                                            context.Request["versionDescripcion"],
                                                            context.Request["versionFecha"],
                                                            context.Request["versionAprobadoANMAT"],
                                                            context.Request["versionFechaAprobadoANMAT"],
                                                            context.Request["versionFechaEstado"],
                                                            context.Request["versionEstado"],
                                                            context.Request["participantes"],
                                                            context.Request["requiereVersion"]);
                        break;

                    case "GRABAR-VERSIONARCHIVOADJUNTO":
                        resultReturn = GrabarArchivoAdjunto(context.Request["idEstudio"],
                                                        context.Request["idDocumento"],
                                                        context.Request["idVersion"]);
                        break;

                    case "GRABAR-VERSIONESTADO":
                        resultReturn = GrabarEstado(context.Request["idEstudio"],
                                                    context.Request["idDocumento"],
                                                    context.Request["idVersion"],
                                                    context.Request["idVersionEstado"],
                                                    context.Request["idEstado"],
                                                    context.Request["fecha"],
                                                    context.Request["idProfesionalAutor"],
                                                    context.Request["idProfesionalPresenta"],
                                                    context.Request["idProfesionalResponsable"]);
                        break;
                    case "ELIMINAR-VERSIONESTADO":
                        resultReturn = EliminarEstado(context.Request["idEstudio"],
                                                    context.Request["idDocumento"],
                                                    context.Request["idVersion"],
                                                    context.Request["idVersionEstado"],
                                                    context.Request["idEstado"]);
                        break;
                    case "GRABAR-VERSIONCOMENTARIO":
                        resultReturn = GrabarComentario(context.Request["idEstudio"],
                                                    context.Request["idDocumento"],
                                                    context.Request["idVersion"],
                                                    context.Request["idVersionComentario"],
                                                    context.Request["fecha"],
                                                    context.Request["idProfesionalAutor"],
                                                    context.Request["observaciones"]);
                        break;
                    case "GRABAR-VERSIONRECORDATORIO":
                        resultReturn = GrabarRecordatorio(context.Request["idEstudio"],
                                                    context.Request["idDocumento"],
                                                    context.Request["idVersion"],
                                                    context.Request["idVersionRecordatorio"],
                                                    context.Request["fecha"],
                                                    context.Request["idProfesionalAutor"],
                                                    context.Request["observaciones"],
                                                    context.Request["pendiente"]);
                        break;
                    case "GRABAR-VERSIONPARTICIPANTES":
                        resultReturn = GrabarParticipantes(context.Request["idDocumento"],
                                                        context.Request["idVersion"],
                                                        context.Request["participantes"]);
                        break;
                    case "OBTENER-ESTADOS":
                        resultReturn = ObtenerEstados(context.Request["idDocumento"],
                                                    context.Request["idVersion"]);
                        break;
                    case "OBTENER-PRIMERESTADO":
                        resultReturn = ObtenerPrimerEstado(context.Request["idTipoDocumento"]);
                        break;
                    case "OBTENER-ACTASDOCUMENTOVERSION":
                        resultReturn = ObtenerActasDocumentoVersion(context.Request["idDocumento"]);
                        break;

                    case "LISTAR-DOCUMENTOS":
                        resultReturn = ListarDocumentos(context.Request["idEstudio"]);
                        break;

                    case "LISTAR-DOCUMENTOSANULADOS":
                        resultReturn = ListarDocumentosAnulados(context.Request["idEstudio"]);
                        break;

                    case "LISTAR-DOCUMENTOVERSIONES":
                        resultReturn = ListarDocumentoVersiones(context.Request["idDocumento"]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
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

        private string Obtener(string idEstudio, string idDocumento)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                return servicio.ObtenerDocumento(idEstudio, idDocumento).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string Eliminar(string idEstudio, string idDocumento)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                servicio.EliminarDocumento(idEstudio, idDocumento);
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string Anular(string idEstudio, string idDocumento)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                servicio.AnularDocumento(idEstudio, idDocumento);
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string Reactivar(string idDocumento)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                servicio.ReactivarDocumento(idDocumento);
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string Grabar(string idEstudio, string idDocumento, string descripion, string idTipoDocumento, string limitante,
                              string requiereAlertaInactividad, string mesesAlertaInactividad, string requiereAlertaInformeAvance, string mesesAlertaInformeAvance)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {                
                ServicioDocumentos servicio = new ServicioDocumentos();

                int _idDocumentoReturn = servicio.GrabarDocumento(idEstudio, idDocumento, descripion, idTipoDocumento, limitante, SessionHelper.ObtenerUsuarioLogin(),
                                                                  requiereAlertaInactividad, mesesAlertaInactividad, requiereAlertaInformeAvance, mesesAlertaInformeAvance);

                return serializer.Serialize(new { result = "OK", id = _idDocumentoReturn });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarVersion(string idEstudio, string idDocumento, string idVersion, string descripion, string fecha, string aprobadoANMAT, string fechaAprobadoANMAT, string fechaEstado, string estado, string participantes)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                servicio.GrabarDocumentoVersion(idEstudio, idDocumento, idVersion, descripion, fecha, aprobadoANMAT, fechaAprobadoANMAT, fechaEstado, estado, participantes, SessionHelper.ObtenerUsuarioLogin());
                
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarDocumentoYVersion(string idEstudio, string idDocumento, string descripcion, string idTipoDocumento, string limitante,
                                               string idVersion, string versionDescripion, string versionFecha, string versionAprobadoANMAT, string versionFechaAprobadoANMAT, string versionFechaEstado, string versionEstado, string participantes,
                                               string requiereVersion)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                int _idDocumentoReturn = servicio.GrabarDocumento(idEstudio, idDocumento, descripcion, idTipoDocumento, limitante, SessionHelper.ObtenerUsuarioLogin(),
                                                                requiereVersion.ConvertirBool(), idVersion, versionDescripion, versionFecha, versionAprobadoANMAT, versionFechaAprobadoANMAT, versionFechaEstado, versionEstado, participantes,
                                                                string.Empty, string.Empty, string.Empty, string.Empty);

                
                return serializer.Serialize(new { result = "OK", id = _idDocumentoReturn });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        public string GrabarArchivoAdjunto(string idEstudio, string idDocumento, string idVersion)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                string archivoTemp = string.Empty;
                string nombreArchivo = string.Empty;
                if (SessionHelper.ObtenerDeSession("DocumentoAdjunto") != null)
                {
                    archivoTemp = SessionHelper.ObtenerDeSession("DocumentoAdjunto").ToString();
                    if (archivoTemp.Trim().Length > 0)
                        nombreArchivo = string.Format("doc{0}_v{1}", idDocumento, idVersion) + Path.GetExtension(archivoTemp);
                }

                servicio.GrabarDocumentoVersionArchivoAdjunto(idEstudio, idDocumento, idVersion, nombreArchivo);

                if (SessionHelper.ObtenerDeSession("DocumentoAdjunto") != null)
                    UtilHelper.GuardarArchivo(archivoTemp, nombreArchivo);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarEstado(string idEstudio, string idDocumento, string idVersion, string idVersionEstado, string idEstado, string fecha, string idProfesionalAutor, string idProfesionalPresenta, string idProfesionalResponsable)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                servicio.GrabarDocumentoVersionEstado(idEstudio, idDocumento, idVersion, idVersionEstado, idEstado, fecha, idProfesionalAutor, idProfesionalPresenta, idProfesionalResponsable, SessionHelper.ObtenerUsuarioLogin());
                
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarEstado(string idEstudio, string idDocumento, string idVersion, string idVersionEstado, string idEstado)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                servicio.EliminarDocumentoVersionEstado(idEstudio, idDocumento, idVersion, idVersionEstado, idEstado);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarComentario(string idEstudio, string idDocumento, string idVersion, string idVersionComentario, string fecha, string idProfesionalAutor, string observaciones)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                servicio.GrabarDocumentoVersionComentario(idEstudio, idDocumento, idVersion, idVersionComentario, fecha, idProfesionalAutor, observaciones);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
        private string GrabarRecordatorio(string idEstudio, string idDocumento, string idVersion, string idVersionRecordatorio, string fecha, string idProfesionalAutor, string observaciones, string pendiente)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                servicio.GrabarDocumentoVersionRecordatorio(idEstudio, idDocumento, idVersion, idVersionRecordatorio, fecha, idProfesionalAutor, observaciones, pendiente);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
        private string GrabarParticipantes(string idDocumento, string idVersion, string participantes)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                servicio.GrabarDocumentoVersionParticipantes(idDocumento, idVersion, participantes, SessionHelper.ObtenerUsuarioLogin());

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
        //TODO: Consultar los estados con linq
        private string ObtenerEstados(string idDocumento, string idVersion)
        {
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                return servicio.DocumentoObtenerEstados(idDocumento, idVersion).SerializaToJson();
            }
            catch { throw; }
        }
        private string ObtenerPrimerEstado(string idTipoDocumento)
        {
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                return servicio.DocumentoPrimerEstado(idTipoDocumento).SerializaToJson();
            }
            catch { throw; }
        }

        private string ObtenerActasDocumentoVersion(string idDocumento)
        {
            try
            {
                ServicioActas servActas = new ServicioActas();
                return servActas.ListarActasXDocumento(idDocumento).SerializaToJson();
            }
            catch { throw; }
        }

        

        private string ListarDocumentos(string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                return servicio.ListarDocumentos(idEstudio).SerializaToJson();                
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarDocumentosAnulados(string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                return servicio.ListarDocumentosAnulados(idEstudio).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarDocumentoVersiones(string idDocumento)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string datosReturn = string.Empty;
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();

                datosReturn = servicio.Obtener(idDocumento).Versiones.ToList<DocumentoVersion>().SerializaToJson();
                return datosReturn;
            }
            catch (Exception ex)
            {
                Logger.LogError("DocumentoDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
}