using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class TipoDocumento : EntidadBaseParametrica
    {
        [DataMember]
        public virtual bool RequiereVersion { get; set; }
        /// <summary>
        /// Si es true se incluyen en las cartas de respuestas, si es false no se incluyen
        /// </summary>
        [DataMember]
        public virtual bool ListarCartaRespuesta { get; set; }
        /// <summary>
        /// Solo aplica para las cartas de Aprobacion de Estudios, si es true se listan los documentos en la seccion del tipo de documento, si es false no se listan y solo queda el Tipo de Documento
        /// </summary>
        [DataMember]
        public virtual bool ListarDocsCartaRespuesta { get; set; }
        /// <summary>
        /// Indica si el el tipo de documento es obligatorio para la aprobacion del estudio, semaforo verde
        /// </summary>
        [DataMember]
        public virtual bool NecesarioAprobacionEstudio { get; set; }
        [DataMember]
        public virtual TipoDocumentoGrupo TipoDocumentoGrupo { get; set; }
        [DataMember]
        public virtual IList<TipoDocumentoFlujo> Flujos { get; set; }
        [DataMember]
        public virtual IList<TipoDocumentoRecordatorio> TiposRecordatorio { get; set; }
        [DataMember]
        public virtual string TipoDocumentoGrupoDescripcion
        {
            get { return TipoDocumentoGrupo == null ? string.Empty : TipoDocumentoGrupo.Descripcion; }
            set { }
        }

        public TipoDocumento() 
        {
            Flujos = new List<TipoDocumentoFlujo>();
            TiposRecordatorio = new List<TipoDocumentoRecordatorio>();
        }
           
        public override void Validar()
        {
            base.Validar();

            if (TipoDocumentoGrupo == null)
                throw new ApplicationException("Debe seleccionar un Grupo.");
        }

        public virtual void ActualizarFlujo(TipoDocumentoFlujo flujo)
        {
            Flujos.ToList<TipoDocumentoFlujo>().ForEach(delegate(TipoDocumentoFlujo tdFlujo)
            {
                if (tdFlujo.Descripcion == flujo.Descripcion)
                    throw new ApplicationException(string.Format("El item {0} ya existe", tdFlujo.Descripcion));
                if (tdFlujo == flujo)
                    Flujos.Remove(tdFlujo);
            });
            
            flujo.TipoDocumento = this;
            this.Flujos.Add(flujo);
            
        }

        

        public virtual void ElimnarFlujo(TipoDocumentoFlujo flujo)
        {
            Flujos.ToList<TipoDocumentoFlujo>().ForEach(delegate(TipoDocumentoFlujo tdFlujo)
            {
                if (tdFlujo == flujo)
                    Flujos.Remove(tdFlujo);
            });
        }

        public virtual TipoDocumentoFlujo ObtenerFlujo(int idFlujo)
        {
            TipoDocumentoFlujo flujoReturn = Flujos.ToList<TipoDocumentoFlujo>().Find(delegate(TipoDocumentoFlujo item)
            {
                return item.Id == idFlujo;
            });
            return flujoReturn;
        }
        public virtual TipoDocumentoFlujo ObtenerFlujoDefault()
        {
            TipoDocumentoFlujo flujoReturn = null;
            Flujos.ToList<TipoDocumentoFlujo>().ForEach(delegate(TipoDocumentoFlujo tdFlujo)
            {
                if (flujoReturn == null)
                    flujoReturn = tdFlujo;
            });
            return flujoReturn;
        }

        public void AgregarTipoRecordatorio(TipoRecordatorio tipoRecordatorio, int meses)
        {
            TipoDocumentoRecordatorio tipoRec = TiposRecordatorio.ToList<TipoDocumentoRecordatorio>().Find(delegate(TipoDocumentoRecordatorio recordatorio)
            {
                return recordatorio.TipoRecordatorio.Id == tipoRecordatorio.Id;
            });

            if (tipoRec == null)
            {
                tipoRec = new TipoDocumentoRecordatorio();
                tipoRec.TipoDocumento = this;
                tipoRec.TipoRecordatorio = tipoRecordatorio;
                tipoRec.Meses = meses;
                TiposRecordatorio.Add(tipoRec);
            }
        }
    }
}
