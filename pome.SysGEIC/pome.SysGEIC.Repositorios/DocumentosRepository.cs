using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Repositorios
{
    public class DocumentosRepository : RepositoryGenerico<Documento>
    {
        public DocumentosRepository() { }

        public List<Documento> ObtenerDocumentosEstudio(int idEstudio)
        {
            ICriterion expresion;

            expresion = Expression.And(Expression.Eq("Vigente", true),
                                       Expression.Eq("IdEstudio", idEstudio));
            
            return repositorio.ObtenerTodos(expresion).ToList<Documento>();
        }

        public List<Documento> ObtenerDocumentosAnulados(int idEstudio)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string hql = @"select D
                        from Documento D   
                        where D.Vigente = False AND D.Estudio.Id = :idestudio";

            IList<Documento> listDocs = session.CreateQuery(hql)
                                            .SetParameter("idestudio", idEstudio)
                                            .List<Documento>();

            return listDocs.ToList<Documento>();
        }
    }
}
