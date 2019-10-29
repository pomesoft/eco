using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class TipoDocumentoGrupo : EntidadBaseParametrica
    {
        public virtual int Orden { get; set; }
        public virtual string TextoActa { get; set; }

        public TipoDocumentoGrupo() { }

        public bool SeEvalua()
        {
            return this.Id == 1;
        }
        public bool SeTomaConocimiento()
        {
            return this.Id == 2;
        }
    }
}
