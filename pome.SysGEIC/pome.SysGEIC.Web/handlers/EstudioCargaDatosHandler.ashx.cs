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
    /// Summary description for EstudioCargaDatosHandler
    /// </summary>
    public class EstudioCargaDatosHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultReturn = string.Empty;
            string accion = string.Empty;

            if (context.Request["accion"] != null)
                accion = context.Request["accion"].ToString();

            switch (accion)
            {
                case "OBTENER":
                    resultReturn = Obtener(context.Request["idEstudio"]);
                    break;

                case "LISTAR-ESTUDIOS":
                    resultReturn = ListarEstudios();
                    break;

                case "GRABAR":
                    resultReturn = GrabarEstudio(context.Request["idEstudio"],
                                        context.Request["codigo"],
                                        context.Request["nombre"],
                                        context.Request["nombreCompleto"],
                                        context.Request["patologia"],
                                        context.Request["poblacion"],
                                        context.Request["estado"], 
                                        context.Request["fechaPresentacion"], 
                                        context.Request["requiereAlerta"],
                                        context.Request["mesesAlerta"],
                                        context.Request["tipoEstudio"]);
                    break;

                case "GRABAR-ESTUDIOESTADO":
                    resultReturn = GrabarEstudioEstado(context.Request["idEstudio"],
                                                    context.Request["estado"]);
                    break;

                case "ELIMINAR-ESTUDIO":
                    resultReturn = Eliminar(context.Request["idEstudio"]);
                    break;

                case "GRABAR-PARTICIPANTE":
                    resultReturn = GrabarParticipante(context.Request["idEstudio"],
                                                context.Request["idParticipante"],
                                                context.Request["idProfesional"],
                                                context.Request["idRol"],
                                                context.Request["desde"],
                                                context.Request["hasta"]);
                    break;

                case "ELIMINAR-PARTICIPANTE":
                    resultReturn = EliminarParticipante(context.Request["idEstudio"],
                                                context.Request["idParticipante"]);
                    break;

                case "GRABAR-CENTROHABILITADO":
                    resultReturn = GrabarCentroHabilitado(context.Request["idEstudio"],
                                                    context.Request["idCentroHabilitado"],
                                                    context.Request["idCentro"],
                                                    context.Request["desde"],
                                                    context.Request["hasta"]);
                    break;

                case "ELIMINAR-CENTROHABILITADO":
                    resultReturn = EliminarCentroHabilitado(context.Request["idEstudio"],
                                                    context.Request["idCentroHabilitado"]);
                    break;

                case "GRABAR-PATROCINADOR":
                    resultReturn = GrabarPatrocinador(context.Request["idEstudio"],
                                                    context.Request["idEstudioPatrocinador"],
                                                    context.Request["idPatrocinador"]);
                    break;

                case "ELIMINAR-PATROCINADOR":
                    resultReturn = EliminarPatrocinador(context.Request["idEstudio"],
                                                    context.Request["idEstudioPatrocinador"]);
                    break;

                case "GRABAR-MONODROGA":
                    resultReturn = GrabarMonodroga(context.Request["idEstudio"],
                                                    context.Request["idEstudioMonodroga"],
                                                    context.Request["idMonodroga"]);
                    break;

                case "ELIMINAR-MONODROGA":
                    resultReturn = EliminarMonodroga(context.Request["idEstudio"],
                                                    context.Request["idEstudioMonodroga"]);
                    break;

                case "OBTENER-ESTADOSESTUDIO":
                    resultReturn = ObtenerEstadosEstudio();
                    break;

                case "LISTAR-INVESTIGADORES-ESTUDIO":
                    resultReturn = ListarInvestigadores(context.Request["idEstudio"]);
                    break;

                case "OBTENER-DEFUALT-ALERTA":
                    resultReturn = ObtenerDefaultAlertaRenovacionEstudio();
                    break;

                case "GRABAR-TIPOSDOCUMENTO_SEMAFORO":
                    resultReturn = GrabarEstudioTiposDocumentosSemaforo(context.Request["idEstudio"],
                                                                        context.Request["tiposDocumento"]);
                    break;
                case "ELIMINAR-TIPOSDOCUMENTO_SEMAFORO":
                    resultReturn = EliminarEstudioTiposDocumentosSemaforo(context.Request["idEstudio"],
                                                                          context.Request["idTipoDocumento"]);
                    break;
                case "LISTAR-TIPOSDOCUMENTO_SEMAFORO":
                    resultReturn = ListarEstudioTiposDocumentosSemaforo(context.Request["idEstudio"]);
                    break;

                case "LISTAR-TIPOSESTUDIO":
                    resultReturn = ListarTiposEstudio();
                    break;

                case "PROCESAR-ESTUDIOS-FINALIZADOS":
                    resultReturn = ProcesarEstudiosFinalizados(resultReturn);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(resultReturn);
            context.Response.End();
        }

        private static string ProcesarEstudiosFinalizados(string resultReturn)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ServicioEstudios servicio = new ServicioEstudios();
            servicio.ProcesarEstudiosFinalizados(SessionHelper.ObtenerUsuarioLogin());
            resultReturn = serializer.Serialize(new { result = "OK" });
            return resultReturn;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string Obtener(string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();
                return servicio.Obtener(idEstudio).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string Eliminar(string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();
                servicio.EliminarEstudio(idEstudio);
                return serializer.Serialize(new { result = "OK"});
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarEstudio(string idEstudio, string codigo, string nombre, string nombreCompleto, string patologia, string poblacion, string estado, string fechaPresentacion, string requiereAlerta, string mesesAlerta, string idTipoEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                int _idEstudioReturn = servicio.GrabarEstudio(idEstudio, codigo, nombre, nombreCompleto, patologia, poblacion, estado, fechaPresentacion, requiereAlerta, mesesAlerta, idTipoEstudio);

                return serializer.Serialize(new { result = "OK", id = _idEstudioReturn });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarParticipante(string idEstudio, string idParticipante, string idProfesional, string idRol, string desde, string hasta)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.GrabarParticipante(idEstudio, idParticipante, idProfesional, idRol, desde, hasta);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarParticipante(string idEstudio, string idParticipante)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.EliminarParticipante(idEstudio, idParticipante);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarCentroHabilitado(string idEstudio, string idCentroHabilitado, string idCentro, string desde, string hasta)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.GrabarCentroHabilitado(idEstudio, idCentroHabilitado, idCentro, desde, hasta);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarCentroHabilitado(string idEstudio, string idCentroHabilitado)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.EliminarCentroHabilitado(idEstudio, idCentroHabilitado);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarPatrocinador(string idEstudio, string idEstudioPatrocinador, string idPatrocinador)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.GrabarPatrocinador(idEstudio, idEstudioPatrocinador, idPatrocinador);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarPatrocinador(string idEstudio, string idEstudioPatrocinador)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.EliminarPatrocinador(idEstudio, idEstudioPatrocinador);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string GrabarMonodroga(string idEstudio, string idEstudioMonodroga, string idMonodroga)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.GrabarMonodroga(idEstudio, idEstudioMonodroga, idMonodroga);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarMonodroga(string idEstudio, string idEstudioMonodroga)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.EliminarMonodroga(idEstudio, idEstudioMonodroga);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarEstudios()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string datosReturn = string.Empty;
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                datosReturn = servicio.ObtenerVigentes().SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }

            return datosReturn;
        }

        private string GrabarEstudioEstado(string idEstudio, string estado)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.GrabarEstado(idEstudio, estado);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ObtenerEstadosEstudio()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioParametricas servicio = new ServicioParametricas();
                return servicio.EstadoEstudioObtenerVigentes(string.Empty).SerializaToJson();
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarInvestigadores(string idEstudio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ServicioEstudios servicio = new ServicioEstudios();

            Estudio estudio = servicio.Obtener(idEstudio);
            if (estudio != null && estudio.InvestigadoresPrincipales != null && estudio.InvestigadoresPrincipales.Count > 0)
                return serializer.Serialize(estudio.InvestigadoresPrincipalesProfesional);
            else
                return serializer.Serialize(new { result = "Error", message = "El estudio no tiene asignado investigadores" });
        }

        private string ObtenerDefaultAlertaRenovacionEstudio()
        {
            ServicioParametricas servParametrica = new ServicioParametricas();
            return servParametrica.ParametroObtener("ALERTA_RENOVACION_ESTUDIO_MESES_DEFAULT");
        }

        private string GrabarEstudioTiposDocumentosSemaforo(string idEstudio, string tiposDocumento)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.GrabarEstudioTiposDocumentosSemaforo(idEstudio, tiposDocumento);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarEstudioTiposDocumentosSemaforo(string idEstudio, string idTipoDocumento)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.EliminarEstudioTipoDocumento(idEstudio, idTipoDocumento);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                Logger.LogError("EstudioCargaDatosHandler", ex);
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarEstudioTiposDocumentosSemaforo(string idEstudio)
        {
            ServicioEstudios servicio = new ServicioEstudios();
            return servicio.ObtenerTiposDocumentoSemaforo(idEstudio).SerializaToJson();
        }

        private string ListarTiposEstudio()
        {
            return string.Empty;
        }
    }
}