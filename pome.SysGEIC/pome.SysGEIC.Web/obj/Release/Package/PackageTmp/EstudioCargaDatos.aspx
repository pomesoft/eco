<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="EstudioCargaDatos.aspx.cs" Inherits="pome.SysGEIC.Web.EstudioCargaDatos" %>

<%@ Register Src="Controles/RecordatorioControl.ascx" TagName="RecordatorioControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/EstudioCargaDatos.js" type="text/javascript"></script>
    <script src="js_page/Recordatorios.js" type="text/javascript"></script>
    <script src="js/spectrum.js" type="text/javascript"></script>
    <link href="css/spectrum.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="toolbar" style="padding: 3px; width: 1190px; border: 1px solid #ccc">
        <a href="#" id="btnGuardar" class="easyui-linkbutton" iconcls="icon-save" plain="true">
            Guardar</a> <a href="#" id="btnVolver" class="easyui-linkbutton" iconcls="icon-back"
                plain="true">Volver</a>
    </div>
    <div id="panelIzquierdo" class="contenedorControlesMitad">
        <div class="easyui-panel" title="Datos del Estudio"  style="width: '100%'; padding: 5px;">
            <div class="fitem">
                <label for="txtCodigo">
                    Código:</label>
                <input class="easyui-textbox campoABM" type="text" id="txtCodigo" name="Nombre"
                    data-options="required:true" />
            </div>
            <div class="fitem">
                <label for="txtNombre">
                    Abreviado:</label>
                <input class="easyui-textbox campoABM" type="text" id="txtNombre" name="Nombre" />
            </div>
            <div class="fitem">
                <label for="txtNombreCompleto">
                    Estudio:</label>
                <input class="easyui-textbox campoTextArea" type="text" id="txtNombreCompleto" name="NombreCompleto" 
                    multiline="true" style="height:90px;padding:5px;"    
                    onkeyup="return maximaLongitud(this,1000)" />
            </div>
            <div class="fitem">
                <label for="Estado">
                    Estado:</label>
                <asp:DropDownList ID="cboEstado" name="Estado" runat="server" CssClass="easyui-combobox"
                    Width="360px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Patologia">
                    Patología:</label>
                <asp:DropDownList ID="cboPatologia" name="Patologia" runat="server" CssClass="easyui-combobox"
                    Width="360px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="TipoEstudio">
                    Tipo:</label>
                <asp:DropDownList ID="cboTipoEstudio" name="TipoEstudio" runat="server" CssClass="easyui-combobox"
                    Width="360px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="txtPoblacion">
                    Población:</label>
                <input class="easyui-textbox" type="text" id="txtPoblacion" name="Nombre" style="width: 360px;" />
            </div>
            <div class="fitem">
                <label class="labelAncho" for="FechaPresentacion">
                    Fecha Presentación:</label>
                <input class="easyui-datebox" id="txtFechaPresentacion" name="FechaPresentacion"
                    data-options="formatter:formatterDateBox, parser:parserDateBox"></input>                
            </div>
            <div class="ftitle">Recoratorios y Alertas</div>
                <div class="fitem">
                    <input type="checkbox" id="chkEstudioRenovacionAnual" name="RenovacionAnual" />
                    Requiere alerta por Reaprobación Anual del Estudio
                    <div id="panelAlertaRenovacioAnual">
                        <label class="label">
                            Meses:</label>
                        <input id="txtMesesAlertaRenovacionAnual" class="easyui-numberspinner" style="width: 50px;"
                            required="required" data-options="min:1,max:12,editable:false">
                        <a href="#" id="btnConfigRecordatorio" class="easyui-linkbutton" iconcls="icon-alert-rojo"
                            plain="true">Configurar</a>
                    </div>
                </div>
        </div>
        <table id="dgParticipantes">
        </table>
    </div>
    <div id="panelDerecho" class="contenedorControlesMitad">
        <table id="dgCentrosHabilitados">
        </table>
        <table id="dgPatrocinadores">
        </table>
        <table id="dgMonodrogas">
        </table>
    </div>
    <div id="dlgParticipante" class="easyui-dialog" style="width: 650px; height: 280px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgParticipante-buttons">
        <div class="ftitle">
            Participante Equipo de Investigación</div>
        <fieldset>
            <div class="fitem">
                <label for="Profesional">
                    Profesional:</label>
                <asp:DropDownList ID="cboParticipanteProfesional" runat="server" AutoPostBack="False"
                    name="Profesional" CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Rol">
                    Rol:</label>
                <asp:DropDownList ID="cboParticipanteRol" runat="server" AutoPostBack="False" name="Rol"
                    CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Vigencia">
                    Vigencia:</label>
                Desde:
                <input class="easyui-datebox" id="txtParticipanteDesde" name="ParticipanteDesde"
                    data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
                - Hasta:
                <input class="easyui-datebox" id="txtParticipanteHasta" name="ParticipanteHasta"
                    data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            </div>
        </fieldset>
    </div>
    <div id="dlgParticipante-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarParticipante()">
            Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarParticipante()">
                Cancelar</a>
    </div>
    <div id="dlgCentroHabilitado" class="easyui-dialog" style="width: 650px; height: 240px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgCentroHabilitado-buttons">
        <div class="ftitle">
            Centro Habilitado</div>
        <fieldset>
            <div class="fitem">
                <label for="Centro">
                    Centro:</label>
                <asp:DropDownList ID="cboCentroHabilitado" runat="server" AutoPostBack="False" name="Centro"
                    CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Vigencia">
                    Vigencia:</label>
                Desde:
                <input class="easyui-datebox" id="txtCentroHabilitadoDesde" name="CentroDesde" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
                - Hasta:
                <input class="easyui-datebox" id="txtCentroHabilitadoHasta" name="CentroHasta" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            </div>
        </fieldset>
    </div>
    <div id="dlgCentroHabilitado-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarCentroHabilitado()">
            Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarCentroHabilitado()">
                Cancelar</a>
    </div>
    <div id="dlgPatrocinador" class="easyui-dialog" style="width: 650px; height: 200px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgPatrocinador-buttons">
        <div class="ftitle">
            Patrocinador</div>
        <fieldset>
            <div class="fitem">
                <label for="Patrocinador">
                    Patrocinador:</label>
                <asp:DropDownList ID="cboPatrocinador" runat="server" AutoPostBack="False" name="Patrocinador"
                    CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
        </fieldset>
    </div>
    <div id="dlgPatrocinador-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarPatrocinador()">
            Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarPatrocinador()">
                Cancelar</a>
    </div>
    <div id="dlgMonodroga" class="easyui-dialog" style="width: 650px; height: 200px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgMonodroga-buttons">
        <div class="ftitle">
            Monodroga</div>
        <fieldset>
            <div class="fitem">
                <label for="Monodroga">
                    Monodroga:</label>
                <asp:DropDownList ID="cboMonodroga" runat="server" AutoPostBack="False" name="Monodroga"
                    CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
        </fieldset>
    </div>
    <div id="dlgMonodroga-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarMonodroga()">
            Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarMonodroga()">
                Cancelar</a>
    </div>
    <div id="dlgRecordatorioRenovacionAnual" class="easyui-dialog" style="padding: 10px 20px" closed="true" modal="true" buttons="#dlgRecordatorioRenovacionAnual-buttons">
        <uc1:RecordatorioControl ID="RecordatorioControl1" runat="server" />
    </div>
    <div id="dlgRecordatorioRenovacionAnual-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarRecordatorioRenovacionAnual()">
            Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarRecordatorioRenovacionAnual()">
                Cancelar</a>
    </div>
</asp:Content>
