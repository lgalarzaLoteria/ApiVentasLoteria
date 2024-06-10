using Microsoft.AspNetCore.Components.Forms;
using System.Transactions;

namespace ApiVentasLoteria.Models
{
    public class JuegosDTO
    {
        public class TransaccionesTradicionalesDTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Usuario cliente que realiza la consulta
            /// </summary>
            public string? UsuarioId { get; set; }
            /// <summary>
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string? token { get; set; }
            /// <summary>
            /// Operacion para transaccionar
            /// </summary>
            public string? operacion { get; set; }
            /// <summary>
            /// Identificación del medio del cliente
            /// </summary>
            public string? medioId { get; set; }
            /// <summary>
            /// Id del juego a consultar
            /// </summary>
            public string? juegoId { get; set; }
            /// <summary>
            /// Id del sorteo
            /// </summary>
            public string? sorteoId { get; set; }
            /// <summary>
            /// Combinacion
            /// </summary>
            public string? combinacion { get; set; }
            /// <summary>
            /// Combinacion
            /// </summary>
            public string? combinacionFigura { get; set; }
            /// <summary>
            /// Sugerir criterio de consulta
            /// </summary>
            public bool? sugerir { get; set; }
            /// <summary>
            /// Cantidad de juegos a mostrar
            /// </summary>
            public int? cantidad { get; set; }
            /// <summary>
            /// Cantidad de registros
            /// </summary>
            public int? registros { get; set; }
            /// <summary>
            /// Producto a vender, posibles valores: Pega3, Raspaditas, Tradicionales
            /// </summary>
            public string? productoVender { get; set; }


        }
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
        public class RecargaBet593DTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// Identificación abreviada del canal de venta: Está compuesto de tres letras mayúsculas
            /// </summary>
            public string canal { get; set; }
            /// <summary>
            /// Medio de venta definido para uso del servicio (valor constante). Otorgado por Lotería Nacional a la Cadena
            /// </summary>
            public int medioId { get; set; }
            /// <summary>
            /// Nombre del punto/local que hace la recarga (En caso de POS, se debe enviar el Id del punto de la suerte)
            /// </summary>
            public int puntooperacionid { get; set; }
            /// <summary>
            /// Número de Cédula del cliente al que se recarga un valor
            /// </summary>
            public string cuentaweb { get; set; }
            /// <summary>
            /// Número de recarga interna de Lotería Nacional, es el identificador único de la recarga realizada
            /// </summary>
            public string? recargaid { get; set; }
            /// <summary>
            /// Número de recarga externa, es el identificador correspondiente al proveedor externo de la página web de Lotería Nacional
            /// </summary>
            public string? serialnumber { get; set; }
            /// <summary>
            /// Valor de la recarga, con dos decimales (separador de decimales punto)
            /// </summary>
            public string? valor { get; set; }
            /// <summary>
            /// Número único de transacción
            /// </summary>
            public string? codigotrn { get; set; }
            /// <summary>
            /// Motivo del reverso
            /// </summary>
            public string? motivo { get; set; }

        }
        public class RespuestaRecargaBet593DTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string operacion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            /// <summary>
            /// Si es 0, indica que se completó la transacción
            /// </summary>
            public string resultado { get; set; }
            /// <summary>
            /// Número de Cédula del cliente al que se le realiza la recarga
            /// </summary>
            public string cuentaweb { get; set; }
            /// <summary>
            /// Nombre del cliente
            /// </summary>
            public string? nombre { get; set; }
            /// <summary>
            /// Apellido del cliente
            /// </summary>
            public string? apellido { get; set; }
            /// <summary>
            /// Tipo de documento de identificación
            /// </summary>
            public string? tipoDocumento { get; set; }
            /// <summary>
            /// Valor de la recarga, con dos decimales (separador de decimales punto)
            /// </summary>
            public string? valor { get; set; }
            /// <summary>
            /// Fecha y hora de la recarga
            /// </summary>
            public DateTime? fecharecarga { get; set; }
            /// <summary>
            /// Número de recarga interna de Lotería Nacional, es el identificador único de la recarga realizada
            /// </summary>
            public string? recargaid { get; set; }
            /// <summary>
            /// Número de recarga externa, es el identificador correspondiente al proveedor externo de la página web de Lotería Naciona
            /// </summary>
            public string? serialnumber { get; set; }
            /// <summary>
            /// Indica el estado de la transacción RECARGADA, CONFIRMADA, ANULADA
            /// </summary>
            public string? estado { get; set; }

        }
        public class RetiroBet593DTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Numero Ip del equipo que se conecta al servicio
            /// </summary>
            public string maquina { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            //public int codError { get; set; }
            ///// <summary>
            ///// Contiene la descripción del error solo en caso de existir
            ///// </summary>
            //public string? msgError { get; set; }
            /// <summary>
            /// RETIROOL, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string operacion { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// Usuario definido para uso del servicio (asociado al ClienteId). Usuario otorgado por Lotería Nacional a la Cadena
            /// </summary>
            public string UsuarioId { get; set; }
            /// <summary>
            /// Código de Cliente definido para uso del servicio (valor constante). Otorgado por Lotería Nacional a la Cadena
            /// </summary>
            public int ClienteId { get; set; }
            /// <summary>
            /// Medio de venta definido para uso del servicio (valor constante). Otorgado por Lotería Nacional a la Cadena
            /// </summary>
            public int MedioId { get; set; }
            /// <summary>
            ///Numero de transacción el cual debe ser único, sirve para identificar esta transacción de retiro. Generado por la Cadena
            /// </summary>
            public string NumeroTransaccion { get; set; }
            /// <summary>
            /// Número de identificación del usuario final que realizar el retiro (Cédula)
            /// </summary>
            public string Identificacion { get; set; }
            /// <summary>
            ///Número de retiro autorizado, el cual se verifica internamente para poder realizar el retiro. Numero otorgado por el Sitio Web al usuario final
            /// </summary>
            public string? NumeroRetiro { get; set; }
            /// <summary>
            ///Breve descripción del motivo del reverso
            /// </summary>
            public string? Motivo { get; set; }

        }
        public class RespuestaRetiroBet593DTO
        {
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string msgError { get; set; }
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string operacion { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// Número de recarga interna de Lotería Nacional, es el identificador único de la orden de pago del retiro realizado
            /// </summary>
            public int? ordenPagoId { get; set; }
            /// <summary>
            ///Número de cédula del cliente que realizó el retiro
            /// </summary>
            public string identificacion { get; set; }
            /// <summary>
            /// Valor o Monto que se retiró
            /// </summary>
            public string valor { get; set; }
            /// <summary>
            ///Número de trasnacción del retiro
            /// </summary>
            public string? numeroTransaccion { get; set; }
            /// <summary>
            ///Nombre de persona dueña de la Orden de Pago
            /// </summary>
            public string? nombre { get; set; }
            /// <summary>
            /// Fecha y hora del retiro
            /// </summary>
            public DateTime fecha { get; set; }
        }
        public class DetalleJuegosPorMedio
        {
            public string? juegoId { get; set; }
            public string? nombreJuego { get; set; }
            public bool? estadoJuego { get; set; }
            public bool? juegoVisible { get; set; }
            public string? sorteoId { get; set; }
            public string? nombreSorteo { get; set; }
            public string? nombreSalaSorteo { get; set; }
            public string? clase { get; set; }
            public string? pvp { get; set; }
            public int? cantidadFraccion { get; set; }
            public string? fechaSorteo { get; set; }
            public string? fechaCierreVenta { get; set; }
            public bool? seAcumula { get; set; }
            public string? montoProximoSorteo { get; set; }
            public string? valorPremio { get; set; }
            public bool? esSorteoDestacado { get; set; }
            public string? nombreSegundaCombinacion { get; set; }
            public string? nombreTerceraCombinacion { get; set; }
            public string? nombreCuartaCombinacion { get; set; }
            public string? nombreQuintaCombinacion { get; set; }
            public bool? tienePremioInstantaneo { get; set; }
            public string? tipoPremioPrimeraSuerte { get; set; }
            public string? nombrePrimeraSuerte { get; set; }
            public string? fechaCaducidadPremios { get; set; }
            public string? nombreNumeroCombinacionPrincipal { get; set; }
            public int? cantidadDigitosCombinacionPrincipal { get; set; }
            public int? cantidadDigitosCombinacionSecundaria { get; set; }
            public bool? tieneRevancha { get; set; }
            public string? juegoRevanchaId { get; set; }
            public string? sorteoRevanchaId { get; set; }
            public string? codigoImagen { get; set; }
            public string? descripcionImagen { get; set; }
            public string? abreviaturaImagen { get; set; }
            public bool? esPrimeraSuerte { get; set; }
            public string? combinacion { get; set; }
            public bool? esPremioEspecial { get; set; }
            public string? desprendible { get; set; }
            public string? identificadorPremio { get; set; }
        }
        public class JuegosPorMedioDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }           
            public List<DetalleJuegosPorMedio>? listaDetalle { get; set; }
        }
        public class DetalleFormasCobro
        {
            public string? codigo { get; set; }
            public string? abreviatura { get; set; }
            public string? descripcion { get; set; }
        }
        public class FormasCobroDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            public List<DetalleFormasCobro>? listaDetalle { get; set; }
        }
        public class DetalleNumerosDisponiblesPorCombinacion
        {
            public string? numero { get; set; }
            public string? numero2 { get; set; }
            public string? numero3 { get; set; }
            public string? numero4 { get; set; }
            public string? numero5 { get; set; }
            public int? cantidad { get; set; }
            public string? figura { get; set; }
            public string? Id { get; set; }
            public string? juegoId { get; set; }
            public string? sorteoId { get; set; }
            public string? fracciones { get; set; }
        }
        public class NumerosDisponiblesPorCombinacionDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            public List<DetalleNumerosDisponiblesPorCombinacion>? listaDetalle { get; set; }
        }
        public class Fraccion 
        {
            public string? fraccionId { get; set; }
        }
        public class Sorteo
        {
            public string? sorteoId { get; set; }
            public int? numeroCombinacionPrincipal { get; set; }
            public int? cantidadBoletosReservar { get; set; }
            public List<Fraccion>? listaFracciones { get; set; }
        }
        public class Juego
        {
            public string juegoId { get; set; }
            public List<Sorteo>? listaSorteos { get; set; }
        }
        public class ReservaBoletosTradicionalesDTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Usuario cliente que realiza la consulta
            /// </summary>
            public string? UsuarioId { get; set; }
            /// <summary>
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string? token { get; set; }
            /// <summary>
            /// Operacion para transaccionar
            /// </summary>
            public string? operacion { get; set; }
            /// <summary>
            /// Identificación del medio del cliente
            /// </summary>
            public string? medioId { get; set; }
            ///// <summary>
            ///// Id del juego a consultar
            ///// </summary>
            //public string? juegoId { get; set; }
            ///// <summary>
            ///// Id del sorteo
            ///// </summary>
            //public string? sorteoId { get; set; }
            /// <summary>
            /// Identificación del medio del cliente
            /// </summary>
            public string? reservaId { get; set; }
            /// <summary>
            /// Observacion
            /// </summary>
            public string? observacion { get; set; }
            /// <summary>
            /// Detalle de juegos, sorteos, fracciones
            /// </summary>
            public List<Juego>? listaJuegos { get; set; }
            /// <summary>
            /// Producto a vender, posibles valores: Pega3, Raspaditas, Tradicionales
            /// </summary>
            public string? productoVender { get; set; }


        }
        public class DetalleReserva
        {
            public string? reservaId { get; set; }
            public string? juegoId { get; set; }
            public string? sorteoId { get; set; }
            public string? medioId { get; set; }
            /// <summary>
            /// Combinación reservada
            /// </summary>
            public string? numeroCombinacionReservada { get; set; }
            /// <summary>
            /// Cantidad de fracciones reservadas de la combinación
            /// </summary>
            public int? cantidadFraccionesReservadas { get; set; }
            /// <summary>
            /// Numero de boletos reservados
            /// </summary>
            public int? numeroBoletosReservados { get; set; }
            public string? fraccionId { get; set; }
            public string? usuario { get; set; }
            public bool? vendida { get; set; }
            public bool? anulada { get; set; }
            public DateTime? fechaReserva { get; set; }
            public DateTime? fechaAnulacion { get; set; }
            public string? ventaId { get; set; }
            public string? observacion { get; set; }
            public string? id { get; set; }
        }
        public class RespuestaResevaBoletosTradcionalesDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            /// <summary>
            /// Identificador de la reserva
            /// </summary>
            public string reservaId { get; set; }

            public List<DetalleReserva>? listaDetalleReserva { get; set; }
        }
        public class RespuestaEliminarResevaBoletosTradcionalesDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            /// <summary>
            /// Identificador de la reserva
            /// </summary>
            public string returnValue { get; set; }

            
        }
        public class VentaBoletosTradicionalesDTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Usuario cliente que realiza la consulta
            /// </summary>
            public string? UsuarioId { get; set; }
            /// <summary>
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string? token { get; set; }
            /// <summary>
            /// Operacion para transaccionar
            /// </summary>
            public string? operacion { get; set; }
            /// <summary>
            /// Identificación del medio del cliente
            /// </summary>
            public string? medioId { get; set; }
            public string? reservaId { get; set; }
            public string? totalVenta { get; set; }
            public string? tipoIdentificacion { get; set; }
            public string? numeroIdentificacion { get; set; }
            public string? nombreComprador { get; set; }
            public string? codigoOrdenCompra { get; set; }
            public string? codigoFormaCobro { get; set; }
            public string? operadorTarjeta { get; set; }
            public string? codigoAutorizacion { get; set; }
            public string? totalVentaFormaCobro { get; set; }
            public string? valorICE { get; set; }
            public string? tipoCredito { get; set; }
            public string? valorInteres { get; set; }
            public string? mesesPlazo { get; set; }
            public string? codigoConfirmacion { get; set; }
            public string? numeroCedula { get; set; }
            public string? numeroCelular { get; set; }
            /// <summary>
            /// Detalle de juegos, sorteos, fracciones
            /// </summary>
            public List<Juego>? listaJuegos { get; set; }

            /// <summary>
            /// Producto a vender, posibles valores: Pega3, Raspaditas, Tradicionales
            /// </summary>
            public string? productoVender { get; set; }


        }
        public class RespuestaVentaC
        {
            public string? numeroFraccion { get; set; }
            public string? nombreCombinacion { get; set; }
        }
        public class RespuestaVentaR
        {
            public string? numeroVendido { get; set; }
            public int? cantidadFracciones { get; set; }
            public string? valorTotalNumeroVendido { get; set; }
            public List<RespuestaVentaC>? listaFracciones { get; set; }
        }
        public class RespuestaVentaSorteos
        {
            public string? juegoId { get; set; }
            public string? nombreJuego { get; set; }
            public string? sorteoId { get; set; }
            public string? nombreSorteo { get; set; }
            public DateTime? fechaSorteo { get; set; }
            public string? precioVentaPublico { get; set; }
            public string? premioMayorPrimeraSuerte { get; set; }
            public string? tipoPremioPrimeraSuerte { get; set; }
            public string? nombrePremioPrimeraSuerte { get; set; }
            public List<RespuestaVentaR>? listaNumerosVendidos{ get; set; }
        }
        public class RespuestaVentaSuerte
        {
            public string? numeroComprobanteSuerte { get; set; }
            public string? anuncio1 { get; set; }
            public string? anuncio2 { get; set; }
            public string? anuncio3 { get; set; }
            public List<RespuestaVentaSorteos>? listaVentaSorteos { get; set; }
        }
        public class RespuestaVentaRPremioInstantaneo
        {
            public string? numeroGanador { get; set; }
            public string? numeroSerie { get; set; }
            public string? fraccionGanadora { get; set; }
            public string? descripcionPremio { get; set; }
            public string? valorPremio { get; set; }
            public string? valorPremioConDescuento { get; set; }
            public string? tipoPremio { get; set; }
        }
        public class RespuestaVentaSorteosPremioInstantaneo
        {
            public string? juegoId { get; set; }
            public string? nombreJuego { get; set; }
            public string? sorteoId { get; set; }
            public string? nombreSorteo { get; set; }
            //public DateTime? fechaSorteo { get; set; }
            //public string? precioVentaPublico { get; set; }
            //public string? premioMayorPrimeraSuerte { get; set; }
            //public string? tipoPremioPrimeraSuerte { get; set; }
            //public string? nombrePrimeraSuerte { get; set; }
            public List<RespuestaVentaRPremioInstantaneo>? listaNumerosGanadoresPremioInstantaneo { get; set; }
        }
        public class RespuestaVentaPremioInstantaneo
        {
            public string? numeroComprobantePremioInstantaneo { get; set; }
            public string? mensaje { get; set; }

            public List<RespuestaVentaSorteosPremioInstantaneo>? listaVentaSorteosPremioInstantaneo { get; set; }
        }
        public class RespuestaVentaDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            public string? ventaId { get; set; }
            public string? tipoIdentificacion { get; set; }
            public string? numeroIdentificacion { get; set; }
            public string? nombreComprador { get; set; }
            public List<RespuestaVentaSuerte>? listaVentaSuerte { get; set; }
            public List<RespuestaVentaPremioInstantaneo>? listaPremiosInstantaneos { get; set; }
        }
        public class AgregarOrdenPagoDTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Usuario cliente que realiza la consulta
            /// </summary>
            public string? UsuarioId { get; set; }
            /// <summary>
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string? token { get; set; }
            /// <summary>
            /// Operacion para transaccionar
            /// </summary>
            public string? operacion { get; set; }
            /// <summary>
            /// Identificación del medio del cliente
            /// </summary>
            public string? medioId { get; set; }
            public bool? requiereTestimonio { get; set; }
            public string? tipoPremio { get; set; }
            public string? valorPagar { get; set; }
            public string? numeroRetiro { get; set; }
            public string? numeroTransaccionWeb { get; set; }
            public string? valorDescontadoWeb { get; set; }
            public string? juegoId { get; set; }
            public string? sorteoId { get; set; }
            public string? boletoId { get; set; }
            public string? premioId { get; set; }
            public string? identificadorVenta { get; set; }
        }
        public class RespuestaAgregarOrdenPagoDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            public string returnValue { get; set; }
        }
        public class RetirarOrdenPagoDTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Usuario cliente que realiza la consulta
            /// </summary>
            public string? UsuarioId { get; set; }
            /// <summary>
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string? token { get; set; }
            /// <summary>
            /// Operacion para transaccionar
            /// </summary>
            public string? operacion { get; set; }
            /// <summary>
            /// Identificación del medio del cliente
            /// </summary>
            public string? medioId { get; set; }
            public int ClienteId { get; set; }
            public string? numeroTransaccion { get; set; }
            public string? identificacion { get; set; }
            public string? numeroRetiro { get; set; }
            
        }
        public class RespuestaRetirarOrdenPagoDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            public string? returnValue { get; set; }
            public string? ordenPagoId { get; set; }
            public string? fechaPago { get; set; }
            public string? numeroIdentificacion { get; set; }
            public string? valorRetirado { get; set; }
        }
        public class AnularOrdenPagoDTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Usuario cliente que realiza la consulta
            /// </summary>
            public string? UsuarioId { get; set; }
            /// <summary>
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string? token { get; set; }
            /// <summary>
            /// Operacion para transaccionar
            /// </summary>
            public string? operacion { get; set; }
            /// <summary>
            /// Identificación del medio del cliente
            /// </summary>
            public string? medioId { get; set; }
            public int ClienteId { get; set; }
            public string? numeroTransaccion { get; set; }
            public string? motivo { get; set; }

        }
        public class RespuestaAnularOrdenPagoDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            public string? ordenPagoId { get; set; }
            public string? numeroIdentificacion { get; set; }
            public string? valorRetirado { get; set; }
            public string? numeroTransaccion { get; set; }
            public string? fechaAnulacion { get; set; }

        }
        public class ConsultaVentaRealizadaDTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Usuario cliente que realiza la consulta
            /// </summary>
            public string? UsuarioId { get; set; }
            /// <summary>
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string? token { get; set; }
            /// <summary>
            /// Operacion para transaccionar
            /// </summary>
            public string? operacion { get; set; }
            /// <summary>
            /// Identificación del medio del cliente
            /// </summary>
            public string? medioId { get; set; }
            public int ClienteId { get; set; }
            public string? numeroTransaccionWeb { get; set; }

        }
        public class RespuestaConsultaVentaRealizadaDTO
        {
            /// <summary>
            /// Usuario definido para uso del servicio
            /// </summary>
            public string usuario { get; set; }
            /// <summary>
            /// Token o ID de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// RECARGA593, valor constante el cual identifica que se realizará una recarga online a la cuenta de un cliente
            /// </summary>
            public string transaccion { get; set; }
            /// <summary>
            /// Si este campo es 0, indica que no hubo error en la ejecución, caso contrario devolverá el código de error que corresponda
            /// </summary>
            public int codError { get; set; }
            /// <summary>
            /// Contiene la descripción del error solo en caso de existir
            /// </summary>
            public string? msgError { get; set; }
            public string? ordenPagoId { get; set; }
            public string? numeroIdentificacion { get; set; }
            public string? valorRetirado { get; set; }
            public string? numeroTransaccion { get; set; }
            public string? fechaAnulacion { get; set; }

        }
    }
}
