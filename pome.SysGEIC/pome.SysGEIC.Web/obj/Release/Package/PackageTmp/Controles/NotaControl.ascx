<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotaControl.ascx.cs"
    Inherits="pome.SysGEIC.Web.Controles.NotaControl" %>
<div class="ftitle">
    Nota</div>
<fieldset>
    <div class="fitem">
        <label for="NotaEsudio">
            Estudio:</label>
        <input class="easyui-combobox" id="cboNotaEstudio" name="NotaEstudio" style="width: 455px" />
    </div>
    <div class="fitem">
        <label for="Fecha">
            Fecha:</label>
        <input class="easyui-datebox" id="txtNotaFecha" name="Fecha" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
        <label for="Numero">
        </label>
        <label for="Numero">
        </label>
        <label for="Numero">
            Número:</label>
        <input class="campoABM" type="text" id="txtNotaNumero" name="Numero" style="width: 90px;"></input>
    </div>
    <div class="fitem">
        <label for="Descripcion">
            Descripción:</label>
        <input class="easyui-validatebox campoABMLargo" type="text" id="txtNotaDescripcion"
            name="Descripcion"></input>
    </div>
    <div class="fitem">
        <label for="Autor">
            Autor:</label>
        <input class="easyui-combobox" id="cboNotaAutor" name="Autor" style="width: 455px"
            data-options="
				    url:'handlers/ProfesionalesHandler.ashx?accion=LISTAR',
				    valueField:'Id',
				    textField:'NombreCompleto',
				    panelHeight:'auto'" />
    </div>
    <div class="fitem">
        <label for="NotaDocumento">
            Documento:</label>
        <input class="easyui-combobox" id="cboNotaDocumento" name="NotaDocumento" style="width: 455px" />
    </div>
    <div class="fitem">
        <label for="NotaDocumentoVersion">
            Versión:</label>
        <input class="easyui-combobox" id="cboNotaDocumentoVersion" name="NotaDocumentoVersion"
            style="width: 455px" />
    </div>
    <div class="fitem">
        Requiere Respuesta:
        <input type="checkbox" id="chkNotaRequiereRespuesta" name="RequiereRespuesta" />
    </div>
    <%--<div class="fitem">
        <label for="Archivo">
            Archivo:</label>
        <asp:FileUpload ID="NotaSeleccionArchivo" runat="server" />
    </div>    --%>
</fieldset>
<div id="panelNotaTexto">
    <textarea class="campoTextArea" id="txtNotaTexto" name="Nota" cols="100" rows="12"
        style="width: 99%; height: 97%;"></textarea>
</div>
