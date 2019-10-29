using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web
{
    public partial class EstudioCargaDatos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCombos();
            }
        }

        private void CargarCombos()
        {
            ServicioEquipos servEquipo = new ServicioEquipos();
            ServicioParametricas servParametricas = new ServicioParametricas();
            ServicioMonodrogas servMonodrogs = new ServicioMonodrogas();

            cboPatologia.DataTextField = "Descripcion";
            cboPatologia.DataValueField = "Id";
            cboPatologia.DataSource = servParametricas.PatologiaObtenerVigentes(string.Empty);
            cboPatologia.DataBind();
            cboPatologia.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboTipoEstudio.DataTextField = "Descripcion";
            cboTipoEstudio.DataValueField = "Id";
            cboTipoEstudio.DataSource = servParametricas.TipoEstudioObtenerVigentes(string.Empty);
            cboTipoEstudio.DataBind();
            cboTipoEstudio.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboEstado.DataTextField = "Descripcion";
            cboEstado.DataValueField = "Id";
            cboEstado.DataSource = servParametricas.EstadoEstudioObtenerVigentes(string.Empty);
            cboEstado.DataBind();
            cboEstado.Items.Insert(0, new ListItem(string.Empty, "-1"));

            //cboEquipo.DataTextField = "Descripcion";
            //cboEquipo.DataValueField = "Id";
            //cboEquipo.DataSource = servEquipo.EquipoObtenerVigentes(string.Empty);
            //cboEquipo.DataBind();
            //cboEquipo.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboParticipanteProfesional.DataTextField = "NombreCompleto";
            cboParticipanteProfesional.DataValueField = "Id";
            cboParticipanteProfesional.DataSource = servEquipo.ProfesionalObtenerVigentes(Constantes.TipoProfesional_Investigador);
            cboParticipanteProfesional.DataBind();
            cboParticipanteProfesional.Items.Insert(0, new ListItem(string.Empty, "-1"));
            
            cboParticipanteRol.DataTextField = "Descripcion";
            cboParticipanteRol.DataValueField = "Id";
            cboParticipanteRol.DataSource = servEquipo.RolObtenerVigentes();
            cboParticipanteRol.DataBind();
            cboParticipanteRol.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboCentroHabilitado.DataTextField = "Descripcion";
            cboCentroHabilitado.DataValueField = "Id";
            cboCentroHabilitado.DataSource = servParametricas.CentroObtenerVigentes(string.Empty);
            cboCentroHabilitado.DataBind();
            cboCentroHabilitado.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboPatrocinador.DataTextField = "Descripcion";
            cboPatrocinador.DataValueField = "Id";
            cboPatrocinador.DataSource = servParametricas.PatrocinadorObtenerVigentes(string.Empty);
            cboPatrocinador.DataBind();
            cboPatrocinador.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboMonodroga.DataTextField = "Descripcion";
            cboMonodroga.DataValueField = "Id";
            cboMonodroga.DataSource = servMonodrogs.MonodrogaObtenerVigentes(string.Empty);
            cboMonodroga.DataBind();
            cboMonodroga.Items.Insert(0, new ListItem(string.Empty, "-1"));
        }
    }
}