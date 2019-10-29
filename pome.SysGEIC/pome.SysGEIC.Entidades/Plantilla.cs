using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Plantilla : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string Texto { get; set; }
        [DataMember]
        public virtual TipoPlantilla TipoPlantilla { get; set; }
        [DataMember]
        public virtual string TipoPlantillaDescripcion 
        { 
            get
            {
                return TipoPlantilla == null ? string.Empty : TipoPlantilla.Descripcion;
            }
            set { } 
        }

        public Plantilla() { }
        
        public override void Validar()
        {
            base.Validar();

            if (Texto.Trim().Length == 0)
                throw new ApplicationException("Debe ingresar texto.");

            if (TipoPlantilla == null)
                throw new ApplicationException("Debe seleccionar tipo de plantilla.");
        }
    }
}
