using System.Collections.Generic; 
using System.Text; 
using System;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades 
{
    [Serializable]
    [DataContract]
    public class EstudioMonodroga : EntidadBase
    {
        
        public virtual Estudio Estudio { get; set; }
        [DataMember]
        public virtual Monodroga Monodroga { get; set; }
        [DataMember]
        public virtual bool Vigente { get; set; }

        public EstudioMonodroga() { }

        public void Validar()
        {
            if (Monodroga== null)
                throw new ApplicationException("Debe seleccionar una monodroga");

        }
    }
}
