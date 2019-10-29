﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="BandejaInicio.aspx.cs" Inherits="pome.SysGEIC.Web.BandejaInicio"
    StylesheetTheme="SkinSysGEIC" %>

<%@ Register src="Controles/NotaControl.ascx" tagname="NotaControl" tagprefix="uc1" %>

<%@ Register src="Controles/PlantillaSeleccionControl.ascx" tagname="PlantillaSeleccionControl" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/datagrid-detailview.js" type="text/javascript"></script>
    <script src="js/datagrid-groupview.js" type="text/javascript"></script>
    <script src="js_page/BandejaInicio.js" type="text/javascript"></script>
    <script src="js_page/BandejaInicio_Data.js" type="text/javascript"></script>
    <script src="js_page/Notas.js" type="text/javascript"></script>
    <script src="js_page/PlantillasCtrolSeleccion.js" type="text/javascript"></script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelEstudios" style="width: 35%; float: left;">
        <div class="easyui-tabs">
            <div title="Estudios Activos" style="padding: 1px;">
                <div id="tbEstudios" style="padding: 2px; height: auto">                    
                    <a id="btnEstudioBusqueda" href="#" class="easyui-linkbutton" iconCls="icon-search" plain="true">Búsqueda</a>
                    <a id="btnEstudioNuevo" href="EstudioCargaDatos.aspx?IdEstudio=-1" class="easyui-linkbutton" iconCls="icon-add" plain="true">Nuevo</a>
                    <a id="btnEstudioDetalle" href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true" >Detalle</a>
                    <asp:LinkButton ID="btnEstudioEliminar" OnClientClick="eliminarEstudioSeleccionado(); return false;"  runat="server" CssClass="easyui-linkbutton" iconCls="icon-cancel" plain="true" ></asp:LinkButton>
                    <div>
                        <input type="text" id="txtEstudioBuscar" class="campoTextBuscador" />
                        <div id="menuEstudioBuscar" style="width:250px">  
                            <div data-options="name:'Codigo'">Código</div>  
                            <div data-options="name:'NombreCompleto'">Estudio</div>  
                            <div data-options="name:'Abreviado'">Abreviado</div>  
                        </div>
                    </div>
                </div>
                <table id="dgEstudios" toolbar="#tbEstudios">
                </table>
            </div>            
        </div>
    </div>
    <div id="panelDocumentos" style="width: 65%; float: left;">
        <div id="tabsDatosEstudio" class="easyui-tabs">            
            <div title="Documentos" style="padding: 1px;">
                <div id="tbDocumentos" style="padding: 2px; height: auto">
                    <%--<a id="btnDocumentoBusqueda" href="#" class="easyui-linkbutton" iconCls="icon-search" plain="true">Búsqueda</a>--%>
                    <a id="btnDocumentoNuevo" href="DocumentoDatos.aspx?IdEstudio=-1" class="easyui-linkbutton" iconCls="icon-add" plain="true">Nuevo</a>
                    <a id="btnDocumentoDetalle" href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true">Detalle</a>
                    <a id="btnDocumentoAnular" href="#" class="easyui-linkbutton" iconCls="icon-remove" plain="true">Anular</a>
                    <a id="btnDocumentoReactivar" href="#" class="easyui-linkbutton" iconCls="icon-undo" plain="true">Reactivar</a>
                    <a id="btnDocumentoVerDocumento" href="#" class="easyui-linkbutton" iconCls="icon-view" plain="true">Ver adjunto</a>
                    <a id="btnDocumentoVersiones" href="#" class="easyui-linkbutton" iconCls="icon-ok" plain="true">Versiones</a>
                    <asp:LinkButton ID="btnDocumentoEliminar" OnClientClick="eliminarDocumentoSeleccionado(); return false;" runat="server" CssClass="easyui-linkbutton" iconCls="icon-cancel" plain="true" ></asp:LinkButton>
                    <a id="btnDocumentoExpandir" href="#" class="easyui-linkbutton" iconcls="icon-expand" plain="true">Expandir</a> 
                    <a id="btnDocumentoContraer" href="#" class="easyui-linkbutton" iconcls="icon-collapse" plain="true">Contraer</a>

                    <%--<a id="btnDocumentoComentarios" href="#" class="easyui-linkbutton" iconCls="icon-view" plain="true">Comentarios</a>
                    <a id="btnDocumentoRecordatorios" href="#" class="easyui-linkbutton" iconCls="icon-view" plain="true">Recordatorios</a>--%>
                    <div>
                        <input type="text" id="txtDocumentoBuscar" class="campoTextBuscador" />
                        <div id="menuDocumentoBuscar" style="width:250px">  
                            <div data-options="name:'TipoDocumento'">Tipo de Documento</div>  
                            <div data-options="name:'NombreDocumento'">Nombre Documento</div>                              
                        </div>
                    </div>
                </div>
                <table id="dgDocumentos" toolbar="#tbDocumentos">
                </table>
            </div>
            <div title="Semáforo por Tipo de Documento" style="padding: 1px;">
                <table id="dgTiposDocumentoSemaforo">
                </table>
            </div>
            <%--<div title="Recordatorios" style="padding: 1px;">
                <div id="toolbar_dgEstudioRecordatorios" style="padding: 5px; height: auto">
                    <div>
                        <div class="fitem">
                                Listar solo los recordatorios pendientes:
                            <input type="checkbox" id="chkRecordatorioPendiente" name="Pendiente" checked="checked" />
                        </div>            
                        <a href="#" id="btnRefrescar" name="btnRefrescarEstudioRecordatorios" class="easyui-linkbutton" iconcls="icon-reload" plain="true">Refrescar datos</a>
                    </div>
                </div>
                <table id="dgEstudioRecordatorios" toolbar="#toolbar_dgEstudioRecordatorios">
                </table>
            </div>--%>
            <div title="Actas" style="padding: 1px;">
                <div id="tbActas" style="padding: 2px; height: auto">
                    <%--<a id="btnActaBuscar" href="#" class="easyui-linkbutton" iconCls="icon-search" plain="true">Búsqueda avanzada</a>--%>
                    <a id="btnActaNuevo" href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true">Nuevo</a>
                    <a id="btnActaDetalle" href="#" class="easyui-linkbutton" iconCls="icon-edit" plain="true">Detalle</a>
                    <div>
                        <input type="text" id="txtActaBuscar" class="campoTextBuscador" />
                        <div id="menuActaBuscar" style="width:250px">  
                            <div data-options="name:'Descripcion'">Descripción</div>
                        </div>
                    </div>
                </div>
                <table id="dgActas" toolbar="#tbActas">
                </table>
            </div>
            <div title="Notas" style="padding: 1px;">|
                <table id="dgNotas">
                </table>
            </div>
        </div>
    </div>
    <%--<div id="panelMasInfo" style="width: 25%; float: left;">
        <div id="tabsMasInfo" class="easyui-tabs">            
            
        </div>
    </div>--%>
    
     <div id="dlgNota" class="easyui-dialog" style="width: 630px; height: 550px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgNota-buttons">
        <uc1:NotaControl ID="NotaControl" runat="server" />
     </div>
     <div id="dlgNota-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="guardarNota()">
            Guardar</a> 
        <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarNota()">
            Cancelar</a>         
    </div>    
    <uc2:PlantillaSeleccionControl ID="PlantillaSeleccionControl1" runat="server" />

    <div id="dlgDocumentoVersiones" class="easyui-dialog" style="width: 650px; height: 400px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgDocumentoVersiones-buttons">
        <div class="fitem">
            <label for="Estudio">Estudio:</label>
            <input class="campoABMLargo" type="text" id="txtDocumentoVersiones_Estudio" name="Estudio" disabled="disabled"></input>                               
        </div>
        <div class="fitem">
            <label for="Documento">Documento:</label>
            <input class="campoABMLargo" type="text" id="txtDocumentoVersiones_Documento" name="Documento" disabled="disabled"></input>                               
        </div>
        <table id="dgDocumentoVersiones">
        </table>
     </div>
     <div id="dlgDocumentoVersiones-buttons">
        <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cerrarDocumentoVersiones()">
            Cerrar</a>         
    </div> 

    <div id="dlgReactivarDocumento" class="easyui-dialog" style="width: 650px; height: 400px;
        padding: 10px 20px" closed="true" modal="true" buttons="#dlgReactivarDocumento-buttons">
        <div class="fitem">
            <label for="Estudio">Estudio:</label>
            <input class="campoABMLargo" type="text" id="txtReactivarDocumento_Estudio" name="Estudio" disabled="disabled"></input>                               
        </div>
        <table id="dgDocumentosAnulados">
        </table>
     </div>
     <div id="dlgReactivarDocumento-buttons">
		<a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="aceptarReactivarDocumento()">
            Reactivar</a>         
        <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="cancelarReactivarDocumento()">
            Cancelar</a>         
    </div> 

    <div id="dlgSeleccionTiposDocumentoSemaforo">
        <table id="dgSeleccionTispoDocumentoSemaforo">
        </table>
    </div>
</asp:Content>