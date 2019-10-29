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
    /// Summary description for ActasHandler
    /// </summary>
    public class ActasHandler : IHttpHandler
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
                    case "OBTENER":
                        resultReturn = Obtener(context.Request["idActa"]);
                        break;

                    case "LISTAR-ACTAS":
                        resultReturn = ListarActas(context.Request["cerrada"],
                                                context.Request["orden"]);
                        break;

                    case "LISTAR-ACTASESTUDIO":
                        resultReturn = ListarActasEstudio(context.Request["idEstudio"]);
                        break;

                    case "GRABAR-ACTA":
                        resultReturn = Grabar(context.Request["idActa"],
                                            context.Request["descripion"],
                                            context.Request["fecha"],
                                            context.Request["hora"],
                                            context.Request["comentarioInicialFijo"],
                                            context.Request["comentarioInicial"],
                                            context.Request["comentarioFinal"],
                                            context.Request["cerrada"],
                                            context.Request["participantes"]);
                        break;

                    case "GRABAR-ACTAPARTICIPANTES":
                        resultReturn = GrabarParticipantes(context.Request["idActa"],
                                            context.Request["participantes"]);
                        break;

                    case "ELIMINAR-ACTAPARTICIPANTES":
                        resultReturn = EliminarParticipante(context.Request["idActa"],
                                                            context.Request["idActaParticipante"]);
                        break;

                    case "GRABAR-ACTADOCUMENTO":
                        resultReturn = GrabarDocumento(context.Request["idActa"],
                                                context.Request["idActaDocumento"],
                                                context.Request["idDocumento"],
                                                context.Request["idDocumentoVersion"],
                                                context.Request["comentario"],
                                                context.Request["idResponsableComite"],
                                                context.Request["imprimirCarta"]);
                        break;

                    case "GRABAR-ACTADOCUMENTOS":
                        resultReturn = GrabarDocumento(context.Request["idActa"], 
                                                       context.Request["documentos"],
                                                       context.Request["idEstudio"]);
                        break;

                    case "GRABAR-ORDENDOCUMENTOS":
                        resultReturn = GrabarOrdenDocumentos(context.Request["idActa"],
                                                            context.Request["documentos"],
                                                            context.Request["estudios"]);
                        break;

                    case "GRABAR-ACTADOCUMENTOCOMENTARIOESTADO":
                        resultReturn = GrabarDocumentoComentarioEstado(context.Request["documentos"]);                        
                        break;

                    case "ELIMINAR-ACTADOCUMENTO":
                        resultReturn = EliminarDocumento(context.Request["idActa"],
                                                         context.Request["idActaDocumento"]);
                        break;

                    case "GRABAR-ACTANOTATRATADA":
                        resultReturn = GrabarNota(context.Request["idActa"],
                                                  context.Request["idNota"],
                                                  context.Request["imprimeAlFinal"]);
                        break;

                    case "GRABAR-ACTANUEVANOTATRATADA":
                        resultReturn = GrabarNuevaNota(context.Request["idActa"],
                                                    context.Request["notas"]);
                        break;

                    case "ELIMINAR-ACTANOTATRATADA":
                        resultReturn = EliminarNota(context.Request["idActa"],
                                                context.Request["idNotaTratada"]);
                        break;

                    case "LISTAR-ESTUDIOSDELACTA":
                        resultReturn = ListarEstudiosDelActa(context.Request["idActa"]);
                        break;

                    case "LISTAR-ACTAESTUDIOS":
                        resultReturn = ListarActaEstudios(context.Request["idActa"]);
                        break;

                    case "OBTENER-ACTADTO":
                        resultReturn = ObtenerActaDTO(context.Request["idActa"]);
                        break;

                    case "GRABAR-ACTADATOSESTUDIO":
                        resultReturn = GrabarDatosEstudio(context.Request["idActa"],
                                                        context.Request["datosActaEstudio"]);
                        break;

                    case "OBTENER-ACTADATOSESTUDIO":
                        resultReturn = ObtenerDatosEstudio(context.Request["idActa"],
                                                           context.Request["IdEstudio"]);
                        break;


                    
                    case "OBTENER-ACTAESTUDIODOCUMENTOS":
                        resultReturn = ObtenerEstudioDocumentos(context.Request["idActa"],
                                                           context.Request["IdEstudio"]);
                        break;

                    case "OBTENER-ACTA_NOTASESTUDIO":
                        resultReturn = ObtenerNotasEstudio(context.Request["idActa"],
                                                           context.Request["IdEstudio"]);
                        break;

                    case "OBTENER-COMENTARIOINICIOFIJO":
                        resultReturn = ObtenerComentarioInicioFijo(context.Request["idActa"]);
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

        private string Obtener(string idActa)
        {
            ServicioActas servicio = new ServicioActas();
            return servicio.Obtener(idActa).SerializaToJson();
        }

        private string ListarActasEstudio(string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                return servicio.ListarActasPorEstudio(idEstudio).SerializaToJson();
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarActas(string cerrada, string orden)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                return servicio.ListarActas(cerrada, orden).SerializaToJson();
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
            
        }

        private string Grabar(string idActa, string descripion, string fecha, string hora, string comentarioInicialFijo, string comentarioInicial, string comentarioFinal, string cerrada, string participantes)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                int _idActaReturn = servicio.Grabar(idActa, descripion, fecha, hora, comentarioInicialFijo, comentarioInicial, comentarioFinal, cerrada, participantes);

                return serializer.Serialize(new { result = "OK", id = _idActaReturn});
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarParticipantes(string idActa, string participantes)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.GrabarParticipantes(idActa, participantes);

                return serializer.Serialize(new { result = "OK"});
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarParticipante(string idActa, string idActaParticipante)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.EliminarParticipante(idActa, idActaParticipante);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarDocumento(string idActa, string idActaDocumento, string idDocumento, string idDocumentoVersion, string comentario, string idResponsableComite, string imprimirCarta)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.GrabarDocumento(idActa, idActaDocumento, idDocumento, idDocumentoVersion, comentario, idResponsableComite, imprimirCarta, SessionHelper.ObtenerUsuarioLogin());

                return serializer.Serialize(new { result = "OK", });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarDocumento(string idActa, string documentos, string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.GrabarDocumento(idActa, documentos, idEstudio, SessionHelper.ObtenerUsuarioLogin());

                return serializer.Serialize(new { result = "OK", });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarOrdenDocumentos(string idActa, string documentos, string estudios)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.GrabarOrdenDocumentos(idActa, documentos, estudios);

                return serializer.Serialize(new { result = "OK", });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }


        private string GrabarDocumentoComentarioEstado(string documentos)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.GrabarDocumentoComentarioEstado(documentos, SessionHelper.ObtenerUsuarioLogin());
                
                return serializer.Serialize(new { result = "OK", });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarDocumento(string idActa, string idActaDocumento)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.EliminarDocumento(idActa, idActaDocumento);

                return serializer.Serialize(new { result = "OK", });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarNota(string idActa, string idNota, string imprimeAlFinal)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.GrabarNota(idActa, idNota, imprimeAlFinal);

                return serializer.Serialize(new { result = "OK", });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarNuevaNota(string idActa, string notas)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.GrabarNuevaNota(idActa, notas);

                return serializer.Serialize(new { result = "OK", });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarNota(string idActa, string idNotaTratada)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();

                servicio.EliminarNota(idActa, idNotaTratada);

                return serializer.Serialize(new { result = "OK", });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
        /// <summary>
        /// Lista los estudios del acta, recorriendo los documentos tratados (REFACTORIZAR: listar los estudios ingresando por ActaEstudios)
        /// </summary>
        /// <param name="idActa"></param>
        /// <returns></returns>
        private string ListarEstudiosDelActa(string idActa)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioActas servicio = new ServicioActas();
                return servicio.ListarEstudiosDelActa(idActa).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioActaHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
        /// <summary>
        /// Lista los estudios del acta, desde ActaEstudios (REFACTORIZAR: listar los estudios ingresando por ActaEstudios)
        /// </summary>
        /// <param name="idActa"></param>
        /// <returns></returns>
        private string ListarActaEstudios(string idActa)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioActas servicio = new ServicioActas();
                return servicio.ListarActaEstudios(idActa).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioActaHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ObtenerActaDTO(string idActa)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string estudiosReturn = string.Empty;
            try
            {
                ServicioActas servicio = new ServicioActas();
                return servicio.ObtenerActaDTO(idActa).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("BandejaInicioActaHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarDatosEstudio(string idActa, string datos)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioActas servActa = new ServicioActas();
                servActa.GrabarDatosEstudio(idActa, datos);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ObtenerDatosEstudio(string idActa, string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            ServicioActas servActa = new ServicioActas();
            return servActa.ObtenerActaEstudio(idActa, idEstudio).SerializaToJson();            
        }

        
        private string ObtenerEstudioDocumentos(string idActa, string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            ServicioActas servActa = new ServicioActas();
            return servActa.ListarDocumentoXActaEstudio(idActa, idEstudio).SerializaToJson();            
        }

        private string ObtenerNotasEstudio(string idActa, string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            ServicioActas servActa = new ServicioActas();
            return servActa.ObtenerActa_NotasEstudio(idActa, idEstudio).SerializaToJson();
        }

        private string ObtenerComentarioInicioFijo(string idActa)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioActas servicio = new ServicioActas();
                Acta acta = servicio.Obtener(idActa);
                return servicio.ArmarComentarioInicialFijo(acta).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("ActasHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
}