using Microsoft.AspNetCore.Components.Forms;
using System.Transactions;

namespace ApiVentasLoteria.Models
{
    public class JuegosDTO
    {
        public class CrearTicketPega3RequerimientoDTO 
        {
            /// <summary>
            /// Id del dispositvo asignado al comercio
            /// </summary>
            public string deviceId { get; set; }
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// Producto a vender, posibles valores: Pega3
            /// </summary>
            public string productoVender { get; set; }
            /// <summary>
            /// Identificador único de transacción de operación de venta
            /// </summary>
            public string customerSessionId { get; set; }
            /// <summary>
            /// Total de la apuesta
            /// </summary>
            public decimal costo { get; set; }
            /// <summary>
            /// Tipo de apuesta
            /// </summary>
            public string entryType { get; set; }
            /// <summary>
            /// Canal de venta, por ejemplo Terminal
            /// </summary>
            public string channel { get; set; }
            /// <summary>
            /// Clase en la cual se transportan las caracterísicas aplicadas en la apuesta para el juego seleccionado
            /// </summary>
            public mainGame mainGame { get; set; }

        }
        public class entries
        {
            /// <summary>
            /// Tipo de entrada
            /// </summary>
            public int? type { get; set; }
            /// <summary>
            /// Selección de juego
            /// </summary>
            public bool? quickPick { get; set; }
            /// <summary>
            /// Tipos de juego
            /// </summary>
            public string[] playTypes { get; set; }
            /// <summary>
            /// Combinaciones para jugar
            /// </summary>
            public int[]? value { get; set; }
        }
        public class panels
        {
            /// <summary>
            /// Tipo de apuesta
            /// </summary>
            public string betType { get; set; }
            /// <summary>
            /// Tipo de entrada
            /// </summary>
            public int typeOfEntry { get; set; }
            /// <summary>
            /// Monto de apuesta
            /// </summary>
            public decimal betAmount { get; set; }
            /// <summary>
            /// Número de chances se usa para cuando la opción es QuickPick
            /// </summary>
            public int? chances { get; set; }
            /// <summary>
            /// Lista de entradas
            /// </summary>
            public IList<entries> entries { get; set; }

        }
        public class addOns
        {
            /// <summary>
            /// Codigo de complemento
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// Valor de complemento posibles valores verdadero o falso
            /// </summary>
            public bool value { get; set; }
            /// <summary>
            /// Número de sorteo
            /// </summary>
            public int noOfDraws { get; set; }
        }
        public class mainGame
        {
            /// <summary>
            /// Código de juego principal
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// Sorteo anticipado
            /// </summary>
            public int advanceDraw { get; set; }
            /// <summary>
            /// Número de sorteo
            /// </summary>
            public int noOfDraws { get; set; }
            /// <summary>
            /// Lista de paneles
            /// </summary>
            public IList<panels> panels { get; set; }
            /// <summary>
            /// Lista de complementos
            /// </summary>
            public IList<addOns> addOns { get; set; }

        }
        public class PagarTicketRequerimientoDTO
        {
            /// <summary>
            /// Id del dispositvo asignado al comercio
            /// </summary>
            public string? deviceId { get; set; }
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// Producto a vender, posibles valores: Pega3
            /// </summary>
            public string productoVender { get; set; }
            /// <summary>
            /// Número de ticket vendido
            /// </summary>
            public string ticketNumber { get; set; }
            /// <summary>
            /// Monto a pagar por premio
            /// </summary>
            public decimal? amount { get; set; }
            /// <summary>
            /// Fecha de juego a validar relacionada al ticket
            /// </summary>
            public string? validationDate { get; set; }
            /// <summary>
            /// Indica si se puede validar más de una vez el ticket
            /// </summary>
            public Boolean? reinquire { get; set; }
            /// <summary>
            /// Id de la transacción para pagar el ticket
            /// </summary>
            public string? transactionID { get; set; }
            /// <summary>
            /// ID de usuario que devuelve el inicio de sesión
            /// </summary>
            public int? userID { get; set; }
            public string? customerSessionId { get; set; }

        }
        public class ConsultarTicketDTO
        {
            /// <summary>
            /// Id del dispositvo o Usuario asignado al comercio
            /// </summary>
            public string? deviceId { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// ID de usuario que devuelve el inicio de sesión
            /// </summary>
            public string? userId { get; set; }
            /// <summary>
            /// Producto a vender, posibles valores: Pega3
            /// </summary>
            public string productoVender { get; set; }
            /// <summary>
            /// Número de ticket vendido
            /// </summary>
            public string? ticketNumber { get; set; }
            /// <summary>
            /// Monto a pagar por premio
            /// </summary>
            public decimal? amount { get; set; }
            /// <summary>
            /// Código de transacción cliente - canal
            /// </summary>
            public string? MOR { get; set; }
            /// <summary>
            /// Tipo de documento de identificación, posibles valores  1 - RUC; 2 - Cédula; 3 - Pasaporte
            /// </summary>
            public int? tipoDocumento { get; set;}
            /// <summary>
            /// Número de identificación
            /// </summary>
            public string? numeroDocumento { get; set; }
            /// <summary>
            /// Nombres y apellidos del ganador
            /// </summary>
            public string? nombreGanador { get; set; }
            /// <summary>
            /// Código de barras del boleto tradicional
            /// </summary>
            public string? CB { get; set; }
            /// <summary>
            /// Código de barras de boleto electrónico
            /// </summary>
            public string? CL { get; set; }
            /// <summary>
            /// Clave de raspe del boleto tradicional preimpreso
            /// </summary>
            public string? CO { get; set; }
        }
        public class ConsultarTicketRaspaditasDTO
        {
            /// <summary>
            /// Número de ticket vendido
            /// </summary>
            public string TicketBarcode { get; set; }
            /// <summary>
            /// ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string SecurityID { get; set; }
            /// <summary>
            /// ID de usuario que devuelve el inicio de sesión
            /// </summary>
            public int UserID { get; set; }
            /// <summary>
            /// Siempre 0
            /// </summary>
            public int TC { get; set; } = 0;
            /// <summary>
            /// Siempre ""
            /// </summary>
            public string SC { get; set; } = "";
        }
        public class PagarTicketRaspaditasDTO
        {
            /// <summary>
            /// Número de ticket vendido
            /// </summary>
            public string TicketBarcode { get; set; }
            /// <summary>
            /// Fecha del ticket a validar
            /// </summary>
            public string ValidationDate { get; set; }
            /// <summary>
            /// Indica si se puede validar más de una vez el ticket
            /// </summary>
            public Boolean reinquire { get; set; }
            /// <summary>
            /// Id de la transacción para pagar el ticket
            /// </summary>
            public string TransactionID { get; set; }
            /// <summary>
            /// ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string SecurityID { get; set; }
            /// <summary>
            /// ID de usuario que devuelve el inicio de sesión
            /// </summary>
            public int UserID { get; set; }
            /// <summary>
            /// Siempre 0
            /// </summary>
            public int TC { get; set; } = 0;
            /// <summary>
            /// Siempre ""
            /// </summary>
            public string SC { get; set; } = "";
        }
        public class BprDTO
        {
            /// <summary>
            /// Código de barras del boleto tradicional
            /// </summary>
            public string? COB { get; set; }
            /// <summary>
            /// Clave de venta del boleto electrónico
            /// </summary>
            public string? CLA { get; set; }
            /// <summary>
            /// Id del juego
            /// </summary>
            public string? JID { get; set;}
            /// <summary>
            /// Nombre del juego
            /// </summary>
            public string? JNO { get; set;}
            /// <summary>
            /// Número del sorteo
            /// </summary>
            public string? SID { get; set;}
            /// <summary>
            /// Nombre del premio
            /// </summary>
            public string? PRE1 { get; set;}
            /// <summary>
            /// Monto del premio
            /// </summary>
            public decimal? MON1 { get; set;}
            /// <summary>
            /// Valor de descuento del premio
            /// </summary>
            public decimal? VDE { get; set;}
            /// <summary>
            /// Flag que indica si el boleto es válido
            /// </summary>
            public string VAL { get; set;}
            /// <summary>
            /// Mensaje de error
            /// </summary>
            public string MEN { get; set;}
            public string? COD { get; set; }
        }
        public class PprDTO
        {
            /// <summary>
            /// Id del movimiento de Pago
            /// </summary>
            public int MPI { get; set; }
            /// <summary>
            /// Id de referencia interna
            /// </summary>
            public int REF { get; set; }
            /// <summary>
            /// Lista de la clase BPR
            /// </summary>
            public IList<BprDTO> BPR { get; set; }
        }
        public class ResultadoDTO
        {
            /// <summary>
            /// Clase PPR y su contenido
            /// </summary>
            public PprDTO PPR { get; set; }
        }
        public class RespuestaConsultarTicketTradicionalDTO
        {
            /// <summary>
            /// Código de error que devuelve el servicio
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Mensaje de error que devuelve el servicio en caso de que hubiere
            /// </summary>
            public string? msgError { get; set; }
            /// <summary>
            /// Usuario establecido para login en MT
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Id de Transacción enviada
            /// </summary>
            public int transaccion { get; set; }
            /// <summary>
            /// Valor devuelto por la transacción de login
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// Clase resultado y su contenido
            /// </summary>
            public ResultadoDTO resultado { get; set; }
        }



    }
}
