using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Repositorios
{
    public class ActasRepository : RepositoryGenerico<Acta>
    {
        public ActasRepository() { }

       
        /// <summary>
        /// Obtiene listado de Actas
        /// </summary>
        /// <param name="cerrada">Indica si se obtienen todas, solo las cerradas ó solo las que no están cerradas</param>
        /// <returns></returns>
        public IList<Acta> ObtenerActas(bool? cerrada)
        {
            ICriterion expresion;

            if (!cerrada.HasValue)
                expresion = Expression.Eq("Vigente", true);
            else
                expresion = Expression.And(Expression.Eq("Vigente", true),
                                           Expression.Eq("Cerrada", cerrada.Value));
            
            return repositorio.ObtenerTodos(expresion).ToList<Acta>();
        }


        public IList<ActaDTO> ListarActasDTO(bool? cerrada, string descripcion, int orden)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sWhere = string.Empty;

            string sql = @" select IdActa, Descripcion, Fecha, Hora, Cerrada 
                            from Actas ";

            if (cerrada.HasValue)
                sWhere += " and Cerrada = :cerrada ";

            if (descripcion.Trim().Length>0)
                sWhere += " and Descripcion like %:descripcion% ";

            if (sWhere.Length > 0)
                sql += " where " + sWhere.Substring(4);

            if (orden == 2)
                sql += " order by Fecha desc ";
            else
                sql += " order by Fecha asc ";


            IQuery query = session.CreateSQLQuery(sql)
                                   .AddEntity(typeof(ActaDTO));
            if (cerrada.HasValue)
                query.SetParameter("cerrada", cerrada.Value);

            if (descripcion.Trim().Length > 0)
                query.SetParameter("Descripcion", descripcion);

            return query.List<ActaDTO>();
        }

        public ActaDTO ObtenerProximaActaDTO()
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sWhere = string.Empty;

            string sql = @" select top 1 IdActa, Descripcion, Fecha, Hora, Cerrada 
                            from Actas 
                            where Fecha > getdate() ";
            
            IQuery query = session.CreateSQLQuery(sql).AddEntity(typeof(ActaDTO));
            
            IList<ActaDTO> list = query.List<ActaDTO>();
            
            ActaDTO actaReturn = new ActaDTO();
            if (list.Count > 0) actaReturn = list[0];

            return actaReturn;
        }

        /// <summary>
        /// Obtiene el listado de documentos tratados de un Acta
        /// </summary>
        /// <param name="idActa"></param>
        /// <returns></returns>
        public IList<ActaDocumento> ObtenerActaDocumentosTratados(int idActa)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sql = @"select *
                        from ActaDocumentos 
                        where IdActa = :idacta
                        order by OrdenEstudio, OrdenDocumento";

            IQuery query = session.CreateSQLQuery(sql)
                                   .AddEntity(typeof(ActaDocumento))
                                   .SetParameter("idacta", idActa);

            return query.List<ActaDocumento>();
        }

        /// <summary>
        /// Obtiene el listado de las notas tratadas en un Acta
        /// </summary>
        /// <param name="idActa"></param>
        /// <returns></returns>
        public IList<Nota> ObtenerActaNotas(int idActa)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sql = @"select *
                        from Notas 
                        where IdActa = :idacta";

            IQuery query = session.CreateSQLQuery(sql)
                                   .AddEntity(typeof(Nota))
                                   .SetParameter("idacta", idActa);

            return query.List<Nota>();
        }

        /// <summary>
        /// Obtiene el listado de las notas tratadas en un Acta
        /// </summary>
        /// <param name="idActa"></param>
        /// <returns></returns>
        public IList<Nota> ObtenerActaNota_Estudio(int idActa, int idEstudio)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sql = @"select *
                        from Notas 
                        where IdActa = :idacta and IdEstudio = :idestudio ";

            IQuery query = session.CreateSQLQuery(sql)
                                   .AddEntity(typeof(Nota))
                                   .SetParameter("idacta", idActa)
                                   .SetParameter("idestudio", idEstudio);

            return query.List<Nota>();
        }

        public IList<Acta> BuscarActas(bool? cerrada, string descripcion)
        {
            ICriteria criteria = repositorio.NHSession.CreateCriteria(typeof(Acta));

            criteria.Add(Expression.Eq("Vigente", true));

            if (cerrada.HasValue)
                criteria.Add(Expression.Eq("Cerrada", cerrada.Value));

            if (!string.IsNullOrEmpty(descripcion))
                criteria.Add(Expression.Like("Descripcion", string.Format("%{0}%", descripcion)));

            return criteria.List<Acta>().ToList<Acta>();
        }
        /// <summary>
        /// Obtener las Actas que estén en un determinado estudio
        /// </summary>
        /// <param name="idEstudio">IdEstudio del que se desea obtener las Actas donde se trató</param>
        /// <returns></returns>
        public IList<Acta> ObtenerActas(int idEstudio)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sql = @"SELECT * 
                        FROM Actas AS A
                        WHERE A.IdActa IN (SELECT AD.IdActa
				                FROM ActaDocumentos AS AD 
					              INNER JOIN DocumentoVersiones AS DV ON DV.IdDocumentoVersion = AD.IdDocumentoVersion 
					              INNER JOIN Documentos AS D ON D.IdDocumento = DV.IdDocumento
				                WHERE D.IdEstudio = :idestudio)
                            OR A.IdActa IN (SELECT N.IdActa FROM Notas AS N WHERE N.IdEstudio = :idestudio)";

            IQuery query = session.CreateSQLQuery(sql)
                                    .AddEntity(typeof(Acta))
                                    .SetParameter("idestudio", idEstudio);

            return query.List<Acta>();
        }
        /// <summary>
        /// Obtener las Actas donde se trató una Versión de Documento
        /// </summary>
        /// <param name="idDocumentoVersion">IdDocumentoVersion del que se desea obtener las Actas donde se trató</param>
        /// <returns></returns>
        public IList<ActaDocumento> ObtenerActasXDocumento(int idDocumento)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string hql = @"select ActaDocs
                        from ActaDocumento ActaDocs join ActaDocs.DocumentoVersion DV
                        where DV.Documento.Id = :iddocumento";

            IList<ActaDocumento> listActas = session.CreateQuery(hql)
                                            .SetParameter("iddocumento", idDocumento)
                                            .List<ActaDocumento>();

            return listActas;
        }

        public IList<ActaDocumento> ObtenerDocumentoTratadosXEstudio(int idEstudio)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string hql = @"select ActaDocs
                        from ActaDocumento ActaDocs join ActaDocs.DocumentoVersion DV
                        where DV.Documento.IdEstudio = :idestudio";

            IList<ActaDocumento> listActas = session.CreateQuery(hql)
                                            .SetParameter("idestudio", idEstudio)
                                            .List<ActaDocumento>();

            return listActas;
        }

        public IList<ActaDocumento> ObtenerDocumentoTratados_ActaEstudio(int idActa, int idEstudio)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string hql = @"select ActaDocs
                        from ActaDocumento ActaDocs join ActaDocs.DocumentoVersion DV
                        where DV.Documento.IdEstudio = :idestudio 
                            and ActaDocs.Acta.Id = :idacta";

            IList<ActaDocumento> listActas = session.CreateQuery(hql)
                                            .SetParameter("idestudio", idEstudio)
                                            .SetParameter("idacta", idActa)
                                            .List<ActaDocumento>();

            return listActas;
        }

        

    }
}