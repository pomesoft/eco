using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Repositorios
{
    public class ReportesRepository
    {

        public ReportesRepository()
        {

        }

        public IList<ItemChart> ObtenerDatosGraficoSemaforo()
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sql = @" select  ROW_NUMBER() OVER(ORDER BY e.EstadoSemaforo ASC) AS Id,
                                    case e.EstadoSemaforo
			                            when 1 then 'SIN REGISTROS'  
			                            when 2 then 'CON PENDIENTES'  
			                            when 3 then 'SIN PENDIENTES'  
		                            end as Descripcion, 
                                    '' as Descripcion2, 
                                    count(*) as Valor,
                                    -1 as Valor2
                            from Estudios e
                            where not e.IdEstado in (3, 4) and e.EstadoSemaforo > 0
                            group by e.EstadoSemaforo
                            order by e.EstadoSemaforo";

            IQuery query = session.CreateSQLQuery(sql)
                                  .AddEntity(typeof(ItemChart));

            return query.List<ItemChart>();
        }

        public IList<ItemChart> ObtenerDatosGraficoEstados()
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sql = @" select  ROW_NUMBER() OVER(ORDER BY ee.Descripcion ASC) AS Id,
                                    ee.Descripcion as Descripcion, 
                                    '' as Descripcion2, 
                                    count(*) as Valor,
                                    -1 as Valor2
                            from Estudios e 
                                inner join EstadosEstudio ee on e.IdEstado = ee.IdEstadoEstudio
                            where not e.IdEstado in (3, 4)
                            group by ee.Descripcion
                            order by Count(*) desc";

            IQuery query = session.CreateSQLQuery(sql)
                                  .AddEntity(typeof(ItemChart));

            return query.List<ItemChart>();
        }

        public IList<ItemChart> ObtenerDatosDocsEvaluadosMes()
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sql = @" 
                            select	ROW_NUMBER() OVER(ORDER BY MONTH(de.Mes) ASC) AS Id,
		                            e.Descripcion as Descripcion,
		                            '' as Descripcion2,
		                            de.Mes as Valor,		
		                            count(*) as Valor2
		
                            from (select	MONTH(a.Fecha)  as Mes, 
				                            ad.IdDocumentoVersion,
				                            MAX(dve.IdVersionEstado) as ultimaVersionEstado
		                            from Actas a
			                            inner join ActaDocumentos ad on a.IdActa = ad.IdActa
			                            inner join DocumentoVersionEstados as dve on dve.IdDocumentoVersion = ad.IdDocumentoVersion 
		                            where YEAR(a.Fecha) = 2019
		                            group by MONTH(a.Fecha), ad.IdDocumentoVersion) as de 

		                            inner join DocumentoVersionEstados dve on dve.IdVersionEstado = de.ultimaVersionEstado 
		                            inner join EstadosDocumento as e on dve.IdEstadoDocumento = e.IdEstadoDocumento
                            group by de.Mes, e.Descripcion
                            order by de.Mes, e.Descripcion
                            ";

            IQuery query = session.CreateSQLQuery(sql)
                                  .AddEntity(typeof(ItemChart));

            return query.List<ItemChart>();

        }
    }
}
