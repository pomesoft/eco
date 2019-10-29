<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentoControl.ascx.cs"
    Inherits="pome.SysGEIC.Web.Controles.DocumentoControl" %>
<div class="ftitle">
    Documento</div>
<fieldset>
    <div class="fitem">
        <label for="Esudio">
            Estudio:</label>
        <input class="easyui-validatebox campoABMLargo" type="text" id="txtEstudio" name="Estudio"></input>
    </div>
    <div class="fitem">
        <label for="txtNombre">
            Nombre:</label>
        <input class="easyui-validatebox campoABMLargo" type="text" id="txtNombre" name="Nombre"
            data-options="required:true"></input>
    </div>
    <div class="fitem">
        <label for="TipoDocumento">
            Tipo:</label>
        <input class="easyui-combobox" id="cboNotaAutor" name="TipoDocumento" 
                style="width: 455px" data-options="
				    url:'handlers/TiposDocumentoHandler.ashx?accion=LISTAR',
				    valueField:'Id',
				    textField:'NombreCompleto',
				    panelHeight:'auto'" />
        </asp:DropDownList>
    </div>
    <div class="fitem">
        <label for="chkLimitante">
            Limitante:</label>
        <input type="checkbox" id="chkLimitante" name="Final" />
    </div>
</fieldset>
