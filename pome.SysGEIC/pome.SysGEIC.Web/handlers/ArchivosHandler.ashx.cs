using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Summary description for ArchivosHandler
    /// </summary>
    public class ArchivosHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string idEstudio = (context.Request["idEstudio"] != null ? context.Request["idEstudio"].ToString() : "-1");
            int idDocumento = int.Parse(context.Request["idDocumento"].ToString());
            int idVersion = int.Parse(context.Request["idVersion"].ToString());

            ServicioEstudios servicio = new ServicioEstudios();

            DocumentoVersion version = servicio.Obtener(idEstudio)
                                                .ObtenerDocumento(idDocumento)
                                                .ObtenerVersion(idVersion);

            string filePath = string.Format(@"{0}\{1}", UtilHelper.DirectorioArchivos, version.Archivo);

            context.Response.Clear();
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + version.Archivo);
            context.Response.Flush();
            context.Response.WriteFile(filePath);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}