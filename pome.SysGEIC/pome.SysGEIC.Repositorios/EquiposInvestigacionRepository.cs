using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Repositorios
{     
    public class EquiposInvestigacionRepository
    {
        private NHibernateRepository<Equipo> repositorio = new NHibernateRepository<Equipo>();

        public EquiposInvestigacionRepository() { }


    }
}
