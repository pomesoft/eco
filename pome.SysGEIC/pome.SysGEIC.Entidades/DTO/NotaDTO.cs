using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class NotaDTO : EntidadBaseParametrica
    {
        public virtual string ImprimeAlFinal { get; set; }
        public virtual string IdNota { get; set; }
        public virtual string IdEstudio { get; set; }
        public virtual string Fecha { get; set; }
        public virtual string IdAutor { get; set; }
        public virtual string Texto { get; set; }

        public NotaDTO() { }
    }
}
