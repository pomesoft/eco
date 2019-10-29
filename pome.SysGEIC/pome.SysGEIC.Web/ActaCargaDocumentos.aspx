<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" 
    CodeBehind="ActaCargaDocumentos.aspx.cs" Inherits="pome.SysGEIC.Web.ActaCargaDocumentos" %>

<%@ Register Src="Controles/DocumentoVersionControl.ascx" TagName="DocumentoVersionControl"
    TagPrefix="uc1" %>
<%@ Register Src="Controles/ActaDocumentoComentarioControl.ascx" TagName="ActaDocumentoComentarioControl"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/datagrid-groupview.js" type="text/javascript"></script>
    <script src="js/jquery.texteditor.js"></script>
    <link href="css/texteditor.css" rel="stylesheet" />
    <script src="js_page/ActaCargaDocumentos.js" type="text/javascript"></script>
    <script src="js_page/PlantillasCtrolSeleccion.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelSeleccionActa" style="padding-top: 5px;">
        <div class="fitem">
            <label class="labelAncho" for="SeleccionActa">
                Seleccione Acta:</label>
            <input class="easyui-combobox" id="cboSeleccionActa" name="SeleccionActa" style="width: 500px" />
            <a id="btnActaDetalle" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit'"
                plain="true">Detalle de Acta</a> <a id="btnNuevaActa" href="#" class="easyui-linkbutton"
                    data-options="iconCls:'icon-add'" plain="true">Crear Acta</a>
        </div>
    </div>
    <div id="panelSeleccionEstudioDocumentos" style="padding-top: 5px;">
        <div class="fitem">
             <label class="labelAncho">
                Seleccione Estudio:</label>
            <input class="easyui-combobox" id="cboSeleccionEstudio" style="width: 700px" />            
        </div>
        <div class="fitem">
            <input class="easyui-textbox" type="text" id="txtEsudioNombreCompleto" data-options="multiline:true, disabled:true" style="width: 970px; height: 40px;"/>
        </div>
    
        <div id="panelDocumentos" style="width: 70%; float: left;">
            <div id="tbDocumentos" style="padding: 2px; height: auto">
                <a id="btnNuevoDocumento" href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true">
                    Nuevo Documento</a> <a id="btnDocumentoDetalle" href="#" class="easyui-linkbutton"
                        iconcls="icon-edit" plain="true">Detalle</a> <a id="btnAgregarDocumentosActa" href="#"
                            class="easyui-linkbutton" iconcls="icon-ok" plain="true">Agregar al Acta</a>
                <a id="btnExpandir" href="#" class="easyui-linkbutton" iconcls="icon-expand" plain="true">
                    Expandir</a> <a id="btnContraer" href="#" class="easyui-linkbutton" iconcls="icon-collapse"
                        plain="true">Contraer</a>
                <div>
                    <input type="text" id="txtDocumentoBuscar" class="campoTextBuscador" />
                    <div id="menuDocumentoBuscar" style="width: 250px">
                        <div data-options="name:'TipoDocumento'">
                            Tipo de Documento</div>
                        <div data-options="name:'NombreDocumento'">
                            Nombre Documento</div>
                    </div>
                </div>
            </div>
            <table id="dgDocumentos" toolbar="#tbDocumentos">
            </table>
        </div>
    </div>
    <table id="dgActaDocumentos">
    </table>
    <div id="dlgNuevaActa" class="easyui-dialog" style="width: 600px; height: 180px;
        padding: 10px 20px" title="Nueva Acta" closed="true" modal="true" toolbar="#dlgNuevaActa-buttons">
        <div class="ftitle">
        </div>
        <div class="fitem">
            <label for="Fecha">
                Fecha:</label>
            <input class="easyui-datebox" id="txtActaFecha" name="Fecha" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            <label>
                Hora:</label>
            <input class="easyui-timespinner" id="txtActaHora" style="width: 60px;" data-options="min:'07:00', max: '22:00'" />
        </div>
        <div class="fitem">
            <label>
                Descripción:</label>
            <input class="easyui-textbox campoABMLargo" type="text" id="txtActaDescripcion"
                name="Descripcion"></input>
        </div>
    </div>
    <div id="dlgNuevaActa-buttons">
        <a id="btnGuardarNuevaActa" href="#" class="easyui-linkbutton" iconcls="icon-ok">Aceptar</a>
        <a id="btnCancelarNuevaActa" href="#" class="easyui-linkbutton" iconcls="icon-cancel">
            Cancelar</a>
    </div>
    <div id="dlgNuevoDocumento" class="easyui-dialog" style="padding: 10px 20px" title="Nuevo Documento"
        closed="true" toolbar="#dlgNuevoDocumento-buttons">
        <div class="ftitle">
            Documento</div>
        <fieldset>
            <div class="fitem">
                <label for="Esudio">
                    Estudio:</label>
                <input class="easyui-textbox campoABMLargo" type="text" id="txtEstudio" data-options="multiline:true, disabled:true"/>
            </div>
            <div class="fitem">
                <label for="TipoDocumento">
                    Tipo:</label>
                <input class="easyui-combobox" id="cboTipoDocumento" name="TipoDocumento" style="width: 455px" />
            </div>
            <div class="fitem">
                <label for="txtNombre">
                    Nombre:</label>
                <textarea class="easyui-validatebox campoTextArea" id="txtNombreDocumento" name="txtNombre"
                    cols="20" rows="3" onkeyup="return maximaLongitud(this,250)"></textarea>
            </div>
        </fieldset>
        <div id="panelVersionControl">
            <uc1:DocumentoVersionControl ID="DocumentoVersionControl" runat="server" />
        </div>
        <div class="ftitle">
            Investigadores</div>
        <fieldset>
            <table id="dgParticipantes">
            </table>
        </fieldset>
    </div>
    <div id="dlgNuevoDocumento-buttons">
        <a id="btnGuardarDocumento" href="#" class="easyui-linkbutton" iconcls="icon-save"
            plain="true">Guardar</a> <a id="btnCancelarNuevoDocumento" href="#" class="easyui-linkbutton"
                iconcls="icon-cancel" plain="true">Cancelar</a>
    </div>
    <uc2:ActaDocumentoComentarioControl ID="ActaDocumentoComentarioControl" runat="server" />
    <div id="dlgActaNotasEstudio" class="easyui-dialog" style="padding: 10px 20px;" 
         closed="true" modal="true" toolbar="#dlgActaNotasEstudio-buttons">
        <div class="fitem">
            <label>Estudio</label>
            <input class="easyui-textbox campoABMLargo" type="text" id="txtNotaEstudioNombre" data-options="disabled:true"/>
        </div>
        <div class="fsubtitle">
            NOTAS DEL ESTUDIO</div>
        <div id="panelNotaEstudioAntesDocumentos">
            <textarea class="campoTextArea" id="txtNotaEstudioAntesDocumentos" name="txtNotaEstudioAntesDocumentos" cols="30" rows="20" style="width: 690px;"></textarea>
        </div> 
        <br />
	    <div id="panelNotaEstudioDespuesDocumentos">
            <textarea class="campoTextArea" id="txtNotaEstudioDespuesDocumentos" name="txtNotaEstudioDespuesDocumentos" cols="30" rows="20" style="width: 690px;"></textarea>
        </div> 
    </div>
    <div id="dlgActaNotasEstudio-buttons">
        <a id="btnActaNotasEstudio_Guardar" href="#" class="easyui-linkbutton" iconcls="icon-ok">Guardar</a> 
        <a id="btnActaNotasEstudio_Cancelar" href="#" class="easyui-linkbutton" iconcls="icon-cancel">Cancelar</a>
    </div>
</asp:Content>
