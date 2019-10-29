using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pome.SysGEIC.Entidades
{
    public class ActaDocumentoJsonDTO
    {
        public virtual string IdActaDocumento { get; set; }
        public virtual string IdEstudio { get; set; }
        public virtual string IdDocumento { get; set; }
        public virtual string IdDocumentoVersion { get; set; }
        public virtual string Comentario { get; set; }
        public virtual string IdEstadoDocumento { get; set; }
        public virtual string ActualizarEstadoFinal { get; set; }
        public virtual string ImprimirCarta { get; set; }
        public virtual string SetearSaltoLinea { get; set; } 

        public ActaDocumentoJsonDTO() { }
    }
}
