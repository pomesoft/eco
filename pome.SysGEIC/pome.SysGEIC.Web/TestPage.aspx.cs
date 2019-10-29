using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace pome.SysGEIC.Web
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerarWord_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("Impresion.aspx?Plantilla={0}&ExportTo=2", txtPlantillaImprimir.Text));
        }

        protected void btnGenerarWord2_Click(object sender, EventArgs e)
        {
            Response.Redirect(txtHandlerEjecuta.Text);
        }
    }
}