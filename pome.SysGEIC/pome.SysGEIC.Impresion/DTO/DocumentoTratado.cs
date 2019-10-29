using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pome.SysGEIC.Impresion.DTO
{
    public class DocumentoTratado
    {
        public string Grupo { get; set; }
        public string Orden { get; set; }
        public string Documento { get; set; }
        public string Version { get; set; }
        public string FechaVersion { get; set; }
        public string ResponsableComite { get; set; }
        public string Comentario { get; set; }

        public DocumentoTratado() { }
    }
}
