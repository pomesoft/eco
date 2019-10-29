using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class DocumentoVersion : EntidadBaseParametrica
    {        
        public virtual Documento Documento { get; set; }
        [DataMember]
        public virtual IList<DocumentoVersionEstado> Estados { get; set; }
        [DataMember]
        public virtual IList<DocumentoVersionComentario> Comentarios { get; set; }
        [DataMember]
        public virtual IList<DocumentoVersionRecordatorio> Recordatorios { get; set; }
        [DataMember]
        public virtual IList<DocumentoVersionParticipante> Participantes { get; set; }
        
        public virtual DateTime? Fecha { get; set; }
        [DataMember]
        public virtual string FechaToString 
        {
            get { return Fecha.HasValue ? Fecha.Value.ToShortDateString() : string.Empty; }
            set { }
        }
        [DataMember]
        public virtual string Archivo { get; set; }
        [DataMember]
        public virtual bool? AprobadoANMAT { get; set; }

        public virtual DateTime? FechaAprobadoANMAT { get; set; }
        [DataMember]
        public virtual string FechaAprobadoANMATToString
        {
            get { return FechaAprobadoANMAT.HasValue ? FechaAprobadoANMAT.Value.ToShortDateString() : string.Empty; }
            set { }
        }

        public DocumentoVersion()
        {
            Estados = new List<DocumentoVersionEstado>();
            Comentarios = new List<DocumentoVersionComentario>();            
            Recordatorios = new List<DocumentoVersionRecordatorio>();
            Participantes = new List<DocumentoVersionParticipante>();
        }

        public override void Validar()
        {
            if (Fecha == DateTime.MinValue)
                throw new ApplicationException("Debe ingresar fecha de versión.");          
        }

        public virtual DocumentoVersionEstado ObtenerVersionEstado(int idVersionEstado)
        {
            DocumentoVersionEstado versionEstadoReturn = null;
            Estados.ToList<DocumentoVersionEstado>().ForEach(delegate(DocumentoVersionEstado versionEstado)
            {
                if (versionEstado.Id == idVersionEstado)
                    versionEstadoReturn = versionEstado;
            });
            return versionEstadoReturn;
        }
        public virtual void EliminarVersionEstado(int idVersionEstado)
        {
            Estados.ToList<DocumentoVersionEstado>().ForEach(delegate(DocumentoVersionEstado versionEstado)
            {
                if (versionEstado.Id == idVersionEstado)
                    Estados.Remove(versionEstado);
            });
        }
        public virtual DocumentoVersionEstado ObtenerVersionEstado()
        {
            DocumentoVersionEstado versionEstadoReturn = null;
            Estados.ToList<DocumentoVersionEstado>().ForEach(delegate(DocumentoVersionEstado versionEstado)
            {
                versionEstadoReturn = versionEstado;
            });
            return versionEstadoReturn;
        }        
        public virtual void AgregarVersionEstado(DocumentoVersionEstado versionEstado)
        {
            Estados.ToList<DocumentoVersionEstado>().ForEach(delegate(DocumentoVersionEstado verEstado)
            {
                if (verEstado.Estado.Equals(versionEstado.Estado) && verEstado.Fecha.Equals(versionEstado.Fecha))
                    throw new ApplicationException(string.Format("El estado {0} ya existe en la versión para la fecha {1}",
                                                                versionEstado.Estado, versionEstado.FechaToString));
            });
            versionEstado.Version = this;
            Estados.Add(versionEstado);
        }
        public virtual DocumentoVersionComentario ObtenerVersionComentario(int idVersionComentario)
        {
            DocumentoVersionComentario versionComentarioReturn = null;
            Comentarios.ToList<DocumentoVersionComentario>().ForEach(delegate(DocumentoVersionComentario versionComentario)
            {
                if (versionComentario.Id == idVersionComentario)
                    versionComentarioReturn = versionComentario;
            });
            return versionComentarioReturn;
        }
        public virtual void AgregarVersionComentario(DocumentoVersionComentario versionComentario)
        {
            versionComentario.Version = this;
            Comentarios.Add(versionComentario);
        }
        public virtual DocumentoVersionRecordatorio ObtenerVersionRecordatorio(int idVersionRecordatorio)
        {
            DocumentoVersionRecordatorio versionRecordatorioReturn = null;
            Recordatorios.ToList<DocumentoVersionRecordatorio>().ForEach(delegate(DocumentoVersionRecordatorio versionRecordatorio)
            {
                if (versionRecordatorio.Id == idVersionRecordatorio)
                    versionRecordatorioReturn = versionRecordatorio;
            });
            return versionRecordatorioReturn;
        }
        public virtual void AgregarVersionRecordatorio(DocumentoVersionRecordatorio versionRecordatorio)
        {
            versionRecordatorio.Version = this;
            Recordatorios.Add(versionRecordatorio);
        }
        public virtual void AgregarParticipante(DocumentoVersionParticipante documentoVersionParticipante)
        {
            if (documentoVersionParticipante.Id == -1)
            {
                DocumentoVersionParticipante participanteExistente = null;
                participanteExistente = Participantes.ToList<DocumentoVersionParticipante>().Find(delegate(DocumentoVersionParticipante versionProf)
                {
                    return versionProf.Profesional == documentoVersionParticipante.Profesional;
                });
                if (participanteExistente != null)
                {
                    throw new ApplicationException(string.Format("El profesional {0} ya está vinculado al documento",
                                                                documentoVersionParticipante.Profesional.NombreCompleto));
                }
            }
            documentoVersionParticipante.DocumentoVersion = this;
            Participantes.Add(documentoVersionParticipante);
        }
        public virtual void EliminarParticipantes()
        {
            if (Participantes.Count > 0)
                Participantes.Clear();
        }
    }
}
