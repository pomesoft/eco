<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="ActaOrdenarDocumentos.aspx.cs" Inherits="pome.SysGEIC.Web.ActaOrdenarDocumentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/datagrid-detailview.js" type="text/javascript"></script>
    <script src="js_page/ActaOrdenarDocumentos.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="toolbar" style="padding: 3px; width: 1190px; border: 1px solid #ccc">
        <a href="#" id="btnActaGuardar" class="easyui-linkbutton" iconcls="icon-save" plain="true">Guardar Acta</a> 
        <a href="#" id="btnActaVolver" class="easyui-linkbutton" iconcls="icon-back" plain="true">Volver</a>
        
    </div>
    
    <div id="panelInfoActa" style="padding-top: 5px;">
        <div class="fitem">
            <label for="Fecha">
                Fecha:</label>
            <input class="easyui-datebox" id="txtActaFecha" name="Fecha" data-options="formatter:formatterDateBox, parser:parserDateBox"></input>
            <label>
                Hora:</label><input class="easyui-timespinner" id="txtActaHora" style="width: 60px;"
                    data-options="min:'07:00', max: '22:00'" />
            <label>
                Descripción:</label><input class="easyui-validatebox campoABMLargo" type="text" id="txtActaDescripcion"
                    name="Descripcion" style="font-size: 14px"></input>
            <label>
                Cerrada:</label><input type="checkbox" id="chkCerrada" name="Cerrada" />
        </div>
    </div>
    
    <div id="panelDocumentos">
        <div id="panelIzquierdo" class="contenedorControles40">
            <table id="dgEstudios">
            </table>
        </div>
        <div id="panelDerecho" class="contenedorControles60">
            <table id="dgDocumentos">
            </table>
        </div>
    </div>
</asp:Content>
