using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class NotaDocumento : EntidadBase
    {
        public virtual Nota Nota { get; set; }
        [DataMember]
        public virtual DocumentoVersion DocumentoVersion { get; set; }

        public NotaDocumento() { }
    }
}
