<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="DocumentoDatos.aspx.cs" Inherits="pome.SysGEIC.Web.DocumentoDatos" %>

<%@ Register Assembly="FUA" Namespace="Subgurim.Controles" TagPrefix="cc1" %>
<%@ Register src="Controles/DocumentoVersionControl.ascx" tagname="DocumentoVersionControl" tagprefix="uc1" %>
<%@ Register src="Controles/ActaDocumentoComentarioControl.ascx" tagname="ActaDocumentoComentarioControl" tagprefix="uc2" %>
<%@ Register src="Controles/RecordatorioControl.ascx" tagname="RecordatorioControl" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/datagrid-detailview.js" type="text/javascript"></script>
    <script src="js_page/DocumentoDatos.js" type="text/javascript"></script>
    <script src="js_page/PlantillasCtrolSeleccion.js" type="text/javascript"></script>
    <script src="js_page/Recordatorios.js" type="text/javascript"></script>
    <script src="js/spectrum.js" type="text/javascript"></script>
    <link href="css/spectrum.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="toolbar" style="padding: 3px; width: 1190px; border: 1px solid #ccc">
        <a href="#" class="easyui-linkbutton" iconcls="icon-save" plain="true" onclick="guardar()">
            Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-back" plain="true"
                onclick="cancelar()">Volver</a>
    </div>
    <div id="panelIzquierdo" class="contenedorControlesMitad">
        
        <div class="easyui-panel" title="Datos del Documento" style="width: 100%; padding: 5px;">
            <div class="fitem">
                <label for="Esudio">
                    Estudio:</label>
                <textarea class="easyui-validatebox campoTextArea" id="txtEstudio" name="Estudio"
                    cols="20" rows="10" disabled="disabled"></textarea>
            </div>
            <div class="fitem">
                <label for="TipoDocumento">
                    Tipo:</label>
                <asp:DropDownList ID="cboTipoDocumento" name="TipoDocumento" runat="server" CssClass="easyui-combobox"
                    Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="txtNombre">
                    Nombre:</label>
                <textarea class="easyui-validatebox campoTextArea" id="txtNombre" name="txtNombre"
                    cols="20" rows="6" onkeyup="return maximaLongitud(this,250)"></textarea>
            </div>
            <div class="fitem">
                <label for="chkLimitante">
                    Limitante:</label>
                <input type="checkbox" id="chkLimitante" name="Final" />
            </div>

            <div class="ftitle">Recoratorios y Alertas</div>
            <div class="fitem">
                    <input type="checkbox" id="chkRequiereAlertaInactividad" />
                <label class="labelAncho250Izq">
                    Requiere alerta Inactividad de Documento:</label>
                <div id="panelAlertaInactividad">
                    <label class="label">
                        Meses:</label>
                    <input id="txtAlertaInactividadMeses" class="easyui-numberspinner" style="width: 40px;"
                        required="required" data-options="min:1,max:12,editable:false">
                    <a href="#" id="btnConfigAlertaInactividad" class="easyui-linkbutton" iconcls="icon-alert-rojo" plain="true">Configurar</a> 
                </div>
            </div>
            <div class="fitem">
                <input type="checkbox" id="chkRequiereAlertaInformeAvance" />
                <label class="labelAncho250Izq">
                    Requiere alerta Informe de Avance:</label> 
                <div id="panelAlertaInformeAvance">
                    <label class="label">
                        Meses:</label>
                    <input id="txtAlertaInformeAvanceMeses" class="easyui-numberspinner" style="width: 40px;" required="required"
                        data-options="min:1,max:12,editable:false">
                    <a href="#" id="btnConfigInformeAvance" class="easyui-linkbutton" iconcls="icon-alert-rojo" plain="true">Configurar</a> 
                </div>
            </div>
            <div class="fitem">
                <input type="checkbox" id="chkRequiereAlertaVencimiento" />
                <label class="labelAncho250Izq">
                    Requiere alerta por Vencimiento:</label> 
                <div id="panelAlertaVencimiento">
                    <label class="label">
                        Meses:</label>
                    <input id="txtAlertaVencimientoMeses" class="easyui-numberspinner" style="width: 40px;" required="required"
                        data-options="min:1,max:12,editable:false">
                    <a href="#" id="btnConfigVencimiento" class="easyui-linkbutton" iconcls="icon-alert-rojo" plain="true">Configurar</a> 
                </div>
            </div>
            <div class="fitem">
                <input type="checkbox" id="chkRequiereAlertaReaprobacion" />
                <label class="labelAncho250Izq">
                    Requiere alerta por Reaprobación:</label> 
                <div id="panelAlertaReaprobacion">
                    <label class="label">
                        Meses:</label>
                    <input id="txtAlertaReaprobacionMeses" class="easyui-numberspinner" style="width: 40px;" required="required"
                        data-options="min:1,max:12,editable:false">
                    <a href="#" id="btnConfigReaprobacion" class="easyui-linkbutton" iconcls="icon-alert-rojo" plain="true">Configurar</a> 
                </div>
            </div>
        </div>        
    </div>
    <div id="panelDerecho" class="easyui-accordion contenedorControlesMitad">
         <div title="Versiones del Documento " data-options="selected:true">
            <table id="dgVersiones">
            </table>
        </div>
        <div title="Actas donde se trató el Documento ">
            <table id="dgComentariosActas">
            </table>
        </div>
        <div title="Investigadores">
            <table id="dgParticipantes">
            </table>
        </div>
        <div title="Estados de la Versión">
            <table id="dgVersionEstados">
            </table>
        </div>
        <div title="Comentarios de la Versión">
            <table id="dgComentarios">
            </table>
        </div>
        <div title="Recordatorios de la Versión">
            <table id="dgRecordatorios">
            </table>
        </div>
    </div>
    
    <div id="dlgEstado" class="easyui-dialog" style="width: 650px; height: 290px; padding: 10px 20px"
        closed="true" modal="true" buttons="#dlgEstado-buttons">
        <div class="ftitle">
            Estado de la Versión</div>
        <fieldset>
            <div class="fitem">
                <label for="Version">
                    Versión:</label>
                <input class="easyui-validatebox campoABMLargo" type="text" id="txtEstdoVersion"
                    name="Version"></input>
            </div>
            <div class="fitem">
                <label for="Fecha">
                    Fecha:</label>
                <input class="easyui-datebox" id="txtEstadoFecha" name="Fecha" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            </div>
            <div class="fitem">
                <label for="Estado">
                    Estado:</label>
                <asp:DropDownList ID="cboEstado" name="Estado" runat="server" CssClass="easyui-combobox"
                    Width="455px" data-options="valueField: 'Id', textField: 'Descripcion'">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Autor">
                    Autor:</label>
                <asp:DropDownList ID="cboEstadoProfesionalAutor" name="Autor" runat="server" CssClass="easyui-combobox"
                    Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Presenta">
                    Presenta:</label>
                <asp:DropDownList ID="cboEstadoProfesionalPresenta" name="Presenta" runat="server"
                    CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Responsable">
                    Responsable:</label>
                <asp:DropDownList ID="cboEstadoProfesionalResponsable" name="Responsable" runat="server"
                    CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
        </fieldset>
    </div>
    <div id="dlgEstado-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarEstado()">Guardar</a>
        <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarEstado()">
            Cancelar</a>
    </div>
    <div id="dlgComentario" class="easyui-dialog" style="width: 620px; height: 440px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgComentario-buttons">
        <div class="ftitle">
            Comentario de la Versión</div>
        <fieldset>
            <div class="fitem">
                <label for="Version">
                    Versión:</label>
                <input class="easyui-validatebox campoABM" type="text" id="txtComentarioVersion"
                    name="Version"></input>
                Fecha:
                <input class="easyui-datebox" id="txtComentarioFecha" name="Fecha" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            </div>
            <div class="fitem">
                <label for="Autor">
                    Autor:</label>
                <asp:DropDownList ID="cboComentarioProfesionalAutor" name="Autor" runat="server"
                    CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <textarea class="easyui-validatebox campoTextArea" id="txtComentarioObservaciones"
                    name="Observaciones" cols="20" rows="15" style="width: 530px;" onkeyup="return maximaLongitud(this,1500)"></textarea>
            </div>
        </fieldset>
    </div>
    <div id="dlgComentario-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarComentario()">
            Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarComentario()">
                Cancelar</a>
    </div>
    <div id="dlgRecordatorio" class="easyui-dialog" style="width: 620px; height: 440px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgRecordatorio-buttons">
        <div class="ftitle">
            Recordatorio de la Versión</div>
        <fieldset>
            <div class="fitem">
                <label for="Version">
                    Versión:</label>
                <input class="easyui-validatebox campoABM" type="text" id="txtRecordatorioVersion"
                    name="Version"></input>
                Fecha:
                <input class="easyui-datebox" id="txtRecordatorioFecha" name="Fecha" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            </div>
            <div class="fitem">
                <label for="Autor">
                    Autor:</label>
                <asp:DropDownList ID="cboRecordatorioProfesionalAutor" name="Autor" runat="server"
                    CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Pendiente">
                    Pendiente:</label>
                <input type="checkbox" id="chkRecordatorioPendiente" name="Pendiente" />
            </div>
            <div class="fitem">
                <textarea class="easyui-validatebox campoTextArea" id="txtRecordatorioObservaciones"
                    name="Observaciones" cols="20" rows="15" style="width: 530px;" onkeyup="return maximaLongitud(this,1500)"></textarea>
            </div>
        </fieldset>
    </div>
    <div id="dlgRecordatorio-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarVersionRecordatorio()">
            Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarVersionRecordatorio()">
                Cancelar</a>
    </div>
    <div id="dlgSeleccionActa" class="easyui-dialog" style="width: 620px; height: 440px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgSeleccionActa-buttons">
        <div class="ftitle">
            Actas sin cerrar</div>
        <fieldset>
            <div class="fitem">
                <label for="Version">
                    Versión:</label>
                <input class="easyui-validatebox campoABMLargo" type="text" id="txtActaDocumentoVersion"
                    name="Version"></input>
            </div>
            <table id="dgSeleccionActa">
            </table>
        </fieldset>
    </div>
    <div id="dlgSeleccionActa-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="seleccionActaAgregar()">
            Agregar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="seleccionActaCancelar()">
                Cancelar</a>
    </div>
    <div id="dlgAdjuntarArchivo" class="easyui-dialog" style="width: 650px; height: 250px; padding: 10px 20px"
        closed="true" modal="true" buttons="#dlgAdjuntarArchivo-buttons">
        <div class="ftitle">
            Adjuntar archivo</div>
        <fieldset>
            <div class="fitem">
                <label for="Documento">Documento:</label>
                <input class="easyui-validatebox campoABMLargo" type="text" id="txtAdjuntarArchivoDocumento"
                    name="Documento"></input>
            </div>
            <div class="fitem">
                <label for="Version">Versión:</label>
                <input class="easyui-validatebox campoABMLargo" type="text" id="txtAdjuntarArchivoVersion"
                    name="Version" onkeyup="return maximaLongitud(this,250)" />
            </div>
            <div class="fitem">
                <label for="Archivo">Archivo:</label>
                <cc1:FileUploaderAJAX ID="FileUploader" runat="server" />
            </div>
        </fieldset>        
    </div>
    <div id="dlgAdjuntarArchivo-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarAdjuntarArchivo()">Guardar</a>
        <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarAdjuntarArchivo()">
            Cancelar</a>
    </div>

    <div id="dlgVersion" class="easyui-dialog" style="width: 650px; height: 350px; padding: 10px 20px"
        closed="true" modal="true" buttons="#dlgVersion-buttons">
        <uc1:DocumentoVersionControl ID="DocumentoVersionControl" runat="server" />
    </div>
    <div id="dlgVersion-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarVersion()">Guardar</a>
        <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarVersion()">Cancelar</a>
    </div>

    <div id="dlgSeleccionParticipantes" class="easyui-dialog" style="width: 550px;
        height: 300px; padding: 10px 20px" closed="true" modal="true" buttons="#dlgSeleccionParticipantes-buttons">
        <table id="dgSeleccionParticipantes">
        </table>
    </div>
    <div id="dlgSeleccionParticipantes-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-save" onclick="aceptarSeleccionParticipantes()">Aceptar</a> 
	    <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarSeleccionParticipantes()">Cancelar</a>
    </div>
    
    <uc2:ActaDocumentoComentarioControl ID="ActaDocumentoComentarioControl" 
        runat="server" />
    
    <div id="dlgRecordatorioAlerta" class="easyui-dialog" style="width: 650px; height: 180px;
	    padding: 10px 20px" closed="true" modal="true" buttons="#dlgRecordatorioAlerta-buttons">
	    <uc3:RecordatorioControl ID="RecordatorioControl1" runat="server" />
    </div>
    <div id="dlgRecordatorioAlerta-buttons">
	    <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarRecordatorioAlerta()">
		    Guardar</a> 
	    <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarRecordatorioAlerta()">
		    Cancelar</a>
    </div>

</asp:Content>
