using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;
using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.Controles
{
    public partial class MenuControl : System.Web.UI.UserControl
    {

        public List<MenuPrincipal> MenuItems
        {
            get { return SessionHelper.ObtenerDeSession("ItemsMenu") as List<MenuPrincipal>; }
            set { SessionHelper.GuardarEnSession("ItemsMenu", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            //CargarMenu();
        }

        public void ObtenerMenu()
        {
            if (this.MenuItems == null)
            {
                ServicioAccesoUsuarios servicio = new ServicioAccesoUsuarios();
                this.MenuItems = servicio.ObtenerMenuPrincipal();
            }
        }

        public void CargarMenu()
        {
            StringBuilder itemsHTML = new StringBuilder();

            itemsHTML.Append(@"<div style=""padding: 3px; width: 99%; border: 1px solid #ccc"">");
            this.MenuItems.ForEach(delegate(MenuPrincipal menuPrincipal)
            {
                if (menuPrincipal.Items == null || menuPrincipal.Items.Count == 0)
                    itemsHTML.Append(string.Format(@"<a href=""{0}"" class=""easyui-linkbutton"" plain=""true"">{1}</a>", menuPrincipal.NavigateURL.Trim(), menuPrincipal.Texto.Trim()));
                else
                    itemsHTML.Append(string.Format(@"<a href=""{0}"" class=""easyui-menubutton"" menu=""#submenu{2}"">{1}</a>", menuPrincipal.NavigateURL.Trim(), menuPrincipal.Texto.Trim(), menuPrincipal.Id.ToString()));
            });
            itemsHTML.Append("</div>");
                                    
            this.MenuItems.ForEach(delegate(MenuPrincipal menuPrincipal)
            {
                List<MenuSecundario> items = menuPrincipal.Items.ToList<MenuSecundario>()
                                                               .OrderBy(item => item.Orden)
                                                               .ToList<MenuSecundario>();
                if (items != null && items.Count > 0)
                {
                    itemsHTML.Append(string.Format(@"<div id=""submenu{0}"" style=""width: 200px;"">", menuPrincipal.Id.ToString()));
                    items.ForEach(delegate(MenuSecundario item)
                    {
                        itemsHTML.Append(string.Format(@"<div style=""padding: 3px;"" data-options=""href:'{0}'"">{1}</div>", item.NavigateURL.Trim(), item.Texto.Trim()));
                    });
                    itemsHTML.Append(@"</div>");
                }
            });

            litMenu.Text = itemsHTML.ToString();
        }
    }
}