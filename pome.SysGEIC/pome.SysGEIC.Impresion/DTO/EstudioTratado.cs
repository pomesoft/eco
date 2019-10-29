using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pome.SysGEIC.Impresion.DTO
{
    public class EstudioTratado
    {
        public int Orden { get; set; }
        public string NombreCompleto { get; set; }
        public string Codigo { get; set; }
        public string Patrocinador { get; set; }
        public string Aprobado { get; set; }
        public string InvestigadorPrincipal { get; set; }
        public string CentroHabilitado { get; set; }
        public string CentroHabilitadoContacto { get; set; }
        public List<DocumentoTratado> Documentos { get; set; }
        public List<string> NotasInicio { get; set; }
        public List<string> NotasFinal { get; set; }
        

        public EstudioTratado() 
        {
            Documentos = new List<DocumentoTratado>();
            NotasInicio = new List<string>();
            NotasFinal = new List<string>();
        }
    }
}
