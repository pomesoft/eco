using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pome.SysGEIC.Impresion.DTO
{
    public class TipoDocumentoTratado
    {
        public virtual string TipoDocumento { get; set; }
        public virtual int Orden { get; set; }
        public virtual string ListarDocumentos { get; set; }
        public virtual List<DocumentoTratado> Documentos { get; set; }
        
        public TipoDocumentoTratado() 
        {
            Documentos = new List<DocumentoTratado>();
        }
    }
}
