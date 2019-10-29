<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="LisasEmails.aspx.cs" Inherits="pome.SysGEIC.Web.LisasEmails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/ListasEmails.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelIzquierdo" class="contenedorControlesMitad">
        <div id="dgListas_toolbar" style="padding: 5px; height: auto">           
             <div class="fitem">
                <label class="labelAncho" for="Descripcion">
                    Descripción:</label>
                <input class="easyui-validatebox campoABM" type="text" id="txtListaDescripcion" data-options="required:true"></input><br />
            </div>
            <div>
                <a href="#" id="btnListasNuevo" class="easyui-linkbutton" iconcls="icon-add" plain="true">Nuevo</a>
                <a href="#" id="btnListasEliminar" class="easyui-linkbutton" iconcls="icon-remove" plain="true">Eliminar</a>
                <a href="#" id="btnListasGrabar" class="easyui-linkbutton" iconcls="icon-save" plain="true">Grabar</a>
                <a href="#" id="btnListasCancelar" class="easyui-linkbutton" iconcls="icon-cancel" plain="true">Cancelar</a>
            </div>            
        </div>
        <table id="dgListas" toolbar="#dgListas_toolbar">
        </table>
    </div>
    <div id="panelDerecho" class="contenedorControlesMitad">
        <div id="dgEmails_toolbar" style="padding: 5px; height: auto">
            <div>
                Correo electrónico:
                <input type="text" id="txtEmail" class="easyui-validatebox" data-options="required:true,validType:'email'" style="width: 350px;" />
                <a href="#" id="btnActualizarEmail" class="easyui-linkbutton" iconcls="icon-ok" plain="true">
                    Actualizar</a>
            </div>
        </div>
        <table id="dgEmails" toolbar="#dgEmails_toolbar">
        </table>
    </div>
</asp:Content>
