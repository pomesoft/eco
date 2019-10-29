using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class TipoDocumentoRecordatorio: EntidadBase
    {
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual TipoRecordatorio TipoRecordatorio { get; set; }
        [DataMember]
        public virtual int Meses { get; set; }
        [DataMember]
        public virtual int IdTipoRecordatorio
        {
            get 
            {
                return TipoRecordatorio != null ? TipoRecordatorio.Id : -1;
            }
            set { }
        }
        public TipoDocumentoRecordatorio() { }
    }
}
