using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class TipoEstudio : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string CR_Texto_1 { get; set; }
        [DataMember]
        public virtual string CR_Texto_2 { get; set; }
        [DataMember]
        public virtual string CR_Texto_3 { get; set; }
        [DataMember]
        public virtual string CR_Texto_4 { get; set; }

        public TipoEstudio() { }

    }
}
