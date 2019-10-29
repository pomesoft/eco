<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true" CodeBehind="PanelPrincipal.aspx.cs" Inherits="pome.SysGEIC.Web.PanelPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/Chart.min.js" type="text/javascript"></script>
    <script src="js/Chart.bundle.min.js" type="text/javascript"></script>
    <script src="js_page/PanelPrincipal.js" type="text/javascript"></script>
    <script src="js_page/RecordatoriosPopup.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div class="card p-2">
        <div class="row">
            <div class="col-xl-4 col-md-6 mb-4">
                <div class="card border-left-success shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">Próxima reunión del Comité</div>
                                <div id="lblProximaActa" class="h5 mb-0 font-weight-bold text-gray-800"> 
                                </div>
                            </div>
                            <div class="col-auto">
                                <a id="btnProximaActa" href="#" ><i class="fas fa-file-import fa-2x text-gray-300"></i></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-4 col-md-6 mb-4">
                <div class="card border-left-success shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                    <a href="ActaCargaDocumentosV2.aspx" >
                                        Recepción de Documentos a Evaluar
                                    </a>
                                </div>
                            </div>
                            <div class="col-auto">
                                <a href="ActaCargaDocumentosV2.aspx" >
                                    <i class="fas fa-file-upload fa-2x text-gray-300"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-4 col-md-6 mb-4">
                <div class="card border-left-warning shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                    <a href="#" onclick="mostrarPopupRecordatorios();">
                                        Recordatorios y alertas pendientes
                                     </a>
                                </div>
                            </div>
                            <div class="col-auto">
                                <a href="#" onclick="mostrarPopupRecordatorios();">
                                    <i class="fas fa-tasks fa-2x text-gray-300"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="card shadow mb-2">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">Estudios Vigentes</h6>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="chart-pie">
                                    <h6 class="m-0 font-weight-bold text-primary">Semáforo</h6>
                                    <canvas id="chartEstudioSemaforo"></canvas>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="chart-pie">
                                    <h6 class="m-0 font-weight-bold text-primary">Ranking por Estado</h6>
                                    <canvas id="chartEstudioEstados"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="card shadow mb-2">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">Documentos evaluados por mes</h6>
                    </div>
                    <div class="card-body">
                        <div class="chart-areabig">
                            <canvas id="chartDocumentosXMes"></canvas>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
