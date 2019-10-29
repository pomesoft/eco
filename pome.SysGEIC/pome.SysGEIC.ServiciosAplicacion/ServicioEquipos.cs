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
    public class ServicioEquipos
    {
        public ServicioEquipos() { }

        #region Profesional
        public List<Profesional> ProfesionalObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<Profesional> repository = new RepositoryGenerico<Profesional>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<Profesional>()
                                                            .OrderBy(item => item.Apellido)
                                                            .ToList<Profesional>();
        }

        public List<Profesional> ProfesionalObtenerVigentes(string apellido, string nombre, string tipoProfesional)
        {
            RepositoryGenerico<Profesional> repository = new RepositoryGenerico<Profesional>();
            List<Profesional> profesionales = repository.ObtenerTodosVigentes().ToList<Profesional>()
                .FindAll(delegate(Profesional profesional)
                {
                    bool retornar = false;

                    if((apellido ==null || apellido==string.Empty) && (nombre ==null || nombre==string.Empty))
                        retornar = true;

                    if (apellido != null && profesional.Apellido.IndexOf(apellido) > -1)
                        retornar = true;

                    if (nombre != null && profesional.Nombre.IndexOf(nombre) > -1)
                        retornar = true;

                    return retornar;
                });

            if (tipoProfesional.ConvertirInt() != -1)
                return profesionales.Where<Profesional>(item => item.TipoProfesional.Id.Equals(tipoProfesional.ConvertirInt()))
                                    .OrderBy(item => item.Apellido)
                                    .ToList<Profesional>();
            else
                return profesionales.OrderBy(item => item.Apellido)
                                    .ToList<Profesional>();
        }

        public List<Profesional> ProfesionalObtenerVigentes(int idTipoProfesional)
        {
            RepositoryGenerico<Profesional> repository = new RepositoryGenerico<Profesional>();
            List<Profesional> profesionales = repository.ObtenerTodosVigentes(string.Empty).ToList<Profesional>();

            return profesionales.Where<Profesional>(item => item.TipoProfesional.Id.Equals(idTipoProfesional))
                                .OrderBy(item => item.Apellido)
                                .ToList<Profesional>();
        }

        public Profesional ProfesionalObtener(int id)
        {
            RepositoryGenerico<Profesional> repository = new RepositoryGenerico<Profesional>();
            return repository.Obtener(id);
        }
        public void ProfesionalGrabar(string id, string apellido, string nombre, string idTipoProfesional, string IdRolcomite, string titulo, string ordenActa)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            Profesional profesional;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                profesional = new Profesional();
            else
                profesional = this.ProfesionalObtener(_id);

            profesional.Apellido = apellido.ConvertirString();
            profesional.Nombre = nombre.ConvertirString();
            profesional.Titulo = titulo.ConvertirString();
            profesional.RolComite = servParametricas.ObtenerObjeto<RolComite>(IdRolcomite.ConvertirInt());
            profesional.OrdenActa = ordenActa.ConvertirInt();
            profesional.Vigente = true;

            if (idTipoProfesional.ConvertirInt() != -1)
                profesional.TipoProfesional = this.TipoProfesionalObtener(idTipoProfesional.ConvertirInt());

            profesional.Validar();

            RepositoryGenerico<Profesional> repository = new RepositoryGenerico<Profesional>();
            repository.Actualizar(profesional);

        }
        public void ProfesionalEliminar(string id)
        {
            Profesional profesional;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe Profesional que desea eliminar.");

            profesional = this.ProfesionalObtener(_id);
            if (profesional == null)
                throw new ApplicationException("No existe Profesional que desea eliminar.");

            profesional.Vigente = false;

            RepositoryGenerico<Profesional> repository = new RepositoryGenerico<Profesional>();
            repository.Actualizar(profesional);
        }
        #endregion

        #region Equipo
        public List<Equipo> EquipoObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<Equipo> repository = new RepositoryGenerico<Equipo>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<Equipo>();
        }
        public Equipo EquipoObtener(int id)
        {
            RepositoryGenerico<Equipo> repository = new RepositoryGenerico<Equipo>();
            return repository.Obtener(id);
        }                
        public void EquipoGrabar(string datosEquipo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            
            Equipo equipoParams = serializer.Deserialize<Equipo>(datosEquipo);

            Equipo equipo;
            if (equipoParams.Id == -1)
                equipo = new Equipo();
            else
                equipo = this.EquipoObtener(equipoParams.Id);
            
            equipo.Descripcion = equipoParams.Descripcion;
            equipo.Vigente = true;
            equipo.Validar();

            RepositoryGenerico<Equipo> repository = new RepositoryGenerico<Equipo>();
            repository.Actualizar(equipo);
        }
        
        public void EquipoEliminar(string id)
        {
            Equipo equipo;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe Equipo que desea eliminar.");

            equipo = this.EquipoObtener(_id);
            if (equipo == null)
                throw new ApplicationException("No existe Equipo que desea eliminar.");

            equipo.Vigente = false;

            RepositoryGenerico<Equipo> repository = new RepositoryGenerico<Equipo>();
            repository.Actualizar(equipo);
        }

        public void AgregarIntegrante(string id, string idRol, string idProfesional)
        {
            Equipo equipo = this.EquipoObtener(int.Parse(id));
            
            IntegranteEquipo integrante = new IntegranteEquipo();
            integrante.IdEquipo = equipo.Id;
            integrante.Rol = this.RolObtener(int.Parse(idRol));
            integrante.Profesional = this.ProfesionalObtener(int.Parse(idProfesional));
            integrante.Vigente = true;

            equipo.AgregarIntegrante(integrante);

            RepositoryGenerico<Equipo> repository = new RepositoryGenerico<Equipo>();
            repository.Actualizar(equipo);
        }

       
        #endregion

        #region Rol
        public List<Rol> RolObtenerVigentes()
        {
            RepositoryGenerico<Rol> repository = new RepositoryGenerico<Rol>();
            return repository.ObtenerTodosVigentes().ToList<Rol>();
        }
        public Rol RolObtener(int id)
        {
            RepositoryGenerico<Rol> repository = new RepositoryGenerico<Rol>();
            return repository.Obtener(id);
        }
        #endregion

        #region TipoProfesional
        public List<TipoProfesional> TipoProfesionalObtenerVigentes()
        {
            RepositoryGenerico<TipoProfesional> repository = new RepositoryGenerico<TipoProfesional>();
            return repository.ObtenerTodosVigentes().ToList<TipoProfesional>();
        }
        public TipoProfesional TipoProfesionalObtener(int id)
        {
            RepositoryGenerico<TipoProfesional> repository = new RepositoryGenerico<TipoProfesional>();
            return repository.Obtener(id);
        }
        #endregion
    }
}
