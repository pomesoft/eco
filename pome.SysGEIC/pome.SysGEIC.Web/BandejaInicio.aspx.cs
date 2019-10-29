using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web
{
    public partial class BandejaInicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string accion = this.Request["accion"];
                bool mostrar=false;
                if (accion != null && accion.Equals("MANTENIMIENTO"))
                    mostrar = true;
                btnEstudioEliminar.Visible = mostrar;
                btnDocumentoEliminar.Visible = mostrar;
            }
        }
    }
}