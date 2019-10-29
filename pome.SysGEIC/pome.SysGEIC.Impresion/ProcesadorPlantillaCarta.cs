using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.Impresion.NVelocity;
using pome.SysGEIC.Impresion.DTO;

namespace pome.SysGEIC.Impresion
{
    public class ProcesadorPlantillaCarta
    {
        public string NombrePlantilla {get;set;}        
        public string HTMLFinal { get; set; }
        public List<string> HTMLProcesado { get; set; }
        public int IdEstadoPedidoCambio { get; set; }
        public int IdEstadoAprobado { get; set; }        
        public string ComiteNombreCompleto { get; set; }
        public string ComiteIniciales { get; set; }
        public Profesional InvestigadorPrincipal { get; set; }
        public List<Profesional> Vocales { get; set; }
        public string PresidenteComite { get; set; }
        public string LeyendaPresidenteComite { get; set; }
        public DateTime FechaActa { get; set; }
        public EstudioTratado estudioTratado { get; set; }
        public ActaEstudio actaEstudio { get; set; }
        public List<Plantilla> plantillasTexto { get; set; }
        public List<Centro> CentrosInternacion { get; set; }
        public List<TipoDocumento> TiposDocumentosTratados { get; set; }
        public List<TipoDocumento> TiposDocumentosEvaluados { get; set; }
        public string UrlDirectorioArchivos { get; set; }

        public ProcesadorPlantillaCarta() 
        {
            HTMLProcesado = new List<string>();
            NombrePlantilla = "PlantillaCartaRespuestaWord.html";
            plantillasTexto = new List<Plantilla>();
            CentrosInternacion = new List<Centro>();
        }
        
        public void ProcesarCartaRespuesta(List<ActaDocumento> actaDocs)
        {
            Estudio estudio = actaEstudio.Estudio;

            if (actaDocs != null && actaDocs.Count > 0)
            {
                if (estudio.Estado == null)
                    throw new ApplicationException(string.Format("En el estudio {0} debe indicar estado.", estudio.Codigo));
                if (estudio.InvestigadoresPrincipales.Count == 0)
                    throw new ApplicationException(string.Format("El estudio {0} no tiene asignado investigador principal.", estudio.Codigo));

                estudio.InvestigadoresPrincipales.ForEach(delegate(EstudioParticipante investigadorPrincipal)
                {
                    this.InvestigadorPrincipal = investigadorPrincipal.Profesional;
                    List<ActaDocumento> documentosInvestigador = new List<ActaDocumento>();
                    actaDocs.ForEach(delegate(ActaDocumento actaDoc)
                    {
                        if (actaDoc.DocumentoVersion.Participantes.Count == 0)
                            documentosInvestigador.Add(actaDoc);
                        else
                        {
                            actaDoc.DocumentoVersion.Participantes.ToList<DocumentoVersionParticipante>().ForEach(delegate(DocumentoVersionParticipante participante)
                            {
                                if (participante.Profesional.Equals(this.InvestigadorPrincipal))
                                    documentosInvestigador.Add(actaDoc);
                            });
                        }
                    });
                    ProcesarDocumentos(documentosInvestigador);
                });               
                    
            }
        }

        private void ProcesarDocumentos(List<ActaDocumento> actaDocumentos)
        {
            if (actaDocumentos == null || actaDocumentos.Count == 0)
                return;

            CartaRespuestaModelo modeloCarta = actaEstudio.CartaRespuestaModelo;

            estudioTratado = ProcesadorHelpers.ObtenerDatosEstudioTratado(actaEstudio.Estudio);
            
            List<DocumentoTratado> documentosEvaluados = new List<DocumentoTratado>();
            List<DocumentoTratado> documentosTomaConocimiento = new List<DocumentoTratado>();
            List<DocumentoTratado> documentosPedidoCambio = new List<DocumentoTratado>();

            DocumentoTratado documentoTratado = new DocumentoTratado();

            int documentosEvaluadosAprobados = 0;

            actaDocumentos.OrderBy(item => item.OrdenGrupoTipoDocumento)
                          .ToList<ActaDocumento>()
                          .ForEach(delegate(ActaDocumento actaDoc) 
            {
                if (actaDoc.DocumentoVersion.Documento.TipoDocumento.ListarCartaRespuesta)
                {
                    if (actaDoc.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo == null)
                        throw new ApplicationException("Debe asignar Grupo para " + actaDoc.DocumentoVersion.Documento.TipoDocumento.Descripcion);

                    if (actaDoc.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo.SeEvalua())
                    {                        
                        if (actaDoc.DocumentoVersion.Documento.IdEstadoActual.Equals(IdEstadoAprobado))
                            documentosEvaluadosAprobados++;
                        documentoTratado = ProcesadorHelpers.ObtenerDocumentoTratado(actaDoc, false);
                        documentoTratado.Orden = string.Format("{0}{1}{2}", actaDoc.Acta.Fecha.ToString("yyyyMMdd"), actaDoc.OrdenGrupoTipoDocumento.ToString(), actaDoc.OrdenDocumento.ToString("00"));
                        documentosEvaluados.Add(documentoTratado);
                    }

                    if (actaDoc.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo.SeTomaConocimiento())
                    {
                        documentoTratado = ProcesadorHelpers.ObtenerDocumentoTratado(actaDoc, false);
                        documentoTratado.Orden = string.Format("{0}{1}{2}", actaDoc.Acta.Fecha.ToString("yyyyMMdd"), actaDoc.OrdenGrupoTipoDocumento.ToString(), actaDoc.OrdenDocumento.ToString("00"));
                        documentosTomaConocimiento.Add(documentoTratado);
                    }

                    EstadoDocumento estadoDocumento = actaDoc.DocumentoVersion.ObtenerVersionEstado().Estado;
                    if (estadoDocumento.Id.Equals(this.IdEstadoPedidoCambio))
                    {
                        documentoTratado = ProcesadorHelpers.ObtenerDocumentoTratado(actaDoc, false);
                        documentoTratado.Orden = string.Format("{0}{1}{2}", actaDoc.Acta.Fecha.ToString("yyyyMMdd"), actaDoc.OrdenGrupoTipoDocumento.ToString(), actaDoc.OrdenDocumento.ToString("00"));
                        documentosPedidoCambio.Add(documentoTratado);
                    }
                }
            });

            List<TipoDocumentoTratado> documentosTratados = ProcesarDocumentosTratados(TiposDocumentosTratados, actaDocumentos);
            List<TipoDocumentoTratado> documentosTratadosEvaluados = ProcesarDocumentosTratados(TiposDocumentosEvaluados, actaDocumentos);

            List<string> vocalesImprime = new List<string>();            
            Vocales.ForEach(delegate(Profesional profesional)
            {
                vocalesImprime.Add(string.Format("{0} ({1})", profesional.NombreYApellido, 
                                                    (profesional.RolComite != null) ? profesional.RolComite.Descripcion : string.Empty));
            });

            IDictionary datos;
            if (this.NombrePlantilla.Contains("PlantillaAprobacionEstudio"))
                datos = ObtenerDatosPlantillaAprobacionEstudio(modeloCarta, documentosTratados, documentosTratadosEvaluados, vocalesImprime);
            else
                datos = ObtenerDatosPlantilla(modeloCarta, documentosEvaluados, documentosTomaConocimiento, documentosPedidoCambio, vocalesImprime);


            INVelocity fileEngine = NVelocityFactory.CreateNVelocityFileEngine(ProcesadorHelpers.DirectorioPlantillas, true);
            
            HTMLProcesado.Add(fileEngine.Process(datos, NombrePlantilla));
        }

        private List<TipoDocumentoTratado> ProcesarDocumentosTratados(List<TipoDocumento> tipoDocsProcesar, List<ActaDocumento> actaDocumentos)
        {
            List<TipoDocumentoTratado> documentosTratados = new List<TipoDocumentoTratado>();
            TipoDocumentoTratado tipoDoc;

            //generamos la coleccion de tipos de documentos con sus documentos para las cartas de  Aprobacion de Estudios
            tipoDocsProcesar.ForEach(delegate(TipoDocumento tipo)
            {
                tipoDoc = new TipoDocumentoTratado();
                tipoDoc.TipoDocumento = tipo.Descripcion;
                tipoDoc.ListarDocumentos = tipo.ListarDocsCartaRespuesta ? "SI" : "NO";
                tipoDoc.Orden = 1;

                if (documentosTratados.Find(item => item.TipoDocumento == tipo.Descripcion) == null)
                {
                    List<ActaDocumento> actaDocs = actaDocumentos.FindAll(item => item.DocumentoVersion.Documento.TipoDocumento.Descripcion.Equals(tipo.Descripcion)).ToList<ActaDocumento>();
                    actaDocs.ForEach(delegate(ActaDocumento actaDoc)
                    {
                        tipoDoc.Documentos.Add(ProcesadorHelpers.ObtenerDocumentoTratado(actaDoc, false));
                    });
                }

                if (tipoDoc.Documentos.Count == 0)
                    tipoDoc.TipoDocumento = string.Concat(tipoDoc.TipoDocumento, ": NO APLICA");
                else
                    if (tipo.ListarDocsCartaRespuesta)
                        tipoDoc.TipoDocumento = string.Concat(tipo.Descripcion, ":");

                documentosTratados.Add(tipoDoc);

            });

            return documentosTratados;
        }

        private IDictionary ObtenerDatosPlantilla(CartaRespuestaModelo modeloCarta, List<DocumentoTratado> documentosEvaluados, List<DocumentoTratado> documentosTomaConocimiento, List<DocumentoTratado> documentosPedidoCambio, List<string> vocalesImprime)
        {
            IDictionary datos = new Hashtable();

            //datos.Add("Logo", string.Format(@"{0}/img/LogoActa.png", URL_Site));
            datos.Add("Logo", string.Format(@"{0}\LogoActa.png", ProcesadorHelpers.DirectorioImagenes));
            datos.Add("headerfooter", string.Format(@"{0}/HeaderFooter.html", ProcesadorHelpers.UrlDirectorioArchivos));
            
            datos.Add("Estudio", estudioTratado.NombreCompleto);
            datos.Add("CodigoEstudio", estudioTratado.Codigo);
            datos.Add("PatrocinadoPor", estudioTratado.Patrocinador);

            if (modeloCarta.PlantillaPiePagina != null)
                datos.Add("TextoPiePagina", modeloCarta.PlantillaPiePagina.Texto.Replace("\n", Constantes.SaldoLinea));
            else
                datos.Add("TextoPiePagina", string.Empty);

            datos.Add("FechaEncabezado", string.Format("C.A.B.A. ____________________________ de {0}", DateTime.Now.Year.ToString()));
            datos.Add("NombreInvestigador", InvestigadorPrincipal.NombreYApellido);
            datos.Add("RolInvestigador", "Investigador Principal");
            datos.Add("FechaActa", FechaActa.ToLongDateString());

            datos.Add("DocumentosSeEvalua", documentosEvaluados.OrderBy(item => item.Orden).ToList<DocumentoTratado>());
            datos.Add("ImprimirDocumentosEvaluados", (modeloCarta.IncluirDocumentosEvaluados && documentosEvaluados.Count > 0) ? "SI" : "NO");

            datos.Add("DocumentosSeTomaConocimiento", documentosTomaConocimiento.OrderBy(item => item.Orden).ToList<DocumentoTratado>());
            datos.Add("ImprimirDocumentosTomaConocimiento", (modeloCarta.IncluirDocumentosTomaConocimiento && documentosTomaConocimiento.Count > 0) ? "SI" : "NO");

            datos.Add("DocumentosConPedidoCambio", documentosPedidoCambio.OrderBy(item => item.Orden).ToList<DocumentoTratado>());
            datos.Add("ImprimirDocumentosPedidoCambio", (modeloCarta.IncluirDocumentosPedidoCambio && documentosPedidoCambio.Count > 0) ? "SI" : "NO");

            datos.Add("PresidenteComite", PresidenteComite);
            datos.Add("LeyendaPresidenteComite", LeyendaPresidenteComite);
            datos.Add("Vocales", vocalesImprime);

            string textoIntro = (modeloCarta.PlantillaIntroduccion != null) ? modeloCarta.PlantillaIntroduccion.Texto : string.Empty;
            //Si existen documentos evaluados, el texto es "Se ha evaluado la siguiente documentacion..."
            //Si no el texto debe ser "Se ha tomado conocimiento..."
            if (modeloCarta.IncluirDocumentosEvaluados && documentosEvaluados.Count == 0
                && modeloCarta.IncluirDocumentosTomaConocimiento && documentosTomaConocimiento.Count > 0)
                textoIntro = (modeloCarta.PlantillaIntroduccionOpcional != null) ? modeloCarta.PlantillaIntroduccionOpcional.Texto : string.Empty;

            datos.Add("TextoIntroduccionCarta", string.Format(textoIntro.Replace("\n", Constantes.SaldoLinea).Replace("&nbsp;", string.Empty), FechaActa.ToLongDateString()));


            bool bloqueAprobacion = false;
            if (modeloCarta.PlantillaTextoAprobacion != null)
            {
                datos.Add("TextoAprobacion", modeloCarta.PlantillaTextoAprobacion.Texto.Replace("\n", Constantes.SaldoLinea).Replace("&nbsp;", string.Empty));
                bloqueAprobacion = true;
            }
            else
                datos.Add("TextoAprobacion", string.Empty);

            if (modeloCarta.PlantillaBuenasPracticas != null)
            {
                //datos.Add("TextoBuenasPracticas", modeloCarta.PlantillaBuenasPracticas.Texto.Replace("\n", Constantes.SaldoLinea).Replace("&nbsp;", string.Empty));
                datos.Add("TextoBuenasPracticas", modeloCarta.PlantillaBuenasPracticas.Texto);
                bloqueAprobacion = true;
            }
            else
                datos.Add("TextoBuenasPracticas", string.Empty);

            datos.Add("BloqueAprobacion", bloqueAprobacion ? "SI" : "NO");

            if (modeloCarta.PlantillaTextoFirmaPresidente != null)
                datos.Add("TextoFirma", modeloCarta.PlantillaTextoFirmaPresidente.Texto.Replace("\n", Constantes.SaldoLinea).Replace("&nbsp;", string.Empty));
            else
                datos.Add("TextoFirma", string.Empty);

            if (actaEstudio.TextoLibreCartaRespuesta != null)
                datos.Add("TextoLibreCartaRespuesta", actaEstudio.TextoLibreCartaRespuesta.Replace("\n", Constantes.SaldoLinea).Replace("&nbsp;", string.Empty));
            else
                datos.Add("TextoLibreCartaRespuesta", string.Empty);

            datos.Add("ComiteNombreCompleto", this.ComiteNombreCompleto.Replace("\n", Constantes.SaldoLinea));
            datos.Add("ComiteIniciales", this.ComiteIniciales.Replace("\n", Constantes.SaldoLinea));

            datos.Add("ImprimirFirmaPresidente", modeloCarta.IncluirFirmaPresidente ? "SI" : "NO");
            datos.Add("ImprimirFirmaParticipantes", modeloCarta.IncluirFirmaMiembros ? "SI" : "NO");
            return datos;
        }

        private IDictionary ObtenerDatosPlantillaAprobacionEstudio(CartaRespuestaModelo modeloCarta, List<TipoDocumentoTratado> tiposDocumentosTratados, List<TipoDocumentoTratado> tiposDocumentosEvaluados, List<string> vocalesImprime)
        {
            IDictionary datos = new Hashtable();

            //datos.Add("Logo", string.Format(@"{0}/img/LogoActa.png", URL_Site));
            datos.Add("Logo", string.Format(@"{0}\LogoActa.png", ProcesadorHelpers.DirectorioImagenes));
            datos.Add("Estudio", estudioTratado.NombreCompleto);
            datos.Add("CodigoEstudio", estudioTratado.Codigo);
            datos.Add("PatrocinadoPor", estudioTratado.Patrocinador);

            datos.Add("FechaActa", FechaActa.ToLongDateString().Substring(FechaActa.ToLongDateString().IndexOf(",") + 2));
            datos.Add("FechaEncabezado", string.Format("Ciudad Autónoma de Buenos Aires, {0}", FechaActa.ToLongDateString().Substring(FechaActa.ToLongDateString().IndexOf(",") + 2)));
            datos.Add("Texto1", this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO 1"));
            datos.Add("Texto2", this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO 2"));
            datos.Add("Texto3", this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO 3"));
            datos.Add("Texto4", this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO 4"));
            datos.Add("Texto5", this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO 5"));
            datos.Add("Texto6", string.Format(this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO 6"), InvestigadorPrincipal.NombreYApellido));
            datos.Add("Texto7", this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO 7"));
            datos.Add("Texto8", this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO 8"));
            
            datos.Add("InvestigadorPrincipal", InvestigadorPrincipal.NombreYApellido);
            string matricula = string.Empty;
            if (InvestigadorPrincipal.MatriculaNacional != null)
                matricula = string.Format("Matrícula nacional {0}", InvestigadorPrincipal.MatriculaNacional);
            else if (InvestigadorPrincipal.MatriculaProvincial != null)
                matricula = string.Format("Matrícula provincial {0}", InvestigadorPrincipal.MatriculaProvincial);
            datos.Add("InvestigadorMatricula", matricula);
            datos.Add("InvestigadorContacto", InvestigadorPrincipal.DatosContacto);

            datos.Add("CentroHabilitado", estudioTratado.CentroHabilitado);
            datos.Add("CentroHabilitadoContacto", estudioTratado.CentroHabilitadoContacto);

            //si no es CIDEA se imprime el texto para estudios subrogados
            if (!actaEstudio.Estudio.CentroHabilitado.Descripcion.Equals(this.ComiteNombreCompleto))
                datos.Add("Texto_EstudioSubrogado", this.ObtenerTextoPlantilla("APROBACION ESTUDIOS - TEXTO ESTUDIOS SUBROGADOS"));
            else
                datos.Add("Texto_EstudioSubrogado", string.Empty);

            datos.Add("TiposDocumentos", tiposDocumentosTratados.OrderBy(item => item.Orden).ToList<TipoDocumentoTratado>());

            if (actaEstudio.TextoLibreCartaRespuesta != null)
                datos.Add("TextoLibreCartaRespuesta", actaEstudio.TextoLibreCartaRespuesta.Replace("\n", Constantes.SaldoLinea).Replace("&nbsp;", string.Empty));
            else
                datos.Add("TextoLibreCartaRespuesta", string.Empty);

            datos.Add("TipoEstudio_Texto1", (actaEstudio.Estudio.TipoEstudio != null && actaEstudio.Estudio.TipoEstudio.CR_Texto_1 != null ? actaEstudio.Estudio.TipoEstudio.CR_Texto_1 : string.Empty));
            datos.Add("TipoEstudio_Texto2", (actaEstudio.Estudio.TipoEstudio != null && actaEstudio.Estudio.TipoEstudio.CR_Texto_2 != null ? actaEstudio.Estudio.TipoEstudio.CR_Texto_2 : string.Empty));
            datos.Add("TipoEstudio_Texto3", (actaEstudio.Estudio.TipoEstudio != null && actaEstudio.Estudio.TipoEstudio.CR_Texto_3 != null ? actaEstudio.Estudio.TipoEstudio.CR_Texto_3 : string.Empty));

            datos.Add("CentrosInternacion", this.CentrosInternacion);

            datos.Add("TiposDocumentosSeEvalua", tiposDocumentosEvaluados.OrderBy(item => item.Orden).ToList<TipoDocumentoTratado>());

            datos.Add("PresidenteComite", PresidenteComite);
            datos.Add("Vocales", vocalesImprime);

            if (modeloCarta.PlantillaPiePagina != null)
                datos.Add("TextoPiePagina", modeloCarta.PlantillaPiePagina.Texto.Replace("\n", Constantes.SaldoLinea));
            else
                datos.Add("TextoPiePagina", string.Empty);
            

            return datos;
        }

        private string ObtenerTextoPlantilla(string descripcion)
        {
            string textoReturn = string.Empty;

            Plantilla plantilla = plantillasTexto.Find(item => item.Descripcion.Equals(descripcion));
            if (plantillasTexto != null)
                textoReturn = plantilla.Texto;

            return textoReturn;
        }        
    }
}
