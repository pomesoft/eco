using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Impresion;
using pome.SysGEIC.Repositorios;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public class ServicioImpresion
    {
        public ServicioImpresion() { }

        public string ImprimirHeaderFooter()
        {
            ProcesadorPlantilla procesador = new ProcesadorPlantilla();
            procesador.NombrePlantilla = "HeaderFooter";
            procesador.Datos = new Hashtable();
            procesador.ProcesarPlantilla();
            return procesador.HTMLProcesado;
        }

        public string ImprimirPlantilla(string plantilla)
        {
            ProcesadorPlantilla procesador = new ProcesadorPlantilla();
            procesador.NombrePlantilla = plantilla;
            procesador.Datos = new Hashtable();
            procesador.ProcesarPlantilla();
            return procesador.HTMLProcesado;
        }

        public void GenerarHTMLHeaderFooter(string idActa, string idEstudio, string pathArchivos)
        {
            ServicioActas servActa = new ServicioActas();
            Acta acta = servActa.Obtener(idActa);
            ActaEstudio actaEstudio = acta.ObtenerEstudio(idEstudio.ConvertirInt());

            ProcesadorPlantillaEncabezadoPiePagina procesador = new ProcesadorPlantillaEncabezadoPiePagina();
            procesador.estudio = actaEstudio.Estudio;
            procesador.modeloCarta = actaEstudio.CartaRespuestaModelo;
            procesador.ProcesarPlantilla();

            string pathHTMLHeaderFooter = pathArchivos + @"\HeaderFooter.html";
            if (File.Exists(pathHTMLHeaderFooter))
                File.Delete(pathHTMLHeaderFooter);

            using (StreamWriter sw = File.CreateText(pathHTMLHeaderFooter))
            {
                sw.Write(procesador.HTMLProcesado);
                sw.Close();
            }

        }

        public string ImprimirActa(string idActa, bool conFormato, bool exportarWord)
        {
            ServicioActas servicio = new ServicioActas();
            Acta acta = servicio.Obtener(idActa);

            ProcesadorPlantillaActa procesador = new ProcesadorPlantillaActa();
            procesador.ProcesarConFormato = conFormato;
            if (exportarWord)
                procesador.nombrePlantilla = "PlantillaActaWord.html";
            else
                procesador.nombrePlantilla = "PlantillaActa.html";

            return procesador.PorcesarActa(acta);
        }

        public List<string> ImprimirCartaRespuesta(string idActa, string idEstudio, bool exportarWord)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            ServicioDocumentos servDocumentos = new ServicioDocumentos();

            string idEstadoPedidoCambio = servParametricas.ParametroObtener("ESTADO_DOC_PEDIDO_CAMBIO");
            string idEstadoAprobado = servParametricas.ParametroObtener("ESTADO_DOC_APROBADO");
            
            ServicioActas servicio = new ServicioActas();
            Acta acta = servicio.Obtener(idActa);

            List<ActaDocumento> actaDocs = acta.Documentos.ToList<ActaDocumento>()
                                                          .FindAll(item => item.DocumentoVersion.Documento.Estudio.Id == idEstudio.ConvertirInt());
                                                                        //&& item.ImprimirCarta);

            if (actaDocs == null || actaDocs.Count == 0) return new List<string>();

            ProcesadorPlantillaCarta procesador = new ProcesadorPlantillaCarta();
            
            procesador.IdEstadoPedidoCambio = idEstadoPedidoCambio.ConvertirInt();
            procesador.IdEstadoAprobado = idEstadoAprobado.ConvertirInt();

            procesador.ComiteNombreCompleto = servParametricas.ParametroObtener("COMITE_NOMBRE_COMPLETO");
            procesador.ComiteIniciales = servParametricas.ParametroObtener("COMITE_INICIALES");
            procesador.Vocales = acta.Vocales;
            procesador.PresidenteComite = acta.PresidenteComite != null ? acta.PresidenteComite.NombreYApellido : string.Empty;
            procesador.LeyendaPresidenteComite = acta.ObtenerRolComiteParticipantes(acta.PresidenteComite) != null ? acta.ObtenerRolComiteParticipantes(acta.PresidenteComite).Descripcion : "Presidente"; 
            procesador.FechaActa = acta.Fecha;
            //ProcesadorHelpers

            ActaEstudio actaEstudio = acta.ObtenerEstudio(idEstudio.ConvertirInt());
            if (actaEstudio.Estudio.IdTipoEstudio.HasValue)
                actaEstudio.Estudio.TipoEstudio = servParametricas.TipoEstudioObtener(actaEstudio.Estudio.IdTipoEstudio.Value);
            procesador.actaEstudio = actaEstudio;

            if (actaEstudio.Estudio.CentroHabilitado != null)
                procesador.CentrosInternacion = servParametricas.CentroObtenerCentrosInternacion(actaEstudio.Estudio.CentroHabilitado.Id);

            procesador.TiposDocumentosTratados = servDocumentos.TiposDocumentoObtenerListarCartaRespuesta(-1);
            procesador.TiposDocumentosEvaluados = servDocumentos.TiposDocumentoObtenerListarCartaRespuesta((int)TIPO_DOCUMENTO_GRUPO.SE_EVALUA);

            if (actaEstudio.CartaRespuestaModelo.IncluirTodosDocumentosEstudio)
                actaDocs = servicio.ListarDocumentoTratadosXEstudio(idEstudio);

            procesador.plantillasTexto = servParametricas.PlantillaObtenerVigentes((int)TIPOS_PLANTILLA.APROBACION_ESTUDIO);

            //procesador.NombrePlantilla = "AprobacionEstudioModelo.html";
            procesador.NombrePlantilla = string.Format("{0}{1}.html", actaEstudio.CartaRespuestaModelo.NombrePlantilla, (exportarWord ? "Word" : string.Empty));            

            procesador.ProcesarCartaRespuesta(actaDocs);

            return procesador.HTMLProcesado;
        }

        public string  NombreArchivoCartaRespuesta(string idActa, string idEstudio)
        {
            ServicioActas servicio = new ServicioActas();
            Acta acta = servicio.Obtener(idActa);

            List<ActaDocumento> actaDocs = acta.Documentos.ToList<ActaDocumento>()
                                                          .FindAll(item => item.DocumentoVersion.Documento.Estudio.Id == idEstudio.ConvertirInt());

            string nombreReturn = "CartaRespuesta";

            if (actaDocs != null && actaDocs.Count > 0)
                nombreReturn = string.Format("{0}_{1}_", nombreReturn, actaDocs[0].CodigoEstudio);

            nombreReturn = string.Format("{0}_{1}", nombreReturn, acta.Fecha.ToString("dd-MM-yy"));

            return nombreReturn;
        }
    }
}
