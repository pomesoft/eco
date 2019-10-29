<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActaDocumentoComentarioControl.ascx.cs"
    Inherits="pome.SysGEIC.Web.Controles.ActaDocumentoComentarioControl" %>
<div id="dlgActaDocumentoComentarioControl" class="easyui-dialog" style="width: 750px;
    height: 500px; padding: 10px 20px" closed="true" modal="true" toolbar="#dlgActaDocumentoComentarioControl-buttons">
    <div class="fitem">
        <label>
            Estudio</label>
        <input class="easyui-textbox campoABMLargo" type="text" id="txtActaDocumentoControlEstudioNombre"
            data-options="disabled:true" />
    </div>
    <div class="fitem">
        <label for="txtNombre">
            Documento:</label>
        <input class="easyui-textbox campoABMLargo" type="text" id="txtActaDocumentoControlDocumentoNombre"
            data-options="disabled:true" />
    </div>
    <div id="panelActaDocumentoComentario">
        <textarea class="easyui-validatebox campoTextArea" id="txtActaDocumentoControlComentario"
            name="ActaDocumentoComentarioControl" cols="30" rows="20" style="width: 690px;"></textarea>
    </div>
</div>
<div id="dlgActaDocumentoComentarioControl-buttons">
    <a id="btnActaDocumentoComentarioControl_Guardar" href="#" class="easyui-linkbutton"
        iconcls="icon-ok">Guardar</a> <a id="btnActaDocumentoComentarioControl_Cancelar"
            href="#" class="easyui-linkbutton" iconcls="icon-cancel">Cancelar</a>
</div>
