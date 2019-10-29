using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pome.SysGEIC.Entidades
{
    public static class Constantes
    {
        public const string SaldoLinea = "&nbsp;<br />&nbsp;";

        public const int TipoProfesional_MiembroComite = 1;
        public const int TipoProfesional_Investigador = 2;

        public const int TipoPlantilla_TextoLibre = 1;
        public const int TipoPlantilla_CartaRespuesta = 2;
    }

    public enum NIVEL_ACCESO
    {
        SIN_ACCESO = 1,
        SOLO_LECTURA = 2,
        AGREGAR_EDITAR = 3,
        ELIMINAR = 4
    }

    public enum TIPOS_PLANTILLA
    {
        TEXTO_LIBRE = 1,
        CARTA_RESPUESTA = 2,
        ACTA = 3,
        APROBACION_ESTUDIO = 4
    }

    public enum EXPORT_TO
    {
        PDF_ = 1,
        WORD_ = 2,
        HTML_ = 3
    }

    public enum ESTADO_SEMAFORO
    {
        ROJO = 1,
        AMARILLO = 2,
        VERDE = 3
    }

    public enum TIPO_DOCUMENTO_GRUPO
    {
        SE_EVALUA = 1,
        TOMA_CONOCIMIENTO = 2
    }
}
