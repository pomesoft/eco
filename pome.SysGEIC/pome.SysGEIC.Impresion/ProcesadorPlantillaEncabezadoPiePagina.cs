using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.Impresion.NVelocity;
using pome.SysGEIC.Impresion.DTO;

namespace pome.SysGEIC.Impresion
{
    public class ProcesadorPlantillaEncabezadoPiePagina
    {
        public string NombrePlantilla { get; set; }
        public IDictionary Datos { get; set; }
        public string HTMLProcesado { get; set; }

        public Estudio estudio { get; set; }
        public CartaRespuestaModelo modeloCarta { get; set; }

        public ProcesadorPlantillaEncabezadoPiePagina() 
        {
            HTMLProcesado = string.Empty;
            NombrePlantilla = "HeaderFooter.html";            
        }
    
        public void ProcesarPlantilla()
        {            
            IDictionary datos = new Hashtable();
            datos.Add("Logo", string.Format(@"{0}/LogoActa.png", ProcesadorHelpers.UrlDirectorioImagenes));
            datos.Add("Estudio", estudio.NombreCompleto);
            datos.Add("CodigoEstudio", estudio.Codigo);
            datos.Add("PatrocinadoPor", ProcesadorHelpers.ConcatenarPatrocinadores(estudio));
            datos.Add("TextoPiePagina", modeloCarta.PlantillaPiePagina.Texto.Replace("\n", Constantes.SaldoLinea));
            
            INVelocity fileEngine = NVelocityFactory.CreateNVelocityFileEngine(ProcesadorHelpers.DirectorioPlantillas, true);
            HTMLProcesado = fileEngine.Process(datos, NombrePlantilla);
        }
    }
}
