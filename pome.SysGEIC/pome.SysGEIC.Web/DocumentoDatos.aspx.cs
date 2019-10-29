using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;
using pome.SysGEIC.Comunes;

using pome.SysGEIC.Web.Helpers;

using Subgurim.Controles;

namespace pome.SysGEIC.Web
{
    public partial class DocumentoDatos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCombos();
                ConfigurarFileUploader();
            }
            if (FileUploader.IsPosting)
                AdministrarPostFileUploader();
            
        }

        private void CargarCombos()
        {
            ServicioParametricas servParametricas = new ServicioParametricas();
            ServicioDocumentos servDocumentos = new ServicioDocumentos();
            
            cboTipoDocumento.DataTextField = "Descripcion";
            cboTipoDocumento.DataValueField = "Id";
            cboTipoDocumento.DataSource = servDocumentos.TiposDocumentoObtenerVigentes(string.Empty);
            cboTipoDocumento.DataBind();
            cboTipoDocumento.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboEstado.DataTextField = "Descripcion";
            cboEstado.DataValueField = "Id";
            cboEstado.DataSource = servParametricas.EstadosDocumentosObtenerVigentes(string.Empty);
            cboEstado.DataBind();
            cboEstado.Items.Insert(0, new ListItem(string.Empty, "-1"));

            ServicioEquipos servEquipos = new ServicioEquipos();

            List<Profesional> profesionales = servEquipos.ProfesionalObtenerVigentes(string.Empty);

            cboEstadoProfesionalAutor.DataTextField = "NombreCompleto";
            cboEstadoProfesionalAutor.DataValueField = "Id";
            cboEstadoProfesionalAutor.DataSource = profesionales;
            cboEstadoProfesionalAutor.DataBind();
            cboEstadoProfesionalAutor.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboEstadoProfesionalPresenta.DataTextField = "NombreCompleto";
            cboEstadoProfesionalPresenta.DataValueField = "Id";
            cboEstadoProfesionalPresenta.DataSource = profesionales;
            cboEstadoProfesionalPresenta.DataBind();
            cboEstadoProfesionalPresenta.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboEstadoProfesionalResponsable.DataTextField = "NombreCompleto";
            cboEstadoProfesionalResponsable.DataValueField = "Id";
            cboEstadoProfesionalResponsable.DataSource = profesionales;
            cboEstadoProfesionalResponsable.DataBind();
            cboEstadoProfesionalResponsable.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboComentarioProfesionalAutor.DataTextField = "NombreCompleto";
            cboComentarioProfesionalAutor.DataValueField = "Id";
            cboComentarioProfesionalAutor.DataSource = profesionales;
            cboComentarioProfesionalAutor.DataBind();
            cboComentarioProfesionalAutor.Items.Insert(0, new ListItem(string.Empty, "-1"));

            cboRecordatorioProfesionalAutor.DataTextField = "NombreCompleto";
            cboRecordatorioProfesionalAutor.DataValueField = "Id";
            cboRecordatorioProfesionalAutor.DataSource = profesionales;
            cboRecordatorioProfesionalAutor.DataBind();
            cboRecordatorioProfesionalAutor.Items.Insert(0, new ListItem(string.Empty, "-1"));
        }

        private void ConfigurarFileUploader()
        {
            FileUploader.text_Add = "Adjuntar archivo";
            FileUploader.text_Delete = "Eliminar archivo";
            FileUploader.text_Uploading = "Subiendo archivo...";
            FileUploader.text_X = "Ocultar";
        }

        private void AdministrarPostFileUploader()
        {
            try
            {
                HttpPostedFileAJAX pf = FileUploader.PostedFile;

                FileUploader.File_RenameIfAlreadyExists = false;
                FileUploader.SaveAs(@"~/Archivos/temp", pf.FileName);

                pf.responseMessage_Uploaded = pf.FileName;

                SessionHelper.GuardarEnSession("DocumentoAdjunto", pf.FileName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }
    }
}