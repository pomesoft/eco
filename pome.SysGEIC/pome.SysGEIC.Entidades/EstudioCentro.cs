using System.Collections.Generic; 
using System.Text; 
using System;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades 
{
    [Serializable]
    [DataContract]    
    public class EstudioCentro : EntidadBase
    {
        
        public virtual Estudio Estudio { get; set; }
        [DataMember]
        public virtual Centro Centro { get; set; }
        [DataMember]
        public virtual bool Vigente { get; set; }
        public virtual DateTime? Desde { get; set; }
        [DataMember]
        public virtual string DesdeToString
        {
            get { return Desde.HasValue  ? Desde.Value.ToShortDateString() : string.Empty; }
            set { }
        }

        public virtual DateTime? Hasta { get; set; }
        [DataMember]
        public virtual string HastaToString
        {
            get { return Hasta.HasValue ? Hasta.Value.ToShortDateString() : string.Empty; }
            set { }
        }


        public EstudioCentro() { }

        public void Validar()
        {
            if (Centro == null)
                throw new ApplicationException("Debe seleccionar un centro");
        }

    }
}
