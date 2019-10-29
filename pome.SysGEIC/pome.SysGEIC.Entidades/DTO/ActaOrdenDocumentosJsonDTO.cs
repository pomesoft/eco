using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pome.SysGEIC.Entidades
{
    public class ActaOrdenDocumentosJsonDTO
    {
        public virtual int IdActaDocumento { get; set; }
        public virtual int IdEstudio { get; set; }
        public virtual int OrdenEstudio { get; set; }
        public virtual int OrdenDocumento { get; set; }

        public ActaOrdenDocumentosJsonDTO() { }
    }
}
