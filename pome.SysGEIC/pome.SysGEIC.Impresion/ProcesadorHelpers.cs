using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Impresion.DTO;

namespace pome.SysGEIC.Impresion
{
    public static class ProcesadorHelpers
    {
        public static string DirectorioImagenes
        {
            get
            {
                return HttpContext.Current.Server.MapPath("img");
            }
        }
        public static string UrlDirectorioImagenes
        {
            get
            {
                string port = HttpContext.Current.Request.Url.Port > 0 ? string.Format(":{0}", HttpContext.Current.Request.Url.Port) : string.Empty;

                return string.Format("http://{0}{1}/{2}", HttpContext.Current.Request.Url.Host, port, "img");
            }
        }        
        public static string DirectorioPlantillas
        {
            get
            {
                return HttpContext.Current.Server.MapPath("Plantillas");
            }
        }
        public static string UrlDirectorioPlantillas
        {
            get
            {
                string port = HttpContext.Current.Request.Url.Port > 0 ? string.Format(":{0}", HttpContext.Current.Request.Url.Port) : string.Empty;

                return string.Format("http://{0}{1}/{2}", HttpContext.Current.Request.Url.Host, port, "Plantillas");
            }
        }
        public static string UrlDirectorioArchivos
        {
            get
            {
                string port = HttpContext.Current.Request.Url.Port > 0 ? string.Format(":{0}", HttpContext.Current.Request.Url.Port) : string.Empty;

                return string.Format("http://{0}{1}/{2}", HttpContext.Current.Request.Url.Host, port, "Archivos");
            }
        }
        
        public static EstudioTratado ObtenerDatosEstudioTratado(Estudio estudio)
        {
            try
            {
                EstudioTratado estTratado = new EstudioTratado();

                estTratado.NombreCompleto = estudio.NombreCompleto == null ? string.Empty : estudio.NombreCompleto;
                estTratado.Codigo = estudio.NombreEstudioListados;
                estTratado.Patrocinador = ConcatenarPatrocinadores(estudio);
                estTratado.InvestigadorPrincipal = ConcatenarInvestigadoresPrincipales(estudio);
                estTratado.Aprobado = estudio.Estado == null ? "NO" : estudio.Estado.EsAprobado || estudio.Estado.EsDocumentosAprobado ? "SI" : "NO";
                Centro centroHabilitado = estudio.CentroHabilitado;
                estTratado.CentroHabilitado = centroHabilitado != null ? centroHabilitado.Descripcion : string.Empty;
                estTratado.CentroHabilitadoContacto = centroHabilitado != null ? centroHabilitado.DatosContacto : string.Empty;
                
                return estTratado;
            }
            catch (Exception ex)
            {
                Logger.LogError("ProcesadorHelpers", ex);
                throw;
            }
        }

        public static DocumentoTratado ObtenerDocumentoTratado(ActaDocumento actaDocumento, bool quitarTagP)
        {
            try
            {
                DocumentoTratado documentoTratado = new DocumentoTratado();

                //agregar property ORDEN

                documentoTratado.Documento = actaDocumento.DocumentoVersion.Documento.Descripcion;
                documentoTratado.Version = actaDocumento.DocumentoVersion.Descripcion;
                documentoTratado.FechaVersion = actaDocumento.DocumentoVersion.FechaToString;
                documentoTratado.ResponsableComite = (actaDocumento.ResponsableComite == null) ? string.Empty : actaDocumento.ResponsableComite.NombreCompleto;

                if (!actaDocumento.SetearSaltosLinea.HasValue || !actaDocumento.SetearSaltosLinea.Value)
                    documentoTratado.Comentario = actaDocumento.ComentarioDocumento.Replace("\n", Constantes.SaldoLinea);
                else
                    documentoTratado.Comentario = actaDocumento.ComentarioDocumento;

                return documentoTratado;
            }
            catch (Exception ex)
            {
                Logger.LogError("ObtenerDocumentoTratado", ex);
                throw;
            }
        }

        public static string QuitarTagP(string texto)
        {
            string textoReturn = string.Empty;
            textoReturn = texto.StartsWith("<p>") ? texto.Substring(3) : texto;
            textoReturn = textoReturn.EndsWith("</p>") ? textoReturn.Substring(0, textoReturn.Length - 4) : textoReturn;
            return textoReturn;
        }

        public static string ConcatenarPatrocinadores(Estudio estudio)
        {
            string datosReturn = string.Empty;

            estudio.Patrocinadores.ToList<EstudioPatrocinador>().ForEach(delegate(EstudioPatrocinador estPatrocinador)
            {
                datosReturn += string.Format("{0} <br />", estPatrocinador.Patrocinador.Descripcion);
            });

            return (datosReturn.Length == 0 ? datosReturn : datosReturn.Substring(0, datosReturn.Length - 7));
        }

        public static string ConcatenarInvestigadoresPrincipales(Estudio estudio)
        {
            string datosReturn = string.Empty;

            estudio.InvestigadoresPrincipales.ForEach(delegate(EstudioParticipante estParticipante)
            {
                datosReturn += string.Format("{0} / ", estParticipante.Profesional.NombreCompleto);
            });

            return (datosReturn.Length == 0 ? datosReturn : datosReturn.Substring(0, datosReturn.Length - 3));
        }
    }
}
