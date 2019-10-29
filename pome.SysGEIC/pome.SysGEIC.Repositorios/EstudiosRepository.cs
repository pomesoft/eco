using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Repositorios
{
    public class EstudiosRepository : RepositoryGenerico<Estudio>
    {
        public EstudiosRepository() { }

        /// <summary>
        /// Realiza la busqueda y devuelve los estudios que cumplen con los criterios de busqueda recibido
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="abreviado"></param>
        /// <param name="nombreCompleto"></param>
        /// <returns></returns>
        public List<Estudio> BuscarEstudios(string codigo, string abreviado, string nombreCompleto)
        {            
            ICriteria criteria = repositorio.NHSession.CreateCriteria(typeof(Estudio));

            criteria.Add(Expression.Eq("Vigente", true));

            if (!string.IsNullOrEmpty(codigo))
                criteria.Add(Expression.Like("Codigo", string.Format("%{0}%", codigo)));

            if (!string.IsNullOrEmpty(abreviado))
                criteria.Add(Expression.Like("Descripcion", string.Format("%{0}%", abreviado)));

            if (!string.IsNullOrEmpty(nombreCompleto))
                criteria.Add(Expression.Like("NombreCompleto", string.Format("%{0}%", nombreCompleto)));

            return criteria.List<Estudio>().ToList<Estudio>();
        }

        public IList<EstudioDTO> ListarEstudiosDTO(string codigo, string abreviado, string nombreCompleto)
        {
            ICriteria criteria = repositorio.NHSession.CreateCriteria(typeof(EstudioDTO));
           
            if (!string.IsNullOrEmpty(codigo))
                criteria.Add(Expression.Like("Codigo", string.Format("%{0}%", codigo)));

            if (!string.IsNullOrEmpty(abreviado))
                criteria.Add(Expression.Like("Descripcion", string.Format("%{0}%", abreviado)));

            if (!string.IsNullOrEmpty(nombreCompleto))
                criteria.Add(Expression.Like("NombreCompleto", string.Format("%{0}%", nombreCompleto)));

            return criteria.List<EstudioDTO>().ToList<EstudioDTO>();
        }

        public IList<EstudioDTO> ListarEstudiosDTO(int idEstado)
        {
            ICriteria criteria = repositorio.NHSession.CreateCriteria(typeof(EstudioDTO));

            if (idEstado > -1)
                criteria.Add(Expression.Eq("Estado.Id", idEstado));

            return criteria.List<EstudioDTO>().ToList<EstudioDTO>();
        }

        /// <summary>
        /// Obtiene el listado de las notas tratadas en un Estudio
        /// </summary>
        /// <param name="idActa"></param>
        /// <returns></returns>
        public IList<Nota> ObtenerEstudioNotas(int idEstudio)
        {
            ISession session = NHibernateSessionSingleton.GetSession();

            string sql = @"select *
                        from Notas 
                        where IdEstudio = :idestudio";

            IQuery query = session.CreateSQLQuery(sql)
                                   .AddEntity(typeof(Nota))
                                   .SetParameter("idestudio", idEstudio);

            return query.List<Nota>();
        }

        public IList<EstudioTipoDocumento> ObtenerTiposEstudioSemaforo(int idEstudio)
        {
            ICriteria criteria = repositorio.NHSession.CreateCriteria(typeof(EstudioTipoDocumento));

            criteria.Add(Expression.Eq("Estudio.Id", idEstudio));

            return criteria.List<EstudioTipoDocumento>().ToList<EstudioTipoDocumento>();
        }


        
    }
}
