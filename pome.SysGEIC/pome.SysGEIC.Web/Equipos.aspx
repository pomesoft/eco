<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="Equipos.aspx.cs" Inherits="pome.SysGEIC.Web.Equipos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/Equipos.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelDatos" style="width: 50%; float: left;">
        <div id="dg_toolbar" style="padding: 5px; height: auto">
            <div>
                Descripción:
                <input type="text" id="txtDescripcionBuscar" name="DescripcionBuscar" class="campoABM"></input>
                <a href="#" id="A1" name="btnBuscar" class="easyui-linkbutton" iconcls="icon-search"
                    plain="true">Buscar</a>
            </div>
        </div>
        <table id="dg" toolbar="#dg_toolbar">
        </table>
        <div id="toolbar" style="padding: 3px; border: 1px solid #ccc">
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="nuevo()">
                Nuevo</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                    onclick="eliminar()">Eliminar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-ok"
                        plain="true" onclick="guardar()">Guardar</a> <a href="#" class="easyui-linkbutton"
                            iconcls="icon-cancel" plain="true" onclick="cancelar()">Cancelar</a>
        </div>
        <fieldset>
            <div class="fitem">
                <label for="txtDescripcion">
                    Descripción:</label>
                <input class="easyui-validatebox campoABM" type="text" id="txtDescripcion" name="Descripcion"
                    data-options="required:true"></input>
            </div>
        </fieldset>
    </div>
    <div id="panelIntegrantes" style="width: 50%; float: left;">
        <table id="dgIntegrantes">
        </table>
        <div id="toolbarIntegrantes" style="padding: 3px; border: 1px solid #ccc">
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="agregarIntegrante()">
                Agregar Integrante</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                    onclick="eliminarIntegrante()">Eliminar Integrante</a> 
        </div>
        <fieldset>            
            <div class="fitem">
                <label for="Rol">
                    Rol:</label>
                <asp:DropDownList ID="cboRol" runat="server" CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
            <div class="fitem">
                <label for="Profesional">
                    Profesional:</label>
                <asp:DropDownList ID="cboProfesional" runat="server" CssClass="easyui-combobox" Width="455px">
                </asp:DropDownList>
            </div>
        </fieldset>        
    </div>
</asp:Content>
