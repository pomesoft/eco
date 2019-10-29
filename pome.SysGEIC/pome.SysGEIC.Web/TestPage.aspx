<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="pome.SysGEIC.Web.TestPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/jquery.texteditor.js" type="text/javascript"></script>
    <link href="css/texteditor.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <asp:TextBox ID="txtPlantillaImprimir" runat="server" Width="681px"></asp:TextBox>
&nbsp;
    <asp:Button ID="btnGenerarWord" runat="server" Text="Generar WORD" 
        onclick="btnGenerarWord_Click" />  
    &nbsp;
    <br />
    <br />
    <br />
    <asp:TextBox ID="txtHandlerEjecuta" runat="server" Width="681px">ExportWordHandler.ashx?accion=EXPORTARWORD_CARTARESPUESTA&amp;IdEstudio=79&amp;IdActa=64</asp:TextBox>
&nbsp;  
    <asp:Button ID="btnGenerarWord2" runat="server" Text="Generar WORD Handler" 
        onclick="btnGenerarWord2_Click" />


    <p>Documento: <span><strong>REPORTE DE EVENTOS ADVERSOS CIOMS</strong></span><br />
        Estado del documento:
        <select id="cboEstadoDocumento0">
            <option value="5" selected="selected">TOMA CONOCIMIENTO</option>
        </select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Imprimir en carta de respuesta
        <input type="checkbox" id="chkImprimirCarta0" name="ImprimirCarta" /></p>
    <div id="panelDocumentoComentario0">
        <div class="easyui-texteditor" id="txtComentario0" data-options="title:Comentario" style="height: 150px; padding: 10px;">
            2016SA183574-FU1-06-Jan-2017
Notificado por el comité 22-Mar-2017

        </div>
    </div>

</asp:Content>
