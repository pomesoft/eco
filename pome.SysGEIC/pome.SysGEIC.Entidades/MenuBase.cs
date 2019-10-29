using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public abstract class MenuBase : EntidadBase
    {
        [DataMember]
        public virtual short Orden { get; set; }
        [DataMember]
        public virtual string Texto { get; set; }
        [DataMember]
        public virtual string Descripcion { get; set; }
        [DataMember]
        public virtual string NavigateURL { get; set; }
        [DataMember]
        public virtual bool Activo { get; set; }

        public MenuBase() { }

        public override string ToString()
        {
            return string.Format("[{3}] {0} - {1} {2}", Id, Texto, NavigateURL, this.GetType().Name);
        }
    }
}
