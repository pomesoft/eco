$(document).ready(function () {
    
    var tasks = [
        cargarProximaActa,
        cargarGraficoDocumentosTratadosXMes,
        cargarGraficoEtudiosXEstado,
        cargarGraficoEtudiosSemaforo
    ];

    executeSequentially(tasks);
});




let executeSequentially = function(tasks) {
    if (tasks && tasks.length > 0) {
        var task = tasks.shift();

        return task().then(function() {
            return executeSequentially(tasks);
        });
    }

    return Promise.resolve();  
};



function cargarProximaActa () {
 
    return new Promise(function (resolve, reject)  {
        console.log('cargarProximaActa');

        parametros = {
            accion: 'OBTENER-PROXIMAACTA'
        };
        controladorAJAX_GET('handlers/PanelPrincipalHandler.ashx', parametros,
            function (data) {            
                if (data.resultado == 'Error') {
                    reject(data.mensaje);
                } else {
                    if (data && data.IdActa > 0) {
                        console.log(data);
                        $('#lblProximaActa').html(data.Fecha);
                        $('#btnProximaActa').attr('href', 'ActaDatosV2.aspx?PaginaReturn=PanelPrincipal&IdActa=' + data.IdActa);
                    }                
                    resolve();
                }
            }
        );
    });
}

function cargarGraficoEtudiosXEstado(valor) {

    return new Promise(function (resolve, reject)  {

        console.log('cargarGraficoEtudiosXEstado');

        parametros = {
            accion: 'CARGARGRAFICO-ESTUDIOSESTADOS'
        };
        controladorAJAX_GET('handlers/PanelPrincipalHandler.ashx', parametros,
            function (data) {
                if (data.resultado == 'Error') {
                    reject(data.mensaje);
                } else {
                    console.log(data);
                    var myChart = new Chart(document.getElementById("chartEstudioEstados"), {
                        type: 'pie',
                        data: {
                            labels: data.labels,
                            datasets: [{
                                backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                                data: data.values
                            }]
                        },
                        options: {
                            legend: {
                                display: true,
                                position: 'left'
                            }
                        }
                    });
                    resolve()
                }
            }
        );
    });
}


function cargarGraficoEtudiosSemaforo() {

    return new Promise(function (resolve, reject)  {

        console.log('cargarGraficoEtudiosSemaforo');

        parametros = {
            accion: 'CARGARGRAFICO-ESTUDIOSSEMAFORO'
        };
        controladorAJAX_GET('handlers/PanelPrincipalHandler.ashx', parametros,
            function (data) {
                if (data.resultado == 'Error') {
                    reject(data.mensaje);
                } else {
                    console.log(data);
                    var myChart = new Chart(document.getElementById("chartEstudioSemaforo"), {
                        type: 'pie',
                        data: {
                            labels: data.labels,
                            datasets: [{
                                backgroundColor: ["#35F222", "#F7F30C", "#F7300C"],
                                data: data.values
                            }]
                        },
                        options: {
                            legend: {
                                display: true,
                                position: 'left'
                            }
                        }
                    });

                    resolve();
                }
            }
        );
    });
}

function cargarGraficoDocumentosTratadosXMes() {

    return new Promise(function (resolve, reject) {

        console.log('cargarGraficoDocumentosTratadosXMes');

        parametros = {
            accion: 'CARGARGRAFICO-DOCSEVAUADOSMES'
        };
        controladorAJAX_GET('handlers/PanelPrincipalHandler.ashx', parametros,
            function (data) {
                if (data.resultado == 'Error') {
                    reject(data.mensaje);
                } else {
                    console.log(data);

                    var myChart = new Chart(document.getElementById("chartDocumentosXMes"), {
                        type: 'bar',
                        data: {
                            labels: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                            datasets: [{
                                label: 'INGRESADO',
                                backgroundColor: "#3e95cd",
                                borderColor: "#3e95cd",
                                data: data.valuesIngresado
                            }, {
                                label: 'EN EVALUACION',
                                backgroundColor: "#e8c3b9",
                                borderColor: "#e8c3b9",
                                data: data.valuesEnEvaluacion
                            }, {
                                label: 'APROBADO',
                                backgroundColor: "#3cba9f",
                                borderColor: "#3cba9f",
                                data: data.valuesAprobado
                            }, {
                                label: 'PEDIDO CAMBIO',
                                backgroundColor: "#c45850",
                                borderColor: "#c45850",
                                data: data.valuesPedidoCambio
                            }, {
                                label: 'TOMA CONOCIMIENTO',
                                backgroundColor: "#8e5ea2",
                                borderColor: "#8e5ea2",
                                data: data.valuesTomaConocimiento
                            }]
                        },
                        options: {
                            title: {
                                display: false,
                                text: 'Chart.js Bar Chart - Stacked'
                            },
                            tooltips: {
                                mode: 'index',
                                intersect: false
                            },
                            responsive: true,
                            scales: {
                                xAxes: [{
                                    stacked: true,
                                }],
                                yAxes: [{
                                    stacked: true,
                                    ticks: {
                                        min: 0,
                                        max: 120
                                    }
                                }]
                            }
                        }
                    });

                    resolve();
                }
            }
        );
    });
}
    



