using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class MenuSecundario : MenuBase
    {        
        public virtual MenuPrincipal Menu { get; set; }

        public MenuSecundario() { }

        public override string ToString()
        {
            return string.Format("[{3}] {0} - {1} {2}", Id, Texto, NavigateURL, this.GetType().Name);
        }
    }
}
