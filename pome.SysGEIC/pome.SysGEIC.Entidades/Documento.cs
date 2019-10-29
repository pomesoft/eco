using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Documento : EntidadBaseParametrica
    {
        public virtual Estudio Estudio { get; set; }
        [DataMember]
        public virtual int IdEstudio { get; set; }
        [DataMember]
        public virtual TipoDocumento TipoDocumento { get; set; }        
        [DataMember]
        public virtual IList<DocumentoVersion> Versiones { get; set; }
        [DataMember]
        public virtual bool Limitante { get; set; }
        [DataMember]
        public virtual int RecordatorioInactividadId { get; set; }
        [DataMember]
        public virtual int RecordatorioInactividadMeses { get; set; }
        [DataMember]
        public virtual int RecordatorioInformeAvanceId { get; set; }
        [DataMember]
        public virtual int RecordatorioInformeAvanceMeses { get; set; }
        [DataMember]
        public virtual int RecordatorioVencimientoId { get; set; }
        [DataMember]
        public virtual int RecordatorioVencimientoMeses { get; set; }

        [DataMember]
        public virtual string TipoDocumentoDescripcion
        {
            get { return TipoDocumento.Descripcion; }
            set { }
        }
        [DataMember]
        public virtual string NombreEstudio 
        {
            get { return (Estudio == null) ? string.Empty : Estudio.NombreEstudioListados; }
            set { }
        }
        [DataMember]
        public virtual string NombreEstudioCompleto
        {
            get { return (Estudio == null) ? string.Empty : Estudio.NombreCompleto; }
            set { }
        }
        [DataMember]
        public virtual DocumentoVersion VersionActual
        {
            get 
            {
                return ObtenerVersion();
            }
            set { }
        }
        [DataMember]
        public virtual DocumentoVersionEstado VersionEstadoActual
        {
            get
            {
                return ObtenerVersionEstado();
            }
            set { }
        }
        [DataMember]
        public virtual string EstadoFinal
        {
            get
            {
                DocumentoVersionEstado versionEstado = ObtenerVersionEstado();
                return versionEstado != null && versionEstado.EstadoFinal.HasValue && versionEstado.EstadoFinal.Value ? "SI" : "NO";
            }
            set { }
        }

        //TODO: Estas propetys se deberian eliminar si solo se utilizaban para la bandeja de inicio
        [DataMember]
        public virtual string VersionActualDescripcion
        {
            get
            {
                DocumentoVersion version = ObtenerVersion();
                return version != null ? version.Descripcion : string.Empty;
            }
            set { }
        }
        [DataMember]
        public virtual string VersionActualFecha
        {
            get 
            {
                DocumentoVersion version = ObtenerVersion();
                return version != null && version.FechaToString != null ? version.FechaToString : string.Empty;
            }
            set { }
        }
        [DataMember]
        public virtual string EstadoActual
        {
            get 
            {
                EstadoDocumento estado = ObtenerEstado();
                return estado != null && estado.Descripcion != null ? estado.Descripcion : string.Empty;
            }
            set { }
        }
        [DataMember]
        public virtual int IdEstadoActual
        {
            get
            {
                EstadoDocumento estado = ObtenerEstado();
                return estado != null ? estado.Id : -1;
            }
            set { }
        }
        [DataMember]
        public virtual string EstadoActualFecha
        {
            get
            {
                DocumentoVersionEstado versionEstado = ObtenerVersionEstado();
                return versionEstado != null && versionEstado.FechaToString != null ? versionEstado.FechaToString : string.Empty;
            }
            set { }
        }
        
        public Documento() 
        {
            Versiones = new List<DocumentoVersion>();
            TipoDocumento = new TipoDocumento();
        }

        public override void Validar() 
        {
            base.Validar();

            if (TipoDocumento == null)
                throw new ApplicationException("Debe seleccionar un tipo de documento.");
        }

        public virtual DocumentoVersion ObtenerVersion(int idVersion)
        {
            DocumentoVersion versionReturn = null;
            Versiones.ToList<DocumentoVersion>().ForEach(delegate(DocumentoVersion docVersion)
            {
                if (docVersion.Id == idVersion)
                    versionReturn = docVersion;
            });
            return versionReturn;
        }
        //TODO: Implementar con Linq
        public virtual DocumentoVersion ObtenerVersion()
        {
            DocumentoVersion versionReturn = null;
            Versiones.ToList<DocumentoVersion>().ForEach(delegate(DocumentoVersion docVersion)
            {
                versionReturn = docVersion;
            });
            return versionReturn;
        }

        public virtual DocumentoVersionEstado ObtenerVersionEstado()
        {
            DocumentoVersionEstado versionEstado = null;
            Versiones.ToList<DocumentoVersion>().ForEach(delegate(DocumentoVersion docVersion)
            {
                versionEstado = docVersion.ObtenerVersionEstado();
            });
            return versionEstado;
        }


        public virtual bool PermitirCargarVersion()
        {
            bool permite = true;

            if (!TipoDocumento.RequiereVersion)
            {
                if (Versiones.Count == 1)
                    permite = this.VersionActual != null && this.VersionActual.Descripcion != string.Empty;
                else
                    permite = true;
            }

            return permite;
        }


        public virtual EstadoDocumento ObtenerEstado()
        {
            DocumentoVersionEstado versionEstado = this.ObtenerVersionEstado();
            EstadoDocumento estado = null;
            if (versionEstado != null)
                estado = versionEstado.Estado;
            return estado;
        }
        public virtual void AgregarVersion(DocumentoVersion version)
        {
            Versiones.ToList<DocumentoVersion>().ForEach(delegate(DocumentoVersion docVersion)
            {
                if (version.Descripcion != string.Empty && docVersion.Descripcion.Equals(version.Descripcion))
                    throw new ApplicationException(string.Format("La versión {0} ya existe en el documento",
                                                                version.Descripcion));
            });
            version.Documento = this;
            Versiones.Add(version);
        }
        

    }
}
