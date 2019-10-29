using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for TiposDocumentoHandler
    /// </summary>
    public class TiposDocumentoHandler : IHttpHandler, IRequiresSessionState
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
                    resultReturn = Obtener(context.Request["id"]);
                    break;
                case "GRABAR":
                    resultReturn = Grabar(context.Request["id"],
                                    context.Request["descripcion"],
                                    context.Request["requiereVersion"],
                                    context.Request["idTipoDocumentoGrupo"],
                                    context.Request["listarCartaRespuesta"],
                                    context.Request["listarDocsCartaRespuesta"],
                                    context.Request["necesarioAprobacionEstudio"],
                                    context.Request["tiposRecordatorio"]).ToString();
                    break;
                case "ELIMINAR":
                    resultReturn = Eliminar(context.Request["id"]).ToString();
                    break;
                case "GRABAR-FLUJO":
                    resultReturn = AgregarFlujo(context.Request["id"],
                                                context.Request["idFlujo"],
                                                context.Request["descripcionFlujo"]).ToString();
                    break;
                case "ELIMINAR-FLUJO":
                    resultReturn = EliminarFlujo(context.Request["id"],
                                                context.Request["idFlujo"]).ToString();
                    break;
                case "GRABAR-ESTADO":
                    resultReturn = AgregarEstado(context.Request["id"],
                                                context.Request["idFlujo"],
                                                context.Request["idEstado"],
                                                context.Request["idEstadoPadre"],
                                                context.Request["final"]);
                    break;
                case "ELIMINAR-ESTADO":
                    resultReturn = EliminarEstado(context.Request["id"],
                                                context.Request["idFlujo"],
                                                context.Request["idFlujoEstado"]);
                    break;
                case "LISTAR_TIPODOCUMENTOGRUPOS":
                    resultReturn = ListarTipoDocumentoGrupos();
                    break;
                default:
                    resultReturn = Listar(context.Request["descripcion"]);
                    break;
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
            ServicioDocumentos servicio = new ServicioDocumentos();

            List<TipoDocumento> lista = servicio.TiposDocumentoObtenerVigentes(descripcion);
            
            return lista.SerializaToJson();
        }

        private string Obtener(string id)
        {
            ServicioDocumentos servicio = new ServicioDocumentos();

            int _id = id.ConvertirInt();
            if (_id != -1)
            {
                TipoDocumento tipoDocumento = servicio.TipoDocumentoObtener(_id);
                return tipoDocumento.SerializaToJson();
            }
            else
                return string.Empty;
        }
        private object Grabar(string id, string descripcion, string requiereVersion, string idTipoDocumentoGrupo, string listarCartaRespuesta, string listarDocsCartaRespuesta, string necesarioAprobacionEstudio, string tiposRecordatorio)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {                
                ServicioDocumentos servicio = new ServicioDocumentos();
                ServicioParametricas servParametricas = new ServicioParametricas();
                TipoDocumento tipoDocumento;

                int _id = id.ConvertirInt();

                if (_id == -1)
                    tipoDocumento = new TipoDocumento();
                else
                    tipoDocumento = servicio.TipoDocumentoObtener(_id);

                tipoDocumento.Descripcion = descripcion == null ? string.Empty : descripcion;
                tipoDocumento.Vigente = true;
                tipoDocumento.RequiereVersion = requiereVersion.ConvertirBool();
                tipoDocumento.ListarCartaRespuesta = listarCartaRespuesta.ConvertirBool();
                tipoDocumento.ListarDocsCartaRespuesta = listarDocsCartaRespuesta.ConvertirBool();
                tipoDocumento.NecesarioAprobacionEstudio = necesarioAprobacionEstudio.ConvertirBool();

                int _idGrupo = idTipoDocumentoGrupo.ConvertirInt();
                tipoDocumento.TipoDocumentoGrupo = _idGrupo != -1 ? servParametricas.ObtenerObjeto<TipoDocumentoGrupo>(_idGrupo) : null;

                if (tipoDocumento.Flujos == null || tipoDocumento.Flujos.Count == 0)
                {
                    TipoDocumentoFlujo tdFlujo = new TipoDocumentoFlujo();
                    tdFlujo.Id = -1;
                    tdFlujo.Descripcion = "NORMAL";
                    tdFlujo.Vigente = true;

                    EstablecerEstados(tdFlujo, tipoDocumento.TipoDocumentoGrupo.SeEvalua(), tipoDocumento.TipoDocumentoGrupo.SeTomaConocimiento());

                    tipoDocumento.ActualizarFlujo(tdFlujo);
                }

                ServicioRecordatorios servRecordatorio = new ServicioRecordatorios();
                //Serializar
                JavaScriptSerializer deserializer = new JavaScriptSerializer();
                deserializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                dynamic datosRecordatorios = deserializer.Deserialize(tiposRecordatorio, typeof(object));
                foreach (var item in datosRecordatorios)
                {
                    TipoRecordatorio tipoRecordatorio = servRecordatorio.TipoRecordatorioObtener(item.IdTipoRecordatorio);
                    string meses = item.Meses;
                    tipoDocumento.AgregarTipoRecordatorio(tipoRecordatorio, meses.ConvertirInt());
                }

                servicio.TipoDocumentoGrabar(tipoDocumento);

                serializer = new JavaScriptSerializer();
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string AgregarFlujo(string id, string idFlujo, string descripcionFlujo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                TipoDocumento tipoDocumento;
                TipoDocumentoFlujo tdFlujo = new TipoDocumentoFlujo();

                int _id = -1;
                if (!int.TryParse(id, out _id))
                    _id = -1;

                if (_id == -1)
                    tipoDocumento = new TipoDocumento();
                else
                    tipoDocumento = servicio.TipoDocumentoObtener(_id);

                tdFlujo.Id = (idFlujo == null ? -1 : int.Parse(idFlujo));
                tdFlujo.Descripcion = (descripcionFlujo == null ? string.Empty : descripcionFlujo);
                tdFlujo.Vigente = true;
                
                tipoDocumento.ActualizarFlujo(tdFlujo);

                servicio.TipoDocumentoGrabar(tipoDocumento);

                return tipoDocumento.SerializaToJson();
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private void EstablecerEstados(TipoDocumentoFlujo flujo, bool seEvalua, bool seTomaConocimiento)
        {
            ServicioParametricas servParametrica = new ServicioParametricas();
            EstadoDocumento _estado = null;
            EstadoDocumento _estadoPadre = null;

            int estado_Ingresado = servParametrica.ParametroObtener("ESTADO_DOC_INGRESADO").ConvertirInt();
            int estado_EnEvaluacion = servParametrica.ParametroObtener("ESTADO_DOC_EN_EVALUACION").ConvertirInt();
            int estado_Aprobado = servParametrica.ParametroObtener("ESTADO_DOC_APROBADO").ConvertirInt();
            int estado_PedidoCambio = servParametrica.ParametroObtener("ESTADO_DOC_PEDIDO_CAMBIO").ConvertirInt();
            int estado_TomaConocimiento = servParametrica.ParametroObtener("ESTADO_DOC_TOMA_CONOCIMIENTO").ConvertirInt();

            _estado = servParametrica.EstadoDocumentoObtener(estado_Ingresado);
            _estadoPadre = null;
            flujo.AgregarEstado(_estado, _estadoPadre, false);

            _estado = servParametrica.EstadoDocumentoObtener(estado_EnEvaluacion);
            _estadoPadre = servParametrica.EstadoDocumentoObtener(estado_Ingresado);
            flujo.AgregarEstado(_estado, _estadoPadre, false);

            if (seEvalua)
            {
                _estado = servParametrica.EstadoDocumentoObtener(estado_Aprobado);
                _estadoPadre = servParametrica.EstadoDocumentoObtener(estado_EnEvaluacion);
                flujo.AgregarEstado(_estado, _estadoPadre, true);

                _estado = servParametrica.EstadoDocumentoObtener(estado_PedidoCambio);
                _estadoPadre = servParametrica.EstadoDocumentoObtener(estado_EnEvaluacion);
                flujo.AgregarEstado(_estado, _estadoPadre, true);
            }
            else if (seTomaConocimiento)
            {
                _estado = servParametrica.EstadoDocumentoObtener(estado_TomaConocimiento);
                _estadoPadre = servParametrica.EstadoDocumentoObtener(estado_EnEvaluacion);
                flujo.AgregarEstado(_estado, _estadoPadre, true);
            }
        }

        private string EliminarFlujo(string id, string idFlujo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                
                int _id = -1;
                if (!int.TryParse(id, out _id))
                    _id = -1;

                TipoDocumento tipoDocumento = servicio.TipoDocumentoObtener(_id);
                TipoDocumentoFlujo flujo = tipoDocumento.ObtenerFlujo(int.Parse(idFlujo));
                
                tipoDocumento.ElimnarFlujo(flujo);

                servicio.TipoDocumentoGrabar(tipoDocumento);

                return tipoDocumento.SerializaToJson();
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string AgregarEstado(string idTipoDocumento, string idFlujo, string idEstado, string idEstadoPadre, string final)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioDocumentos servicio = new ServicioDocumentos();
                ServicioParametricas servParametrica = new ServicioParametricas();
                TipoDocumento tipoDocumento = servicio.TipoDocumentoObtener(int.Parse(idTipoDocumento));
                TipoDocumentoFlujo flujo = tipoDocumento.ObtenerFlujo(int.Parse(idFlujo));

                if (flujo == null)
                    return serializer.Serialize(new { result = "Error", message = "No existe ítem, refresque la pantalla" });

                EstadoDocumento estado = servParametrica.EstadoDocumentoObtener(int.Parse(idEstado));
                
                EstadoDocumento estadoPadre = null;
                int _idEstadoPadre = -1;
                if (int.TryParse(idEstadoPadre, out _idEstadoPadre))
                    estadoPadre = servParametrica.EstadoDocumentoObtener(int.Parse(idEstadoPadre));

                bool estadoFinal = (string.IsNullOrEmpty(final)) ? false : bool.Parse(final);
                flujo.AgregarEstado(estado, estadoPadre, estadoFinal);

                servicio.TipoDocumentoGrabar(tipoDocumento);
                tipoDocumento = servicio.TipoDocumentoObtener(int.Parse(idTipoDocumento));
                flujo = tipoDocumento.ObtenerFlujo(int.Parse(idFlujo));

                return flujo.SerializaToJson();
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string EliminarEstado(string idTipoDocumento, string idFlujo, string idFlujoEstado)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioDocumentos servParametrica = new ServicioDocumentos();
                TipoDocumento tipoDocumento = servParametrica.TipoDocumentoObtener(int.Parse(idTipoDocumento));
                TipoDocumentoFlujo flujo = tipoDocumento.ObtenerFlujo(int.Parse(idFlujo));

                if (flujo == null)
                    return serializer.Serialize(new { result = "Error", message = "No existe ítem, refresque la pantalla" });

                flujo.EliminarEstado(int.Parse(idFlujoEstado));
                
                servParametrica.TipoDocumentoGrabar(tipoDocumento);
                tipoDocumento = servParametrica.TipoDocumentoObtener(int.Parse(idTipoDocumento));
                flujo = tipoDocumento.ObtenerFlujo(int.Parse(idFlujo));

                return flujo.SerializaToJson();
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
                ServicioDocumentos servicio = new ServicioDocumentos();
                TipoDocumento objeto;

                int _id = -1;
                if (!int.TryParse(id, out _id))
                    throw new ApplicationException("No existe tipo de documento que desea eliminar.");

                objeto = servicio.TipoDocumentoObtener(_id);
                if (objeto == null)
                    throw new ApplicationException("No existe tipo de documento que desea eliminar.");

                objeto.Vigente = false;

                servicio.TipoDocumentoEliminar(objeto);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarTipoDocumentoGrupos()
        {
            ServicioDocumentos servicio=new ServicioDocumentos();
            return servicio.TipoDocumentoGruposObtenerVigentes().SerializaToJson();
        }
    }
}