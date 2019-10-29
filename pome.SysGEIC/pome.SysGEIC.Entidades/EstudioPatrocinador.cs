using System.Collections.Generic; 
using System.Text; 
using System;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades 
{
    [Serializable]
    [DataContract]    
    public class EstudioPatrocinador : EntidadBase
    {
        
        public virtual Estudio Estudio { get; set; }
        [DataMember]
        public virtual Patrocinador Patrocinador { get; set; }
        [DataMember]
        public virtual bool Vigente { get; set; }

        public EstudioPatrocinador() { }

        public void Validar()
        {
            if (Patrocinador == null)
                throw new ApplicationException("Debe seleccionar un patrocinador");
            
        }
    }
}
