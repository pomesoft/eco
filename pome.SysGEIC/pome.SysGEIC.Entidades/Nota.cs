using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Nota : EntidadBaseParametrica
    {
        [DataMember]
        public virtual int IdEstudio { get; set; }        
        public virtual Estudio Estudio { get; set; }        
        public virtual DateTime Fecha { get; set; }
        [DataMember]
        public virtual int IdActa { get; set; }        
        public virtual Acta Acta { get; set; }        
        [DataMember]
        public virtual Profesional Autor { get; set; }
        [DataMember]
        public virtual string NombreArchivo { get; set; }
        [DataMember]
        public virtual string PathArchivo { get; set; }
        [DataMember]
        public virtual bool RequiereRespuesta { get; set; }
        [DataMember]
        public virtual IList<NotaDocumento> Documentos { get; set; }
        [DataMember]
        public virtual string Texto { get; set; }
        [DataMember]
        public virtual bool ActaImprimeAlFinal { get; set; }
        [DataMember]
        public virtual int ActaOrden { get; set; }      

        [DataMember]
        public virtual string NombreEstudio
        {
            get { return Estudio == null ? string.Empty : Estudio.NombreEstudioListados; }
            set { }
        }
        [DataMember]
        public virtual string NombreEstudioCompleto
        {
            get { return Estudio == null ? string.Empty : Estudio.NombreCompleto; }
            set { }
        }
        [DataMember]
        public virtual string FechaToString
        {
            get { return Fecha.ToShortDateString(); }
            set { }
        }
        
        [DataMember]
        public virtual string NombreDocumento
        {
            get 
            {
                return Documentos.Count > 0 ? Documentos[0].DocumentoVersion.Documento.Descripcion : string.Empty; 
            }
            set { }
        }
        [DataMember]
        public virtual string VersionDocumento
        {
            get
            {
                return Documentos.Count > 0 ? Documentos[0].DocumentoVersion.Descripcion : string.Empty;
            }
            set { }
        }

        public Nota()
        {            
            Documentos = new List<NotaDocumento>();
        }

        public virtual void AgregarDocumento(DocumentoVersion documentoVersion)
        {
            NotaDocumento notaDocumento = new NotaDocumento();
            notaDocumento.Nota = this;
            notaDocumento.DocumentoVersion = documentoVersion;
            Documentos.Add(notaDocumento);
        }

        public virtual void EliminarDocumentos()
        {
            if (Documentos.Count > 0)
                Documentos.Clear();
        }

        public override void Validar()
        {
            base.Validar();
        }
    }
}
