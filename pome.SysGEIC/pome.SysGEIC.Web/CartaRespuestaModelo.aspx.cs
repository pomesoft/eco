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
    public partial class CartaRespuestaModelo : System.Web.UI.Page
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
            List<Plantilla> plantillas = servParametricas.PlantillaObtenerVigentes((int)TIPOS_PLANTILLA.CARTA_RESPUESTA);

            cboPlantillaIntroduccion.DataTextField = "Descripcion";
            cboPlantillaIntroduccion.DataValueField = "Id";
            cboPlantillaIntroduccion.DataSource = plantillas;
            cboPlantillaIntroduccion.DataBind();
            cboPlantillaIntroduccion.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboPlantillaIntroduccion2.DataTextField = "Descripcion";
            cboPlantillaIntroduccion2.DataValueField = "Id";
            cboPlantillaIntroduccion2.DataSource = plantillas;
            cboPlantillaIntroduccion2.DataBind();
            cboPlantillaIntroduccion2.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboPlantillaPiePagina.DataTextField = "Descripcion";
            cboPlantillaPiePagina.DataValueField = "Id";
            cboPlantillaPiePagina.DataSource = plantillas;
            cboPlantillaPiePagina.DataBind();
            cboPlantillaPiePagina.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboPlantillaTextoAprobacion.DataTextField = "Descripcion";
            cboPlantillaTextoAprobacion.DataValueField = "Id";
            cboPlantillaTextoAprobacion.DataSource = plantillas;
            cboPlantillaTextoAprobacion.DataBind();
            cboPlantillaTextoAprobacion.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboPlantillaBuenasPracticas.DataTextField = "Descripcion";
            cboPlantillaBuenasPracticas.DataValueField = "Id";
            cboPlantillaBuenasPracticas.DataSource = plantillas;
            cboPlantillaBuenasPracticas.DataBind();
            cboPlantillaBuenasPracticas.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboPlantillaTextoFirmaPresidente.DataTextField = "Descripcion";
            cboPlantillaTextoFirmaPresidente.DataValueField = "Id";
            cboPlantillaTextoFirmaPresidente.DataSource = plantillas;
            cboPlantillaTextoFirmaPresidente.DataBind();
            cboPlantillaTextoFirmaPresidente.Items.Insert(0, new ListItem(string.Empty, "-1"));
        }
    }
}