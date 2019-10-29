using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace pome.SysGEIC.Repositorios
{
    internal interface IRepository<T>
    {

        T Obtener(object id);
        T Obtener(ICriterion expresion);
        void Actualizar(T value);
        void Eliminar(T value);
        IList<T> ObtenerTodos();
        IList<T> ObtenerTodos(ICriterion expresion);
    }
}
