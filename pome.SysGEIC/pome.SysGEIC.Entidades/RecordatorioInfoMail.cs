using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class RecordatorioInfoMail : EntidadBase
    {
        public virtual Recordatorio Recordatorio { get; set; }
        

        public RecordatorioInfoMail() { }
    }
}
