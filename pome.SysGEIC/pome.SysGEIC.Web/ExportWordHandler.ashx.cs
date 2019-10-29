using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;
using pome.SysGEIC.Comunes;
using pome.SysGEIC.Web.Helpers;
using pome.SysGEIC.Impresion;



namespace pome.SysGEIC.Web
{
    /// <summary>
    /// Summary description for ExportWordHandler
    /// </summary>
    public class ExportWordHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string resultReturn = string.Empty;
            string accion = string.Empty;
            List<string> contenidoHTML = new List<string>();
            string fileName = "Documento.doc";

            try
            {
                if (context.Request["accion"] != null)
                    accion = context.Request["accion"].ToString();

                switch (accion)
                {
                    case "EXPORTARWORD_EJEMPLO":
                        contenidoHTML.Add(ObtenerEjemplo());
                        break;

                    case "EXPORTARWORD_ACTA":
                        contenidoHTML = this.ImprimirActa(context.Request["idActa"]);
                        fileName = fileName = "Acta_" + context.Request["IdActa"].ToString();
                        break;

                    case "EXPORTARWORD_CARTARESPUESTA":
                        contenidoHTML = this.ImprimirCartaRespuesta(context.Request["idActa"],
                                                                   context.Request["idEstudio"]);
                        fileName = this.NombreArchivoCartaRespuesta(context.Request["IdActa"],
                                                                    context.Request["idEstudio"]);
                        break;
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(this.GetType().Name, ex);
                resultReturn = serializer.Serialize(new { result = "Error", message = ex.Message });
            }


            contenidoHTML.ForEach(delegate(string html)
            {
                html = html.Replace("page-break-after: always;", string.Empty);
                html = html.Replace("page-break-before: always;", string.Empty);

                resultReturn += html;
            });
            
            context.Response.AppendHeader("Content-Type", "application/msword");
            context.Response.AppendHeader("Content-disposition", "attachment; filename=" + fileName + ".doc");
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

        private List<string> ImprimirActa(string idActa)
        {
            List<string> listReturn = new List<string>();
            ServicioImpresion servImpresion = new ServicioImpresion();
            listReturn.Add(servImpresion.ImprimirActa(idActa, true, true));
            return listReturn;
        }

        private List<string> ImprimirCartaRespuesta(string idActa, string idEstudio)
        {
            ServicioImpresion servImpresion = new ServicioImpresion();
            servImpresion.GenerarHTMLHeaderFooter(idActa, idEstudio, UtilHelper.DirectorioArchivos);
            return servImpresion.ImprimirCartaRespuesta(idActa, idEstudio, true);
        }

        private string NombreArchivoCartaRespuesta(string idActa, string idEstudio)
        {
            ServicioImpresion servImpresion = new ServicioImpresion();
            return servImpresion.NombreArchivoCartaRespuesta(idActa, idEstudio);
        }

        private string ObtenerEjemplo()
        {
            StringBuilder sbTop = new System.Text.StringBuilder();
            sbTop.Append(@"
                    <html 
                    xmlns:o='urn:schemas-microsoft-com:office:office' 
                    xmlns:w='urn:schemas-microsoft-com:office:word'
                    xmlns='http://www.w3.org/TR/REC-html40'>
                    <head><title></title>

                    <!--[if gte mso 9]>
                    <xml>
                    <w:WordDocument>
                    <w:View>Print</w:View>
                    <w:Zoom>90</w:Zoom>
                    <w:DoNotOptimizeForBrowser/>
                    </w:WordDocument>
                    </xml>
                    <![endif]-->


                    <style>
                    p.MsoFooter, li.MsoFooter, div.MsoFooter
                    {
                    margin:0in;
                    margin-bottom:.0001pt;
                    mso-pagination:widow-orphan;
                    tab-stops:center 3.0in right 6.0in;
                    font-size:12.0pt;
                    }
                    <style>

                    <!-- /* Style Definitions */

                    @page Section1
                    {
                    size:8.5in 11.0in; 
                    margin:1.0in 1.25in 1.0in 1.25in ;
                    mso-header-margin:.5in;
                    mso-header:h1;
                    mso-footer: f1; 
                    mso-footer-margin:.5in;
                    }


                    div.Section1
                    {
                    page:Section1;
                    }

                    table#hrdftrtbl
                    {
                    margin:0in 0in 0in 9in;
                    }
                    -->
                    </style></head>

                    <body lang=EN-US style='tab-interval:.5in'>
                    <div class=Section1>
                    
                    <table id='hrdftrtbl' border='1' cellspacing='0' cellpadding='0'>
                    <tr><td>
                    <div style='mso-element:header' id=h1 >
                    <p class=MsoHeader style='text-align:center'>Confidential</p>
                    </div>
                    </td>
                    <td>
                    <div style='mso-element:footer' id=f1>
                    <p class=MsoFooter>Draft
                    <span style=mso-tab-count:2'></span><span style='mso-field-code:"" PAGE ""'></span>
                    of <span style='mso-field-code:"" NUMPAGES ""'></span></p></div>
                    /td></tr>
                    </table>
                    </body></html>
                    ");

            return sbTop.ToString();
        }
    }
}