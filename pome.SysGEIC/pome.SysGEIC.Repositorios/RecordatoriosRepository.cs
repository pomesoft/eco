using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Repositorios
{
    public class RecordatoriosRepository : RepositoryGenerico<Recordatorio>
    {
        public RecordatoriosRepository() { }

        public List<Recordatorio> ObtenerRecordatoriosActivos(bool? avisoPopup, bool? avisoEmail)
        {
            string sqlActivarRecordatorios = @"UPDATE Recordatorios SET IdEstadoRecordatorio = 2 WHERE FechaActivacion <= getdate() AND IdEstadoRecordatorio = 1 ";
            this.ActualizarSQL(sqlActivarRecordatorios);

            ISession session = NHibernateSessionSingleton.GetSession();

            string hql = @"select R
                        from Recordatorio R   
                        where R.Vigente = True 
                          and R.EstadoRecordatorio.Id = 2 ";
            if (avisoPopup.HasValue)
                hql += " and R.AvisoPopup = :avisoPopup";
            if (avisoEmail.HasValue)
                hql += " and R.AvisoEmail = :avisoEmail";

            IQuery query = session.CreateQuery(hql);
            if (avisoPopup.HasValue)
                query.SetParameter("avisoPopup", avisoPopup.Value);
            if (avisoEmail.HasValue)
                query.SetParameter("avisoEmail", avisoEmail.Value);

            return query.List<Recordatorio>().ToList<Recordatorio>();
        }

        public List<Recordatorio> ObtenerRecordatoriosEstudio(int idEstudio, int idTipoRecordatorio)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string hql = @"select R
                        from Recordatorio R   
                        where R.Vigente = True 
                          and R.Estudio.Id = :idestudio
                          and R.TipoRecordatorio.Id = :idTipoRecordatorio";

            IList<Recordatorio> list = session.CreateQuery(hql)
                                            .SetParameter("idestudio", idEstudio)
                                            .SetParameter("idTipoRecordatorio", idTipoRecordatorio)
                                            .List<Recordatorio>();                                            

            return list.ToList<Recordatorio>();
        }

        public List<Recordatorio> BuscarRecordatorios(string tipoRecordatorio, string descripcion, string codigoEstudio, string estado)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sWhere = string.Empty;
            if (descripcion != string.Empty) sWhere += " AND R.Descripcion LIKE :descripcion ";
            if (tipoRecordatorio != string.Empty) sWhere += " AND (TR.Descripcion LIKE :tipoRecordatorio) ";
            if (codigoEstudio != string.Empty) sWhere += " AND (E.Codigo LIKE :codigoEstudio) ";
            if (estado != string.Empty) sWhere += " AND (R.IdEstadoRecordatorio = :estado) ";

            if (sWhere != string.Empty) sWhere = " WHERE " + sWhere.Substring(5);

            string sql = @"SELECT R.*
                        FROM Recordatorios AS R
                        INNER JOIN TiposRecordatorio AS TR ON R.IdTipoRecordatorio = TR.IdTipoRecordatorio
                        LEFT JOIN Estudios AS E ON E.IdEstudio = R.IdEstudio "
                       + sWhere;

            IQuery query = session.CreateSQLQuery(sql).AddEntity(typeof(Recordatorio));

            if (descripcion != string.Empty) query.SetParameter("descripcion", string.Format("%{0}%", descripcion));
            if (tipoRecordatorio != string.Empty) query.SetParameter("tipoRecordatorio", string.Format("%{0}%", tipoRecordatorio));
            if (codigoEstudio != string.Empty) query.SetParameter("codigoEstudio", string.Format("%{0}%", codigoEstudio));
            if (estado != string.Empty) query.SetParameter("estado", string.Format("{0}", estado));

            return query.List<Recordatorio>().ToList<Recordatorio>();
        }


        
    }
}
