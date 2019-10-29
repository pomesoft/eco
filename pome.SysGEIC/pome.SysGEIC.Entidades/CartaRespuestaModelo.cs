using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class CartaRespuestaModelo : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string NombrePlantilla { get; set; }
        [DataMember]
        public virtual bool IncluirDocumentosEvaluados { get; set; }
        [DataMember]
        public virtual bool IncluirDocumentosTomaConocimiento { get; set; }
        [DataMember]
        public virtual bool IncluirDocumentosPedidoCambio { get; set; }
        [DataMember]
        public virtual bool IncluirTodosDocumentosEstudio { get; set; }
        [DataMember]
        public virtual Plantilla PlantillaIntroduccion { get; set; }
        [DataMember]
        public virtual Plantilla PlantillaIntroduccionOpcional { get; set; }
        [DataMember]
        public virtual Plantilla PlantillaPiePagina { get; set; }
        [DataMember]
        public virtual Plantilla PlantillaBuenasPracticas { get; set; }
        [DataMember]
        public virtual Plantilla PlantillaTextoAprobacion { get; set; }
        [DataMember]
        public virtual Plantilla PlantillaTextoFirmaPresidente { get; set; }
        [DataMember]
        public virtual bool IncluirFirmaPresidente { get; set; }
        [DataMember]
        public virtual bool IncluirFirmaMiembros { get; set; }
        [DataMember]
        public virtual string TextoLibre { get; set; }

        public CartaRespuestaModelo() { }

        public override void Validar()
        {
            base.Validar();
        }
    }
}