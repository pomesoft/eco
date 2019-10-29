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
    public class ServicioRecordatorios
    {
        RecordatoriosRepository repository;

        public ServicioRecordatorios() 
        {
            repository = new RecordatoriosRepository();
        }

        #region Recordatorio

        public Recordatorio Obtener(string id)
        {
            Recordatorio recordatorio = null;

            if (id.ConvertirInt() != -1)
                recordatorio = repository.Obtener(id.ConvertirInt());

            return recordatorio;
        }

        public List<Recordatorio> Listar()
        {
            List<Recordatorio> recordatorios = repository.ObtenerTodosVigentes().ToList<Recordatorio>();
            return recordatorios;
        }

        public List<Recordatorio> ListarActivosPopup()
        {
            return repository.ObtenerRecordatoriosActivos(true, null)
                             .OrderBy(item => item.FechaActivacion)
                             .ToList<Recordatorio>();
        }

        public List<Recordatorio> ListarActivosEmail()
        {
            return repository.ObtenerRecordatoriosActivos(null, true)
                             .OrderBy(item => item.FechaActivacion)
                             .ToList<Recordatorio>();
        }

        public List<Recordatorio> BuscarRecordatorios(string tipoRecordatorio, string descripcion, string codigoEstudio, string estado)
        {
            return repository.BuscarRecordatorios(tipoRecordatorio, descripcion, codigoEstudio, estado);
        }

        public Recordatorio ObtenerRecordatorioEstudio(int idEstudio, int idTipoRecordatorio)
        {
            List<Recordatorio> recordatorios = repository.ObtenerRecordatoriosEstudio(idEstudio, idTipoRecordatorio);
            
            Recordatorio recordatorio = recordatorios.Find(delegate(Recordatorio rec)
            {
                return ((rec.Estudio != null && rec.Estudio.Id == idEstudio)
                     && (rec.TipoRecordatorio != null && rec.TipoRecordatorio.Id == idTipoRecordatorio));
            });

            return recordatorio;
        }

        public Recordatorio ObtenerRecordatorioDocumento(int idEstudio, int idDocumento, int idTipoRecordatorio)
        {
            RecordatorioDocumento recordatorioDocumento = null;
            List<Recordatorio> recordatorios = repository.ObtenerRecordatoriosEstudio(idEstudio, idTipoRecordatorio);

            recordatorios.ForEach(delegate(Recordatorio rec)
            {
                recordatorioDocumento = rec.Documentos.ToList<RecordatorioDocumento>().Find(delegate(RecordatorioDocumento recDoc)
                {
                    return recDoc.Documento.Id == idDocumento;
                });
            });

            return recordatorioDocumento != null ? recordatorioDocumento.Recordatorio : null;
        }

        public void Grabar(string id, string datos, string fechaAlta, string fechaActivacion, string idEstudio, string documentos)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Recordatorio recordatorio = null;
            Recordatorio recordatorioAux = serializer.Deserialize<Recordatorio>(datos);

            if (id.ConvertirInt() == -1)
            {
                recordatorio = new Recordatorio();
                recordatorio.FechaAlta = DateTime.Now;
            }
            else
                recordatorio = this.Obtener(id);

            recordatorio.Descripcion = recordatorioAux.Descripcion == null ? string.Empty : recordatorioAux.Descripcion;
            recordatorio.Vigente = true;
            recordatorio.TipoRecordatorio = this.TipoRecordatorioObtener(recordatorioAux.TipoRecordatorio.Id);
            recordatorio.EstadoRecordatorio = this.EstadoRecordatorioObtener(recordatorioAux.EstadoRecordatorio.Id);
            if (fechaActivacion.ConvertirDateTime()!=DateTime.MinValue) 
                recordatorio.FechaActivacion = fechaActivacion.ConvertirDateTime();
            recordatorio.AvisoMail = recordatorioAux.AvisoMail.HasValue ? recordatorioAux.AvisoMail : false;
            recordatorio.AvisoPopup = recordatorioAux.AvisoPopup.HasValue ? recordatorioAux.AvisoPopup : false;
            recordatorio.Color = recordatorioAux.Color;
            recordatorio.Texto = recordatorioAux.Texto;
            recordatorio.Destinatarios = recordatorioAux.Destinatarios;
            recordatorio.DestinatariosCC = recordatorioAux.DestinatariosCC;
            recordatorio.Asunto = recordatorioAux.Asunto;
            recordatorio.TextoMail = recordatorioAux.TextoMail;
            recordatorio.EstadoMail = recordatorioAux.EstadoMail;
            
            ServicioEstudios servEstudio = new ServicioEstudios();
            recordatorio.Estudio = servEstudio.Obtener(idEstudio);

            decimal mesesAlerta = -1;
            if (recordatorio.FechaActivacion.HasValue)
                mesesAlerta = Math.Abs((DateTime.Now.Month - recordatorio.FechaActivacion.Value.Month) + 12 * (DateTime.Now.Year - recordatorio.FechaActivacion.Value.Year));
            
            string[] recordatorioDocumentos = documentos.Split(';');
            foreach (string documento in recordatorioDocumentos)
                if (documento.ConvertirInt() > 0)
                {
                    Documento docAgregar = recordatorio.Estudio.Documentos.ToList<Documento>().Find(delegate(Documento doc)
                    {
                        return doc.Id == documento.ConvertirInt();
                    });

                    if (docAgregar != null) recordatorio.AgregarDocumento(docAgregar, int.Parse(mesesAlerta.ToString()));
                }

            recordatorio.Validar();
            repository.Actualizar(recordatorio);
        }

        public void CambiarEstado(string idRecordatorio, string idEstadoRecordatorio)
        {
            Recordatorio recordatorio = this.Obtener(idRecordatorio);

            if (recordatorio == null) throw new ApplicationException("No existe el recordatorio.");

            recordatorio.EstadoRecordatorio = this.EstadoRecordatorioObtener(idEstadoRecordatorio.ConvertirInt());
            
            recordatorio.Validar();
            repository.Actualizar(recordatorio);
        }

        public void EnviarMail(string idRecordatorio)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();

            Recordatorio recordatorio = this.Obtener(idRecordatorio);

            Parametro parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "SMTP_HOST");
            string hostSMTP = (parms != null) ? parms.Valor : string.Empty;
            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "SMTP_PUERTO");
            string puertoSMTP = (parms != null) ? parms.Valor : "0";
            
            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "SMTP_USER");
            string userSMTP = (parms != null) ? parms.Valor : string.Empty;
            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "SMTP_PWD");
            string pwdSMTP = (parms != null) ? parms.Valor : string.Empty;

            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "REMITENTE_AVISOS");
            string remitente = (parms != null) ? parms.Valor : string.Empty;

            CorreoElectronico email = new CorreoElectronico(hostSMTP, puertoSMTP.ConvertirInt(), userSMTP, pwdSMTP);

            string[] destinatarios = recordatorio.Destinatarios.Split(';');
            email.Destinatarios.AddRange(destinatarios.ToList<string>());

            email.Remitente = remitente;
            email.Asunto = recordatorio.Asunto;
            email.Contenido = recordatorio.TextoMail;
            email.EnviarMail();
        }

        #endregion

        #region EstadoRecordatorio
        public List<EstadoRecordatorio> EstadosRecordatoriosObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<EstadoRecordatorio> repository = new RepositoryGenerico<EstadoRecordatorio>();
            
            return repository.ObtenerTodosVigentes(descripcion).ToList<EstadoRecordatorio>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<EstadoRecordatorio>();
        }
        public EstadoRecordatorio EstadoRecordatorioObtener(int id)
        {
            RepositoryGenerico<EstadoRecordatorio> repository = new RepositoryGenerico<EstadoRecordatorio>();
            return repository.Obtener(id);
        }
        public void EstadoRecordatorioGrabar(string id, string descripcion)
        {
            EstadoRecordatorio EstadoRecordatorio;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                EstadoRecordatorio = new EstadoRecordatorio();
            else
                EstadoRecordatorio = this.EstadoRecordatorioObtener(_id);

            EstadoRecordatorio.Descripcion = descripcion == null ? string.Empty : descripcion;
            EstadoRecordatorio.Vigente = true;

            RepositoryGenerico<EstadoRecordatorio> repository = new RepositoryGenerico<EstadoRecordatorio>();

            repository.Actualizar(EstadoRecordatorio);
        }
        public void EstadoRecordatorioEliminar(string id)
        {
            EstadoRecordatorio EstadoRecordatorio;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe estado que desea eliminar.");

            EstadoRecordatorio = this.EstadoRecordatorioObtener(_id);
            if (EstadoRecordatorio == null)
                throw new ApplicationException("No existe estado que desea eliminar.");

            EstadoRecordatorio.Vigente = false;

            RepositoryGenerico<EstadoRecordatorio> repository = new RepositoryGenerico<EstadoRecordatorio>();
            repository.Actualizar(EstadoRecordatorio);
        }
        #endregion

        #region TipoRecordatorio
        public List<TipoRecordatorio> TipoRecordatoriosObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<TipoRecordatorio> repository = new RepositoryGenerico<TipoRecordatorio>();

            return repository.ObtenerTodosVigentes(descripcion).ToList<TipoRecordatorio>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<TipoRecordatorio>();
        }
        public TipoRecordatorio TipoRecordatorioObtener(int id)
        {
            RepositoryGenerico<TipoRecordatorio> repository = new RepositoryGenerico<TipoRecordatorio>();
            return repository.Obtener(id);
        }
        public void TipoRecordatorioGrabar(string id, string datos)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            TipoRecordatorio TipoRecordatorio = null;
            TipoRecordatorio tipoAux = serializer.Deserialize<TipoRecordatorio>(datos);

            if (id.ConvertirInt() == -1)
                TipoRecordatorio = new TipoRecordatorio();
            else
                TipoRecordatorio = this.TipoRecordatorioObtener(id.ConvertirInt());

            TipoRecordatorio.Descripcion = tipoAux.Descripcion == null ? string.Empty : tipoAux.Descripcion;
            TipoRecordatorio.Vigente = true;
            TipoRecordatorio.AvisoMail = tipoAux.AvisoMail.HasValue ? tipoAux.AvisoMail : false;
            TipoRecordatorio.AvisoPopup = tipoAux.AvisoPopup.HasValue ? tipoAux.AvisoPopup : false;
            TipoRecordatorio.Color = tipoAux.Color;
            
            RepositoryGenerico<TipoRecordatorio> repository = new RepositoryGenerico<TipoRecordatorio>();

            repository.Actualizar(TipoRecordatorio);
        }
        public void TipoRecordatorioEliminar(string id)
        {
            TipoRecordatorio TipoRecordatorio;

            if (id.ConvertirInt()==-1)
                throw new ApplicationException("No existe estado que desea eliminar.");

            TipoRecordatorio = this.TipoRecordatorioObtener(id.ConvertirInt());
            if (TipoRecordatorio == null)
                throw new ApplicationException("No existe estado que desea eliminar.");

            TipoRecordatorio.Vigente = false;

            RepositoryGenerico<TipoRecordatorio> repository = new RepositoryGenerico<TipoRecordatorio>();
            repository.Actualizar(TipoRecordatorio);
        }
        #endregion

        #region Emails

        public List<Email> EmailsObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<Email> repository = new RepositoryGenerico<Email>();
            
            return repository.ObtenerTodosVigentes(descripcion).ToList<Email>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<Email>();
        }
        public Email EmailObtener(int id)
        {
            RepositoryGenerico<Email> repository = new RepositoryGenerico<Email>();
            return repository.Obtener(id);
        }
        public void EmailGrabar(string id, string descripcion)
        {
            Email Email;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                Email = new Email();
            else
                Email = this.EmailObtener(_id);

            Email.Descripcion = descripcion == null ? string.Empty : descripcion;
            Email.Vigente = true;

            RepositoryGenerico<Email> repository = new RepositoryGenerico<Email>();

            repository.Actualizar(Email);
        }
        public void EmailEliminar(string id)
        {
            Email Email;
            
            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe email que desea eliminar.");

            Email = this.EmailObtener(_id);
            if (Email == null)
                throw new ApplicationException("No existe email que desea eliminar.");

            Email.Vigente = false;

            RepositoryGenerico<Email> repository = new RepositoryGenerico<Email>();
            repository.Actualizar(Email);
        }

        #endregion

        #region ListaListaEmailss

        public List<ListaEmails> ListaEmailsObtenerVigentes()
        {
            RepositoryGenerico<ListaEmails> repository = new RepositoryGenerico<ListaEmails>();

            return repository.ObtenerTodosVigentes(string.Empty).ToList<ListaEmails>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<ListaEmails>();
        }
        public ListaEmails ListaEmailsObtener(int id)
        {
            RepositoryGenerico<ListaEmails> repository = new RepositoryGenerico<ListaEmails>();
            return repository.Obtener(id);
        }
        public void ListaEmailsGrabar(string id, string descripcion, string emails)
        {
            ListaEmails listaEmails;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                listaEmails = new ListaEmails();
            else
                listaEmails = this.ListaEmailsObtener(_id);

            listaEmails.Descripcion = descripcion == null ? string.Empty : descripcion;
            listaEmails.Vigente = true;

            listaEmails.ListEmails.Clear();
            
            JavaScriptSerializer deserializer = new JavaScriptSerializer();
            deserializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            dynamic listEmails = deserializer.Deserialize(emails, typeof(object));
            foreach (var item in listEmails)
            {
                Email email = new Email(-1, item.Descripcion);
                listaEmails.AgregarEmail(email);
            }

            RepositoryGenerico<ListaEmails> repository = new RepositoryGenerico<ListaEmails>();

            repository.Actualizar(listaEmails);
        }
        public void ListaEmailsEliminar(string id)
        {
            ListaEmails ListaEmails;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe email que desea eliminar.");

            ListaEmails = this.ListaEmailsObtener(_id);
            if (ListaEmails == null)
                throw new ApplicationException("No existe email que desea eliminar.");

            ListaEmails.Vigente = false;

            RepositoryGenerico<ListaEmails> repository = new RepositoryGenerico<ListaEmails>();
            repository.Actualizar(ListaEmails);
        }

        #endregion
    }
}

