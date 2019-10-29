using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class TipoDocumentoFlujo : EntidadBaseParametrica
    {
        public virtual TipoDocumento TipoDocumento { get; set; }
        [DataMember]
        public virtual IList<TipoDocumentoFlujoEstado> Estados { get; set; }
       
        public TipoDocumentoFlujo() 
        {
            Estados = new List<TipoDocumentoFlujoEstado>();
        }

        public override void Validar()
        {
            base.Validar();
        }

        public virtual void AgregarEstado(EstadoDocumento estado, EstadoDocumento estadoPadre, bool final)
        {
            Estados.ToList<TipoDocumentoFlujoEstado>().ForEach(delegate(TipoDocumentoFlujoEstado tdFlujoEstado)
            {
                if (tdFlujoEstado.Estado == estado && tdFlujoEstado.EstadoPadre == estadoPadre)
                    throw new ApplicationException(string.Format("El estado {0} con el estado padre {1} ya existe", tdFlujoEstado.Estado, tdFlujoEstado.EstadoPadre));
                
            });

            TipoDocumentoFlujoEstado flujoEstado = new TipoDocumentoFlujoEstado();
            flujoEstado.Estado = estado;
            flujoEstado.EstadoPadre = estadoPadre;
            flujoEstado.Final = final;
            flujoEstado.TipoDocumentoFlujo = this;
            flujoEstado.Validar();
            Estados.Add(flujoEstado);
        }

        public virtual TipoDocumentoFlujoEstado ObtenerFlujoEstado(EstadoDocumento estado)
        {
            TipoDocumentoFlujoEstado flujoEstadoReturn = null;
            Estados.ToList<TipoDocumentoFlujoEstado>().ForEach(delegate(TipoDocumentoFlujoEstado tdFlujoEstado)
            {
                if (tdFlujoEstado.Estado == estado)
                    flujoEstadoReturn = tdFlujoEstado;

            });
            return flujoEstadoReturn;
        }

        public virtual void EliminarEstado(int idFlujoEstado)
        {
            Estados.ToList<TipoDocumentoFlujoEstado>().ForEach(delegate(TipoDocumentoFlujoEstado flujoEstado)
            {
                if (flujoEstado.Id.Equals(idFlujoEstado))
                    Estados.Remove(flujoEstado);
            });
        }
        public virtual List<EstadoDocumento>  ObtenerEstados(EstadoDocumento estadoPadre)
        {
            List<EstadoDocumento> estadosReturn = new List<EstadoDocumento>();
            Estados.ToList<TipoDocumentoFlujoEstado>().ForEach(delegate(TipoDocumentoFlujoEstado flujoEstado)
            {
                if (flujoEstado.EstadoPadre == null && estadoPadre == null)
                    estadosReturn.Add(flujoEstado.Estado);
                else
                    if (flujoEstado.EstadoPadre != null && flujoEstado.EstadoPadre.Equals(estadoPadre))
                        estadosReturn.Add(flujoEstado.Estado);
            });
            return estadosReturn;
        }
        public virtual List<EstadoDocumento> ObtenerEstadosFinales()
        {
            List<EstadoDocumento> estadosReturn = new List<EstadoDocumento>();
            Estados.ToList<TipoDocumentoFlujoEstado>().ForEach(delegate(TipoDocumentoFlujoEstado flujoEstado)
            {
                if (flujoEstado.Final) 
                    estadosReturn.Add(flujoEstado.Estado);
            });
            return estadosReturn;
        }
    }
}
