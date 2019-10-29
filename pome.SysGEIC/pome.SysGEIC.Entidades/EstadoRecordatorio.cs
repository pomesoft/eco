using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class EstadoRecordatorio : EntidadBaseParametrica
    {
        [DataMember]
        public virtual bool EstaProgramado
        {
            get { return this.Id == 1; }
            set { }
        }
        [DataMember]
        public virtual bool EstaActivo
        {
            get { return this.Id == 2; }
            set { }
        }
        [DataMember]
        public virtual bool EstaCerrado
        {
            get { return this.Id == 3; }
            set { }
        }
        public EstadoRecordatorio() { }


    }
}
