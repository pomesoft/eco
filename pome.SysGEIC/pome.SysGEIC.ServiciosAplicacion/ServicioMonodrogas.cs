using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Repositorios;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public class ServicioMonodrogas
    {

        public ServicioMonodrogas() { }

        #region Monodroga
        public List<Monodroga> MonodrogaObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<Monodroga> repository = new RepositoryGenerico<Monodroga>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<Monodroga>();
        }
        public Monodroga MonodrogaObtener(int id)
        {
            RepositoryGenerico<Monodroga> repository = new RepositoryGenerico<Monodroga>();
            return repository.Obtener(id);
        }
        public void MonodrogaGrabar(string id, string descripcion)
        {
            Monodroga Monodroga;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                Monodroga = new Monodroga();
            else
                Monodroga = this.MonodrogaObtener(_id);

            Monodroga.Descripcion = descripcion == null ? string.Empty : descripcion;
            Monodroga.Vigente = true;
            Monodroga.Validar();

            RepositoryGenerico<Monodroga> repository = new RepositoryGenerico<Monodroga>();
            repository.Actualizar(Monodroga);

        }
        public void MonodrogaEliminar(string id)
        {
            Monodroga Monodroga;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe monodroga que desea eliminar.");

            Monodroga = this.MonodrogaObtener(_id);
            if (Monodroga == null)
                throw new ApplicationException("No existe monodroga que desea eliminar.");

            Monodroga.Vigente = false;

            RepositoryGenerico<Monodroga> repository = new RepositoryGenerico<Monodroga>();
            repository.Actualizar(Monodroga);
        }
        #endregion

    }
}
