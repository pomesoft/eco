using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class ItemChart : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string Descripcion2 { get; set; }
        [DataMember]
        public virtual int Valor { get; set; }
        [DataMember]
        public virtual int Valor2 { get; set; }

        public ItemChart() { }
    }
}
