<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true" CodeBehind="ActaCargaDocumentosV3.aspx.cs" Inherits="pome.SysGEIC.Web.ActaCargaDocumentosV3" %>


<%@ Register src="Controles/DocumentoVersionControl.ascx" tagname="DocumentoVersionControl" tagprefix="uc1" %>
<%@ Register src="Controles/ActaDocumentoComentarioControl.ascx" tagname="ActaDocumentoComentarioControl" tagprefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/datagrid-groupview.js" type="text/javascript"></script>
    <script src="js_page/ActaCargaDocumentosV3.js" type="text/javascript"></script>
    <script src="js_page/PlantillasCtrolSeleccion.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">

   
    <div id="panelSeleccionEstudioDocumentos" style="padding-top: 5px;">
        <div class="fitem">
             <label class="labelAncho">
                Seleccione Estudio:</label>
            <input class="easyui-combobox" id="cboSeleccionEstudio" style="width: 350px" />
            <a id="bntDetalleEstudio" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit'"
                plain="true">Detalle de Estudio</a>
            <a id="bntNuevoEstudio" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add'"
                plain="true">Agregar Nuevo Estudio</a>
            <a id="btnMotrarNotasEstudioSeleccionado" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-view'"
                plain="true">Notas</a>
            <a id="btnEvaluarDocumentacion" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-ok'"
                plain="true">Evaluar Documentación</a>
        </div>
        <div class="fitem">
            <input class="easyui-textbox" type="text" id="txtEsudioNombreCompleto" data-options="multiline:true, disabled:true" style="width: 100%; height: 40px;"/>
        </div>
    
        <div id="panelDocumentos" style="width: 100%; float: left;">
            <div id="tbDocumentos" style="padding: 2px; height: auto">
                <a id="btnNuevoDocumento" href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true">
                    Nuevo Documento</a> <a id="btnDocumentoDetalle" href="#" class="easyui-linkbutton"
                        iconcls="icon-edit" plain="true">Detalle</a> 
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
            <input class="easyui-validatebox campoABMLargo" type="text" id="txtActaDescripcion"
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
                    cols="20" rows="5" onkeyup="return maximaLongitud(this,250)"></textarea>
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
            <input class="easyui-textbox campoABMLargo" type="text" id="txtNotaEstudioNombre" data-options="disabled:true"/>
        </div>
        <div class="fsubtitle">
            Imprimir: ANTES DE DOCUMENTOS TRATADOS</div>
        <div class="fitem">
            <input class="easyui-textbox" type="text" id="txtNotaEstudioAntesDocumentos" data-options="multiline:true" style="width: 750px; height: 230px;"/>
        </div> 
	    <div class="fsubtitle">
            Imprimir: A CONTINUACIÓN DE DOCUMENTOS TRATADOS</div>
        <div class="fitem">
            <input class="easyui-textbox" type="text" id="txtNotaEstudioDespuesDocumentos" data-options="multiline:true" style="width: 750px; height: 230px;"/>
        </div> 
    </div>
    <div id="dlgActaNotasEstudio-buttons">
        <a id="btnActaNotasEstudio_Guardar" href="#" class="easyui-linkbutton" iconcls="icon-ok">Guardar</a> 
        <a id="btnActaNotasEstudio_Cancelar" href="#" class="easyui-linkbutton" iconcls="icon-cancel">Cancelar</a>
    </div>
</asp:Content>
