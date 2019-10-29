<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true" CodeBehind="BandejaInicioActas.aspx.cs" Inherits="pome.SysGEIC.Web.BandejaInicioActas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/datagrid-detailview.js" type="text/javascript"></script>
    <script src="js_page/BandejaInicioActas.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelActas" style="width: 25%; float: left;">
        <div class="easyui-tabs">
            <div title="Actas" style="padding: 1px;">
                <div id="tbActas" style="padding: 2px; height: auto">
                    <%--<a id="btnActaBuscar" href="#" class="easyui-linkbutton" iconCls="icon-search" plain="true">Búsqueda avanzada</a>--%>
                    <a id="btnActaNuevo" href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true">Nuevo</a>
                    <a id="btnActaDetalle" href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true">Detalle</a>
                    <div>
                        <input type="text" id="txtActaBuscar" class="campoTextBuscador" />
                        <div id="menuActaBuscar" style="width:250px">
                            <div data-options="name:'Abiertas'">Actas sin Cerrar</div>
                            <div data-options="name:'Cerradas'">Actas Cerradas</div>
                            <div data-options="name:'Descripcion'">Descripción</div>
                        </div>
                    </div>
                </div>
                <table id="dgActas" toolbar="#tbActas">
                </table>
            </div>            
        </div>
    </div>
    
    <div id="panelEstudios" style="width: 25%; float: left;">
        <div class="easyui-tabs">
            <div title="Estudios Tratados" style="padding: 1px;">
                <div id="tbEstudios" style="padding: 2px; height: auto">                    
                    <a id="btnEstudioBusqueda" href="#" class="easyui-linkbutton" iconCls="icon-search" plain="true">Búsqueda</a>
                    <%--<a id="btnEstudioNuevo" href="EstudioCargaDatos.aspx?IdEstudio=-1" class="easyui-linkbutton" iconCls="icon-add" plain="true">Nuevo</a>--%>
                    <a id="btnEstudioDetalle" href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true" >Detalle</a>                    
                    <%--<div>
                        <input type="text" id="txtEstudioBuscar" class="campoTextBuscador" />
                        <div id="menuEstudioBuscar" style="width:250px">  
                            <div data-options="name:'Codigo'">Código</div>  
                            <div data-options="name:'NombreCompleto'">Estudio</div>  
                            <div data-options="name:'Abreviado'">Abreviado</div>  
                        </div>
                    </div>--%>
                </div>
                <table id="dgEstudios" toolbar="#tbEstudios">
                </table>
            </div>            
        </div>
    </div>

    <div id="panelDocumentos" style="width: 50%; float: left;">
        <div id="tabsDatosEstudio" class="easyui-tabs">            
            <div title="Documentos Tratados" style="padding: 1px;">
                <div id="tbDocumentos" style="padding: 2px; height: auto">
                    <a id="btnDocumentoBusqueda" href="#" class="easyui-linkbutton" iconCls="icon-search" plain="true">Búsqueda</a>
                    <a id="btnDocumentoDetalle" href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true">Detalle</a>
                    <%--<div>
                        <input type="text" id="txtDocumentoBuscar" class="campoTextBuscador" />
                        <div id="menuDocumentoBuscar" style="width:250px">  
                            <div data-options="name:'TipoDocumento'">Tipo de Documento</div>  
                            <div data-options="name:'NombreDocumento'">Nombre Documento</div>                              
                        </div>
                    </div>--%>
                </div>
                <table id="dgDocumentos" toolbar="#tbDocumentos">
                </table>
            </div>            
        </div>
    </div>
</asp:Content>
