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
    /// Summary description for ProfesionalesHandler
    /// </summary>
    public class ProfesionalesHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string resultReturn = string.Empty;
            string accion = string.Empty;

            if (context.Request["accion"] != null)
                accion = context.Request["accion"].ToString();

            if (accion == string.Empty || accion.Equals("LISTAR"))
                resultReturn = Listar(context.Request["apellido"],
                                      context.Request["nombre"],
                                      context.Request["tipoProfesional"]);

            else if (accion.Equals("OBTENER"))
                resultReturn = Obtener(context.Request["id"]);

            else if (accion.Equals("GRABAR"))
                resultReturn = Grabar(context.Request["id"],
                                    context.Request["apellido"],
                                    context.Request["nombre"],
                                    context.Request["idTipoProfesional"],
                                    context.Request["idRolComite"],
                                    context.Request["titulo"],
                                    context.Request["ordenActa"]).ToString();

            else if (accion.Equals("ELIMINAR"))
                resultReturn = Eliminar(context.Request["id"]).ToString();

            else if (accion.Equals("LISTAR-MIEMBOSCOMITE"))
                resultReturn = ListarMiembrosComite();

            else if (accion.Equals("LISTAR-INVESTIGADORES"))
                resultReturn = ListarInvestigadores();


            else if (accion.Equals("LISTAR-TIPOSPROFESIONALES"))
                resultReturn = ListarTiposProfesionales();

            else if (accion.Equals("LISTAR-ROLESCOMITE"))
                resultReturn = ListarRolesComite();

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

        private string Listar(string apellido, string nombre, string tipoProfesional)
        {
            ServicioEquipos servicio = new ServicioEquipos();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(servicio.ProfesionalObtenerVigentes(apellido, nombre, tipoProfesional));
        }

        private string ListarInvestigadores()
        {
            ServicioEquipos servicio = new ServicioEquipos();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(servicio.ProfesionalObtenerVigentes(Constantes.TipoProfesional_Investigador));
        }

        private string ListarMiembrosComite()
        {
            ServicioEquipos servicio = new ServicioEquipos();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            List<Profesional> profesionales = servicio.ProfesionalObtenerVigentes(Constantes.TipoProfesional_MiembroComite);

            return serializer.Serialize(profesionales.OrderBy(item => item.OrdenActa).ToList<Profesional>());
        }

        private string Obtener(string id)
        {
            ServicioEquipos servicio = new ServicioEquipos();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            int _id = -1;
            if (int.TryParse(id, out _id))
                return serializer.Serialize(servicio.ProfesionalObtener(_id));
            else
                return string.Empty;
        }
        private object Grabar(string id, string apellido, string nombre, string idTipoProfesional, string IdRolcomite, string titulo, string ordenActa)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioEquipos servicio = new ServicioEquipos();
                servicio.ProfesionalGrabar(id, apellido, nombre, idTipoProfesional, IdRolcomite, titulo, ordenActa);
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
                servicio.ProfesionalEliminar(id);
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }

        private string ListarTiposProfesionales()
        {
            ServicioEquipos servicio = new ServicioEquipos();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(servicio.TipoProfesionalObtenerVigentes());
        }

        private string ListarRolesComite()
        {
            ServicioParametricas servicio = new ServicioParametricas();
            return servicio.RolComiteObtenerVigentes().SerializaToJson();
        }
    }
}