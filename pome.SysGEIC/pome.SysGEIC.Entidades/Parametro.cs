using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Parametro: EntidadBaseParametrica
    {
        [DataMember]
        public string Valor { get; set; }
        
        public Parametro() { } 
    }
}
