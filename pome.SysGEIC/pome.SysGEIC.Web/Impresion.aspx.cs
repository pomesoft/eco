using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;
using pome.SysGEIC.Impresion;

using pome.SysGEIC.Web.Helpers;

using Winnovative.WnvHtmlConvert.PdfDocument;


namespace pome.SysGEIC.Web
{
    public partial class Impresion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Request["Plantilla"] != null)
                {
                    try
                    {
                        List<string> contenidoHTML = new List<string>();
                        string plantilla = this.Request["Plantilla"].ToString();
                        string fileName = "Documento";
                        
                        string exportTo = string.Empty;
                        if (this.Request["ExportTo"] != null)
                            exportTo = this.Request["ExportTo"].ToString();
                        else
                            exportTo = "1";
                        
                        
                        ProcesadorPDF procesador = new ProcesadorPDF();
                        Document _docPDF = null;

                        switch (plantilla)
                        {
                            case "PLANTILLA_ACTA":
                                if (exportTo.ConvertirInt() == (int)EXPORT_TO.WORD_)
                                {
                                    Response.Redirect(string.Format("ExportWordHandler.ashx?accion=EXPORTARWORD_ACTA&IdActa={0}",
                                                                    this.Request["IdActa"].ToString()));
                                }
                                else
                                {
                                    fileName = "Acta_" + this.Request["IdActa"].ToString();
                                    contenidoHTML.Add(this.ImprimirActa(exportTo.ConvertirInt()));
                                    procesador.MostrarEncabezado = false;
                                    procesador.MostrarPieDePagina = false;                                    
                                }
                                break;

                            case "PLANTILLA_CARTARESPUESTA":
                                if (exportTo.ConvertirInt() == (int)EXPORT_TO.WORD_)
                                {
                                    Response.Redirect(string.Format("ExportWordHandler.ashx?accion=EXPORTARWORD_CARTARESPUESTA&IdActa={0}&IdEstudio={1}",
                                                                    this.Request["IdActa"].ToString(), 
                                                                    this.Request["idEstudio"].ToString()));
                                }
                                else
                                {
                                    fileName = this.NombreArchivoCartaRespuesta();
                                    contenidoHTML = this.ImprimirCartaRespuesta();
                                    procesador.MostrarEncabezado = true;
                                    procesador.MostrarPieDePagina = true;
                                    procesador.AltoEncabezado = 190;
                                    procesador.AltoPieDePagina = 55;
                                    procesador.TextoPieDePagina = true;
                                    procesador.PathFuente = HttpContext.Current.Server.MapPath("Plantillas");
                                }
                             
                                break;

                            
                            case "PLANTILLA_PATHFUENTE":
                                contenidoHTML.Add(HttpContext.Current.Server.MapPath("Plantillas"));
                                exportTo = "3";
                                break;

                            default:
                                contenidoHTML.Add(this.ImprimirPlantilla(plantilla));
                                procesador.MostrarEncabezado = false;
                                procesador.MostrarPieDePagina = false;
                                break;
                        }

                        StringBuilder strHTMLFinal = new StringBuilder();
                        contenidoHTML.ForEach(delegate(string html)
                        {
                            if (exportTo.ConvertirInt() == (int)EXPORT_TO.WORD_)
                            {
                                html = html.Replace("page-break-after: always;", string.Empty);
                                html = html.Replace("page-break-before: always;", string.Empty);
                            }
                            strHTMLFinal.Append(html);
                        });

                        HttpResponse httpResponse = HttpContext.Current.Response;

                        switch (exportTo.ConvertirInt())
                        {
                            case (int)EXPORT_TO.PDF_: //exportar a PDF  
                                if (contenidoHTML.Count == 0)
                                {
                                    litHTML.Text = "No hay documentos cargados para la carta de respuesta. En tipo de documento verifique si los documentos tiene el tílde de Carta de Respuesta ";
                                }
                                else
                                {
                                    _docPDF = procesador.ProcesarDocumentoPDF(contenidoHTML);
                                    //_docPDF.Save(httpResponse, true, string.Format("{0}.pdf", fileName));

                                    byte[] outPdfBuffer = _docPDF.Save();

                                    System.IO.MemoryStream rptStream = new System.IO.MemoryStream(outPdfBuffer);
                                    httpResponse.Clear();
                                    httpResponse.ClearHeaders();
                                    httpResponse.ClearContent();
                                    httpResponse.Buffer = true;
                                    httpResponse.AddHeader("Content-Type", "application/pdf");
                                    httpResponse.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.pdf; size={1}", fileName, outPdfBuffer.Length.ToString()));
                                    httpResponse.Flush();
                                    httpResponse.BinaryWrite(outPdfBuffer);
                                    httpResponse.End();
                                    httpResponse.Flush();                                    

                                }
                                break;

                            case (int)EXPORT_TO.WORD_://exportar a WORD

                                httpResponse.Clear();
                                httpResponse.Charset = "";
                                httpResponse.ContentType = "application/msword";
                                httpResponse.AddHeader("Content-Disposition", "inline;filename=Documento.doc");
                                httpResponse.Write(strHTMLFinal.ToString());
                                httpResponse.End();
                                httpResponse.Flush();                               
                                break;

                            case (int)EXPORT_TO.HTML_: // exportar a HTML
                                litHTML.Text = strHTMLFinal.ToString();
                                
                                break;
                        }
                        
                    }
                    catch (ApplicationException ex)
                    {
                        Logger.LogError("Impresion", ex);
                        litHTML.Text = string.Format("Ocurrió un error al imprimir.<br />{0}", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Impresion", ex);
                        litHTML.Text = string.Format("Ocurrió un error al imprimir.<br /><br />{0}<br /><br />{1}", ex.Message, ex.StackTrace);
                    }
                }
            }
        }

        private string ImprimirActa(int exportTo)
        {            
            ServicioImpresion servImpresion = new ServicioImpresion();
            return servImpresion.ImprimirActa(this.Request["IdActa"].ToString(), (exportTo == 1 || exportTo == 3), false);
        }

        private List<string> ImprimirCartaRespuesta()
        {
            ServicioImpresion servImpresion = new ServicioImpresion();
            return servImpresion.ImprimirCartaRespuesta(this.Request["IdActa"].ToString(), this.Request["idEstudio"].ToString(), false);
        }

        private string NombreArchivoCartaRespuesta()
        {
            ServicioImpresion servImpresion = new ServicioImpresion();
            return servImpresion.NombreArchivoCartaRespuesta(this.Request["IdActa"].ToString(), this.Request["idEstudio"].ToString());
        }

        private string ImprimirPlantilla(string _plantilla)
        {
            ServicioImpresion servImpresion = new ServicioImpresion();
            return servImpresion.ImprimirPlantilla(_plantilla);
        }
    }
}