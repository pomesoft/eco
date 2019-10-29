using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web
{
    public partial class Equipos : System.Web.UI.Page
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
            ServicioEquipos servicio = new ServicioEquipos();

            cboProfesional.DataTextField = "NombreCompleto";
            cboProfesional.DataValueField = "Id";
            cboProfesional.DataSource = servicio.ProfesionalObtenerVigentes(string.Empty);
            cboProfesional.DataBind();
            cboProfesional.Items.Insert(0, new ListItem(string.Empty, string.Empty));

            cboRol.DataTextField = "Descripcion";
            cboRol.DataValueField = "Id";
            cboRol.DataSource = servicio.RolObtenerVigentes();
            cboRol.DataBind();
            cboRol.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        }

       
    }
}