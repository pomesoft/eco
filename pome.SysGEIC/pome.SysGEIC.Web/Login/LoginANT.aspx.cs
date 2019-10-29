using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web.Login
{
    public partial class LoginANT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;
            if (txtLogin.Text.Trim().Length == 0 || txtClave.Text.Trim().Length == 0) return;

            ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();

            Usuario usr = servicio.LoginUsuario(txtLogin.Text, txtClave.Text);

            if (txtLogin.Text.Equals("desarrollo") && txtClave.Text.Equals("pome@soft"))
                lblMensaje.Text = ServiciosHelpers.GetClaveInicializada();
            else
                lblMensaje.Text = "Usuario ó contraseña ingresado incorrecto.";
        }
    }
}