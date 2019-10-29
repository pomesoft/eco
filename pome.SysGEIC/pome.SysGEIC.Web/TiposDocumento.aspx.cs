using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web
{
    public partial class TiposDocumento : System.Web.UI.Page
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
            ServicioParametricas servParametricas = new ServicioParametricas();

            List<EstadoDocumento> listaEstados = servParametricas.EstadosDocumentosObtenerVigentes(string.Empty);

            cboEstado.DataTextField = "Descripcion";
            cboEstado.DataValueField = "Id";
            cboEstado.DataSource = listaEstados;
            cboEstado.DataBind();
            cboEstado.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboEstadoPadre.DataTextField = "Descripcion";
            cboEstadoPadre.DataValueField = "Id";
            cboEstadoPadre.DataSource = listaEstados;
            cboEstadoPadre.DataBind();
            cboEstadoPadre.Items.Insert(0, new ListItem(string.Empty, "-1"));
        }
    }
}