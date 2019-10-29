using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class RecordatorioDocumento : EntidadBase
    {
        public virtual Recordatorio Recordatorio { get; set; }
        public virtual Documento Documento { get; set; }
        public virtual int Meses { get; set; }

        public RecordatorioDocumento()
        { }
    }
}
