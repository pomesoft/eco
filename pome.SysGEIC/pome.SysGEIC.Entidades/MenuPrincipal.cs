using System.Collections.Generic; 
using System.Text; 
using System;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades 
{
    [Serializable]
    [DataContract]
    public class MenuPrincipal : MenuBase 
    {
        [DataMember]
        public virtual IList<MenuSecundario> Items { get; set; }

        public MenuPrincipal() 
        {
            Items = new List<MenuSecundario>();
        }

        public override string ToString()
        {
            return string.Format("[{3}] {0} - {1} {2}", Id, Texto, NavigateURL, this.GetType().Name);
        }
    }
}
