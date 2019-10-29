using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.Web.Helpers;
using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web
{
    public partial class Sitio : System.Web.UI.MasterPage
    {
        public bool? RecordatoriosActivos
        {
            get { return SessionHelper.ObtenerDeSession("RecordatoriosActivos") as bool?; }
            set { SessionHelper.GuardarEnSession("RecordatoriosActivos", value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //MenuSitio.Visible = false;
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    //MenuSitio.ObtenerMenu();
                    //MenuSitio.CargarMenu();
                    //MenuSitio.Visible = true;
                    lblUsuarioLogin.Text = SessionHelper.ObtenerUsuarioLogin().NombreCompleto;                    
                }
            }
            VerificarRecordatorios();
        }

        protected void btnCerrarSesión_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("BandejaInicio.aspx", true);
        }

        private void VerificarRecordatorios()
        {
            if (!this.RecordatoriosActivos.HasValue)
            {
                ServicioRecordatorios servicio = new ServicioRecordatorios();
                List<Recordatorio> recordatoriosActivos = servicio.ListarActivosPopup();
                this.RecordatoriosActivos = (recordatoriosActivos.Count > 0);
            }

            if (this.RecordatoriosActivos.Value)
                imgIconoAlertaProncipal.ImageUrl = "img/alertRojo.png";
            else
                imgIconoAlertaProncipal.ImageUrl = "img/alertVerde.png";
        }
    }
}