<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecordatorioControl.ascx.cs"
    Inherits="pome.SysGEIC.Web.Controles.RecordatorioControl" %>
<div class="easyui-tabs">
    <div title="Datos del Recordatorio" style="padding: 5px;">
        <div style="width: 100%;">
            <div class="fitem">
                <label for="Descripcion">
                    Descripción:</label>
                <input class="easyui-validatebox campoABM" type="text" id="txtDescripcion" name="Descripcion" />
            </div>
            <div class="fitem">
                <label>
                    Tipo:</label>                
                <asp:DropDownList ID="cboTipoRecordatorio" runat="server" CssClass="easyui-combobox" Width="360px" />
            </div>
            <div class="fitem">
                <label for="Estado">
                    Estado:</label>
                <input class="easyui-validatebox " type="text" id="txtEstadoRecordatorio"
                    name="Estado" disabled="disabled" style="width: 200px;" />
                <a id="btnReactivarRecordatorio" href="#" class="easyui-linkbutton" iconcls="icon-redo"
                    plain="true">Reactivar recordatorio</a>
            </div>
            <div class="fitem">
                <label class="label" for="FechaActivacion">
                    Activación:</label>
                <input class="easyui-datebox" id="txtFechaActivacion" name="FechaActivacion" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            </div>
            <div class="fitem">
                <label class="labelAncho" for="AvisoMail">
                    Aviso por e-mail:</label>
                <input type="checkbox" id="chkAvisoMail" name="AvisoMail" />
            </div>
            <div class="fitem">
                <label class="labelAncho" for="AvisoPopup">
                    Aviso con popup:</label>
                <input type="checkbox" id="chkAvisoPopup" name="AvisoPopup" />
                <label class="labelAncho" for="Seperador">
                </label>
                <label for="Color">
                    Color:</label>
                <input id='SelectorColor' />
            </div>
            <div class="fitem">
                <label for="Estado">
                    Estudio:</label>
                <asp:DropDownList ID="cboEstudio" runat="server" CssClass="easyui-combobox" Width="350px" />
                <a id="btnSeleccionarDocumentos" href="#" class="easyui-linkbutton" iconcls="icon-view"
                    plain="true">Seleccionar Documentos</a>
            </div>
            <table id="dgRecordatorioDocumentos">
            </table>
            <div id="panelTextoAviso">
                <textarea class="easyui-validatebox campoTextAreaPanel" id="txtTextoAviso" name="TextoAviso"
                    cols="5" rows="7" onkeyup="return maximaLongitud(this,2000)"></textarea>
            </div>
        </div>
    </div>
    <div title="Información Correo Electrónico" style="padding: 5px;">
        <div style="width: 100%; height: 520px;">
            <div class="fitem">
                <label for="Estado">
                    Estado:</label>
                <input class="easyui-combobox" id="cboInfoMailEstado" name="Estado" style="width: 200px"
                    data-options="data: infoMailEstados,
					                valueField:'id',
					                textField:'descripcion',
					                panelHeight:'auto'" />
                <a id="btnInfoMailReenviar" href="#" class="easyui-linkbutton" iconcls="icon-redo"
                    plain="true">Enviar Correo Electrónico</a>
            </div>
            <div class="fitem">
                <label for="Asunto">
                    Asunto:</label>
                <input type="text" id="txtInfoMailAsunto" name="Asunto" style="width: 350px;"></input>
            </div>
            <br />           
            <table id="dgInfoMailToEmails">
            </table>
            <table id="dgInfoMailCCEmails">
            </table>
            
            <div id="panelInfoMailTexto">
                <textarea class="easyui-validatebox campoTextAreaPanel" id="txtInfoMailTexto" name="TextoAviso"
                    cols="20" rows="7"></textarea>
            </div>
        </div>
    </div>
</div>

<div id="dlgSeleccionDocumentos" class="easyui-dialog" style="width: 850px; height: 600px;
    padding: 10px 20px" closed="true" modal="true" buttons="#dlgSeleccionDocumentos-buttons">
    <table id="dgSeleccionDocumentos">
    </table>
</div>
<div id="dlgSeleccionDocumentos-buttons">
    <a id="btnSeleccionDocumentosAceptar" href="#" class="easyui-linkbutton" iconcls="icon-save"">Aceptar</a> 
    <a id="btnSeleccionDocumentosCancelar" href="#" class="easyui-linkbutton" iconcls="icon-cancel" >Cancelar</a>
</div>

<div id="dlgIngresoEmails" class="easyui-dialog" style="width: 550px; height: 150px;
    padding: 10px 20px" closed="true" modal="true" buttons="#dlgIngresoEmails-buttons">
    
    <div class="fitem">
        <label class="labelAncho">Correo electrónico:</label>
        <input type="text" id="txtInfoMailEmail" class="easyui-validatebox" data-options="validType:'email'" style="width: 350px;" />
    </div>
    <div class="fitem">
        <label class="labelAncho">Listas:</label>
        <asp:DropDownList ID="cboInfoMailListas" runat="server" CssClass="easyui-combobox" Width="350px" />
    </div>

</div>
<div id="dlgIngresoEmails-buttons">
    <a id="btnIngresoEmailsAceptar" href="#" class="easyui-linkbutton" iconcls="icon-ok"" >Aceptar</a> 
    <a id="btnIngresoEmailsCancelar" href="#" class="easyui-linkbutton" iconcls="icon-cancel" >Cerrar</a>
</div>