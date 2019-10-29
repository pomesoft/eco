using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Repositorios;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public class ServicioEstudios
    {
        //RepositoryGenerico<Estudio> repository;
        EstudiosRepository repository;

        public ServicioEstudios() 
        {
            repository = new EstudiosRepository();
        }

        #region Estudios
        
        public Estudio ObtenerSoloEstudio(string idEstudio)
        {
            Estudio estudio = null;

            int _idEstudio = idEstudio.ConvertirInt();
            if (_idEstudio != -1)
                estudio = repository.Obtener(_idEstudio);

            return estudio;
        }

        public Estudio Obtener(string idEstudio)
        {
            Estudio estudio = this.ObtenerSoloEstudio(idEstudio);

            if (estudio != null)
            {
                estudio.Documentos = this.ObtenerDocumentos(idEstudio);
                estudio.Notas = this.ObtenerEstudioNotas(idEstudio);
                estudio.IdRecordatorioRenovacionAnual = this.ObtenerRecodatorioRenovacionAnual(idEstudio);

                estudio.EstadoSemaforo = DeterminarSemaforoEstudio(estudio);

                if (estudio.IdTipoEstudio.HasValue)
                {
                    ServicioParametricas servParams = new ServicioParametricas();
                    estudio.TipoEstudio = servParams.TipoEstudioObtener(estudio.IdTipoEstudio.Value);
                }
                else
                    estudio.TipoEstudio = null;

            }

            return estudio;
        }

        public EstudioDTO ObtenerDTO(Estudio estudio, string idActa)
        {
            return this.ObtenerDTO(estudio, idActa, false);
        }
        public EstudioDTO ObtenerDTO(Estudio estudio, string idActa, bool soloPendientes)
        {
            ServicioActas servActas = new ServicioActas();
            EstudioDTO estudioDTO = this.ObtenerDTO(estudio, soloPendientes);

            List<ActaDocumento> docsTratadsActa = servActas.ListarDocumentoXActaEstudio(idActa, estudio.Id.ToString());

            estudioDTO.Documentos.ForEach(delegate(DocumentoDTO doc)
            {
                if (docsTratadsActa.Find(item => item.IdVersion == doc.IdVersionActual) != null)
                {
                    doc.VersionActualActa = idActa.ConvertirInt();
                }
            });

            return estudioDTO; 
        }

        public EstudioDTO ObtenerDTO(Estudio estudio, bool soloPendientes)
        {
            EstudioDTO estudioDTO = null;
            DocumentoDTO documentoDTO = null;
            NotaDTO notaDTO = null;

            estudioDTO = DameEstudioDTO(estudio);
            estudioDTO.InvestigadoresPrincipalesProfesional = estudio.InvestigadoresPrincipalesProfesional;

            var docsTratados = from docs in estudio.Documentos
                               orderby docs.TipoDocumentoDescripcion, docs.Id
                               select docs;


            docsTratados.ToList<Documento>().ForEach(delegate(Documento documento)
            {
                bool agregarDoc = true;

                if (soloPendientes)
                    agregarDoc = !documento.VersionEstadoActual.EstadoFinal.Value;


                if (agregarDoc)
                {
                    documentoDTO = new DocumentoDTO();

                    documentoDTO.Id = documento.Id;
                    documentoDTO.Descripcion = documento.Descripcion;
                    documentoDTO.IdTipoDocumento = documento.TipoDocumento.Id;
                    documentoDTO.TipoDocumentoDescripcion = documento.TipoDocumento.Descripcion;
                    documentoDTO.TipoDocumentoIdDescripcion = string.Format("{0}-{1}", documento.TipoDocumento.Id, documento.TipoDocumento.Descripcion);
                    documentoDTO.TipoDocumentoRequiereVersion = documento.TipoDocumento.RequiereVersion.ConvertirBool();
                    documentoDTO.IdEstudio = estudio.Id;
                    documentoDTO.NombreEstudio = estudio.NombreEstudioListados;
                    documentoDTO.IdVersionActual = (documento.VersionActual != null) ? documento.VersionActual.Id : -1;
                    documentoDTO.VersionActualDescripcion = (documento.VersionActual != null) ? documento.VersionActual.Descripcion : string.Empty;
                    documentoDTO.VersionActualFecha = (documento.VersionActual != null) ? documento.VersionActual.FechaToString : string.Empty;
                    documentoDTO.VersionActualArchivo = (documento.VersionActual != null) ? documento.VersionActual.Archivo : string.Empty;
                    documentoDTO.EstadoActual = (documento.VersionEstadoActual != null && documento.VersionEstadoActual.Estado != null) ? documento.VersionEstadoActual.Estado.Descripcion : string.Empty;
                    documentoDTO.EstadoActualFecha = (documento.VersionEstadoActual != null && documento.VersionEstadoActual.Estado != null) ? documento.VersionEstadoActual.FechaToString : string.Empty;
                    documentoDTO.EstadoFinal = documento.EstadoFinal;

                    if (documento.VersionActual != null && documento.VersionActual.Participantes != null)
                    {
                        documento.VersionActual.Participantes.ToList<DocumentoVersionParticipante>().ForEach(delegate(DocumentoVersionParticipante dvParticipante)
                        {
                            documentoDTO.Participantes.Add(dvParticipante.Profesional);
                        });
                    }

                    estudioDTO.Documentos.Add(documentoDTO);
                }
            });

            estudio.Notas.ToList<Nota>().ForEach(delegate(Nota nota)
            {
                notaDTO = new NotaDTO();

                notaDTO.Id = nota.Id;
                notaDTO.Descripcion = nota.Descripcion;
                notaDTO.IdEstudio = nota.IdEstudio.ToString();
                notaDTO.Fecha = nota.FechaToString;
                notaDTO.IdAutor = (nota.Autor != null) ? nota.Autor.Id.ToString() : string.Empty;

                estudioDTO.Notas.Add(notaDTO);
            });

            return estudioDTO;
        }

        //TODO: PENDIENTE Tener en cuenta el usuario que inició sesión, para obtener solamente los estudios enlos que el participa
        public List<EstudioDTO> ObtenerVigentes()
        {
            List<EstudioDTO> estudiosReturn = new List<EstudioDTO>();

            estudiosReturn = repository.ListarEstudiosDTO(string.Empty, string.Empty, string.Empty).ToList<EstudioDTO>();

            return estudiosReturn.OrderBy(item => item.Codigo).ToList<EstudioDTO>();
        }

        public List<EstudioDTO> ObtenerNoFinalizados()
        {
            List<EstudioDTO> estudiosReturn = new List<EstudioDTO>();

            this.ObtenerVigentes().ForEach(delegate(EstudioDTO estudio) 
            {
                if (estudio.Estado == null) 
                    estudiosReturn.Add(estudio);
                else
                    if(!estudio.Estado.Final)
                        estudiosReturn.Add(estudio);                
            });

            return estudiosReturn.OrderBy(item => item.Codigo)
                                 .ToList<EstudioDTO>();
        }

        public List<EstudioDTO> BuscarEstudios(string codigo, string abreviado, string nombreCompleto)
        {
            //List<Estudio> estudios = repository.BuscarEstudios(codigo, abreviado, nombreCompleto);
            //List<EstudioDTO> estudiosReturn = new List<EstudioDTO>();
            //estudios.ForEach(delegate(Estudio estudio) { estudiosReturn.Add(this.DameEstudioDTO(estudio)); });
            //return estudiosReturn;

            List<EstudioDTO> estudiosReturn = repository.ListarEstudiosDTO(codigo, abreviado, nombreCompleto).ToList<EstudioDTO>();

            return estudiosReturn.OrderBy(item => item.Codigo).ToList<EstudioDTO>();
        }

        public Estudio BuscarDocumentosDelEstudio(string idEstudio, string tipoDocumento, string nombreDocumento)
        {
            Estudio estudioReturn = null;
            Estudio estudio = this.Obtener(idEstudio);
            
            if (estudio != null)
            {
                estudioReturn = new Estudio();
                List<Documento> documentos = estudio.Documentos.ToList<Documento>();

                estudioReturn.Id = estudio.Id;
                estudioReturn.Codigo = estudio.Codigo;
                estudioReturn.Descripcion = estudio.Descripcion;
                estudioReturn.NombreCompleto = estudio.NombreCompleto;
                
                documentos.ForEach(delegate(Documento documento)
                {
                    if ((!string.IsNullOrEmpty(tipoDocumento) && documento.TipoDocumento.Descripcion.ToUpper().IndexOf(tipoDocumento.ToUpper()) >= 0)
                        || (!string.IsNullOrEmpty(nombreDocumento) && documento.Descripcion.ToUpper().IndexOf(nombreDocumento.ToUpper()) >= 0))
                    {
                        estudioReturn.AgregarDocumento(documento);
                    }
                });                
            }

            return estudioReturn;
        }


        public EstudioDTO DameEstudioDTO(Estudio estudio)
        {
            EstudioDTO estudioDTO = new EstudioDTO();

            estudioDTO.Id = estudio.Id;
            estudioDTO.Descripcion = estudio.Descripcion;
            estudioDTO.NombreCompleto = estudio.NombreCompleto;
            estudioDTO.Codigo = estudio.Codigo;
            estudioDTO.Estado = estudio.Estado;
            estudioDTO.EstadoSemaforo = estudio.EstadoSemaforo;

            return estudioDTO;
        }

        public ActaEstudioDTO DameActaEstudioDTO(Estudio estudio)
        {
            ActaEstudioDTO dto = new ActaEstudioDTO();
            dto.Id = estudio.Id;
            dto.Descripcion = estudio.Descripcion;
            dto.Codigo = estudio.Codigo;
            dto.IdEstado = estudio.Estado == null ? -1 : estudio.Estado.Id;
            dto.Estado = estudio.Estado == null ? string.Empty : estudio.Estado.Descripcion;
            dto.NombreCompleto = estudio.NombreEstudioListados;
            return dto;
        }

        public ActaEstudioDTO DameActaEstudioDTO(Estudio estudio, ActaDocumento actaDocumento)
        {
            ActaEstudioDTO dto = this.DameActaEstudioDTO(estudio);
            dto.OrdenEstudio = actaDocumento.OrdenEstudio;
            return dto;
        }

        public List<EstudioRecordatoriosDTO> ObtenerRecordatorios(Estudio estudio, string pendiente)
        {
            List<EstudioRecordatoriosDTO> recordatoriosReturn = new List<EstudioRecordatoriosDTO>();
            EstudioRecordatoriosDTO recordatorioDTO = null;
            
            bool soloPendiente = pendiente.ConvertirBool();

            if (estudio != null && estudio.Documentos != null)
            {
                estudio.Documentos.ToList<Documento>().ForEach(delegate(Documento documento)
                {
                    documento.Versiones.ToList<DocumentoVersion>().ForEach(delegate(DocumentoVersion version)
                    {
                        version.Recordatorios.ToList<DocumentoVersionRecordatorio>().ForEach(delegate(DocumentoVersionRecordatorio recordatorio)
                        {
                            if (!soloPendiente || soloPendiente == recordatorio.Pendiente.Value)
                            {
                                recordatorioDTO = new EstudioRecordatoriosDTO();
                                recordatorioDTO.Id = recordatorio.Id;
                                recordatorioDTO.NombreDocumento = documento.Descripcion;
                                recordatorioDTO.VersionDocumento = version.Descripcion;
                                recordatorioDTO.Fecha = recordatorio.FechaToString;
                                recordatorioDTO.Autor = recordatorio.ProfesionalAutor.NombreCompleto;
                                recordatorioDTO.Pendiente = (recordatorio.Pendiente.HasValue && recordatorio.Pendiente.Value) ? "SI" : "NO";
                                recordatorioDTO.Observaciones = recordatorio.Observaciones;

                                recordatoriosReturn.Add(recordatorioDTO);
                            }
                        });
                    });
                });
            }

            return recordatoriosReturn;
        }


        public int Grabar(Estudio estudio)
        {            
            estudio.Vigente = true;
            estudio.EstadoSemaforo = this.DeterminarSemaforoEstudio(estudio);
            repository.Actualizar(estudio);

            return estudio.Id;            
        }

        public void EliminarEstudio(string idEstudio)
        {
            Estudio estudio = Obtener(idEstudio);

            if (estudio == null)
                throw new ApplicationException("No existe el estudio que desea eliminar");

            repository.Eliminar(estudio);
        }

        public int GrabarEstudio(string idEstudio, string codigo, string nombre, string nombreCompleto, string patologia, string poblacion, string estado, string fechaPresentacion, string requiereAlerta, string mesesAlerta, string idTipoEstudio)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();

            Estudio estudio = null;

            int _idEstudio = idEstudio.ConvertirInt();

            if (_idEstudio == -1)
                estudio = new Estudio();
            else
                estudio = Obtener(idEstudio);

            estudio.Codigo = codigo;
            estudio.Descripcion = nombre;
            estudio.NombreCompleto = nombreCompleto;
            estudio.Patologia = servParametricas.PatologiaObtener(patologia.ConvertirInt());
            estudio.Estado = servParametricas.EstadoEstudioObtener(estado.ConvertirInt());
            estudio.Poblacion = poblacion;
            //TODO: Rediseñar modelo Estudio ---> EstudioRecordatorios
            if (fechaPresentacion.ConvertirDateTime() != DateTime.MinValue)
                estudio.FechaPresentacion = fechaPresentacion.ConvertirDateTime();
            estudio.RequiereAlerta = requiereAlerta.ConvertirBool();
            estudio.MesesAlerta = mesesAlerta.ConvertirInt();
            if (idTipoEstudio.ConvertirInt() > 0)
                estudio.IdTipoEstudio = idTipoEstudio.ConvertirInt();

            estudio.Validar();

            return this.Grabar(estudio);
        }

        public void GrabarEstado(string idEstudio, string estado)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();

            Estudio estudio = Obtener(idEstudio);

            estudio.Estado = servParametricas.EstadoEstudioObtener(estado.ConvertirInt());

            this.Grabar(estudio);
        }

        public void GrabarEstudioTiposDocumentosSemaforo(string idEstudio, string tiposDocumento)
        {
            ServicioDocumentos servDocumentos = new ServicioDocumentos();
            RepositoryGenerico<EstudioTipoDocumento> repositoryETD = new RepositoryGenerico<EstudioTipoDocumento>();

            EstudioTipoDocumento estudioTipoDoc = null;
            Estudio estudio = Obtener(idEstudio);

            if (estudio == null)
                throw new ApplicationException("No existe estudio");

            repositoryETD.EliminarRegistros("EstudioTiposDocumento", "(IdEstudio = " + estudio.Id + ")");

            dynamic tiposDocs = ServiciosHelpers.DeserializarGenerico(tiposDocumento);

            foreach (var tipo in tiposDocs)
            {
                estudioTipoDoc = new EstudioTipoDocumento();
                estudioTipoDoc.Estudio = estudio;
                string _aux = string.Format("{0}", tipo.Id);
                estudioTipoDoc.TipoDocumento = servDocumentos.TipoDocumentoObtener(_aux.ConvertirInt());
                repositoryETD.Actualizar(estudioTipoDoc);
            }
            this.GrabarEstudioSemaforo(idEstudio);
        }

        public void EliminarEstudioTipoDocumento(string idEstudio, string idTipoDocumento)
        {
            RepositoryGenerico<EstudioTipoDocumento> repositoryETD = new RepositoryGenerico<EstudioTipoDocumento>();

            EstudioTipoDocumento etd = repositoryETD.Obtener("Estudio.Id", idEstudio.ConvertirInt(), 
                                                             "TipoDocumento.Id", idTipoDocumento.ConvertirInt());

            if (etd == null)
                throw new ApplicationException("No existe la relacion Estudio-TipoDocumento");

            repositoryETD.Eliminar(etd);
        }

        #endregion

        #region Participantes

        public void GrabarParticipante(string idEstudio, string idParticipante, string idProfesional, string idRol, string desde, string hasta)
        {
            ServicioEquipos servEquipo = new ServicioEquipos();
            Profesional profesional = servEquipo.ProfesionalObtener(idProfesional.ConvertirInt());
            Rol rol = servEquipo.RolObtener(idRol.ConvertirInt());


            Estudio estudio = this.Obtener(idEstudio);
            EstudioParticipante participante = null;

            int _idParticipante = idParticipante.ConvertirInt();
            if (_idParticipante == -1)
                participante = new EstudioParticipante();
            else
                participante = estudio.ObtenerParticipante(_idParticipante);

            participante.Profesional = profesional;
            participante.Rol = rol;
            if (!string.IsNullOrEmpty(desde)) participante.Desde = DateTime.Parse(desde);
            if (!string.IsNullOrEmpty(hasta)) participante.Hasta = DateTime.Parse(hasta);

            participante.Validar();

            if (_idParticipante == -1)
                estudio.AgregarParticipante(participante);

            this.Grabar(estudio);
        }

        public void EliminarParticipante(string idEstudio, string idParticipante)
        {
            int _idParticipante = idParticipante.ConvertirInt();

            if (_idParticipante == -1)
                throw new ApplicationException("No seleccionó participante que desea eliminar");

            Estudio estudio = this.Obtener(idEstudio);
            EstudioParticipante participante = estudio.ObtenerParticipante(_idParticipante);

            estudio.EliminarParticipante(participante);

            this.Grabar(estudio);
        }

        #endregion

        #region CentrosHabilitados
        
        public void GrabarCentroHabilitado(string idEstudio, string idCentroHabilitado, string idCentro, string desde, string hasta)
        {
            ServicioParametricas servicio = new ServicioParametricas();

            Estudio estudio = this.Obtener(idEstudio);
            EstudioCentro estudioCentro = null;

            int _idCentroHabilitado = idCentroHabilitado.ConvertirInt();

            if (_idCentroHabilitado == -1)
                estudioCentro = new EstudioCentro();
            else
                estudioCentro = estudio.ObtenerCentroHabilitado(_idCentroHabilitado);

            estudioCentro.Centro = servicio.CentroObtener(idCentro.ConvertirInt());
            if (!string.IsNullOrEmpty(desde)) estudioCentro.Desde = DateTime.Parse(desde);
            if (!string.IsNullOrEmpty(hasta)) estudioCentro.Hasta = DateTime.Parse(hasta);

            estudioCentro.Validar();

            if (_idCentroHabilitado == -1)
                estudio.AgregarCentroHabilitado(estudioCentro);

            this.Grabar(estudio);
        }

        public void EliminarCentroHabilitado(string idEstudio, string idCentroHabilitado)
        {
            int _idCentroHabilitado = idCentroHabilitado.ConvertirInt();
            if (_idCentroHabilitado == -1)
                throw new ApplicationException("No seleccionó centro habilitado que desea eliminar");

            Estudio estudio = this.Obtener(idEstudio);
            EstudioCentro estudioCentro = estudio.ObtenerCentroHabilitado(_idCentroHabilitado);

            estudio.EliminarCentroHabilitado(estudioCentro);

            this.Grabar(estudio);
        }

        #endregion

        #region Patrocinadores
        
        public void GrabarPatrocinador(string idEstudio, string idEstiudioPatrocinador, string idPatrocinador)
        {
            ServicioParametricas servicio = new ServicioParametricas();

            Estudio estudio = this.Obtener(idEstudio);
            EstudioPatrocinador estudioPatrocinador = null;

            int _idEstudioPatrocinador = idEstiudioPatrocinador.ConvertirInt();

            if (_idEstudioPatrocinador == -1)
                estudioPatrocinador = new EstudioPatrocinador();
            else
                estudioPatrocinador = estudio.ObtenerPatrocinador(_idEstudioPatrocinador);

            estudioPatrocinador.Patrocinador = servicio.PatrocinadorObtener(idPatrocinador.ConvertirInt());

            estudioPatrocinador.Validar();

            if (_idEstudioPatrocinador == -1)
                estudio.AgregarPatrocinador(estudioPatrocinador);

            this.Grabar(estudio);
        }

        public void EliminarPatrocinador(string idEstudio, string idEstudioPatrocinador)
        {
            int _idEstudioPatrocinador = idEstudioPatrocinador.ConvertirInt();
            if (_idEstudioPatrocinador == -1)
                throw new ApplicationException("No seleccionó patrocinador que desea eliminar");

            Estudio estudio = this.Obtener(idEstudio);
            EstudioPatrocinador estudioPatrocinador = estudio.ObtenerPatrocinador(_idEstudioPatrocinador);

            estudio.EliminarPatrocinador(estudioPatrocinador);

            this.Grabar(estudio);
        }

        #endregion

        #region Monodrogas
        
        public void GrabarMonodroga(string idEstudio, string idEstiudioMonodroga, string idMonodroga)
        {
            ServicioParametricas servicio = new ServicioParametricas();
            ServicioMonodrogas servMonodroga = new ServicioMonodrogas();

            Estudio estudio = this.Obtener(idEstudio);
            EstudioMonodroga estudioMonodroga = null;

            int _idEstudioMonodroga = idEstiudioMonodroga.ConvertirInt();
            if (_idEstudioMonodroga == -1)
                estudioMonodroga = new EstudioMonodroga();
            else
                estudioMonodroga = estudio.ObtenerMonodroga(_idEstudioMonodroga);

            estudioMonodroga.Monodroga = servMonodroga.MonodrogaObtener(idMonodroga.ConvertirInt());

            estudioMonodroga.Validar();

            if (_idEstudioMonodroga == -1)
                estudio.AgregarMonodroga(estudioMonodroga);

            this.Grabar(estudio);
        }

        public void EliminarMonodroga(string idEstudio, string idEstudioMonodroga)
        {
            int _idEstudioMonodroga = idEstudioMonodroga.ConvertirInt();
            if (_idEstudioMonodroga == -1)
                throw new ApplicationException("No seleccionó monodroga que desea eliminar");

            Estudio estudio = this.Obtener(idEstudio);
            EstudioMonodroga estudioMonodroga = estudio.ObtenerMonodroga(_idEstudioMonodroga);

            estudio.EliminarMonodroga(estudioMonodroga);

            this.Grabar(estudio);
        }
        #endregion

        #region Documentos

        private List<Documento> ObtenerDocumentos(string idEstudio)
        {
            ServicioDocumentos servDocumentos = new ServicioDocumentos();
            return servDocumentos.ListarDocumentos(idEstudio);
        }

        
        

        public List<EstudioTipoDocumento> ObtenerTiposDocumentoSemaforo(string idEstudio)
        {
            if (idEstudio.ConvertirInt() == -1) throw new ApplicationException("");

            Estudio estudio = this.Obtener(idEstudio);

            return this.ObtenerTiposDocumentoSemaforo(estudio);
        }

        public List<EstudioTipoDocumento> ObtenerTiposDocumentoSemaforo(Estudio estudio)
        {            
            List<EstudioTipoDocumento> listReturn = repository.ObtenerTiposEstudioSemaforo(estudio.Id).ToList<EstudioTipoDocumento>();
            List<Documento> docs = new List<Documento>();

            listReturn.ForEach(delegate(EstudioTipoDocumento etd) 
            {
                docs = estudio.Documentos.ToList<Documento>().FindAll(item => item.TipoDocumento == etd.TipoDocumento);
                //empieza el analisis del semaforo
                if (docs.Count == 0)
                {
                    //no existen documentos del tipo en el Estudio, es ROJO
                    etd.EstadoSemaforo = (int)ESTADO_SEMAFORO.ROJO;
                }
                else
                {
                    //existen documentos, verificamos si estan con el estado final, es VERDE, caso contrario AMARILLO
                    int cantDocsEstadoFinal = 0;
                    docs.ToList<Documento>().ForEach(delegate(Documento doc)
                    {
                        if (doc.VersionActual != null)
                        {
                            DocumentoVersionEstado versionEstado = doc.VersionActual.ObtenerVersionEstado();
                            if (versionEstado != null && versionEstado.EstadoFinal.Value)
                                cantDocsEstadoFinal++;
                        }
                    });
                    etd.EstadoSemaforo = (docs.Count == cantDocsEstadoFinal) ? (int)ESTADO_SEMAFORO.VERDE : (int)ESTADO_SEMAFORO.AMARILLO;
                }
            });

            return listReturn;
        }

        #endregion

        #region Recodatorios y Alertas

        private int ObtenerRecodatorioRenovacionAnual(string idEstudio)
        {
            ServicioRecordatorios servRecodatorios = new ServicioRecordatorios();
            Recordatorio recordatorioRenovacionAnual = servRecodatorios.ObtenerRecordatorioEstudio(idEstudio.ConvertirInt(), 2);
            return recordatorioRenovacionAnual != null ? recordatorioRenovacionAnual.Id : -1;
        }

        #endregion

        #region Notas

        public void GrabarNota(string idEstudio, string idNota, string descripion, string fecha, string idAutor, string nombreArchivo, string pathArchivo, string requiereRespuesta, string idDocumento, string idDocumentoVersion, string texto)
        {

            ServicioEquipos servEquipo = new ServicioEquipos();

            Estudio estudio = this.Obtener(idEstudio);
            Nota nota = null;

            int _idDocumento = idDocumento.ConvertirInt();
            int _idDocumentoVersion = idDocumentoVersion.ConvertirInt();

            DocumentoVersion docVersion = null;
            if (_idDocumento != -1 && _idDocumentoVersion == -1)
                throw new ApplicationException("Debe seleccionar Documento y Versión del mismo");

            int _idNota = idNota.ConvertirInt();
            if (_idNota == -1)
                nota = new Nota();
            else
                nota = estudio.ObtenerNota(_idNota);

            nota.Descripcion = string.IsNullOrEmpty(descripion) ? string.Empty : descripion;
            nota.Fecha = string.IsNullOrEmpty(fecha) ? DateTime.MinValue : DateTime.Parse(fecha);
            nota.Autor = servEquipo.ProfesionalObtener(idAutor.ConvertirInt());
            nota.NombreArchivo = nombreArchivo;
            nota.PathArchivo = pathArchivo;
            bool _requiereRespuesta = false;
            if (bool.TryParse(requiereRespuesta, out _requiereRespuesta))
                nota.RequiereRespuesta = _requiereRespuesta;

            if (_idDocumento != -1 && _idDocumentoVersion != -1)
                docVersion = estudio.ObtenerDocumento(_idDocumento)
                                    .ObtenerVersion(_idDocumentoVersion);
            nota.Texto = texto;

            if (docVersion != null)
                nota.AgregarDocumento(docVersion);

            nota.Vigente = true;

            nota.Validar();

            if (_idNota == -1)
                estudio.AgregarNota(nota);

            this.Grabar(estudio);
        }

        public Nota ObtenerNota(string idNota)
        {
            RepositoryGenerico<Nota> repositoryNota = new RepositoryGenerico<Nota>();
            Nota nota = null;

            int _idNota = idNota.ConvertirInt();
            if (_idNota != -1)
                nota = repositoryNota.Obtener(_idNota);

            return nota;
        }

        public List<Nota> ObtenerEstudioNotas(string idEstudio)
        {
            return repository.ObtenerEstudioNotas(idEstudio.ConvertirInt()).ToList<Nota>();
        }

        #endregion

        #region Semaforos

        public int DeterminarSemaforoEstudio(Estudio estudio)
        {
            int valorReturn = -1;
            //semaforo
            List<EstudioTipoDocumento> listTiposSemaforo = this.ObtenerTiposDocumentoSemaforo(estudio);
            if (listTiposSemaforo.Count > 0)
            {
                List<EstudioTipoDocumento> listRojo = listTiposSemaforo.FindAll(item => item.EstadoSemaforo == (int)ESTADO_SEMAFORO.ROJO).ToList<EstudioTipoDocumento>();
                List<EstudioTipoDocumento> listVerde = listTiposSemaforo.FindAll(item => item.EstadoSemaforo == (int)ESTADO_SEMAFORO.VERDE).ToList<EstudioTipoDocumento>();
                //si todos los tipos estan en verde, el estudio es verde
                //si alguno esta en amarillo, el estudio es amarillo
                if (listTiposSemaforo.Count == listRojo.Count)
                    valorReturn = (int)ESTADO_SEMAFORO.ROJO;
                else
                    if (listTiposSemaforo.Count == listVerde.Count)
                        valorReturn = (int)ESTADO_SEMAFORO.VERDE;
                    else
                        valorReturn = (int)ESTADO_SEMAFORO.AMARILLO;
            }
            return valorReturn;
        }

        public void ProcesarSemaforoEstudios()
        {
            List<EstudioDTO> estudios = this.ObtenerVigentes().ToList<EstudioDTO>();
            Estudio estudio;
            estudios.ForEach(delegate(EstudioDTO dto) 
            {
                estudio = this.Obtener(dto.Id.ToString());
                this.Grabar(estudio);
            });
        }

        public void GrabarEstudioSemaforo(string idEstudio)
        {
            Estudio estudio = this.Obtener(idEstudio);
            this.Grabar(estudio);
        }

        public void ProcesarEstudiosFinalizados(Usuario usuarioLogin)
        {
            ServicioDocumentos servDocs = new ServicioDocumentos();            
            ServicioParametricas servParams = new ServicioParametricas();

            List<EstudioDTO> estudios = this.repository.ListarEstudiosDTO(4).ToList<EstudioDTO>().ToList<EstudioDTO>();
            List<Documento> docsEstudio = new List<Documento>();

            estudios.ForEach(delegate(EstudioDTO dto)
            {
                docsEstudio = this.ObtenerDocumentos(dto.Id.ToString());

                docsEstudio.ForEach(delegate(Documento doc)
                {                    
                    try
                    {
                        if (doc.TipoDocumento.Vigente)
                        {
                            doc.Versiones.ToList<DocumentoVersion>().ForEach(delegate(DocumentoVersion dv)
                            {

                                EstadoDocumento proximoEstado = null;
                                DocumentoVersionEstado estadoDoc = dv.ObtenerVersionEstado();
                                if (!estadoDoc.EstadoFinal.Value)
                                {
                                    /*
                                     IdEstado	Descripcion
                                        1	    INGRESADO
                                        2	    EN EVALUACION
                                        3	    APROBADO
                                        4	    PEDIDO DE CAMBIOS
                                        5	    TOMA CONOCIMIENTO
                                     */
                                    switch (estadoDoc.Estado.Id)
                                    {
                                        case 1: //
                                            proximoEstado = servParams.EstadoDocumentoObtener(2);
                                            servDocs.GrabarDocumentoVersionEstado(dv, proximoEstado, false, usuarioLogin);  //grabamos en evaluacion
                                            servDocs.Grabar(doc);
                                            servDocs.GrabarDocumentoVersionEstado(dv, null, true, usuarioLogin);            //grabamos estado final
                                            servDocs.Grabar(doc);
                                            break;
                                        case 2: //
                                            servDocs.GrabarDocumentoVersionEstado(dv, null, true, usuarioLogin);            //grabamos estado final
                                            servDocs.Grabar(doc);
                                            break;
                                    }
                                }

                            }); 
                        }
                    }
                    catch
                    {
                        //servDocs.Sincronizar();
                    }
                });
            });
        }

        #endregion
        
    }
    
}
