using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Equipo : EntidadBaseParametrica
    {
        [DataMember]
        public virtual IList<IntegranteEquipo> Integrantes { get; set; }

        public virtual List<IntegranteEquipo> IntegrantesListaVacia
        {
            get 
            {
                List<IntegranteEquipo> integrantes = new List<IntegranteEquipo>();
                integrantes.Add(new IntegranteEquipo());
                return integrantes;
            }
        }

        public Equipo() 
        {
            this.Id = -1;
        }

        public virtual void AgregarIntegrante(IntegranteEquipo integranteAgregar)
        {            
            Integrantes.ToList<IntegranteEquipo>().ForEach(delegate(IntegranteEquipo integrante)
            {
                if (integrante.Profesional == integranteAgregar.Profesional)
                    throw new ApplicationException(string.Format("El profesional {0} ya es integrante del equipo {1}",
                                                                integranteAgregar.Profesional.NombreCompleto,
                                                                this.Descripcion));
            });

            this.Integrantes.Add(integranteAgregar);
        }

        public override void Validar()
        {
            StringBuilder errores = new StringBuilder();

            try
            {
                base.Validar();
            }
            catch (Exception ex)
            {
                errores.AppendLine(ex.Message + Constantes.SaldoLinea);
            }

            
            if (Integrantes == null)
                errores.AppendLine("Debe ingresar los integrantes del equipo" + Constantes.SaldoLinea);
            else
                Integrantes.ToList<IntegranteEquipo>().ForEach(delegate(IntegranteEquipo integrante)
                {
                    string msjError = integrante.Validar();
                    if (msjError != string.Empty)
                        errores.AppendLine(msjError + Constantes.SaldoLinea);
                });

            if (errores.Length != 0)
                throw new ApplicationException(errores.ToString());
        }

        
    }
}
