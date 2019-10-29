using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class EstadoEstudio : EntidadBaseParametrica
    {
        [DataMember]
        public virtual bool Final { get; set; }

        public virtual bool EsIngresado
        {
            get { return this.Id == 1; }
            set { }
        }
        public virtual bool EsAprobado 
        {
            get { return this.Id == 2; }
            set { }
        }
        public virtual bool EsDocumentosAprobado
        {
            get { return this.Id == 7; }
            set { }
        }

        public EstadoEstudio() { }       
    }
}
