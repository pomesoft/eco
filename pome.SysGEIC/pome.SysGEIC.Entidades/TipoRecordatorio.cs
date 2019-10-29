using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class TipoRecordatorio : EntidadBaseParametrica
    {
        [DataMember]
        public virtual bool? AvisoMail { get; set; }
        [DataMember]
        public virtual bool? AvisoPopup { get; set; }
        [DataMember]
        public virtual string Color { get; set; }
        [DataMember]
        public virtual bool Editable { get; set; }

        public TipoRecordatorio() { }
    }
}
