using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class ActaDocumento : EntidadBaseParametrica
    {
        public virtual Acta Acta { get; set; }        
        public virtual DocumentoVersion DocumentoVersion { get; set; }
        [DataMember]
        public virtual int OrdenEstudio { get; set; }
        [DataMember]
        public virtual int OrdenDocumento { get; set; }
        [DataMember]
        public virtual Profesional ResponsableComite { get; set; }
        [DataMember]
        public virtual bool ImprimirCarta { get; set; }
        [DataMember]
        public virtual bool? SetearSaltosLinea { get; set; }
        [DataMember]
        public virtual long Orden
        {
            get
            {
                return long.Parse(string.Format("1{0}{1}{2}", this.OrdenEstudio.ToString("000"),
                                                            this.OrdenGrupoTipoDocumento.ToString(),
                                                            this.OrdenDocumento.ToString("000")));
            }
            set { }
        }

        [DataMember]
        public virtual int OrdenGrupoTipoDocumento
        {
            get 
            {
                if (this.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo != null)
                    return this.DocumentoVersion.Documento.TipoDocumento.TipoDocumentoGrupo.Orden;
                else
                    return 2;//si el tipo de documento no tiene seteado el grupo, lo forzamos como SE TOMA CONOCIMIENTO
            }
            set { }
        }
        [DataMember]
        public virtual int IdEstudio
        {
            get { return DocumentoVersion.Documento.IdEstudio; }
            set { }
        }
        [DataMember]
        public virtual string CodigoEstudio
        {
            get { return DocumentoVersion.Documento.Estudio.Codigo; }
            set { }
        }
        [DataMember]
        public virtual string NombreEstudio
        {
            get { return DocumentoVersion.Documento.NombreEstudio; }
            set { }
        }
        [DataMember]
        public virtual string NombreEstudioCompleto
        {
            get { return DocumentoVersion.Documento.NombreEstudioCompleto; }
            set { }
        }
        [DataMember]
        public virtual int IdEstadoEstudio
        {
            get { return (DocumentoVersion.Documento.Estudio.Estado != null) ? DocumentoVersion.Documento.Estudio.Estado.Id : -1; }
            set { }
        }
        [DataMember]
        public virtual int IdDocumento
        {
            get { return DocumentoVersion.Documento.Id; }
            set { }
        }
        [DataMember]
        public virtual string NombreDocumento
        {
            get { return DocumentoVersion.Documento.Descripcion; }
            set { }
        }
        [DataMember]
        public virtual string TipoDocumentoDescripcion
        {
            get { return DocumentoVersion.Documento.TipoDocumento.Descripcion; }
            set { }
        }
        [DataMember]
        public virtual int IdVersion
        {
            get { return DocumentoVersion.Id; }
            set { }
        }        
        [DataMember]
        public virtual string VersionDocumento
        {
            get { return DocumentoVersion.Descripcion; }
            set { }
        }
        [DataMember]
        public virtual string VersionFecha
        {
            get { return DocumentoVersion.FechaToString; }
            set { }
        }
        [DataMember]
        public virtual string ComentarioDocumento
        {
            get { return this.Descripcion; }
            set { }
        }
        [DataMember]
        public virtual int IdEstadoActual
        {
            get 
            {
                DocumentoVersionEstado versionEstado = DocumentoVersion.ObtenerVersionEstado();
                return (versionEstado == null) ? 0 : versionEstado.Estado.Id; 
            }
            set { }
        }
        [DataMember]
        public virtual List<EstadoDocumento> EstadosDocumento
        {
            get
            {
                TipoDocumentoFlujo flujo = DocumentoVersion.Documento.TipoDocumento.ObtenerFlujoDefault();
                DocumentoVersionEstado estadoActual = DocumentoVersion.ObtenerVersionEstado();
                EstadoDocumento estado = estadoActual == null ? null : estadoActual.Estado;
                
                List<EstadoDocumento> estadosReturn = new List<EstadoDocumento>();

                if (estadoActual.EstadoFinal.HasValue && estadoActual.EstadoFinal.Value)
                {
                    estadosReturn.AddRange(flujo.ObtenerEstadosFinales());
                }
                else
                {
                    estadosReturn.Add(estado);
                    estadosReturn.AddRange(flujo.ObtenerEstados(estado));
                }
                return estadosReturn;
            }
            set { }
        }

        public ActaDocumento() { }

        public override void Validar()
        {
            if (DocumentoVersion == null)
                throw new ApplicationException("Debe seleccionar un documento y versión");
            if (DocumentoVersion.Documento.Estudio.Estado == null)
                throw new ApplicationException(string.Format("En el estudio {0} debe indicar estado.", DocumentoVersion.Documento.Estudio.Codigo));
            if (DocumentoVersion.Documento.Estudio.InvestigadoresPrincipales.Count == 0)
                throw new ApplicationException(string.Format("El estudio {0} no tiene asignado investigador principal.", DocumentoVersion.Documento.Estudio.Codigo));
        }
    }
}
