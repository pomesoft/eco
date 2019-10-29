using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades 
{
    [Serializable]
    [DataContract]
    public class ListaEmail : EntidadBase
    {
        public virtual ListaEmails ListaEmails { get; set; }
        public virtual Email Email { get; set; }

        public ListaEmail() { }
    }
}
