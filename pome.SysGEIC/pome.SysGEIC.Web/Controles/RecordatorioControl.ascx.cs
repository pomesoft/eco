using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web.Controles
{
    public partial class RecordatorioControl : System.Web.UI.UserControl
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
            ServicioRecordatorios servRecordatorios = new ServicioRecordatorios();

            cboTipoRecordatorio.DataTextField = "Descripcion";
            cboTipoRecordatorio.DataValueField = "Id";
            cboTipoRecordatorio.DataSource = servRecordatorios.TipoRecordatoriosObtenerVigentes(string.Empty);
            cboTipoRecordatorio.DataBind();
            cboTipoRecordatorio.SelectedValue = "";

            cboInfoMailListas.DataTextField = "Descripcion";
            cboInfoMailListas.DataValueField = "Id";
            cboInfoMailListas.DataSource = servRecordatorios.ListaEmailsObtenerVigentes();
            cboInfoMailListas.DataBind();

            ServicioEstudios servicio = new ServicioEstudios();

            cboEstudio.DataTextField = "NombreEstudioListados";
            cboEstudio.DataValueField = "Id";
            cboEstudio.DataSource = servicio.ObtenerVigentes();
            cboEstudio.DataBind();
            
        }
    }
}