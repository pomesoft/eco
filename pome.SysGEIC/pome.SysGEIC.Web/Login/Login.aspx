<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="pome.SysGEIC.Web.Login.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SysGEIC - Inicio de sesión</title>
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../js/easyui-lang-es.js" type="text/javascript"></script>
    <script src="../js_page/Utiles.js" type="text/javascript"></script>
    <script src="../js_page/UtilesAJAX.js" type="text/javascript"></script>
    <link href="../css/Estilo.css" rel="stylesheet" type="text/css" />
    <link href="../css/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../css/icon.css" rel="stylesheet" type="text/css" />   
    
    <script language="javascript" type="text/javascript">        
        function mostrarCargandoLogin() {
            $('#dlgCargandoLogin').dialog('open').dialog('setTitle', '');
        }
    </script>
</head>
<body>
    <form id="formLogin" runat="server" defaultbutton="btnAceptar" 
    defaultfocus="txtLogin" submitdisabledcontrols="False">
    <div id="ContenedorPrincipal">
        <div id="panelLogin">
            <div class="easyui-panel" style="width: 660px; height: 360px; padding: 10px 10px"
                data-options="title:'Iniciar sesión'">
                
                <div class="ftitle" style="width:100%; vertical-align: middle; text-align: left;">                    
                    <img src="../img/login_icono.png" />
                    <span>Bienvenido, por favor ingrese usuario y contraseña</span>
                </div>                    
                
                <table id="tblLogin" cellspacing="1" >
                    <tr>
                        <td rowspan="2" style="margin-top: 20px; padding: 30px;">
                            <img src="../img/LogoLogin.png"/>
                        </td>
                        <td style="margin-top: 10px; padding: 10px;">
                            <div class="fitem">
                                <label for="Usuario" class="labelAncho250Izq ftitlelabel">
                                    Usuario:</label>
                                <asp:TextBox ID="txtLogin" runat="server" class="easyui-textbox" data-options="required:true" Width="220px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLogin" runat="server" 
                                    ControlToValidate="txtLogin" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            <div class="fitem">
                                <label for="Clave"  class="labelAncho250Izq  ftitlelabel">
                                    Contraseña:</label>
                                <asp:TextBox ID="txtClave" runat="server" class="easyui-textbox" data-options="required:true" Width="220px" TextMode="Password"></asp:TextBox>                                
                            </div>
                            <div class="fitem">
                                <asp:Label ID="lblMensaje" runat="server" CssClass="NotificaError"></asp:Label>
                            </div>                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="panelLogin-buttons" 
                                style="width:100%; vertical-align: middle; text-align: center;">
                                <asp:LinkButton ID="btnAceptar" runat="server" class="easyui-linkbutton ftitlelabel" iconcls="icon-ok"
                                    plain="true" OnClick="btnAceptar_Click" onclientclick="mostrarCargandoLogin()">Iniciar sesión</asp:LinkButton>
                                <%-- <asp:LinkButton ID="btnCancelar" runat="server" class="easyui-linkbutton" iconcls="icon-cancel"
                                    plain="true" OnClick="btnCancelar_Click">Cancelar</asp:LinkButton>--%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="dlgCargandoLogin" class="easyui-dialog" style="width: 600px; height: 180px;
        padding: 30px 20px" closed="true" modal="true">
        <div class="ftitle">                    
            <img src="../img/Loader.gif" />
                    Iniciando sesión, espere por favor...
        </div>                    
    </div>
    </form>
</body>
</html>
