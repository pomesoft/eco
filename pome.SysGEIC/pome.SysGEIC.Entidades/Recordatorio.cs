using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class Recordatorio : EntidadBaseParametrica
    {
        [DataMember]
        public virtual TipoRecordatorio TipoRecordatorio { get; set; }
        [DataMember]
        public virtual EstadoRecordatorio EstadoRecordatorio { get; set; }
        [DataMember]
        public virtual DateTime? FechaAlta { get; set; }
        [DataMember]
        public virtual DateTime? FechaActivacion { get; set; }
        [DataMember]
        public virtual bool? AvisoMail { get; set; }
        [DataMember]
        public virtual bool? AvisoPopup { get; set; }
        [DataMember]
        public virtual string Color { get; set; }
        [DataMember]
        public virtual string Texto { get; set; }
        [DataMember]
        public virtual string Destinatarios { get; set; }
        [DataMember]
        public virtual string DestinatariosCC { get; set; }
        [DataMember]
        public virtual string Asunto { get; set; }
        [DataMember]
        public virtual string TextoMail { get; set; }
        [DataMember]
        public virtual int EstadoMail { get; set; }
        //[DataMember]
        //public virtual string EstadoMail
        //{
        //    get
        //    {
        //        string estadoMail = string.Empty;
        //        switch (Estado)
        //        {
        //            case 1:
        //                estadoMail = "PENDIENTE";
        //                break;
        //            case 2:
        //                estadoMail = "ENVIADO";
        //                break;
        //            case 3:
        //                estadoMail = "REENVIAR";
        //                break;
        //        }
        //        return estadoMail;
        //    }
        //}

        public virtual Estudio Estudio { get; set; }
        
        public virtual IList<RecordatorioDocumento> Documentos { get; set; }

        [DataMember]
        public virtual string TipoRecordatorioDescripcion
        {
            get
            {
                return TipoRecordatorio != null ? TipoRecordatorio.Descripcion : string.Empty;
            }
            set { }
        }
        [DataMember]
        public virtual string EstadoRecordatorioDescripcion
        {
            get
            {
                return EstadoRecordatorio != null ? EstadoRecordatorio.Descripcion : string.Empty;
            }
            set { }
        }
        [DataMember]
        public virtual int EstudioId
        {
            get
            {
                return Estudio != null ? Estudio.Id : -1;
            }
            set { }
        }
        [DataMember]
        public virtual List<Documento> ListadoDocumentos 
        { 
            get
            {
                List<Documento> docs = new List<Documento>();
                Documentos.ToList<RecordatorioDocumento>().ForEach(delegate(RecordatorioDocumento recDoc) { docs.Add(recDoc.Documento); });
                return docs;
            }
            set { } 
        }
        [DataMember]
        public virtual int MesesAlertaDocumento
        {
            get
            {
                RecordatorioDocumento recDoc = Documentos.ToList<RecordatorioDocumento>().FirstOrDefault();
                return recDoc != null ? recDoc.Meses : -1;
            }
            set { }
        }

        [DataMember]
        public string FechaAltaToString
        {
            get 
            {
                return FechaAlta.HasValue ? FechaAlta.Value.ToShortDateString() : string.Empty;
            }
            set { }
        }
        [DataMember]
        public string FechaActivacionToString
        {
            get
            {
                return FechaActivacion.HasValue ? FechaActivacion.Value.ToShortDateString() : string.Empty;
            }
            set { }
        }

        public Recordatorio() 
        {
            Documentos = new List<RecordatorioDocumento>();
        }

        public override void Validar()
        {
            if (TipoRecordatorio == null)
                throw new ApplicationException("Debe seleccionar tipo de recordatorio.");

            if (EstadoRecordatorio == null)
                throw new ApplicationException("Debe seleccionar estado.");

            if (FechaAlta == null)
                throw new ApplicationException("Debe ingresar fecha de alta del recordatorio.");

            if (FechaActivacion == null)
                throw new ApplicationException("Debe ingresar fecha de activación del recordatorio.");

        }

        public void AgregarDocumento(Documento documento, int meses)
        {
            RecordatorioDocumento recDocumento = Documentos.ToList<RecordatorioDocumento>().Find(delegate(RecordatorioDocumento doc) 
            { 
                return doc.Documento.Id == documento.Id; 
            });

            if (recDocumento == null)
            {
                recDocumento = new RecordatorioDocumento();
                recDocumento.Recordatorio = this;
                recDocumento.Documento = documento;
                recDocumento.Meses = meses;
                Documentos.Add(recDocumento);
            }
        }
       
    }
}
