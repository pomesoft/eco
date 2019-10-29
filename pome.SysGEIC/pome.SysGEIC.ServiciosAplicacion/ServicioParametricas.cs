using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using pome.SysGEIC.Comunes;
using pome.SysGEIC.Entidades;
using pome.SysGEIC.Repositorios;


namespace pome.SysGEIC.ServiciosAplicacion
{
    public class ServicioParametricas
    {
        public ServicioParametricas() { }

        public string BaseDatos
        {
            get 
            {
                RepositoryGenerico<TipoUsuario> repository = new RepositoryGenerico<TipoUsuario>();
                return repository.BaseDatos;
            }
        }

        public string ServidorBaseDatos
        {
            get
            {
                RepositoryGenerico<TipoUsuario> repository = new RepositoryGenerico<TipoUsuario>();
                return repository.ServidorBaseDatos;
            }
        }

        public T ObtenerObjeto<T>(int id) where T : EntidadBase
        {
            RepositoryGenerico<T> repository = new RepositoryGenerico<T>();
            return repository.Obtener(id);
        }

        public T ObtenerObjeto<T>(string nombreCampo, string valorBuscado) where T : EntidadBaseParametrica
        {
            RepositoryGenerico<T> repository = new RepositoryGenerico<T>();
            return repository.Obtener(nombreCampo, valorBuscado);
        }

        public string ParametroObtener(string descripcion)
        {
            Parametro parametro = this.ObtenerObjeto<Parametro>("Descripcion", descripcion);
            return (parametro != null) ? parametro.Valor : string.Empty;
        }

        #region EstadoDocumento
        public List<EstadoDocumento> EstadosDocumentosObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<EstadoDocumento> repository = new RepositoryGenerico<EstadoDocumento>();
            //return repository.ObtenerTodos().Where<EstadoDocumento>(item => item.Vigente)
            //                                .ToList<EstadoDocumento>();

            return repository.ObtenerTodosVigentes(descripcion).ToList<EstadoDocumento>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<EstadoDocumento>();
        }
        public EstadoDocumento EstadoDocumentoObtener(int id)
        {
            RepositoryGenerico<EstadoDocumento> repository = new RepositoryGenerico<EstadoDocumento>();
            return repository.Obtener(id);
        }
        public void EstadoDocumentoGrabar(string id, string descripcion)
        {
            EstadoDocumento estadoDocumento;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                estadoDocumento = new EstadoDocumento();
            else
                estadoDocumento = this.EstadoDocumentoObtener(_id);

            estadoDocumento.Descripcion = descripcion == null ? string.Empty : descripcion;
            estadoDocumento.Vigente = true;

            RepositoryGenerico<EstadoDocumento> repository = new RepositoryGenerico<EstadoDocumento>();
            
            repository.Actualizar(estadoDocumento);
        }
        public void EstadoDocumentoEliminar(string id)
        {
            EstadoDocumento estadoDocumento;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe estado que desea eliminar.");

            estadoDocumento = this.EstadoDocumentoObtener(_id);
            if (estadoDocumento == null)
                throw new ApplicationException("No existe estado que desea eliminar.");

            estadoDocumento.Vigente = false;

            RepositoryGenerico<EstadoDocumento> repository = new RepositoryGenerico<EstadoDocumento>();            
            repository.Actualizar(estadoDocumento);
        }
        #endregion
               
        #region EstadoEstudio
        public List<EstadoEstudio> EstadoEstudioObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<EstadoEstudio> repository = new RepositoryGenerico<EstadoEstudio>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<EstadoEstudio>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<EstadoEstudio>();
        }
        public EstadoEstudio EstadoEstudioObtener(int id)
        {
            RepositoryGenerico<EstadoEstudio> repository = new RepositoryGenerico<EstadoEstudio>();
            return repository.Obtener(id);
        }
        public EstadoEstudio EstadoEstudioObtener(string id)
        {
            return EstadoEstudioObtener(id.ConvertirInt());
        }
        public void EstadoEstudioGrabar(string id, string descripcion, string final)
        {
            EstadoEstudio estadoEstudio;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;
            
            if (_id == -1)
                estadoEstudio = new EstadoEstudio();
            else
                estadoEstudio = this.EstadoEstudioObtener(_id);

            estadoEstudio.Descripcion = descripcion == null ? string.Empty : descripcion;
            estadoEstudio.Vigente = true;
            estadoEstudio.Final = final == null ? false : bool.Parse(final);
            estadoEstudio.Validar();

            RepositoryGenerico<EstadoEstudio> repository = new RepositoryGenerico<EstadoEstudio>();
            repository.Actualizar(estadoEstudio);

        }
        public void EstadoEstudioEliminar(string id)
        {
            EstadoEstudio estadoEstudio;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe estado que desea eliminar.");

            estadoEstudio = this.EstadoEstudioObtener(_id);
            if (estadoEstudio == null)
                throw new ApplicationException("No existe estado que desea eliminar.");

            estadoEstudio.Vigente = false;

            RepositoryGenerico<EstadoEstudio> repository = new RepositoryGenerico<EstadoEstudio>();
            repository.Actualizar(estadoEstudio);
        }
        #endregion

        #region TipoEstudio
        public List<TipoEstudio> TipoEstudioObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<TipoEstudio> repository = new RepositoryGenerico<TipoEstudio>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<TipoEstudio>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<TipoEstudio>();
        }
        public TipoEstudio TipoEstudioObtener(int id)
        {
            RepositoryGenerico<TipoEstudio> repository = new RepositoryGenerico<TipoEstudio>();
            return repository.Obtener(id);
        }
        public TipoEstudio TipoEstudioObtener(string id)
        {
            return TipoEstudioObtener(id.ConvertirInt());
        }
        public void TipoEstudioGrabar(string id, string descripcion, string datos)
        {
            // faltan los textos para carta de respuesta
            TipoEstudio estadoEstudio;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                estadoEstudio = new TipoEstudio();
            else
                estadoEstudio = this.TipoEstudioObtener(_id);

            estadoEstudio.Descripcion = descripcion == null ? string.Empty : descripcion;
            estadoEstudio.Vigente = true;
            estadoEstudio.Validar();

            RepositoryGenerico<TipoEstudio> repository = new RepositoryGenerico<TipoEstudio>();
            repository.Actualizar(estadoEstudio);

        }
        public void TipoEstudioEliminar(string id)
        {
            TipoEstudio estadoEstudio;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe estado que desea eliminar.");

            estadoEstudio = this.TipoEstudioObtener(_id);
            if (estadoEstudio == null)
                throw new ApplicationException("No existe estado que desea eliminar.");

            estadoEstudio.Vigente = false;

            RepositoryGenerico<TipoEstudio> repository = new RepositoryGenerico<TipoEstudio>();
            repository.Actualizar(estadoEstudio);
        }
        #endregion

        #region Centro
        public List<Centro> CentroObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<Centro> repository = new RepositoryGenerico<Centro>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<Centro>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<Centro>();
        }
        public List<Centro> CentroObtenerCentrosInternacion(int idCentro)
        {
            RepositoryGenerico<Centro> repository = new RepositoryGenerico<Centro>();
            return repository.ObtenerTodos("IdCentroVinculado", idCentro).ToList<Centro>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<Centro>();
        }

        public Centro CentroObtener(int id)
        {
            RepositoryGenerico<Centro> repository = new RepositoryGenerico<Centro>();
            return repository.Obtener(id);
        }
        public void CentroGrabar(string id, string descripcion)
        {
            Centro centro;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                centro = new Centro();
            else
                centro = this.CentroObtener(_id);

            centro.Descripcion = descripcion == null ? string.Empty : descripcion;
            centro.Vigente = true;
            centro.Validar();

            RepositoryGenerico<Centro> repository = new RepositoryGenerico<Centro>();
            repository.Actualizar(centro);

        }
        public void CentroEliminar(string id)
        {
            Centro centro;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe centro que desea eliminar.");

            centro = this.CentroObtener(_id);
            if (centro == null)
                throw new ApplicationException("No existe centro que desea eliminar.");

            centro.Vigente = false;

            RepositoryGenerico<Centro> repository = new RepositoryGenerico<Centro>();
            repository.Actualizar(centro);
        }
        #endregion

        #region Patrocinador
        public List<Patrocinador> PatrocinadorObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<Patrocinador> repository = new RepositoryGenerico<Patrocinador>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<Patrocinador>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<Patrocinador>();
        }
        public List<Patrocinador> PatrocinadorObtenerListaVacia()
        {
            List<Patrocinador> patrocinadores = new List<Patrocinador>();
            patrocinadores.Add(new Patrocinador());
            return patrocinadores;
        }
        public Patrocinador PatrocinadorObtener(int id)
        {
            RepositoryGenerico<Patrocinador> repository = new RepositoryGenerico<Patrocinador>();
            return repository.Obtener(id);
        }
        public void PatrocinadorGrabar(string id, string descripcion)
        {
            Patrocinador patrocinador;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                patrocinador = new Patrocinador();
            else
                patrocinador = this.PatrocinadorObtener(_id);

            patrocinador.Descripcion = descripcion == null ? string.Empty : descripcion;
            patrocinador.Vigente = true;
            patrocinador.Validar();

            RepositoryGenerico<Patrocinador> repository = new RepositoryGenerico<Patrocinador>();
            repository.Actualizar(patrocinador);

        }
        public void PatrocinadorEliminar(string id)
        {
            Patrocinador patrocinador;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe patrocinador que desea eliminar.");

            patrocinador = this.PatrocinadorObtener(_id);
            if (patrocinador == null)
                throw new ApplicationException("No existe patrocinador que desea eliminar.");

            patrocinador.Vigente = false;

            RepositoryGenerico<Patrocinador> repository = new RepositoryGenerico<Patrocinador>();
            repository.Actualizar(patrocinador);
        }
        #endregion

        #region Patologia
        public List<Patologia> PatologiaObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<Patologia> repository = new RepositoryGenerico<Patologia>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<Patologia>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<Patologia>();
        }
        public Patologia PatologiaObtener(int id)
        {
            RepositoryGenerico<Patologia> repository = new RepositoryGenerico<Patologia>();
            return repository.Obtener(id);
        }
        public void PatologiaGrabar(string id, string descripcion)
        {
            Patologia patologia;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                _id = -1;

            if (_id == -1)
                patologia = new Patologia();
            else
                patologia = this.PatologiaObtener(_id);

            patologia.Descripcion = descripcion == null ? string.Empty : descripcion;
            patologia.Vigente = true;
            patologia.Validar();

            RepositoryGenerico<Patologia> repository = new RepositoryGenerico<Patologia>();
            repository.Actualizar(patologia);

        }
        public void PatologiaEliminar(string id)
        {
            Patologia patologia;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe patología que desea eliminar.");

            patologia = this.PatologiaObtener(_id);
            if (patologia == null)
                throw new ApplicationException("No existe patología que desea eliminar.");

            patologia.Vigente = false;

            RepositoryGenerico<Patologia> repository = new RepositoryGenerico<Patologia>();
            repository.Actualizar(patologia);
        }
        #endregion

        #region Plantilla
        public List<Plantilla> PlantillaObtenerVigentes(string descripcion)
        {
            RepositoryGenerico<Plantilla> repository = new RepositoryGenerico<Plantilla>();
            return repository.ObtenerTodosVigentes(descripcion).ToList<Plantilla>()
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<Plantilla>();
        }
        public List<Plantilla> PlantillaObtenerVigentes(int idTipoPlantilla)
        {
            RepositoryGenerico<Plantilla> repository = new RepositoryGenerico<Plantilla>();
            return repository.ObtenerTodosVigentes(string.Empty).ToList<Plantilla>()
                                                            .Where(item => item.TipoPlantilla.Id == idTipoPlantilla)
                                                            .OrderBy(item => item.Descripcion)
                                                            .ToList<Plantilla>();
        }

        public List<Plantilla> PlantillaObtenerVigentes(string descripcion, string idTipoPlantilla)
        {
            RepositoryGenerico<Plantilla> repository = new RepositoryGenerico<Plantilla>();

            return repository.ObtenerTodosVigentes(descripcion).ToList<Plantilla>()
                                                .Where(item => item.TipoPlantilla.Id == idTipoPlantilla.ConvertirInt() 
                                                            || idTipoPlantilla.ConvertirInt() == -1)
                                                .OrderBy(item => item.Descripcion)
                                                .ToList<Plantilla>();
        }
        

        public Plantilla PlantillaObtener(int id)
        {
            RepositoryGenerico<Plantilla> repository = new RepositoryGenerico<Plantilla>();
            return repository.Obtener(id);
        }
        public Plantilla PlantillaObtener(string descripcion)
        {
            RepositoryGenerico<Plantilla> repository = new RepositoryGenerico<Plantilla>();
            return repository.Obtener("Descripcion", descripcion);
        }
        public string PlantillaObtenerTexto(string descripcion)
        {
            RepositoryGenerico<Plantilla> repository = new RepositoryGenerico<Plantilla>();
            Plantilla plantilla = repository.Obtener("Descripcion", descripcion);
            return (plantilla != null) ? plantilla.Texto : string.Empty;
        }
        public void PlantillaGrabar(string id, string descripcion, string texto, string idTipo)
        {
            Plantilla plantilla;

            int _id = id.ConvertirInt();

            if (_id == -1)
                plantilla = new Plantilla();
            else
                plantilla = this.PlantillaObtener(_id);
            
            plantilla.Descripcion = descripcion == null ? string.Empty : descripcion;
            plantilla.Texto = texto;
            plantilla.Vigente = true;
            plantilla.TipoPlantilla = idTipo == null ? null : ObtenerObjeto<TipoPlantilla>(idTipo.ConvertirInt());

            plantilla.Validar();

            RepositoryGenerico<Plantilla> repository = new RepositoryGenerico<Plantilla>();
            repository.Actualizar(plantilla);

        }
        public void PlantillaEliminar(string id)
        {
            Plantilla plantilla;

            int _id = -1;
            if (!int.TryParse(id, out _id))
                throw new ApplicationException("No existe plantilla que desea eliminar.");

            plantilla = this.PlantillaObtener(_id);
            if (plantilla == null)
                throw new ApplicationException("No existe plantilla que desea eliminar.");

            plantilla.Vigente = false;

            RepositoryGenerico<Plantilla> repository = new RepositoryGenerico<Plantilla>();
            repository.Actualizar(plantilla);
        }
       
        public List<RolComite> RolComiteObtenerVigentes()
        {
            RepositoryGenerico<RolComite> repository = new RepositoryGenerico<RolComite>();
            return repository.ObtenerTodosVigentes().ToList<RolComite>()
                                                    .OrderBy(item => item.Descripcion)
                                                    .ToList<RolComite>();
        }

        public void PlantillaProcesarTextosPlanos()
        {
            List<Plantilla> listPlantillas = this.PlantillaObtenerVigentes(string.Empty);

            listPlantillas.ForEach(delegate(Plantilla plantilla) 
            {
                this.PlantillaGrabar(plantilla.Id.ToString(), 
                                    plantilla.Descripcion, 
                                    ServiciosHelpers.ObtenerTextoPlano(plantilla.Texto), 
                                    plantilla.TipoPlantilla.Id.ToString());
            });

        }
        #endregion 
    }
}
