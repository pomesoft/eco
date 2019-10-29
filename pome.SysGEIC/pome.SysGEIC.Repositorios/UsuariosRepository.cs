using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Repositorios
{
    public class UsuariosRepository 
    {
        private NHibernateRepository<Usuario> repositorio = new NHibernateRepository<Usuario>();
        
        public UsuariosRepository() { }

        public void ActualizarUsuario(Usuario usuario)
        {
            usuario.Vigente = true;
            repositorio.Actualizar(usuario);
        }
        public void EliminarUsuario(Usuario usuario)
        {
            usuario.Vigente = false;
            this.ActualizarUsuario(usuario);
        }
        public Usuario ObtenerUsuario(int id)
        {
            return repositorio.Obtener(id);
        }
        public Usuario ObtenerUsuario(string loginUsuario, string loginClave)
        {
            ICriterion expresion = Expression.And(Expression.Eq("LoginUsuario", loginUsuario),
                                                  Expression.Eq("LoginClave", loginClave));
            return repositorio.Obtener(expresion);
        }
        public List<Usuario> ObtenerUsuarios()
        {
            ICriterion expresion = Expression.Eq("Vigente", true);
            return repositorio.ObtenerTodos(expresion).ToList<Usuario>();
        }
        public List<Usuario> ObtenerUsuarios(Usuario usuarioBuscar)
        {

            ICriteria criteria = repositorio.NHSession.CreateCriteria(typeof(Usuario));

            criteria.Add(Expression.Eq("Vigente", true));

            if (usuarioBuscar.Apellido.Length > 0)
                criteria.Add(Expression.Like("Apellido", string.Format("%{0}%", usuarioBuscar.Apellido)));
            
            if (usuarioBuscar.Nombre.Length > 0)
                criteria.Add(Expression.Like("Nombre", string.Format("%{0}%", usuarioBuscar.Nombre)));

            return criteria.List<Usuario>().ToList<Usuario>();
        }
    }
}
