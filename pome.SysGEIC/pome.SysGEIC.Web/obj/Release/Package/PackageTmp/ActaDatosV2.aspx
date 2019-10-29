<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true" CodeBehind="ActaDatosV2.aspx.cs" Inherits="pome.SysGEIC.Web.ActaDatosV2" %>

<%@ Register Src="Controles/PlantillaSeleccionControl.ascx" TagName="PlantillaSeleccionControl" TagPrefix="uc1" %>
<%@ Register Src="Controles/NotaControl.ascx" TagName="NotaControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/datagrid-detailview.js" type="text/javascript"></script>
    <script src="js/datagrid-filter.js" type="text/javascript"></script>
    <script src="js/jquery.texteditor.js" type="text/javascript"></script>
    <script src="js_page/ActasV2.js" type="text/javascript"></script>
    <script src="js_page/PlantillasCtrolSeleccion.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="toolbar" style="padding: 3px; width: 100%; border: 1px solid #ccc">
        <a href="#" id="btnActaGuardar" class="easyui-linkbutton" iconcls="icon-save" plain="true"
            onclick="guardarActa()">Guardar Acta</a> <a href="#" id="btnActaImprimirActa" class="easyui-linkbutton"
                iconcls="icon-pdf" plain="true">Imprimir Acta</a> <a href="#" id="btnActaExportarWord"
                    class="easyui-linkbutton" iconcls="icon-word" plain="true">Exportar Acta a Word</a>
        <a href="#" id="btnActaVolver" class="easyui-linkbutton" iconcls="icon-back" plain="true"
            onclick="volver()">Volver</a>
    </div>
    <div id="panelInfoActa" style="padding-top: 10px;">
        <div class="fitem">
            <label for="Fecha">
                Fecha:</label>
            <input class="easyui-datebox" id="txtActaFecha" name="Fecha" style="width: 120px;"
                data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            <label>
                Hora:</label><input class="easyui-timespinner" id="txtActaHora" style="width: 60px;"
                    data-options="min:'07:00', max: '22:00'" />
            <label>
                Descripción:</label><input class="easyui-textbox campoABMLargo" type="text" id="txtActaDescripcion"
                    name="Descripcion" style="font-size: 14px"></input>
            <label>
                Cerrada:</label>
            <input class="easyui-switchbutton" id="chkActaCerrada" type="checkbox" />
        </div>


        <div id="panelComentariosParticipantes">
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
        </div>

        <div id="panelEstudios" class="contenedorControles20">
            <table id="dgActaEstudios">
            </table>
        </div>
        <div class="contenedorControles80">
            <div id="panelDatosEstudio">
                <div class="datagrid-toolbar">
                    <a id="btnEstudioGuardar" href="#" class="easyui-linkbutton" iconcls="icon-save" plain="true">Guardar</a>
                    <a id="btnEstudioExportarCartaPDF" href="#" class="easyui-linkbutton" iconcls="icon-pdf" plain="true">Imprimir Carta</a>
                    <a id="btnEstudioExportarCartaWORD" href="#" class="easyui-linkbutton" iconcls="icon-word" plain="true">Exportar Carta a WORD</a>                    
                </div>

                <div id="tituloEstudio" class="titulo"></div>
                
                <div class="contenedorControlesAnchoFull">
                    <div class="contenedorControlesTercio">
                        <div class="fitem">
                            <input class="easyui-combobox" id="cboActaEstadosEstudio" name="cboActaEstadosEstudio" label="Estado:" labelposition="left" style="width: 90%;" />
                        </div>
                    </div>
                    <div class="contenedorControles2Tercios">
                        <div class="fitem">
                            <input class="easyui-combobox" id="cboActaCartaRespuestaModelo" name="cboActaCartaRespuestaModelo" label="Modelo carta:" labelposition="left" style="width: 90%;" />
                        </div>
                    </div>
                </div>
                
                <div id="panelNotaCartaRespuesta">
                    <textarea class="campoTextAreaPanel" id="txtNotaCartaRespuesta" name="NotaCartaRespuesta"
                        cols="20" rows="6"></textarea>
                </div>
                <div id="panelNotaAntesDocsTratados">
                    <textarea class="campoTextAreaPanel" id="txtComentarioAntesDocumentos" name="NotaAntesDocumentos"
                        cols="20" rows="6"></textarea>
                </div>
            </div>
            <table id="dgActaDocumentos">
            </table>
            <div id="panelNotaDespuesDocsTratados">
                <textarea class="campoTextAreaPanel" id="txtComentarioDespuesDocumentos" name="NotaDespuesDocumentos"
                    cols="20" rows="6"></textarea>
            </div>
        
        </div>
    </div>

    <uc1:PlantillaSeleccionControl ID="PlantillaSeleccionControl1" runat="server" />

</asp:Content>
