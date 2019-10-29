using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Impresion.NVelocity;
using pome.SysGEIC.Impresion.DTO;

namespace pome.SysGEIC.Impresion
{
    public class ProcesadorPlantillaActa
    {

        public string nombrePlantilla { get; set; }

        public bool ProcesarConFormato { get; set; }

        public ProcesadorPlantillaActa()
        {
            this.nombrePlantilla = "PlantillaActa.html";
            this.ProcesarConFormato = true;
        }

        public string PorcesarActa(Acta acta)
        {
            List<EstudioTratado> estudiosTratados = new List<EstudioTratado>();
            EstudioTratado estudioTratado = null;
            DocumentoTratado documentoTratado;
            Estudio estudio = null;
            int orden = 0;
            int ordenDocumento = 0;
            int ordenGrupo = -1;
            int ordenGrupoImprime = 0;

            List<ActaDocumento> documentosTratadosActa = acta.Documentos.ToList<ActaDocumento>()
                                                                        .OrderBy(Item => Item.Orden)
                                                                        .ToList<ActaDocumento>();

            documentosTratadosActa.ForEach(delegate(ActaDocumento actaDocumento)
            {
                try
                {
                    if (estudio == null || estudio != actaDocumento.DocumentoVersion.Documento.Estudio)
                    {
                        if (orden > 0)
                            estudiosTratados.Add(estudioTratado);

                        estudio = actaDocumento.DocumentoVersion.Documento.Estudio;
                        estudioTratado = ProcesadorHelpers.ObtenerDatosEstudioTratado(estudio);
                        orden++;
                        estudioTratado.Orden = orden;
                        ordenDocumento = 0;
                        ordenGrupo = 0;
                        ordenGrupoImprime = 0;
                    }

                    if (actaDocumento.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo == null)
                        throw new ApplicationException(string.Format("Para el tipo de documento {0} debe configurar el Tratamiento en Acta", actaDocumento.DocumentoVersion.Documento.TipoDocumento.Descripcion));
               
                    if (ordenGrupo != actaDocumento.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo.Orden)
                    {
                        ordenGrupo = actaDocumento.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo.Orden;
                        ordenGrupoImprime++;
                        ordenDocumento = 0;
                    }

                    documentoTratado = ProcesadorHelpers.ObtenerDocumentoTratado(actaDocumento, true);

                    documentoTratado.Grupo = string.Format("{0} - {1}", ordenGrupoImprime.ToString(), actaDocumento.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo.TextoActa.Trim());

                    ordenDocumento++;
                    documentoTratado.Orden = string.Format("{0}.{1}.{2}", estudioTratado.Orden.ToString(), ordenGrupoImprime.ToString(), ordenDocumento.ToString());

                    estudioTratado.Documentos.Add(documentoTratado);
                }
                catch (Exception ex)
                {
                    Logger.LogError("ProcesadorPlantillaActa", ex);
                    throw;
                }
            });
            if (estudioTratado != null)
                estudiosTratados.Add(estudioTratado);

            List<Nota> notasTratadas = acta.Notas.OrderBy(item => item.NombreEstudio)
                                                .ToList<Nota>();

            notasTratadas.ForEach(delegate(Nota nota)
            {
                estudioTratado = null;

                estudioTratado = estudiosTratados.Find(delegate(EstudioTratado estTratado)
                {
                    return estTratado.Codigo == nota.Estudio.NombreEstudioListados;
                });

                bool agregar = false;

                if (estudioTratado == null)
                {
                    estudioTratado = ProcesadorHelpers.ObtenerDatosEstudioTratado(nota.Estudio);
                    estudioTratado.Orden = ++orden;
                    agregar = true;
                }

                if (!nota.ActaImprimeAlFinal)
                    estudioTratado.NotasInicio.Add(ProcesadorHelpers.QuitarTagP(nota.Texto.Replace("\n", Constantes.SaldoLinea)));
                else
                    estudioTratado.NotasFinal.Add(ProcesadorHelpers.QuitarTagP(nota.Texto.Replace("\n", Constantes.SaldoLinea)));

                if (agregar)
                    estudiosTratados.Add(estudioTratado);

            });

            IDictionary datos = new Hashtable();
            datos.Add("ConFormato", ProcesarConFormato ? 1 : 0);
            datos.Add("Logo", string.Format(@"{0}\LogoActa.png", ProcesadorHelpers.DirectorioImagenes));
            datos.Add("Titulo", acta.Descripcion);
            datos.Add("ComentarioInicialFijo", acta.ComentarioInicialFijo == null ? string.Empty : acta.ComentarioInicialFijo.Replace("\n", Constantes.SaldoLinea));
            datos.Add("ComentarioInicial", acta.ComentarioInicial == null ? string.Empty : acta.ComentarioInicial.Replace("\n", Constantes.SaldoLinea));
            datos.Add("ComentarioFinal", acta.ComentarioFinal == null ? string.Empty : acta.ComentarioFinal.Replace("\n", Constantes.SaldoLinea));
            datos.Add("EstudiosTratados", estudiosTratados);

            INVelocity fileEngine = NVelocityFactory.CreateNVelocityFileEngine(ProcesadorHelpers.DirectorioPlantillas, true);
            return fileEngine.Process(datos, nombrePlantilla);
        }

    }   
}
