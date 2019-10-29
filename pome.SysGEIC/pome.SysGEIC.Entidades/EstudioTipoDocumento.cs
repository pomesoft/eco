using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.Serialization;


namespace pome.SysGEIC.Entidades
{
    /// <summary>
    /// Entidad que se utiliza para la configuracion del semaforo de cada estudio
    /// </summary>
    [DataContract]
    public class EstudioTipoDocumento : EntidadBase
    {
        public virtual Estudio Estudio { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }

        [DataMember]
        public virtual int EstadoSemaforo { get; set; }
        [DataMember]
        public virtual string TipoDocumentoDescripcion 
        {
            get { return TipoDocumento.Descripcion; }
            set { } 
        }
        

        public EstudioTipoDocumento() { }

        public void Validar()
        {
            if (Estudio == null)
                throw new ApplicationException("Debe ingresar un Estudio.");
            if (TipoDocumento == null)
                throw new ApplicationException("Debe ingresar un Tipo de Documento.");
        }
    }
}
