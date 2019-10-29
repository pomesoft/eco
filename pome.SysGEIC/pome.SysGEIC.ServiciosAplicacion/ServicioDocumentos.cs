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
    
    public class ServicioDocumentos
    {
        //RepositoryGenerico<Documento> repository;
        DocumentosRepository repository;
        
        public ServicioDocumentos()
        {
            repository = new DocumentosRepository();            
        }

        public void Sincronizar()
        {
            this.repository.Sincronizar();
        }
        
        public List<Documento> ObtenerVigentes()
        {
            return repository.ObtenerTodosVigentes().ToList<Documento>();
        }

        public int Grabar(Documento documento)
        {
            documento.Validar();
            repository.Actualizar(documento);

            ServicioEstudios servEstudio = new ServicioEstudios();
            servEstudio.GrabarEstudioSemaforo(documento.IdEstudio.ToString());

            return documento.Id;
        }

        public List<Documento> ListarDocumentos(string idEstudio)
        {
            List<Documento> documentos = repository.ObtenerDocumentosEstudio(idEstudio.ConvertirInt());
            return documentos.OrderBy(item => item.Descripcion)
                             .ToList<Documento>();
        }

        public List<Documento> ListarDocumentosAnulados(string idEstudio)
        {
            int _idEstudio = idEstudio.ConvertirInt();
            List<Documento> documentos = new List<Documento>();
            if (_idEstudio != -1) 
                documentos = repository.ObtenerDocumentosAnulados(_idEstudio);
            return documentos;
        }

        public Documento ObtenerDocumento(string idEstudio, string idDocumento)
        {
            ServicioEstudios servEstudio = new ServicioEstudios();
            Estudio estudio = servEstudio.Obtener(idEstudio);
            Documento documento = null;

            int _idDocumento = idDocumento.ConvertirInt();
            if (_idDocumento == -1)
                documento = new Documento();
            else
            {
                documento = estudio.ObtenerDocumento(_idDocumento);
                this.ObtenerRecordatoriosAlertas(documento);
            }
            documento.Estudio = estudio;

            return documento;
        }

        public Documento Obtener(string idDocumento)
        {
            ServicioEstudios servEstudio = new ServicioEstudios();            
            Documento documento = null;

            int _idDocumento = idDocumento.ConvertirInt();
            if (_idDocumento != -1)
            {
                documento = repository.Obtener(_idDocumento);
                documento.Estudio = servEstudio.Obtener(documento.IdEstudio.ToString());
                this.ObtenerRecordatoriosAlertas(documento);
            }

            return documento;

        }

        public void ObtenerRecordatoriosAlertas(Documento documento)
        {
            Recordatorio recAux = null;

            documento.RecordatorioInformeAvanceId = -1;
            recAux = this.ObtenerRecodatorioDocumento(documento, 3);
            if (recAux != null)
            {
                documento.RecordatorioInformeAvanceId = recAux.Id;
                documento.RecordatorioInformeAvanceMeses = recAux.MesesAlertaDocumento;
            }

            documento.RecordatorioInactividadId = -1;
            recAux = this.ObtenerRecodatorioDocumento(documento, 4);
            if (recAux != null)
            {
                documento.RecordatorioInactividadId = recAux.Id;
                documento.RecordatorioInactividadMeses = recAux.MesesAlertaDocumento;
            }

            documento.RecordatorioVencimientoId = -1;
            recAux = this.ObtenerRecodatorioDocumento(documento, 5);
            if (recAux != null)
            {
                documento.RecordatorioVencimientoId = recAux.Id;
                documento.RecordatorioVencimientoMeses = recAux.MesesAlertaDocumento;
            }
        }

        public void EliminarDocumento(string idEstudio, string idDocumento)
        {
            ServicioEstudios servEstudio = new ServicioEstudios();
            Estudio estudio = servEstudio.Obtener(idEstudio);
            Documento documento = null;

            int _idDocumento = idDocumento.ConvertirInt();
            if (estudio != null && _idDocumento != -1)
                documento = estudio.ObtenerDocumento(_idDocumento);

            if (documento == null)
                throw new ApplicationException("No existe el documento que desea eliminar");

            estudio.EliminarDocumento(documento);
            servEstudio.Grabar(estudio);
        }

        public void AnularDocumento(string idEstudio, string idDocumento)
        {
            Documento documento = this.Obtener(idDocumento);
            documento.Vigente = false;
            this.Grabar(documento);
        }

        public void ReactivarDocumento(string idDocumento)
        {
            Documento documento = this.Obtener(idDocumento);
            documento.Vigente = true;
            this.Grabar(documento);
        }

        public int GrabarDocumento(string idEstudio, string idDocumento, string descripion, string idTipoDocumento, string limitante, Usuario usuarioLogin,
                                   string requiereAlertaInactividad, string mesesAlertaInactividad, string requiereAlertaInformeAvance, string mesesAlertaInformeAvance)
        {
            return this.GrabarDocumento(idEstudio, idDocumento, descripion, idTipoDocumento, limitante, usuarioLogin,
                                        false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                        requiereAlertaInactividad, mesesAlertaInactividad, requiereAlertaInformeAvance, mesesAlertaInformeAvance);
        }

        public int GrabarDocumento(string idEstudio, string idDocumento, string descripion, string idTipoDocumento, string limitante, Usuario usuarioLogin,
                                   bool verificarVersion, string idVersion, string versionDescripion, string versionFecha, string versionAprobadoANMAT, string versionFechaAprobadoANMAT, string versionFechaEstado, string versionIdEstado, string participantes,
                                   string requiereAlertaInactividad, string mesesAlertaInactividad, string requiereAlertaInformeAvance, string mesesAlertaInformeAvance)
        {
            //ServiciosHelpers.ValidarClave(estudio);

            Documento documento = null;

            int _idDocumento = idDocumento.ConvertirInt();
            if (_idDocumento != -1)
                documento = repository.Obtener(_idDocumento);

            if (documento == null)
                documento = new Documento();

            documento.Descripcion = string.IsNullOrEmpty(descripion) ? string.Empty : descripion;
            documento.TipoDocumento = this.TipoDocumentoObtener(idTipoDocumento.ConvertirInt());
            documento.Limitante = limitante.ConvertirBool();
            documento.Vigente = true;
            documento.IdEstudio = idEstudio.ConvertirInt();

            documento.Validar();

           
            if (verificarVersion)
            {
                this.ActualizarDocumentoVersion(documento, idVersion, versionDescripion, versionFecha, versionAprobadoANMAT, versionFechaAprobadoANMAT, versionFechaEstado, versionIdEstado, participantes, usuarioLogin);
            }
            else if (documento.TipoDocumento != null && !documento.TipoDocumento.RequiereVersion && documento.Versiones.Count == 0)
            {
                ServicioParametricas servParametricas = new ServicioParametricas();
                DocumentoVersion version = new DocumentoVersion();
                version.Fecha = null;
                version.Descripcion = string.Empty;

                Parametro parmsEstado = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "ESTADO_DOC_INGRESADO");
                string idEstado = (parmsEstado != null) ? parmsEstado.Valor : null;

                DocumentoVersionEstado versionEstado = this.CrearDocumentoVersionEstado(documento.TipoDocumento, idEstado, DateTime.Now.ToString(), usuarioLogin);
                version.AgregarVersionEstado(versionEstado);

                this.AgregarParticipantes(participantes, documento, version);

                documento.AgregarVersion(version);
            }


            return this.Grabar(documento);
        }

        public void GrabarDocumentoVersion(string idEstudio, string idDocumento, string idVersion, string descripion, string fecha, string aprobadoANMAT, string fechaAprobadoANMAT, string fechaEstado, string idEstado, string participantes, Usuario usuarioLogin)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();

            Documento documento = this.Obtener(idDocumento);
            
            if (documento == null)
                throw new ApplicationException("Error al obtener el Documento.");

            this.ActualizarDocumentoVersion(documento, idVersion, descripion, fecha, aprobadoANMAT, fechaAprobadoANMAT, fechaEstado, idEstado, participantes, usuarioLogin);

            this.Grabar(documento);
        }

        private void ActualizarDocumentoVersion(Documento documento, string idVersion, string descripion, string fecha, string aprobadoANMAT, string fechaAprobadoANMAT, string fechaEstado, string idEstado, string participantes, Usuario usuarioLogin)
        {
            DocumentoVersion version = null;
            int _idVersion = idVersion.ConvertirInt();
            if (_idVersion == -1)
            {
                if (!documento.PermitirCargarVersion())
                    throw new ApplicationException("El documento NO requiere versión, deberá editar la versión actual.");

                version = new DocumentoVersion();
            }
            else
                version = documento.ObtenerVersion(_idVersion);

            version.Descripcion = descripion.ConvertirString();
            if (!string.IsNullOrEmpty(fecha)) version.Fecha = fecha.ConvertirDateTime();
            version.AprobadoANMAT = aprobadoANMAT.ConvertirBool();
            if (!string.IsNullOrEmpty(fechaAprobadoANMAT))
                version.FechaAprobadoANMAT = fechaAprobadoANMAT.ConvertirDateTime();

            version.EliminarParticipantes();
            this.AgregarParticipantes(participantes, documento, version);

            version.Validar();

            if (_idVersion == -1)
            {
                DocumentoVersionEstado versionEstado = this.CrearDocumentoVersionEstado(documento.TipoDocumento, idEstado, fechaEstado, usuarioLogin);
                version.AgregarVersionEstado(versionEstado);
                
                documento.AgregarVersion(version);
            }

        }

        /// <summary>
        /// Si el parametro participantes es distinto a null se cargan estos participantes
        /// Si el documento no tiene ninguna version ingresada se setean los investigadores principales por dafult
        /// Si el documento ya tiene versiones se setean los investigadores de la ultima version 
        /// Tiene que invocarse antes de agregar la version al documento
        /// </summary>
        /// <param name="version"></param>
        public void AgregarParticipantes(string participantes, Documento documento, DocumentoVersion version)
        {
            if (participantes != null && participantes.Trim().Length > 0)
            {
                ServicioEquipos servEquipo = new ServicioEquipos();
                if (participantes != null && participantes.Trim().Length > 0)
                {
                    string[] idProfesionales = participantes.Substring(1).Split(ServiciosHelpers.ID_SEP);
                    foreach (string idProf in idProfesionales)
                        this.AgregarParticipante(version, servEquipo.ProfesionalObtener(idProf.ConvertirInt()));
                }
            }
            else
            {
                if (documento.Versiones.Count > 0 && documento.VersionActual.Participantes.Count > 0)
                    documento.VersionActual.Participantes.ToList<DocumentoVersionParticipante>().ForEach(delegate(DocumentoVersionParticipante participante)
                    {
                        this.AgregarParticipante(version, participante.Profesional);
                    });
                else
                {
                    ServicioEstudios servEstudio = new ServicioEstudios();
                    documento.Estudio = servEstudio.Obtener(documento.IdEstudio.ToString());
                    documento.Estudio.InvestigadoresPrincipalesProfesional.ForEach(delegate(Profesional profesional)
                    {
                        this.AgregarParticipante(version, profesional);
                    });
                }
            }
        }

        public void AgregarParticipante(DocumentoVersion documentoVersion, Profesional profesional)
        {
            DocumentoVersionParticipante docVersionParticipante = new DocumentoVersionParticipante();
            docVersionParticipante.DocumentoVersion = documentoVersion;
            docVersionParticipante.Profesional = profesional;
            docVersionParticipante.Validar();
            documentoVersion.AgregarParticipante(docVersionParticipante);
        }

        public void GrabarDocumentoVersionArchivoAdjunto(string idEstudio, string idDocumento, string idVersion, string nombreArchivo)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();

            Documento documento = this.Obtener(idDocumento);

            DocumentoVersion version = version = documento.ObtenerVersion(idVersion.ConvertirInt());

            version.Archivo = nombreArchivo;

            this.Grabar(documento);
        }

        public DocumentoVersionEstado CrearDocumentoVersionEstado(TipoDocumento tipoDocumento, string idEstado, string fechaEstado, Usuario usuarioLogin)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();

            DocumentoVersionEstado versionEstado = new DocumentoVersionEstado();

            versionEstado.Fecha = fechaEstado.ConvertirDateTime();

            EstadoDocumento estadoActual = servParametricas.EstadoDocumentoObtener(idEstado.ConvertirInt());
            versionEstado.Estado = estadoActual;

            versionEstado.Usuario = usuarioLogin;
            versionEstado.ProfesionalAutor = null;
            versionEstado.ProfesionalPresenta = null;
            versionEstado.ProfesionalResponsable = null;
            versionEstado.Observaciones = string.Empty;

            TipoDocumentoFlujoEstado flujoEstado = tipoDocumento.ObtenerFlujoDefault().ObtenerFlujoEstado(estadoActual);
            if (flujoEstado != null)
                versionEstado.EstadoFinal = flujoEstado.Final;
            else
                versionEstado.EstadoFinal = false;

            versionEstado.Validar();

            return versionEstado;
        }
               
        public void GrabarDocumentoVersionEstado(string idEstudio, string idDocumento, string idVersion, string idVersionEstado, string idEstado, string fecha, string idProfesionalAutor, string idProfesionalPresenta, string idProfesionalResponsable, Usuario usuarioLogin)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            ServicioEquipos servEquipo = new ServicioEquipos();

            Documento documento = this.Obtener(idDocumento);
            DocumentoVersion version = documento.ObtenerVersion(idVersion.ConvertirInt());

            DocumentoVersionEstado versionEstadoActual = version.ObtenerVersionEstado();
            if (versionEstadoActual != null && versionEstadoActual.Estado != null)
            {
                //para no duplicar el estado, si ya lo tiene no hce nada
                if (versionEstadoActual.Estado.Id == idEstado.ConvertirInt())
                    return;
                //si ya tiene un estado final se quita primero, para que quede un solo estado final
                if (versionEstadoActual.EstadoFinal.HasValue && versionEstadoActual.EstadoFinal.Value)
                    version.EliminarVersionEstado(versionEstadoActual.Id);
            }

            DocumentoVersionEstado versionEstado = null;

            int _idVersionEstado = idVersionEstado.ConvertirInt();
            if (_idVersionEstado == -1)
                versionEstado = new DocumentoVersionEstado();
            else
                versionEstado = version.ObtenerVersionEstado(_idVersionEstado);

            versionEstado.Fecha = string.IsNullOrEmpty(fecha) ? DateTime.MinValue : DateTime.Parse(fecha);

            EstadoDocumento nuevoEstado = null;
            nuevoEstado = servParametricas.EstadoDocumentoObtener(idEstado.ConvertirInt());
            versionEstado.Estado = nuevoEstado;

            versionEstado.Usuario = usuarioLogin;
            versionEstado.ProfesionalAutor = servEquipo.ProfesionalObtener(idProfesionalAutor.ConvertirInt());
            versionEstado.ProfesionalPresenta = servEquipo.ProfesionalObtener(idProfesionalPresenta.ConvertirInt());
            versionEstado.ProfesionalResponsable = servEquipo.ProfesionalObtener(idProfesionalResponsable.ConvertirInt());
            versionEstado.Observaciones = string.Empty;

            TipoDocumentoFlujoEstado flujoEstado = documento.TipoDocumento.ObtenerFlujoDefault().ObtenerFlujoEstado(nuevoEstado);
            if (flujoEstado != null)
                versionEstado.EstadoFinal = flujoEstado.Final;
            else
                versionEstado.EstadoFinal = false;

            versionEstado.Validar();

            if (_idVersionEstado == -1)
                version.AgregarVersionEstado(versionEstado);

            this.Grabar(documento);
        }

        public void GrabarDocumentoVersionEstado(DocumentoVersion version, EstadoDocumento estado, bool actualizarEstadoFinal, Usuario usuarioLogin)
        {

            if (estado == null && !actualizarEstadoFinal)
                return;

            ServicioParametricas servParametricas = new ServicioParametricas();

            DocumentoVersionEstado versionEstadoActual = version.ObtenerVersionEstado();
            if (versionEstadoActual != null && versionEstadoActual.Estado != null)
            {
                //para no duplicar el estado, si ya lo tiene no hce nada
                if (versionEstadoActual.Estado == estado)
                    return;

                //si ya tiene un estado final y no se está actualizando estado final, se quita primero para que quede un solo estado final
                if (versionEstadoActual.EstadoFinal.HasValue && versionEstadoActual.EstadoFinal.Value)
                {
                    if (actualizarEstadoFinal)
                        return;
                    else
                        version.EliminarVersionEstado(versionEstadoActual.Id);
                }
            }

            EstadoDocumento estadoAprobadoExistente = null;
            EstadoDocumento estadoTomaConocimientoExistente = null;
            EstadoDocumento nuevoEstado = null;

            if (actualizarEstadoFinal)
            {
                List<EstadoDocumento> listEstados = version.Documento.TipoDocumento.ObtenerFlujoDefault().ObtenerEstados(versionEstadoActual.Estado);

                Parametro parmsEstado = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "ESTADO_DOC_APROBADO");
                string idEstadoAprobado = (parmsEstado != null) ? parmsEstado.Valor : null;
                estadoAprobadoExistente = listEstados.Find(delegate(EstadoDocumento match) { return match.Id == idEstadoAprobado.ConvertirInt(); });

                parmsEstado = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "ESTADO_DOC_TOMA_CONOCIMIENTO");
                string idEstadoTomaConocimiento = (parmsEstado != null) ? parmsEstado.Valor : null;
                estadoTomaConocimientoExistente = listEstados.Find(delegate(EstadoDocumento match) { return match.Id == idEstadoTomaConocimiento.ConvertirInt(); });

                if (estadoAprobadoExistente == null && estadoTomaConocimientoExistente == null)
                    return;

                nuevoEstado = (estadoAprobadoExistente != null) ? estadoAprobadoExistente : estadoTomaConocimientoExistente;
            }
            else
            {
                nuevoEstado = estado;
            }

            DocumentoVersionEstado versionEstado = new DocumentoVersionEstado();

            versionEstado.Fecha = DateTime.Now;
            versionEstado.Estado = nuevoEstado;
            versionEstado.Usuario = usuarioLogin;
            versionEstado.Observaciones = string.Empty;

            TipoDocumentoFlujoEstado flujoEstado = version.Documento.TipoDocumento.ObtenerFlujoDefault().ObtenerFlujoEstado(nuevoEstado);
            if (flujoEstado != null)
                versionEstado.EstadoFinal = flujoEstado.Final;
            else
                versionEstado.EstadoFinal = false;

            versionEstado.Validar();

            version.AgregarVersionEstado(versionEstado);
        }

        public void EliminarDocumentoVersionEstado(string idEstudio, string idDocumento, string idVersion, string idVersionEstado, string idEstado)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            ServicioEquipos servEquipo = new ServicioEquipos();

            Documento documento = this.Obtener(idDocumento);
            DocumentoVersion version = documento.ObtenerVersion(idVersion.ConvertirInt());

            version.EliminarVersionEstado(int.Parse(idVersionEstado));

            this.Grabar(documento);
        }

        public void GrabarDocumentoVersionComentario(string idEstudio, string idDocumento, string idVersion, string idVersionComentario, string fecha, string idProfesionalAutor, string observaciones)
        {
            ServicioEquipos servEquipo = new ServicioEquipos();

            Documento documento = this.Obtener(idDocumento);
            DocumentoVersion version = documento.ObtenerVersion(idVersion.ConvertirInt());
            DocumentoVersionComentario versionComentario = null;

            int _idVersionComentario = idVersionComentario.ConvertirInt();
            if (_idVersionComentario == -1)
                versionComentario = new DocumentoVersionComentario();
            else
                versionComentario = version.ObtenerVersionComentario(_idVersionComentario);

            versionComentario.Fecha = string.IsNullOrEmpty(fecha) ? DateTime.MinValue : DateTime.Parse(fecha);
            versionComentario.ProfesionalAutor = servEquipo.ProfesionalObtener(idProfesionalAutor.ConvertirInt());
            versionComentario.Observaciones = observaciones;

            versionComentario.Validar();

            if (_idVersionComentario == -1)
                version.AgregarVersionComentario(versionComentario);

            this.Grabar(documento);
        }

        public void GrabarDocumentoVersionRecordatorio(string idEstudio, string idDocumento, string idVersion, string idVersionRecordatorio, string fecha, string idProfesionalAutor, string observaciones, string pendiente)
        {
            ServicioEquipos servEquipo = new ServicioEquipos();

            Documento documento = this.Obtener(idDocumento);
            DocumentoVersion version = documento.ObtenerVersion(idVersion.ConvertirInt());
            DocumentoVersionRecordatorio versionRecordatorio = null;

            int _idVersionRecordatorio = idVersionRecordatorio.ConvertirInt();
            if (_idVersionRecordatorio == -1)
                versionRecordatorio = new DocumentoVersionRecordatorio();
            else
                versionRecordatorio = version.ObtenerVersionRecordatorio(_idVersionRecordatorio);

            versionRecordatorio.Fecha = string.IsNullOrEmpty(fecha) ? DateTime.MinValue : DateTime.Parse(fecha);
            versionRecordatorio.ProfesionalAutor = servEquipo.ProfesionalObtener(idProfesionalAutor.ConvertirInt());
            versionRecordatorio.Observaciones = observaciones;
            bool _pendiente = false;
            if (bool.TryParse(pendiente, out _pendiente))
                versionRecordatorio.Pendiente = _pendiente;

            versionRecordatorio.Validar();

            if (_idVersionRecordatorio == -1)
                version.AgregarVersionRecordatorio(versionRecordatorio);

            this.Grabar(documento);
        }

        public void GrabarDocumentoVersionParticipantes(string idDocumento, string idVersion, string participantes, Usuario usuarioLogin)
        {
            ServicioEquipos servEquipo = new ServicioEquipos();

            Documento documento = this.Obtener(idDocumento);
            DocumentoVersion version = documento.ObtenerVersion(idVersion.ConvertirInt());
            DocumentoVersionEstado versionEstado = version.ObtenerVersionEstado();
            
            this.ActualizarDocumentoVersion(documento, version.Id.ToString(), version.Descripcion, version.FechaToString, version.AprobadoANMAT.ToString(), version.FechaAprobadoANMATToString, versionEstado.FechaToString, versionEstado.Estado.Id.ToString(), participantes, usuarioLogin);

            this.Grabar(documento);
        }

        public List<EstadoDocumento> DocumentoObtenerEstados(string idDocumento, string idVersion)
        {
            Documento documento = this.Obtener(idDocumento);
            DocumentoVersion version = documento.ObtenerVersion(idVersion.ConvertirInt());
            TipoDocumentoFlujo flujo = documento.TipoDocumento.ObtenerFlujoDefault();

            DocumentoVersionEstado estadoActual = version.ObtenerVersionEstado();

            EstadoDocumento estado = estadoActual == null ? null : estadoActual.Estado;

            return flujo.ObtenerEstados(estado);
        }

        public List<EstadoDocumento> DocumentoPrimerEstado(string idTipoDocumento)
        {
            TipoDocumento tipoDocumento = TipoDocumentoObtener(idTipoDocumento.ConvertirInt());
            TipoDocumentoFlujo flujo = tipoDocumento.ObtenerFlujoDefault();

            return flujo.ObtenerEstados(null);
        }


        #region Recodatorios y Alertas

        private Recordatorio ObtenerRecodatorioDocumento(Documento documento, int idTipoRecordatorio)
        {
            ServicioRecordatorios servRecodatorios = new ServicioRecordatorios();
            Recordatorio recordatorioRenovacionAnual = servRecodatorios.ObtenerRecordatorioDocumento(documento.IdEstudio, documento.Id, idTipoRecordatorio);
            return recordatorioRenovacionAnual;
        }

        #endregion

        #region TipoDocumento
        public List<TipoDocumento> TiposDocumentoObtenerVigentes()
        {
            RepositoryGenerico<TipoDocumento> repository = new RepositoryGenerico<TipoDocumento>();
            return repository.ObtenerTodosVigentes().ToList<TipoDocumento>()
                                                    .OrderBy(item => item.Descripcion)
                                                    .ToList<TipoDocumento>();
        }
        public List<TipoDocumento> TiposDocumentoObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<TipoDocumento> repository = new RepositoryGenerico<TipoDocumento>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<TipoDocumento>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<TipoDocumento>();
        }
        public List<TipoDocumento> TiposDocumentoObtenerListarCartaRespuesta(int idGrupo)
        {
            RepositoryGenerico<TipoDocumento> repository = new RepositoryGenerico<TipoDocumento>();
            List<TipoDocumento> listTD = repository.ObtenerTodosVigentes()
                                                   .ToList<TipoDocumento>()
                                                   .FindAll(item => item.ListarCartaRespuesta)
                                                   .ToList<TipoDocumento>();

            List<TipoDocumento> listReturn;

            if(idGrupo==-1)
                listReturn= listTD;
            else
                listReturn = listTD.FindAll(item => item.TipoDocumentoGrupo.Id == idGrupo);

            return listReturn.OrderBy(item => item.Descripcion)
                             .ToList<TipoDocumento>();

        }


        public TipoDocumento TipoDocumentoObtener(int id)
        {
            RepositoryGenerico<TipoDocumento> repository = new RepositoryGenerico<TipoDocumento>();
            return repository.Obtener(id);
        }
        public void TipoDocumentoGrabar(TipoDocumento tipoDocumento)
        {
            RepositoryGenerico<TipoDocumento> repository = new RepositoryGenerico<TipoDocumento>();
            repository.Actualizar(tipoDocumento);
        }
        public void TipoDocumentoEliminar(TipoDocumento tipoDocumento)
        {
            RepositoryGenerico<TipoDocumento> repository = new RepositoryGenerico<TipoDocumento>();
            repository.Actualizar(tipoDocumento);
        }
        #endregion

        #region TipoDocumentoGrupo
        public List<TipoDocumentoGrupo> TipoDocumentoGruposObtenerVigentes()
        {
            RepositoryGenerico<TipoDocumentoGrupo> repository = new RepositoryGenerico<TipoDocumentoGrupo>();
            return repository.ObtenerTodosVigentes().ToList<TipoDocumentoGrupo>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<TipoDocumentoGrupo>();
        }
        #endregion
    }
}
