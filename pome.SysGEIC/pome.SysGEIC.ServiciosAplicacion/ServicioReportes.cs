using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Repositorios;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public class ServicioReportes
    {
        ReportesRepository repository;
        public ServicioReportes() 
        {
            repository = new ReportesRepository();
        }

        public List<ItemChart> ObtenerDatosGraficoSemaforo()
        {
            return repository.ObtenerDatosGraficoSemaforo().ToList<ItemChart>();
        }

        public List<ItemChart> ObtenerDatosGraficoEstados()
        {
            return repository.ObtenerDatosGraficoEstados().ToList<ItemChart>();
        }

        public List<ItemChart> ObtenerDatosDocsEvaluadosMes()
        {
            return repository.ObtenerDatosDocsEvaluadosMes().ToList<ItemChart>();
        }
    }
}
