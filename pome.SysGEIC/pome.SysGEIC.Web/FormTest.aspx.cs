using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace pome.SysGEIC.Web
{
    public partial class FormTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTestTexto();
            }
        }

        private void CargarTestTexto()
        {
            Literal1.Text = string.Empty;
            string text = "123456789 ";

            for (int i = 0; i < 400; i++)
                Literal1.Text += text;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.DataItemIndex, true);
                e.Row.Attributes["onmouseover"] = "this.className='datagrid-body datagrid-row datagrid-row-selected';";
                e.Row.Attributes["onmouseout"] = "this.className='datagrid-body datagrid-row';";
            }

        }
    }
}