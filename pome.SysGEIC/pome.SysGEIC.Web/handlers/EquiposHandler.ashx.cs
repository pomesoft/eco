using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for EquiposHandler
    /// </summary>
    public class EquiposHandler : IHttpHandler
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
                    resultReturn = Grabar(context.Request["equipo"]).ToString();
                    break;
                case "ELIMINAR":
                    resultReturn = Eliminar(context.Request["id"]).ToString();
                    break;
                case "AGREGAR_INTEGRANTE":
                    resultReturn = AgregarIntegrante(context.Request["id"],
                                                    context.Request["idRol"],
                                                    context.Request["idProfesional"]).ToString();
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
            ServicioEquipos servicio = new ServicioEquipos();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(servicio.EquipoObtenerVigentes(descripcion));
        }

        private string Obtener(string id)
        {
            ServicioEquipos servicio = new ServicioEquipos();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            int _id = -1;
            if (int.TryParse(id, out _id))
                return serializer.Serialize(servicio.EquipoObtener(_id));
            else
                return string.Empty;
        }
        private object Grabar(string datosEquipo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEquipos servicio = new ServicioEquipos();
                servicio.EquipoGrabar(datosEquipo);
                return serializer.Serialize(new { result = "OK" });
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
                ServicioEquipos servicio = new ServicioEquipos();
                servicio.EquipoEliminar(id);
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private object AgregarIntegrante(string id, string idRol, string idProfesional)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEquipos servicio = new ServicioEquipos();
                servicio.AgregarIntegrante(id, idRol, idProfesional);
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

    }
}