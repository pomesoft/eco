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
    public class ProcesadorPlantilla
    {
        public string NombrePlantilla { get; set; }
        public IDictionary Datos { get; set; }
        public string HTMLProcesado { get; set; }

        public ProcesadorPlantilla() { }

        public void ProcesarPlantilla()
        {
            INVelocity fileEngine = NVelocityFactory.CreateNVelocityFileEngine(ProcesadorHelpers.DirectorioPlantillas, true);
            HTMLProcesado += fileEngine.Process(Datos, string.Format("{0}.html", NombrePlantilla));
        }

    }
}
