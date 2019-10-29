<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true" CodeBehind="ActaDatos.aspx.cs" Inherits="pome.SysGEIC.Web.ActaDatos" %>

<%@ Register Src="Controles/PlantillaSeleccionControl.ascx" TagName="PlantillaSeleccionControl"
    TagPrefix="uc1" %>
<%@ Register Src="Controles/NotaControl.ascx" TagName="NotaControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/datagrid-detailview.js" type="text/javascript"></script>
    <script src="js/datagrid-filter.js" type="text/javascript"></script>
    <script src="js/jquery.texteditor.js" type="text/javascript"></script>
    <script src="js_page/Actas.js" type="text/javascript"></script>   
    <script src="js_page/PlantillasCtrolSeleccion.js" type="text/javascript"></script>
    <link href="css/texteditor.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="toolbar" style="padding: 3px; width: 1190px; border: 1px solid #ccc">
        <a href="#" id="btnActaGuardar" class="easyui-linkbutton" iconcls="icon-save" plain="true"
            onclick="guardarActa()">Guardar Acta</a> <a href="#" id="btnActaImprimirActa" class="easyui-linkbutton"
                iconcls="icon-pdf" plain="true">Imprimir Acta</a> <a href="#" id="btnActaExportarWord"
                    class="easyui-linkbutton" iconcls="icon-word" plain="true">Exportar Acta a Word</a>
        <a href="#" id="btnActaCartaRespuesta" class="easyui-linkbutton" iconcls="icon-view"
            plain="true" onclick="mostrarSeleccionCartaRespuesta()">Carta de Respuesta</a>
        <a href="#" id="btnActaVolver" class="easyui-linkbutton" iconcls="icon-back" plain="true"
            onclick="volver()">Volver</a>
    </div>
<div id="panelInfoActa" style="padding-top: 5px;">
    <div class="fitem">
        <label for="Fecha">
            Fecha:</label>
        <input class="easyui-datebox" id="txtActaFecha" name="Fecha" style="width: 80px;"
            data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
        <label>
            Hora:</label><input class="easyui-timespinner" id="txtActaHora" style="width: 60px;"
                data-options="min:'07:00', max: '22:00'" />
        <label>
            Descripción:</label><input class="easyui-textbox campoABMLargo" type="text" id="txtActaDescripcion"
                name="Descripcion" style="font-size: 14px"></input>
        <label>
            Cerrada:</label><input type="checkbox" id="chkCerrada" name="Cerrada" />
    </div>
</div>

<div id="panelComentarioInicialFijo">
      <textarea class="campoTextAreaPanel" id="txtActaComentarioInicialFijo" name="ComentarioInicialFijo"
        cols="25" rows="5"></textarea>
</div>
<div id="panelComentarioInicial">
    <textarea class="campoTextAreaPanel" id="txtActaComentarioInicial" name="ComentarioInicial"
        cols="20" rows="10"></textarea>
</div>
<div id="panelComentarioFinal">
    <textarea class="campoTextAreaPanel" id="txtActaComentarioFinal" name="ComentarioFinal"
        cols="20" rows="10"></textarea>
</div>
<div id="panelParticipantes">
    <table id="dgActaParticipantes">
    </table>
</div>
<table id="dgActaDocumentos">
</table>
<table id="dgNotasTratadas">
</table>
<div id="dlgActaSeleccionParticipantes" class="easyui-dialog" style="width: 550px;
    height: 300px; padding: 10px 20px" closed="true" modal="true" buttons="#dlgActaSeleccionParticipantes-buttons">
    <table id="dgActaSeleccionParticipantes">
    </table>
</div>
<div id="dlgActaSeleccionParticipantes-buttons">
    <a href="#" class="easyui-linkbutton" iconcls="icon-save" onclick="aceptarActaSeleccionParticipantes()">
        Aceptar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarActaSeleccionParticipantes()">
            Cancelar</a>
</div>
<div id="dlgActaDocumento" class="easyui-dialog" style="width: 660px; height: 460px;
    padding: 10px 20px" closed="true" modal="true" buttons="#dlgActaDocumento-buttons">
    <div class="ftitle">
        Documento a tratar en el acta</div>
    <fieldset>
        <div class="fitem">
            <label for="ActaEstudio">
                Estudio:</label>
            <input class="easyui-combobox" id="cboActaEstudio" name="ActaEstudio" style="width: 500px" />
        </div>
        <div class="fitem">
            <label for="ActaDocumento">
                Documento:</label>
            <input class="easyui-combobox" id="cboActaDocumento" name="ActaDocumento" style="width: 500px" />
        </div>
        <div class="fitem">
            <label for="ActaDocumentoVersion">
                Versión:</label>
            <input class="easyui-combobox" id="cboActaDocumentoVersion" name="ActaDocumentoVersion"
                style="width: 500px" />
        </div>
        <div class="fitem">
            <label for="ResponsableComite">
                Responsable:</label>
            <input class="easyui-combobox" id="cboActaDocumentoResponsableComite" name="ResponsableComite"
                style="width: 500px" />
        </div>
        <div id="panelActaDocumentoComentario">
            <textarea class="easyui-validatebox campoTextArea" id="txtActaDocumentoComentario"
                name="ActaDocumentoComentario" style="width: 580px; height: 180px;"></textarea>
        </div>
    </fieldset>
</div>
<div id="dlgActaDocumento-buttons">
    <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarActaDocumento()">
        Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarActaDocumento()">
            Cancelar</a>
</div>
<div id="dlgNotaTratada" class="easyui-dialog" style="width: 650px; height: 450px;
    padding: 10px 20px" closed="true" modal="true" buttons="#dlgNotaTratada-buttons">
    <div class="ftitle">
        Nota a tratar en el acta</div>
    <fieldset>
        <div class="fitem">
            <label for="ActaEstudio">
                Estudio:</label>
            <input class="easyui-combobox" id="cboActaEstudioNota" name="ActaEstudio" style="width: 500px" />
        </div>
        <div class="fitem">
            <label for="ActaNotaPosicionImprime">
                Imprimir:</label>
            <select id="cboActaNotaPosicionImprime" class="easyui-combobox" name="ActaNotaPosicionImprime"
                style="width: 500px;" data-options="required:true">
                <option value="0">ANTES DE DOCUMENTOS TRATADOS</option>
                <option value="1">A CONTINUACIÓN DE DOCUMENTOS TRATADOS</option>
            </select>
        </div>
        <div class="fitem">
            <label for="NotaTratada">
                Nota:</label>
            <input class="easyui-combobox" id="cboActaNotaTratada" name="NotaTratada" style="width: 500px" />
        </div>
        <div id="panelNotaTratadaTexto">
            <textarea class="easyui-validatebox campoTextArea" id="txtNotaTratadaTexto" name="NotaTratadaTexto"
                style="width: 580px; height: 250px;" disabled="disabled"></textarea>
        </div>
    </fieldset>
</div>
<div id="dlgNotaTratada-buttons">
    <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarNotaTratada()">
        Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarNotaTratada()">
            Cancelar</a>
</div>
<div id="dlgNuevaNotaTratada" class="easyui-dialog" style="width: 625px; height: 480px;
    padding: 10px 10px" closed="true" modal="true" buttons="#dlgNuevaNotaTratada-buttons">
    <div class="ftitle">
        Nota a tratar en el acta</div>
    <fieldset>
        <div class="fitem">
            <label for="ActaEstudio">
                Estudio:</label>
            <input class="easyui-combobox" id="cboActaEstudioNuevaNota" name="ActaEstudio" style="width: 455px" />
        </div>
        <div class="fitem">
            <label for="ActaNuevaNotaPosicionImprime">
                Imprimir:</label>
            <select id="cboActaNuevaNotaPosicionImprime" class="easyui-combobox" name="ActaNotaPosicionImprime"
                style="width: 455px;" data-options="required:true">
                <option value="0">ANTES DE DOCUMENTOS TRATADOS</option>
                <option value="1">A CONTINUACIÓN DE DOCUMENTOS TRATADOS</option>
            </select>
        </div>       
        <div class="fitem">
            <label for="Autor">
                Autor:</label>
            <input class="easyui-combobox" id="cboNuevaNotaAutor" name="Autor" style="width: 455px"/>
        </div>
    </fieldset>
    <div id="panelNuevaNotaTexto">
        <textarea class="easyui-validatebox campoTextArea" id="txtNuevaNotaTexto" name="Nota"
            cols="90" rows="15" style="width: 99%; height: 97%;"></textarea>
    </div>
</div>
<div id="dlgNuevaNotaTratada-buttons">
    <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarNuevaNotaTratada()">
        Guardar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarNuevaNotaTratada()">
            Cancelar</a>
</div>
<div id="dlgActaSeleccionCartaRespuesta" class="easyui-dialog" style="width: 900px;
    height: 430px; padding: 10px 20px" closed="true" modal="true" buttons="#dlgActaSeleccionCartaRespuesta-buttons">
    <div class="ftitle">
        Seleccionar Estudio para generar Carta de Respuesta</div>
    <table id="dgActaSeleccionCartaRespuesta">
    </table>
</div>
<div id="dlgActaSeleccionCartaRespuesta-buttons">
    <a href="#" id="btnExportarCartaRespuestaPDF" class="easyui-linkbutton" iconcls="icon-pdf">
        Imprimir Carta</a> <a href="#" id="btnExportarCartaRespuestaWord" class="easyui-linkbutton"
            iconcls="icon-word">Exportar Carta a WORD</a> <a href="#" class="easyui-linkbutton"
                iconcls="icon-cancel" onclick="cerrarActaSeleccionCartaRespuesta()">Cerrar</a>
</div>
<div id="dlgActaActualizarEstudio" class="easyui-dialog" style="width: 650px; height: 550px;
    padding: 10px 20px" closed="true" modal="true" buttons="#dlgActaActualizarEstudio-buttons">
    <div class="fsubtitle">
        Actualizar información del Estudio</div>
    <div class="fitem">
        <label for="txtActaCodigoEstudio">
            Código:</label>
        <input type="text" class="campoABMLargo" id="txtActaCodigoEstudio" name="txtActaCodigoEstudio"
            disabled="disabled"></input>
    </div>
    <div class="fitem">
        <label for="txtActaNombreCompletoEstudio">
            Estudio:</label>
        <textarea class="easyui-validatebox campoTextArea" id="txtActaNombreCompletoEstudio"
            name="txtActaNombreCompletoEstudio" cols="20" rows="7" disabled="disabled"></textarea>
    </div>
    <div class="fitem">
        <label for="cboActaEstadosEstudio">
            Estado:</label>
        <input class="easyui-combobox" id="cboActaEstadosEstudio" name="cboActaEstadosEstudio"
            style="width: 455px" data-options="url: 'handlers/EstudioCargaDatosHandler.ashx?accion=OBTENER-ESTADOSESTUDIO',
					  valueField:'Id',
					  textField:'Descripcion',
					  panelHeight:'auto'" />
    </div>
    <br />
    <div class="fsubtitle">
        Configuración Carta de Respuesta</div>
    <div class="fitem">
        <label for="cboActaCartaRespuestaModelo">
            Modelo carta:</label>
        <input class="easyui-combobox" id="cboActaCartaRespuestaModelo" name="cboActaCartaRespuestaModelo"
            style="width: 455px" data-options="url: 'handlers/CartaRespuestaHandler.ashx?accion=LISTAR-CARTARESPUESTAMODELO',
					  valueField:'Id',
					  textField:'Descripcion',
					  panelHeight:'auto'" />
    </div>
    <div class="fitem">
        <label>Texto libre:</label>
    </div>
    <div class="fitem">
        <%--    <div class="easyui-texteditor" id="txtTextoLibre" data-options="title:'Comentario'" style="width:'100%';height:200px;padding:20px"></div>--%>
        <textarea class="campoTextAreaPanel" id="txtTextoLibre" name="TextoLibre"
                cols="25" rows="12" style="width: 550px;"></textarea>
    </div>
</div>
<div id="dlgActaActualizarEstudio-buttons">
    <a href="#" class="easyui-linkbutton" iconcls="icon-pdf" id="btnExportarCartaPDFActaActualizarEstudio" >Imprimir Carta</a> 
    <a href="#" class="easyui-linkbutton" iconcls="icon-word" id="btnExportarCartaWORDActaActualizarEstudio">Exportar Carta a WORD</a>
    <a href="#" class="easyui-linkbutton" iconcls="icon-save" onclick="aceptarActaActualizarEstudio()">Guardar</a> 
    <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cerrarActaActualizarEstudio()">Cerrar</a>
</div>

<uc1:PlantillaSeleccionControl ID="PlantillaSeleccionControl1" runat="server" />

</asp:Content>
