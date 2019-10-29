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
    /// Summary description for PlantillasHandler
    /// </summary>
    public class PlantillasHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultReturn = string.Empty;
            string accion = string.Empty;

            if (context.Request["accion"] != null)
                accion = context.Request["accion"].ToString();

            switch (accion)
            {
                case "LISTAR":
                    resultReturn = Listar();
                    break;
                case "LISTAR-TODOS":
                    resultReturn = ListarTodos(context.Request["descripcion"],
                                               context.Request["idTipo"]);
                    break;
                case "OBTENER":
                    resultReturn = Obtener(context.Request["id"]);
                    break;
                case "GRABAR":
                    resultReturn = Grabar(context.Request["id"],
                                        context.Request["nombre"],
                                        context.Request["texto"],
                                        context.Request["idTipo"]);
                    break;
                case "ELIMINAR":
                    resultReturn = Eliminar(context.Request["id"]);
                    break;

                case "PROCESAR-TEXTOS-PLANOS":
                    resultReturn = ProcesarTextosPlanos();
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

        private string ListarTodos(string descripcion, string idTipo)
        {
            ServicioParametricas servicio = new ServicioParametricas();

            return servicio.PlantillaObtenerVigentes(descripcion, idTipo).SerializaToJson();
        }

        private string Listar()
        {
            ServicioParametricas servicio = new ServicioParametricas();

            return servicio.PlantillaObtenerVigentes(Constantes.TipoPlantilla_TextoLibre).SerializaToJson();
        }

        private string Obtener(string id)
        {
            ServicioParametricas servicio = new ServicioParametricas();
            int _id = -1;
            if (int.TryParse(id, out _id))
                return servicio.PlantillaObtener(_id).SerializaToJson();
            else
                return string.Empty;
        }

        private string Grabar(string id, string descripcion, string texto, string idTipo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioParametricas servicio = new ServicioParametricas();
                
                servicio.PlantillaGrabar(id, descripcion, texto, idTipo);

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string Eliminar(string id)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioParametricas servicio = new ServicioParametricas();
                
                servicio.PlantillaEliminar(id);
                
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ProcesarTextosPlanos()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioParametricas servicio = new ServicioParametricas();

                servicio.PlantillaProcesarTextosPlanos();

                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
}