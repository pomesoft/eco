using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for NotasHandler
    /// </summary>
    public class NotasHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultReturn = string.Empty;
            string accion = string.Empty;

            if (context.Request["accion"] != null)
                accion = context.Request["accion"].ToString();

            switch (accion)
            {
                case "OBTENER-NOTA":
                    resultReturn = ObtenerNota(context.Request["idNota"]);
                    break;
                case "LISTAR-NOTAS":
                    resultReturn = ListarNotas(context.Request["idEstudio"]);
                    break;
                case "LISTAR-NOTAS-SIN-ACTA":
                    resultReturn = ListarNotasSinActas(context.Request["idEstudio"]);
                    break;
                case "GRABAR-NOTA":
                    resultReturn = GrabarNota(context.Request["idEstudio"],
                                            context.Request["idNota"],
                                            context.Request["descripion"],
                                            context.Request["fecha"],
                                            context.Request["idAutor"],
                                            context.Request["nombreArchivo"],
                                            context.Request["pathArchivo"],
                                            context.Request["requiereRespuesta"],
                                            context.Request["idDocumento"],
                                            context.Request["idDocumentoVersion"],
                                            context.Request["texto"]);
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

        private string GrabarNota(string idEstudio, string idNota, string descripion, string fecha, string idAutor, string nombreArchivo, string pathArchivo, string requiereRespuesta, string idDocumento, string idDocumentoVersion, string texto)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                ServicioEstudios servicio = new ServicioEstudios();

                servicio.GrabarNota(idEstudio, idNota, descripion, fecha, idAutor, nombreArchivo, pathArchivo, requiereRespuesta, idDocumento, idDocumentoVersion, texto);

                Estudio estudio = servicio.Obtener(idEstudio);
                string estudioJSON = estudio.SerializaToJson();

                return serializer.Serialize(new { result = "OK", estudio = estudioJSON });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ObtenerNota(string idNota)
        {
            ServicioEstudios servicio = new ServicioEstudios();

            return servicio.ObtenerNota(idNota).SerializaToJson();
        }

        private string ListarNotas(string idEstudio)
        {
            ServicioEstudios servicio = new ServicioEstudios();
            Estudio estudio = servicio.Obtener(idEstudio);

            return estudio.Notas.ToList<Nota>().SerializaToJson();
        }

        private string ListarNotasSinActas(string idEstudio)
        {
            ServicioEstudios servicio = new ServicioEstudios();
            Estudio estudio = servicio.Obtener(idEstudio);

            List<Nota> listNotas = estudio.Notas.ToList<Nota>()
                                                .Where(item => item.Acta == null)
                                                .ToList<Nota>();
            
            return listNotas.SerializaToJson();
        }
    }
}