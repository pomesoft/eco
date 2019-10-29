<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentoVersionControl.ascx.cs"
    Inherits="pome.SysGEIC.Web.Controles.DocumentoVersionControl" %>
<div class="ftitle">
    Versión de Documento
</div>
<fieldset>
    <div class="contenedorControles2Tercios">
        <div id="panelVersionDocumento" class="fitem">
            <label for="Documento">
                Documento:</label>
            <input class="easyui-validatebox campoABMLargo" type="text" id="txtVersionDocumento"
                name="Documento"></input>
        </div>
        <div class="fitem">
            <label for="Fecha">
                Fecha:</label>
            <input class="easyui-datebox" id="txtVersionFecha" name="Fecha" data-options="formatter:formatterDateBox, parser:parserDateBox" />
        </div>
        <div class="fitem">
            <label for="Version">
                Versión:</label>
            <textarea class="campoTextArea" id="txtVersionDescripcion" name="Vesion"
                cols="20" rows="3" onkeyup="return maximaLongitud(this,250)"></textarea>
        </div>
        <div class="fitem">
            <label for="AprobadoANMAT">
                ANMAT:</label>
            Aprobado
        <input type="checkbox" id="chkVersionAprobadoANMAT" name="AprobadoANMAT" />
            Fecha
        <input type="text" class="easyui-datebox" id="txtVersionFechaAprobadoANMAT" name="FechaAprobadoANMAT"
            data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
        </div>
    </div>
    <div id="panelVersionEstado">
        <div class="fitem">
            <label for="Estado">
                Estado:</label>
            <input class="easyui-combobox" id="cboVersionEstado" name="Estado" style="width: 180px" />
        </div>
        <div class="fitem">
            <label for="FechaEstado" class="">
                Fecha:</label>
            <input class="easyui-datebox" id="txtVersionFechaEstado" name="FechaEstado" data-options="formatter:formatterDateBox, parser:parserDateBox" style="width: 150px"></input>
        </div>
    </div>

</fieldset>
