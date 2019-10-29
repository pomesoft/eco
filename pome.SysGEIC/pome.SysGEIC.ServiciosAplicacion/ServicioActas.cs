using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Repositorios;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public class ServicioActas
    {
        ActasRepository repository;

        public ServicioActas()
        {
            repository = new ActasRepository();
        }

        public Acta Obtener(string idActa)
        {
            ServicioEstudios servEstudio = new ServicioEstudios();
            Acta acta = null;

            if (idActa.ConvertirInt() != -1)
            {
                acta = repository.Obtener(idActa.ConvertirInt());
                acta.ComentarioInicialFijo = ServiciosHelpers.ObtenerTextoPlano(acta.ComentarioInicialFijo);
                acta.ComentarioInicial = ServiciosHelpers.ObtenerTextoPlano(acta.ComentarioInicial);
                acta.ComentarioFinal = ServiciosHelpers.ObtenerTextoPlano(acta.ComentarioFinal);
                //TODO: Para optimizar performance si quitamos del hbm el mapeo de documentos y notas, se deberian modificar todos las grabaciones de estos objetos
                //acta.Documentos = repository.ObtenerActaDocumentosTratados(idActa.ConvertirInt());
                acta.Notas = repository.ObtenerActaNotas(idActa.ConvertirInt());
            }

            if (acta != null)
            {
                acta.Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDoc)
                {
                    actaDoc.DocumentoVersion.Documento.Estudio = servEstudio.ObtenerSoloEstudio(actaDoc.DocumentoVersion.Documento.IdEstudio.ToString());
                    actaDoc.Descripcion = ServiciosHelpers.ObtenerTextoPlano(actaDoc.ComentarioDocumento);
                    acta.Notas.ToList<Nota>().FindAll(item => item.IdEstudio == actaDoc.IdEstudio)
                                             .ForEach(delegate(Nota nota)
                                             {
                                                 nota.ActaOrden = actaDoc.OrdenEstudio;
                                             });                    
                });
                List<Nota> listNotas = acta.Notas.ToList<Nota>()
                                                 .OrderBy(item => item.ActaOrden)
                                                 .ToList<Nota>();

                listNotas.ForEach(delegate(Nota nota)
                {
                    nota.Estudio = servEstudio.ObtenerSoloEstudio(nota.IdEstudio.ToString());
                    nota.Texto = ServiciosHelpers.ObtenerTextoPlano(nota.Texto);
                });
                
                acta.Notas.Clear();
                acta.Notas = listNotas;
                // .OrderBy(item => (item.ActaImprimeAlFinal ? 0 : 1)
            }            

            return acta;
        }

        public List<Acta> Listar()
        {
            List<Acta> actasReturn = repository.ObtenerTodosVigentes().ToList<Acta>();
            return actasReturn;
        }

        public List<ActaDocumentoDTO> ListarActasPorEstudio(string idEstudio)
        {
            List<Acta> listActas = null;
            
            int _idEstudio = -1;
            if (int.TryParse(idEstudio, out _idEstudio))
                listActas = repository.ObtenerActas(_idEstudio).ToList<Acta>();
            
            List<ActaDocumentoDTO> listReturn = new List<ActaDocumentoDTO>();

            listActas.OrderByDescending(item => item.Fecha).ToList<Acta>()
                    .ForEach(delegate(Acta acta) 
                    { 
                        listReturn.Add(DameActaDocumentoDTO(acta)); 
                    });

            return listReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cerrada">True/False para filtrar actas cerradas</param>
        /// <param name="orden">1 ascendente - 2 descendente</param>
        /// <returns></returns>
        public List<ActaDocumentoDTO> ListarActas(string cerrada, string orden)
        {
            List<Acta> listActas = null;

            if (orden.ConvertirInt() == 2)
                listActas = repository.ObtenerActas(cerrada.ConvertirBool()).ToList<Acta>()
                                                                            .OrderByDescending(item => item.Fecha)
                                                                            .ToList<Acta>();
            else
                listActas = repository.ObtenerActas(cerrada.ConvertirBool()).ToList<Acta>()
                                                                            .OrderBy(item => item.Fecha)
                                                                            .ToList<Acta>();
            
            List<ActaDocumentoDTO> listReturn = new List<ActaDocumentoDTO>();
            listActas.ForEach(delegate(Acta acta) { listReturn.Add(DameActaDocumentoDTO(acta)); });

            return listReturn;
        }

        public List<ActaDTO> ListarActasDTO(string cerrada, string descripcion, string orden)
        {
            IList<ActaDTO> listActas = repository.ListarActasDTO(cerrada.ConvertirBoolNulleable(), descripcion, orden.ConvertirInt());

           return listActas.ToList<ActaDTO>();           
        }

        public List<ActaDocumentoDTO> BuscarActas(string cerrada, string descripcion)
        {
            bool? _cerrada = null;

            if (!string.IsNullOrEmpty(cerrada))
                _cerrada = cerrada.ConvertirBool();

            List<Acta> listActas = repository.BuscarActas(_cerrada, descripcion).ToList<Acta>()
                                                                                .OrderByDescending(item => item.Fecha)
                                                                                .ToList<Acta>();

            List<ActaDocumentoDTO> listReturn = new List<ActaDocumentoDTO>();
            listActas.ForEach(delegate(Acta acta) { listReturn.Add(DameActaDocumentoDTO(acta)); });

            return listReturn;
        }

        public ActaDTO ObtenerActaDTO(string idActa)
        {
            ServicioEstudios servEstudios = new ServicioEstudios();

            Acta acta = this.Obtener(idActa);

            ActaDTO actaReturn = new ActaDTO();

            actaReturn.IdActa = acta.Id;
            actaReturn.Descripcion = acta.Descripcion;
            actaReturn.FechaActa = acta.Fecha;
            actaReturn.Hora = acta.Hora;
            actaReturn.Cerrada = acta.Cerrada;
            actaReturn.ComentarioInicialFijo = acta.ComentarioInicialFijo;
            actaReturn.ComentarioInicial = acta.ComentarioInicial;
            actaReturn.ComentarioFinal = acta.ComentarioFinal;

            actaReturn.EstudiosTratados = ListarEstudiosDelActa(acta);

            actaReturn.Participantes = acta.Participantes;

            return actaReturn;
        }

        public ActaDTO ObtenerProximaActaDTO()
        {
            return repository.ObtenerProximaActaDTO();
        }

        public ActaEstudio ObtenerActaEstudio(string idActa, string idEstudio)
        {
            Acta acta = this.Obtener(idActa);
            ActaEstudio actaEstudioReturn = null;
            if (acta != null)
            {
                actaEstudioReturn = acta.ObtenerEstudio(idEstudio.ConvertirInt());
                if (actaEstudioReturn != null)
                {
                    List<Nota> notasEstudio = acta.Notas.ToList<Nota>().FindAll(item => item.IdEstudio == idEstudio.ConvertirInt());
                    actaEstudioReturn.ComentarioAntesDocumentos = "";
                    actaEstudioReturn.ComentarioDespuesDocumentos = "";
                    notasEstudio.ForEach(delegate(Nota nota) 
                    {
                        if (!nota.ActaImprimeAlFinal)
                            actaEstudioReturn.ComentarioAntesDocumentos += nota.Texto;
                        if (nota.ActaImprimeAlFinal)
                            actaEstudioReturn.ComentarioDespuesDocumentos += nota.Texto;
                    });
                    if (actaEstudioReturn.TextoLibreCartaRespuesta.ConvertirString() == string.Empty)
                        actaEstudioReturn.TextoLibreCartaRespuesta = actaEstudioReturn.ComentarioDespuesDocumentos;
                }
            }
            return actaEstudioReturn; 
        }


        public List<Nota> ObtenerActa_NotasEstudio(string idActa, string idEstudio)
        {
            Acta acta = this.Obtener(idActa);
            return acta.Notas.ToList<Nota>().FindAll(item => item.IdEstudio == idEstudio.ConvertirInt());
        }

        public List<ActaEstudio> ListarActaEstudios(string idActa)
        {
            Acta acta = this.Obtener(idActa);
            return acta.Estudios.ToList<ActaEstudio>()
                                .OrderBy(item => item.OrdenEstudio)
                                .ToList<ActaEstudio>();
        }

        public List<ActaEstudioDTO> ListarEstudiosDelActa(string idActa)
        {
            ServicioEstudios servEstudios = new ServicioEstudios();

            Acta acta = this.Obtener(idActa);

            return ListarEstudiosDelActa(acta);
        }

        public List<ActaEstudioDTO> ListarEstudiosDelActa(Acta acta)
        {
            ServicioEstudios servEstudios = new ServicioEstudios();

            List<ActaEstudioDTO> estudiosReturn = new List<ActaEstudioDTO>();

            acta.Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDocumento) 
            {
                ActaEstudioDTO estudioDTO = estudiosReturn.Find(delegate(ActaEstudioDTO estudio) 
                { 
                    return estudio.Id == actaDocumento.DocumentoVersion.Documento.Estudio.Id; 
                });
                if (estudioDTO == null)
                {
                    ActaEstudioDTO dto = servEstudios.DameActaEstudioDTO(actaDocumento.DocumentoVersion.Documento.Estudio, actaDocumento);
                    //dto.DocumentosTratados = ListarDocumentosDelActa(acta, dto.Id);
                    estudiosReturn.Add(dto);
                }
            });
            
            acta.Notas.ToList<Nota>().ForEach(delegate(Nota nota) 
            {
                ActaEstudioDTO estudioDTO = estudiosReturn.Find(delegate(ActaEstudioDTO estudio)
                {
                    return estudio.Id == nota.IdEstudio;
                });
                if (estudioDTO == null)
                {
                    Estudio estudio = servEstudios.Obtener(nota.IdEstudio.ToString());
                    estudiosReturn.Add(servEstudios.DameActaEstudioDTO(estudio));
                }
            });
            
            return estudiosReturn;
        }

        private List<ActaDocumentoDTO> ListarDocumentosDelActa(Acta acta, int idEstudio)
        {
            List<ActaDocumentoDTO> documentosReturn = new List<ActaDocumentoDTO>();
            ActaDocumentoDTO documentoDTO = null;

            acta.Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDoc)
            {
                if (actaDoc.IdEstudio == idEstudio)
                {
                    documentoDTO = new ActaDocumentoDTO();
                    documentoDTO = DameActaDocumentoDTO(actaDoc.Acta);
                    documentoDTO.IdDocumento = actaDoc.DocumentoVersion.Documento.Id;
                    documentoDTO.IdActaDocumento = actaDoc.Id;
                    documentoDTO.TipoDocumentoDescripcion = actaDoc.DocumentoVersion.Documento.TipoDocumento.Descripcion;
                    documentoDTO.Documento = actaDoc.NombreDocumento;
                    documentoDTO.DocumentoVersion = actaDoc.VersionDocumento;
                    documentoDTO.DocumentoVersionFecha = actaDoc.VersionFecha;
                    documentoDTO.DocumentoVersionEstado = actaDoc.DocumentoVersion.ObtenerVersionEstado().Estado.Descripcion;
                    documentoDTO.OrdenEstudio = actaDoc.OrdenEstudio;
                    documentoDTO.OrdenDocumento = actaDoc.OrdenDocumento;

                    documentosReturn.Add(documentoDTO);
                }
            });

            return documentosReturn;
        }

        public List<Documento> ListarDocumentosDelEstudiosDelActa(string idActa, string idEstudio)
        {
            ServicioEstudios servEstudios = new ServicioEstudios();

            List<Documento> documentosReturn = new List<Documento>();
            Documento documento = null;
            //DocumentoVersion documentoVersion = null;

            Acta acta = this.Obtener(idActa);
            int _idEstudio = idEstudio.ConvertirInt();
            
            if(_idEstudio!=-1)
            {
                acta.Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDocumento)
                {
                    if (actaDocumento.DocumentoVersion.Documento.Estudio.Id == _idEstudio)
                    {
                        documento = new Documento();
                        documento.Id = actaDocumento.DocumentoVersion.Documento.Id;
                        documento.Descripcion = actaDocumento.DocumentoVersion.Documento.Descripcion;
                        documento.TipoDocumento = actaDocumento.DocumentoVersion.Documento.TipoDocumento;
                        documento.Limitante = false;
                        documento.Estudio = actaDocumento.DocumentoVersion.Documento.Estudio;

                        documento.AgregarVersion(actaDocumento.DocumentoVersion);

                        documentosReturn.Add(documento);
                    }
                });
            }            

            return documentosReturn;
        }

        public ActaDocumentoDTO DameActaDocumentoDTO(Acta acta)
        {
            ActaDocumentoDTO entidadDTO = new ActaDocumentoDTO();
            entidadDTO.Id = acta.Id;
            entidadDTO.Descripcion = acta.Descripcion;
            entidadDTO.Fecha = acta.FechaToString;
            entidadDTO.Cerrada = acta.Cerrada;            
            return entidadDTO;
        }

        public List<ActaDocumentoDTO> ListarActasXDocumento(string idDocumento)
        {
            List<ActaDocumentoDTO> listReturn = new List<ActaDocumentoDTO>();
            ActaDocumentoDTO entidadDTO = null;

            List<ActaDocumento> listActas = repository.ObtenerActasXDocumento(idDocumento.ConvertirInt()).ToList<ActaDocumento>();

            listActas.ForEach(delegate(ActaDocumento actaDoc)
            {
                if (actaDoc.IdDocumento == idDocumento.ConvertirInt())
                {
                    entidadDTO = DameActaDocumentoDTO(actaDoc.Acta);
                    entidadDTO.IdActaDocumento = actaDoc.Id;
                    entidadDTO.DocumentoVersion = actaDoc.VersionDocumento;
                    entidadDTO.DocumentoVersionEstado = actaDoc.DocumentoVersion.ObtenerVersionEstado().Estado.Descripcion;
                    entidadDTO.OrdenEstudio = actaDoc.OrdenEstudio;
                    entidadDTO.OrdenDocumento = actaDoc.OrdenDocumento;

                    entidadDTO.Comentario = actaDoc.ComentarioDocumento;

                    listReturn.Add(entidadDTO);
                }
            });

            return listReturn;
        }

        public List<ActaDocumento> ListarDocumentoXActaEstudio(string idActa, string idEstudio)
        {
            List<ActaDocumento> listReturn = repository.ObtenerDocumentoTratados_ActaEstudio(idActa.ConvertirInt(), 
                                                                                            idEstudio.ConvertirInt())
                                                      .ToList<ActaDocumento>();

            return listReturn;
        }

        public List<ActaDocumento> ListarDocumentoTratadosXEstudio(string idEstudio)
        {
            List<ActaDocumento> docsTratados = new List<ActaDocumento>();
            if (idEstudio.ConvertirInt() != -1)
                docsTratados = repository.ObtenerDocumentoTratadosXEstudio(idEstudio.ConvertirInt()).ToList<ActaDocumento>();
            
            List<ActaDocumento> docsTratadosReturn = new List<ActaDocumento>();

            docsTratados.ForEach(delegate(ActaDocumento actaDoc)
            {
                ActaDocumento actaDocReturn = docsTratadosReturn.Find(delegate(ActaDocumento ad)
                {
                    return ad.DocumentoVersion.Documento.Equals(actaDoc.DocumentoVersion.Documento);
                });

                if (actaDocReturn != null)
                    docsTratadosReturn.Remove(actaDocReturn);

                docsTratadosReturn.Add(actaDoc);
            });
    
            return docsTratadosReturn;
        }

        public int Grabar(Acta acta)
        {
            acta.Validar();
            repository.Actualizar(acta);
            return acta.Id;
        }

        public int Grabar(string idActa, string descripcion, string fecha, string hora, string comentarioInicialFijo, string comentarioInicial, string comentarioFinal, string cerrada, string participantes)
        {
            ServicioEstudios servEstudio = new ServicioEstudios();
            ServicioEquipos servEquipo = new ServicioEquipos();
            ServicioParametricas servParametricas = new ServicioParametricas();

            Acta acta = null;

            int _idActa = idActa.ConvertirInt();

            descripcion = string.IsNullOrEmpty(descripcion) ? string.Empty : descripcion;

            if (descripcion.StartsWith("DOSSIER"))
                descripcion = string.Format("{0} - Acta número: {1} - {2}", descripcion, idActa.ToString(), DateTime.Now.ToString("dd/mm/yyyy"));

            if (_idActa == -1)
            {
                List<Acta> actasMismaDescripcion = repository.ObtenerTodosVigentes(descripcion).ToList<Acta>();
                if (actasMismaDescripcion.Count > 0)
                    throw new ApplicationException(string.Format(@"Ya existe una Acta con la Descripcion {0}", descripcion));

                acta = new Acta();

                this.InicializarParticipantes(acta);
            }
            else
            {
                acta = this.Obtener(idActa);

                string[] actaParticipantes = participantes.Split(';');
                foreach (string participante in actaParticipantes)
                    if (participante.Trim().Length > 0)
                    {
                        string[] infoParticipante = participante.Split(',');
                        int idActaParticipante = infoParticipante[0].ConvertirInt();
                        Profesional profesional = servEquipo.ProfesionalObtener(infoParticipante[1].ConvertirInt());
                        RolComite rolComite = servParametricas.ObtenerObjeto<RolComite>(infoParticipante[2].ConvertirInt());

                        acta.ActualizarParticipanteRolcomite(idActaParticipante, rolComite);
                    }

            }
            acta.Descripcion = descripcion.ConvertirString();
            acta.Fecha = fecha.ConvertirDateTime();
            acta.Hora = hora;

            acta.ComentarioInicialFijo = comentarioInicialFijo;
            acta.ComentarioInicial = comentarioInicial.ConvertirString();
            acta.ComentarioInicial = comentarioInicial.ConvertirString();
            acta.ComentarioFinal = comentarioFinal.ConvertirString();
            acta.Cerrada = cerrada.ConvertirBool();
            
            acta.Vigente = true;

            return this.Grabar(acta);
        }

        
        public string ArmarComentarioInicialFijo(Acta acta)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            string plantillaCometarioInicial = servParametricas.PlantillaObtenerTexto("ACTA - TEXTO INTRODUCTORIO");
            //texto a obtener de plantillas
            if (plantillaCometarioInicial == null || plantillaCometarioInicial == string.Empty)
                plantillaCometarioInicial = "En la Ciudad Autónoma de Buenos Aires, a las {0} hs del día {1}, se reúnen: {2} para dar comienzo a la reunión del Comité de Ética en Farmacología Clínica de la Fundación CIDEA (CEFC) con los siguientes temas a tratar: ";

            return string.Format(plantillaCometarioInicial, acta.Hora, acta.Fecha.ToLongDateString(), acta.ParticipantesToString);
        }


        public void GrabarDocumento(string idActa, string idActaDocumento, string idDocumento, string idDocumentoVersion, string comentario, string idResponsableComite, string imprimirCarta, Usuario usuarioLogin)
        {
            Acta acta = this.Obtener(idActa);
            ServiciosHelpers.ValidarClave(acta);

            GrabarDocumento(acta, idActaDocumento, idDocumento, idDocumentoVersion, comentario, idResponsableComite, imprimirCarta, usuarioLogin);

            this.Grabar(acta);
        }
        public void GrabarDocumento(string idActa, string documentos, string idEstudio, Usuario usuarioLogin)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            List<ActaDocumentoJsonDTO> actaDocumentos = serializer.Deserialize<List<ActaDocumentoJsonDTO>>(documentos);

            Acta acta = this.Obtener(idActa);
            ServiciosHelpers.ValidarClave(acta);

            actaDocumentos.ForEach(delegate(ActaDocumentoJsonDTO doc)
            {                
                this.GrabarDocumento(acta, doc.IdActaDocumento, doc.IdDocumento, doc.IdDocumentoVersion, doc.Comentario, null, doc.ImprimirCarta, usuarioLogin);
            });
            this.Grabar(acta);

            acta = this.Obtener(idActa);
            this.GrabarEstudioActa(acta, idEstudio);
            this.Grabar(acta);
        }

        public void GrabarOrdenDocumentos(string idActa, string documentos, string estudios)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Acta acta = this.Obtener(idActa);
            ServiciosHelpers.ValidarClave(acta);

            //actualizo orden de los documentos
            List<ActaOrdenDocumentosJsonDTO> actaDocumentosDTO = serializer.Deserialize<List<ActaOrdenDocumentosJsonDTO>>(documentos);
            actaDocumentosDTO.ForEach(delegate(ActaOrdenDocumentosJsonDTO docDTO)
            {
                ActaDocumento actaDocumento = acta.Documentos.ToList<ActaDocumento>().Find(delegate(ActaDocumento actaDoc) { return actaDoc.Id == docDTO.IdActaDocumento; });
                if (actaDocumento != null)
                    actaDocumento.OrdenDocumento = docDTO.OrdenDocumento;                
            });

            //actualizo orden de los estudios
            actaDocumentosDTO = serializer.Deserialize<List<ActaOrdenDocumentosJsonDTO>>(estudios);
            actaDocumentosDTO.ForEach(delegate(ActaOrdenDocumentosJsonDTO docDTO)
            {
                List<ActaDocumento> actaDocumentos = acta.Documentos.ToList<ActaDocumento>().FindAll(delegate(ActaDocumento actaDoc) { return actaDoc.IdEstudio == docDTO.IdEstudio; });
                if (actaDocumentos != null)
                    actaDocumentos.ForEach(delegate(ActaDocumento actaDoc)
                    {
                        actaDoc.OrdenEstudio = docDTO.OrdenEstudio;
                    });
            });
            
            this.Grabar(acta);
        }

        public void GrabarDocumento(Acta acta, string idActaDocumento, string idDocumento, string idDocumentoVersion, string comentario, string idResponsableComite, string imprimirCarta, Usuario usuarioLogin)
        {
            ServicioDocumentos servDocumento = new ServicioDocumentos();
            ServicioEquipos servEquipo = new ServicioEquipos();
            
            ActaDocumento actaDocumento = null;

            int _idDocumentoVersion = idDocumentoVersion.ConvertirInt();
            if (_idDocumentoVersion == -1)
                throw new ApplicationException("Debe seleccionar documento y versión a comentar");

            int _idActaDocumento = idActaDocumento.ConvertirInt();
            if (_idActaDocumento == -1)
                actaDocumento = new ActaDocumento();
            else
                actaDocumento = acta.ObtenerDocumento(_idActaDocumento);
            
            DocumentoVersion docVersion = servDocumento.Obtener(idDocumento)
                                                        .ObtenerVersion(_idDocumentoVersion);
            actaDocumento.DocumentoVersion = docVersion;
            actaDocumento.Descripcion = comentario;
            actaDocumento.ResponsableComite = servEquipo.ProfesionalObtener(idResponsableComite.ConvertirInt());
            actaDocumento.OrdenEstudio = acta.ObtenerOrdenEstudio(docVersion.Documento.Estudio.Id);
            actaDocumento.OrdenDocumento = acta.ObtenerOrdenUltimoDocumentoDelEstudio(docVersion.Documento.Estudio.Id);
            actaDocumento.ImprimirCarta = imprimirCarta.ConvertirBool();

            if (_idActaDocumento == -1)
            {                
                ServicioParametricas servParametricas = new ServicioParametricas();
                Parametro parmsEstado = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "ESTADO_DOC_EN_EVALUACION");
                string _idEstado = (parmsEstado != null) ? parmsEstado.Valor : null;
                EstadoDocumento estado = servParametricas.EstadoDocumentoObtener(_idEstado.ConvertirInt());
                servDocumento.GrabarDocumentoVersionEstado(docVersion, estado, false, usuarioLogin);
            }

            actaDocumento.Validar();

            if (_idActaDocumento == -1)
            {
                /*Si el estudio al cual pertenece el documento no existe en el acta, 
                 * se crea automaticamente notas al pie y al continuacion de los documentos tratados*/
                if (!acta.EstudioTieneNotas(docVersion.Documento.IdEstudio))
                {
                    string descripcionNota = string.Format("{0} - ESTUDIO {1}", acta.Descripcion, docVersion.Documento.NombreEstudio);
                    this.GrabarNuevaNota(acta.Id.ToString(), "0", "-1", docVersion.Documento.Estudio.Id.ToString(), string.Format("{0} - INICIO",descripcionNota), DateTime.Now.ToString(), null, string.Empty);
                    this.GrabarNuevaNota(acta.Id.ToString(), "1", "-1", docVersion.Documento.Estudio.Id.ToString(), string.Format("{0} - FINAL", descripcionNota), DateTime.Now.ToString(), null, string.Empty);
                }
                acta.AgregarDocumento(actaDocumento);
            }
        }

        //metodo que se invoca al agregar documentos
        public void GrabarEstudioActa(Acta acta, string idEstudio)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            ServicioEstudios servEstudios = new ServicioEstudios();

            ActaEstudio actaEstudio = null;
            Estudio estudio = null;

            if (acta.Estudios.Count > 0)
                actaEstudio = acta.ObtenerEstudio(idEstudio.ConvertirInt());

            if (actaEstudio == null)
            {                
                estudio = servEstudios.Obtener(idEstudio);
                
                actaEstudio = new ActaEstudio();
                actaEstudio.Estudio = estudio;
                actaEstudio.EstadoEstudio = estudio.Estado;
                actaEstudio.OrdenEstudio = acta.ObtenerOrdenEstudio(estudio.Id);
                
                acta.AgregarEstudio(actaEstudio);                
            }
        }

        //metodo que se invoca cuando setea estados, modelo carta de respuesta, texto desde el acta
        public void GrabarDatosEstudio(string idActa, string datosActaEstudio)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            ServicioEstudios servEstudios = new ServicioEstudios();
            
            Acta acta = this.Obtener(idActa);

            ActaEstudio actaEstudio = null;
            
            dynamic datosAux = ServiciosHelpers.DeserializarGenerico(datosActaEstudio);

            int idEstudio = datosAux.IdEstudio;
            actaEstudio = acta.ObtenerEstudio(idEstudio);

            if (actaEstudio == null)
            {
                actaEstudio = new ActaEstudio();
                actaEstudio.Estudio = servEstudios.Obtener(idEstudio.ToString());
                actaEstudio.OrdenEstudio = acta.ObtenerOrdenEstudio(idEstudio);
            }

            actaEstudio.EstadoEstudio = servParametricas.EstadoEstudioObtener(datosAux.IdEstadoEstudio);
            actaEstudio.CartaRespuestaModelo = this.CartaRespuestaModeloObtener(datosAux.IdCartaRespuestaModelo);
            actaEstudio.TextoLibreCartaRespuesta = datosAux.TextoLibreCartaRespuesta;

            acta.AgregarEstudio(actaEstudio);

            this.Grabar(acta);

            /*actualizamos las notas del estudio*/
            RepositoryGenerico<Nota> notaRepository = new RepositoryGenerico<Nota>();
            Nota nota = null;

            int idNotaAntesDocumentos = acta.Notas.ToList<Nota>().Find(item => item.IdEstudio == idEstudio && !item.ActaImprimeAlFinal).Id;                
            nota = notaRepository.Obtener(idNotaAntesDocumentos);
            nota.Texto = string.Format("{0}", datosAux.ComentarioAntesDocumentos); 
            notaRepository.Actualizar(nota);

            nota = null;
            int idNotaDespuesDocumentos = acta.Notas.ToList<Nota>().Find(item => item.IdEstudio == idEstudio && item.ActaImprimeAlFinal).Id;
            nota = notaRepository.Obtener(idNotaDespuesDocumentos);
            nota.Texto = string.Format("{0}", datosAux.ComentarioDespuesDocumentos);
            notaRepository.Actualizar(nota);
            

            if (!acta.Cerrada && actaEstudio.EstadoEstudio != null)
                servEstudios.GrabarEstado(actaEstudio.Estudio.Id.ToString(), actaEstudio.EstadoEstudio.Id.ToString());
        }

        public void GrabarDocumentoComentarioEstado(string documentos, Usuario usuarioLogin)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            List<ActaDocumentoJsonDTO> actaDocumentos = serializer.Deserialize<List<ActaDocumentoJsonDTO>>(documentos);

            actaDocumentos.ForEach(delegate(ActaDocumentoJsonDTO doc)
            {
                this.GrabarDocumentoComentarioEstado(doc.IdActaDocumento, doc.Comentario, doc.IdEstadoDocumento, doc.ActualizarEstadoFinal, doc.ImprimirCarta, doc.SetearSaltoLinea, usuarioLogin);
            });

            //TODO: Falta procesar semaforo de cada estudio
            //ServicioEstudios servEstudio = new ServicioEstudios();
            //servEstudio.GrabarEstudioSemaforo(actaDocumento.IdEstudio.ToString());
        }

        public void GrabarDocumentoComentarioEstado(string idActaDocumento, string comentario, string idEstadoDocumento, string actualizarEstadoFinal, string imprimirCarta, string setearSaltoLinea, Usuario usuarioLogin)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            ServicioDocumentos servDocumento = new ServicioDocumentos();
                        
            int _idActaDocumento = idActaDocumento.ConvertirInt();
            if (_idActaDocumento == -1)
                return;

            RepositoryGenerico<ActaDocumento> repository = new RepositoryGenerico<ActaDocumento>();
            ActaDocumento actaDocumento = repository.Obtener(_idActaDocumento);

            
            actaDocumento.Descripcion = comentario;
            actaDocumento.ImprimirCarta = imprimirCarta.ConvertirBool();
            actaDocumento.SetearSaltosLinea = setearSaltoLinea.ConvertirBoolNulleable();

            EstadoDocumento estado = servParametricas.EstadoDocumentoObtener(idEstadoDocumento.ConvertirInt());
            DocumentoVersion docVersion = actaDocumento.DocumentoVersion;            
            servDocumento.GrabarDocumentoVersionEstado(docVersion, estado, actualizarEstadoFinal.ConvertirBool(), usuarioLogin);
            
            repository.Actualizar(actaDocumento);            
        }

        public void EliminarDocumento(string idActa, string idActaDocumento)
        {
            int _idActaDocumento = idActaDocumento.ConvertirInt();
            if (_idActaDocumento == -1)
                throw new ApplicationException("No seleccionó documento que desea eliminar");

            Acta acta = this.Obtener(idActa);
            ActaDocumento actaDocumento = acta.ObtenerDocumento(_idActaDocumento);
            int idEstudio = actaDocumento.IdEstudio;

            //verifico si el ultimo estado es EN EVALUACION, se elimina
            ServicioParametricas servParametricas = new ServicioParametricas();
            Parametro parmsEstado = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "ESTADO_DOC_EN_EVALUACION");
            string idEstado = (parmsEstado != null) ? parmsEstado.Valor : null;

            DocumentoVersion docVersion = actaDocumento.DocumentoVersion;
            if(docVersion.ObtenerVersionEstado().Id.Equals(idEstado.ConvertirInt()))
            {
                ServicioDocumentos servDocumentos = new ServicioDocumentos();
                //Refactorizar para no tener que enviarle todos los parametros cada vez que se invoque, enviar solo docVersion
                servDocumentos.EliminarDocumentoVersionEstado(docVersion.Documento.IdEstudio.ToString(), docVersion.Documento.Id.ToString(), docVersion.Id.ToString(), docVersion.ObtenerVersionEstado().Id.ToString(), idEstado);
            }

            acta.EliminarDocumento(actaDocumento);


            //Si el estudio quedo sin documentos y sin notas, se debe eliminar el estudio del acta ActaEstudios y Notas por IdEstudio

            bool estudioConDocumentos = acta.Documentos.ToList<ActaDocumento>()
                                                    .FindAll(item => item.IdEstudio == idEstudio).Count > 0;

            bool estudioConNotas = acta.Notas.ToList<Nota>()
                                        .FindAll(item => item.IdEstudio == idEstudio && item.Texto.Trim().Length > 0)
                                        .Count > 0;

            if (!estudioConDocumentos && !estudioConNotas)
            {
                acta.EliminarEstudio(idEstudio);
                acta.Notas.ToList<Nota>()
                    .FindAll(item => item.IdEstudio == idEstudio)
                    .ForEach(delegate(Nota nota)
                    {
                        acta.EliminarNota(nota);
                    });
            }
            this.Grabar(acta);
        }

        public void GrabarParticipantes(string idActa, string participantes)
        {
            ServicioEquipos servEquipo = new ServicioEquipos();
            ServicioParametricas servParametricas = new ServicioParametricas();

            Acta acta = this.Obtener(idActa);
            
            acta.EliminarParticipantes();
            if (participantes.Trim().Length > 0)
            {
                string[] actaParticipantes = participantes.Substring(1).Split(ServiciosHelpers.ID_SEP);

                foreach (string participante in actaParticipantes)
                {
                    string[] infoParticipante = participante.Split(',');
                    Profesional profesional = servEquipo.ProfesionalObtener(infoParticipante[0].ConvertirInt());
                    RolComite rolComite = servParametricas.ObtenerObjeto<RolComite>(infoParticipante[1].ConvertirInt());

                    this.AgregarParticipante(acta, profesional, rolComite);
                }
            }

            if (acta.ComentarioInicialFijo.Trim().Length > 0)
                acta.ComentarioInicialFijo = this.ArmarComentarioInicialFijo(acta);
                        
            this.Grabar(acta);
        }

        public void InicializarParticipantes(Acta acta)
        {
            ServicioEquipos servEquipos = new ServicioEquipos();

            List<Profesional> participantes = servEquipos.ProfesionalObtenerVigentes(Constantes.TipoProfesional_MiembroComite);
            participantes.ForEach(delegate(Profesional profesional) 
            {
                this.AgregarParticipante(acta, profesional, profesional.RolComite);
            });
        }

        public void AgregarParticipante(Acta acta, Profesional profesional, RolComite rolComite)
        {
            ActaProfesional actaProfesional = new ActaProfesional();
            actaProfesional.Profesional = profesional;
            actaProfesional.RolComite = rolComite;
            actaProfesional.Validar();
            acta.AgregarParticipante(actaProfesional);
        }

        public void EliminarParticipante(string idActa, string idActaParticipante)
        {
            int _idActaParticipante = idActaParticipante.ConvertirInt();
            if (_idActaParticipante == -1)
                throw new ApplicationException("No seleccionó participante que desea eliminar");

            Acta acta = this.Obtener(idActa);
            ActaProfesional actaProfesional = acta.ObtenerParticipante(_idActaParticipante);

            acta.EliminarParticipante(actaProfesional);

            if (acta.ComentarioInicialFijo != null && acta.ComentarioInicialFijo.Trim().Length > 0)
                acta.ComentarioInicialFijo = this.ArmarComentarioInicialFijo(acta);

            this.Grabar(acta);
        }

        public void GrabarNota(string idActa, string idNota, string imprimeAlFinal)
        {
            ServicioEstudios servEstudio = new ServicioEstudios();

            Acta acta = this.Obtener(idActa);
            Nota nota = servEstudio.ObtenerNota(idNota);

            nota.ActaImprimeAlFinal = string.IsNullOrEmpty(imprimeAlFinal) ? false : (imprimeAlFinal.Equals("1") ? true : false);

            acta.AgregarNota(nota);

            this.Grabar(acta);
        }
        public void GrabarNuevaNota(string idActa, string notas)
        {   
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            
            List<NotaDTO> actaNotas = serializer.Deserialize<List<NotaDTO>>(notas);

            actaNotas.ForEach(delegate(NotaDTO nota)
            {
                this.GrabarNuevaNota(idActa, nota.ImprimeAlFinal, nota.IdNota, nota.IdEstudio, nota.Descripcion, nota.Fecha, nota.IdAutor, nota.Texto);
            });


        }
        public void GrabarNuevaNota(string idActa, string imprimeAlFinal, string idNota, string idEstudio, string descripion, string fecha, string idAutor, string texto)
        {
            RepositoryGenerico<Nota> notaRepository = new RepositoryGenerico<Nota>();
            ServicioEquipos servEquipo = new ServicioEquipos();

            /*
            ActasRepository actaRepository = new ActasRepository();
            Nota nota = acta.Notas.ToList<Nota>().Find(item => item.IdEstudio == idEstudio.ConvertirInt() && item.ActaImprimeAlFinal == _imprimeAlFinal);
            List<Nota> notas = actaRepository.ObtenerActaNota_Estudio(idActa.ConvertirInt(), idEstudio.ConvertirInt()).ToList<Nota>();
            Nota nota = notas.Find(item => item.IdEstudio == idEstudio.ConvertirInt() && item.ActaImprimeAlFinal == _imprimeAlFinal);
             */
            Nota nota;
            if (idNota.ConvertirInt() != -1)
            {
                nota = notaRepository.Obtener(idNota.ConvertirInt());
            }
            else
            {
                nota = new Nota();
                nota.Id = -1;
            }
            nota.IdActa = idActa.ConvertirInt();
            nota.IdEstudio = idEstudio.ConvertirInt();
            nota.ActaImprimeAlFinal = string.IsNullOrEmpty(imprimeAlFinal) ? false : (imprimeAlFinal.Equals("1") ? true : false); 
            nota.Descripcion = string.IsNullOrEmpty(descripion) ? string.Empty : descripion;
            nota.Fecha = string.IsNullOrEmpty(fecha) ? DateTime.MinValue : DateTime.Parse(fecha);
            nota.Autor = servEquipo.ProfesionalObtener(idAutor.ConvertirInt());
            nota.RequiereRespuesta = false;
            nota.Texto = texto;            

            nota.Vigente = true;

            nota.Validar();
                        
            notaRepository.Actualizar(nota);
            
        }

        public void EliminarNota(string idActa, string idNota)
        {
            ServicioEstudios servEstudio = new ServicioEstudios();

            int _idNota = idNota.ConvertirInt();
            if (_idNota== -1)
                throw new ApplicationException("No seleccionó nota que desea eliminar");

            Acta acta = this.Obtener(idActa);
            Nota nota = servEstudio.ObtenerNota(idNota);

            acta.EliminarNota(nota);

            this.Grabar(acta);
        }


        #region CartaRespuestaModelo
        public List<CartaRespuestaModelo> CartaRespuestaModeloObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<CartaRespuestaModelo> repository = new RepositoryGenerico<CartaRespuestaModelo>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<CartaRespuestaModelo>()
                                                               .OrderBy(item => item.Descripcion)
                                                               .ToList<CartaRespuestaModelo>();
        }        
        public CartaRespuestaModelo CartaRespuestaModeloObtener(int id)
        {
            RepositoryGenerico<CartaRespuestaModelo> repository = new RepositoryGenerico<CartaRespuestaModelo>();
            return repository.Obtener(id);
        }
        public CartaRespuestaModelo CartaRespuestaModeloObtener(string id)
        {
            return this.CartaRespuestaModeloObtener(id.ConvertirInt());
        }
        public void CartaRespuestaModeloGrabar(string id, string datos)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            CartaRespuestaModelo CartaRespuestaModelo;

            dynamic datosAux = ServiciosHelpers.DeserializarGenerico(datos);

            int _id = id.ConvertirInt();
            
            if (_id == -1)
                CartaRespuestaModelo = new CartaRespuestaModelo();
            else
                CartaRespuestaModelo = this.CartaRespuestaModeloObtener(_id);

            CartaRespuestaModelo.Descripcion = datosAux.Descripcion == null ? string.Empty : datosAux.Descripcion;
            CartaRespuestaModelo.Vigente = true;
            CartaRespuestaModelo.IncluirDocumentosEvaluados = datosAux.IncluirDocumentosEvaluados;
            CartaRespuestaModelo.IncluirDocumentosTomaConocimiento = datosAux.IncluirDocumentosTomaConocimiento;
            CartaRespuestaModelo.IncluirDocumentosPedidoCambio = datosAux.IncluirDocumentosPedidoCambio;
            CartaRespuestaModelo.IncluirTodosDocumentosEstudio = datosAux.IncluirTodosDocumentosEstudio;
            CartaRespuestaModelo.PlantillaIntroduccion = (datosAux.IdPlantillaIntroduccion == null) ? null : servParametricas.PlantillaObtener(datosAux.IdPlantillaIntroduccion);
            CartaRespuestaModelo.PlantillaIntroduccionOpcional = (datosAux.IdPlantillaIntroduccion2 == null) ? null : servParametricas.PlantillaObtener(datosAux.IdPlantillaIntroduccion2);
            CartaRespuestaModelo.PlantillaPiePagina = (datosAux.IdPlantillaPiePagina == null) ? null : servParametricas.PlantillaObtener(datosAux.IdPlantillaPiePagina);
            CartaRespuestaModelo.PlantillaBuenasPracticas = (datosAux.IdPlantillaBuenasPracticas == null) ? null : servParametricas.PlantillaObtener(datosAux.IdPlantillaBuenasPracticas);
            CartaRespuestaModelo.PlantillaTextoAprobacion = (datosAux.IdPlantillaTextoAprobacion == null) ? null : servParametricas.PlantillaObtener(datosAux.IdPlantillaTextoAprobacion);
            CartaRespuestaModelo.PlantillaTextoFirmaPresidente = (datosAux.IdPlantillaTextoFirmaPresidente == null) ? null : servParametricas.PlantillaObtener(datosAux.IdPlantillaTextoFirmaPresidente);
            CartaRespuestaModelo.IncluirFirmaPresidente = datosAux.IncluirFirmaPresidente;
            CartaRespuestaModelo.IncluirFirmaMiembros = datosAux.IncluirFirmaMiembros;
            CartaRespuestaModelo.TextoLibre = datosAux.TextoLibre;
            
            CartaRespuestaModelo.Validar();

            RepositoryGenerico<CartaRespuestaModelo> repository = new RepositoryGenerico<CartaRespuestaModelo>();
            repository.Actualizar(CartaRespuestaModelo);

        }
        public void CartaRespuestaModeloEliminar(string id)
        {
            CartaRespuestaModelo CartaRespuestaModelo;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe CartaRespuestaModelo que desea eliminar.");

            CartaRespuestaModelo = this.CartaRespuestaModeloObtener(_id);
            if (CartaRespuestaModelo == null)
                throw new ApplicationException("No existe CartaRespuestaModelo que desea eliminar.");

            CartaRespuestaModelo.Vigente = false;

            RepositoryGenerico<CartaRespuestaModelo> repository = new RepositoryGenerico<CartaRespuestaModelo>();
            repository.Actualizar(CartaRespuestaModelo);
        }

        
        #endregion 
    }
}
