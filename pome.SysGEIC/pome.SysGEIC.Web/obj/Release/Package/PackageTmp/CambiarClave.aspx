<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/CambiarClave.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="toolbar" style="padding: 3px; width: 1190px; border: 1px solid #ccc">
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok" plain="true" onclick="cambiarClave()">
            Cambiar</a> 
    </div>
    <div id="panelIzquierdo" class="contenedorControlesMitad">
        <div class="panel datagrid" style="width: 400px; border: 1px solid gray;
            padding-bottom: 2px;">
            <div class="panel-header " style="width: 100%;">
                <div class="panel-title">
                    Cambiar contraseña</div>
            </div>
            <div class="fitem">
                <label for="Apellido">Apellido:</label>
                <input class="campoABMsinTransform" type="text" id="txtApellido" name="Apellido" disabled="disabled"></input>
            </div>
            <div class="fitem">
                <label for="Nombre">Nombre:</label>
                <input class="campoABMsinTransform" type="text" id="txtNombre" name="Nombre" disabled="disabled"></input>
            </div>
            <div class="fitem">
                <label for="Login">Login:</label>
                <input class="campoABMsinTransform" type="text" id="txtLogin" name="Login" disabled="disabled"></input>
            </div>
            <div class="fitem">
                <label class="labelAncho" for="ClaveActual">Contraseña actual:</label>
                <asp:TextBox ID="txtClaveActual" runat="server" Font-Size="11px" 
                    TextMode="Password" Width="210px" Text=""></asp:TextBox>
            </div>
            <div class="fitem">
                <label class="labelAncho" for="NuevaClave1">Nueva contraseña:</label>
                <asp:TextBox ID="txtNuevaClave1" runat="server" Font-Size="11px" 
                    TextMode="Password" Width="210px" Text=""></asp:TextBox>
            </div>
            <div class="fitem">
                <label class="labelAncho" for="NuevaClave2">Repetir contraseña:</label>
                <asp:TextBox ID="txtNuevaClave2" runat="server" Font-Size="11px" 
                    TextMode="Password" Width="210px" Text=""></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>
