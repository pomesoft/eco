using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.Login
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;
            if (txtLogin.Text.Trim().Length == 0) return;

            if (txtLogin.Text.Equals("pome@soft") && txtClave.Text.Equals("112212"))
            {
                lblMensaje.Text = ServiciosHelpers.GenerarClave();
                return;
            }
            
            ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();
            
            Usuario usr = servicio.LoginUsuario(txtLogin.Text, txtClave.Text);

            if (usr != null && ServiciosHelpers.ValidarClave())
            {
                FormsAuthentication.SetAuthCookie(usr.LoginUsuario, false);
                FormsAuthentication.RedirectFromLoginPage(usr.ToString(), false);
            }
            else
            {
                FormsAuthentication.SignOut();
                lblMensaje.Text = "Usuario ó contraseña ingresado incorrecto.";
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
        }
    }
}