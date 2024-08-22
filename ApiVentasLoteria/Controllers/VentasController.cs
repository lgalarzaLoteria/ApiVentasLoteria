using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static ApiVentasLoteria.Models.SeguridadDTO;
using ApiVentasLoteria.Data;
using NuGet.Protocol.Plugins;
using RestSharp;
using static ApiVentasLoteria.Models.JuegosDTO;

namespace ApiVentasLoteria.Controllers
{
    /// <summary>
    /// API de venta para productos de Lotería Nacional
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly JuegosData _juegos;
        private readonly SeguridadData _seguridad;

        public VentasController(JuegosData repositorio, SeguridadData seguridadData)
        {
            _juegos = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _seguridad = seguridadData ?? throw new ArgumentNullException(nameof(seguridadData));
        }

        /// <summary>
        /// Recupera los datos para trabajo de entorno de aplicación
        /// como el token, las funciones para armar el menú de la interfaz
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDTO entradaDTO)
        {
            string respuesta = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega2":
                case "Pega3":
                case "Pega4":
                    {
                        respuesta = await _seguridad.LoginPega3(entradaDTO);
                    }
                    break;
                case "Raspaditas":
                    {
                        respuesta = await _seguridad.LoginRaspaditas(entradaDTO);
                    }
                    break;
                case "Tradicionales":
                    {
                        respuesta = await _seguridad.LoginTradicionales(entradaDTO);
                    }
                    break;
                case "Bet593":
                    {
                        respuesta = await _seguridad.LoginBet593(entradaDTO);
                    }
                    break;
            }
            return Ok(respuesta);
        }
        /// <summary>
        /// Desconecta al usuario de la sesión activa
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<ActionResult> Logout(LoginDTO entradaDTO)
        {
            string respuesta = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega3":
                    {
                        respuesta = await _seguridad.LogoutPega3(entradaDTO);
                    }
                    break;
                case "Raspaditas":
                    {
                        respuesta = await _seguridad.LogoutRaspaditas(entradaDTO);
                    }
                    break;
            }
            return Ok(respuesta);
        }
        /// <summary>
        /// Cambia la clave del usuario
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("CambiarClave")]
        public async Task<ActionResult> CambiarClave(LoginDTO entradaDTO)
        {
            string respuesta = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Tradicionales":
                    {
                        respuesta = await _seguridad.CambiarClaveTradicionales(entradaDTO);
                    }
                    break;
            }
            return Ok(respuesta);
        }
        /// <summary>
        /// Recupera las opciones de los juegos que vende Lotería Nacional
        /// entre ellos tenemos Pega3, Lotto, etc
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("VentaProductos")]
        public async Task<ActionResult> VentaProductos(LoginDTO entradaDTO)
        {
            string respuesta = string.Empty;
            string codigoJuego = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega2":
                    {
                        codigoJuego = "1003";
                    }
                    break;
                case "Pega3":
                    {
                        codigoJuego = "1001";
                    }
                    break;
                case "Pega4":
                    {
                        codigoJuego = "1002";
                    }
                    break;
            }
            respuesta = await _juegos.ObtieneOpcionesPega(entradaDTO, codigoJuego);
            return Ok(respuesta);
        }
        /// <summary>
        /// Crea un ticket electrónico para el producto que se está vendiendo
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("CrearTicket")]
        public async Task<ActionResult> CrearTicket(CrearTicketPega3RequerimientoDTO entradaDTO)
        {
            string respuesta = string.Empty;
            //switch (entradaDTO.productoVender)
            //{
            //    case "Pega3":
            //        {

            //        }
            //        break;
            //    case "Pega4":
            //        {
            //            respuesta = await _juegos.CrearTicketPega(entradaDTO);
            //        }
            //        break;
            //}
            respuesta = await _juegos.CrearTicketPega(entradaDTO);
            return Ok(respuesta);
        }
        /// <summary>
        /// Consulta un ticket electrónico vendido
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ConsultarTicket")]
        public async Task<ActionResult> ConsultarTicket(ConsultarTicketDTO entradaDTO)
        {
            string respuesta = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega2":
                case "Pega3":
                case "Pega4":
                    {
                        respuesta = await _juegos.ConsultarTicketPega3(entradaDTO);
                    }
                    break;
                case "Raspaditas":
                    {
                        respuesta = await _juegos.ConsultarTicketRaspaditas(entradaDTO);
                    }
                    break;
                case "Tradicionales":
                    {
                        respuesta = await _juegos.ConsultarTicketTradicional(entradaDTO);
                    }
                    break;
            }
            return Ok(respuesta);
        }
        /// <summary>
        /// Paga un ticket electrónico vendido
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("PagarTicket")]
        public async Task<ActionResult> PagarTicket(PagarTicketRequerimientoDTO entradaDTO)
        {
            string respuesta = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega3":
                case "Pega4":  
                    {
                        respuesta = await _juegos.PagarTicketPega3(entradaDTO);
                    }
                    break;
                case "Raspaditas":
                    {
                        respuesta = await _juegos.PagarTicketRaspaditas(entradaDTO);
                    }
                    break;
            }
            return Ok(respuesta);
        }
        /// <summary>
        /// Cancela un ticket electrónico vendido
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("CancelarTicket")]
        public async Task<ActionResult> CancelarTicket(PagarTicketRequerimientoDTO entradaDTO)
        {
            string respuesta = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega3":
                case "Pega4":
                    {
                        respuesta = await _juegos.CancelarTicketPega3(entradaDTO);
                    }
                    break;
            }
            return Ok(respuesta);
        }
        /// <summary>
        /// Consulta las ventas de Pega3 realizadas por un comercio específico,
        /// se consulta por rango de fechas
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpGet("ConsultarVentas")]
        public async Task<ActionResult> ConsultarVentas(CrearTicketPega3RequerimientoDTO entradaDTO)
        {
            string respuesta = string.Empty;
            //switch (entradaDTO.productoVender)
            //{
            //    case "Pega3":
            //        {
            //            respuesta = await _juegos.ConsultarVentasPega3();
            //        }
            //        break;
            //}
            return Ok(respuesta);
        }
        /// <summary>
        /// Recupera el último sorteos del juego el status siempre es Open
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ObtieneUltimosSorteosxJuego")]
        public async Task<ActionResult> ObtieneUltimosSorteosxJuego(LoginDTO entradaDTO)
        {
            string respuesta = string.Empty;
            string codigoJuego = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega2":
                    {
                        codigoJuego = "1003";
                    }
                    break;
                case "Pega3":
                    {
                        codigoJuego = "1001";
                    }
                    break;
                case "Pega4":
                    {
                        codigoJuego = "1002";
                    }
                    break;
            }
            respuesta = await _juegos.ObtieneUltimosSorteosxJuego(entradaDTO, codigoJuego);
            return Ok(respuesta);
        }
        /// <summary>
        /// Obtiene el sorteo activo del juego el estado es SaleOpen
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ObtieneSorteosActivoxJuego")]
        public async Task<ActionResult> ObtieneSorteosActivoxJuego(LoginDTO entradaDTO)
        {
            string respuesta = string.Empty;
            string codigoJuego = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega2":
                    {
                        codigoJuego = "1003";
                    }
                    break;
                case "Pega3":
                    {
                        codigoJuego = "1001";
                    }
                    break;
                case "Pega4":
                    {
                        codigoJuego = "1002";
                    }
                    break;
            }
            respuesta = await _juegos.ObtieneSorteosActivoxJuego(entradaDTO, codigoJuego);
            return Ok(respuesta);
        }
        /// <summary>
        /// Recarga electrónica de Bet593
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RecargarBet593")]
        public async Task<ActionResult> RecargarBet593(RecargaBet593DTO entradaDTO)
        {
           return Ok(await _juegos.RecargarBet593(entradaDTO));
        }
        /// <summary>
        /// Confirma recarga electrónica de Bet593
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ConfirmarBet593")]
        public async Task<ActionResult> ConfirmarBet593(RecargaBet593DTO entradaDTO)
        {
            return Ok(await _juegos.ConfirmarBet593(entradaDTO));
        }
        /// <summary>
        /// Valida recarga electrónica de Bet593
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ValidarBet593")]
        public async Task<ActionResult> ValidarBet593(RecargaBet593DTO entradaDTO)
        {
            return Ok(await _juegos.ValidarBet593(entradaDTO));
        }
        /// <summary>
        /// Revesa recarga electrónica de Bet593
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ReversarBet593")]
        public async Task<ActionResult> ReversarBet593(RecargaBet593DTO entradaDTO)
        {
            return Ok(await _juegos.ReversarBet593(entradaDTO));
        }
        /// <summary>
        /// Retiro de Bet593
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RetirarBet593")]
        public async Task<ActionResult> RetirarBet593(RetiroBet593DTO entradaDTO)
        {
            return Ok(await _juegos.RetirarBet593(entradaDTO));
        }
        /// <summary>
        /// Reverso de retiro de Bet593
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ReversarRetiroBet593")]
        public async Task<ActionResult> ReversarRetiroBet593(RetiroBet593DTO entradaDTO)
        {
            return Ok(await _juegos.ReversarRetiroBet593(entradaDTO));
        }
        /// <summary>
        /// Consulta retiro de Bet593
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ConsultarRetiroBet593")]
        public async Task<ActionResult> ConsultarRetiroBet593(RetiroBet593DTO entradaDTO)
        {
            return Ok(await _juegos.ConsultarRetiroBet593(entradaDTO));
        }
        /// <summary>
        /// Recupera los juegos configurados para el medio de venta
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RecuperarJuegosPorMedio")]
        public async Task<ActionResult> RecuperarJuegosPorMedio(TransaccionesTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.RecuperarJuegosPorMedio(entradaDTO));
        }
        /// <summary>
        /// Recupera los sorteos disponibles para cada juego, si se envía el id juego en 0 trae todos los sorteos disponibles
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RecuperarSorteosDisponibles")]
        public async Task<ActionResult> RecuperarSorteosDisponibles(TransaccionesTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.RecuperarSorteosDisponibles(entradaDTO));
        }
        /// <summary>
        /// Recupera las figuras por juego
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RecuperarFigurasPorJuego")]
        public async Task<ActionResult> RecuperarFigurasPorJuego(TransaccionesTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.RecuperarFigurasPorJuego(entradaDTO));
        }
        /// <summary>
        /// Recupera los sorteos jugados
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RecuperarSorteosJugados")]
        public async Task<ActionResult> RecuperarSorteosJugados(TransaccionesTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.RecuperarSorteosJugados(entradaDTO));
        }
        /// <summary>
        /// Recupera las suertes principales de un juego
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RecuperarSuertesPrincipales")]
        public async Task<ActionResult> RecuperarSuertesPrincipales(TransaccionesTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.RecuperarSuertesPrincipales(entradaDTO));
        }
        /// <summary>
        /// Recupera las formas de cobro
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RecuperarFormasCobro")]
        public async Task<ActionResult> RecuperarFormasCobro(TransaccionesTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.RecuperarFormasCobro(entradaDTO));
        }
        /// <summary>
        /// Recupera los números disponibles por combinación
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RecuperarNumerosDisponiblesPorCombinacion")]
        public async Task<ActionResult> RecuperarNumerosDisponiblesPorCombinacion(TransaccionesTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.RecuperarNumerosDisponiblesPorCombinacion(entradaDTO));
        }
        /// <summary>
        /// Realiza la reserva de boletos de productos tradicionales
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ReservarBoletos")]
        public async Task<ActionResult> ReservarBoletos(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.ReservarBoletos(entradaDTO));
        }
        /// <summary>
        /// Realiza la consulta de boletos reservados
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ConsultarBoletosReservados")]
        public async Task<ActionResult> ConsultarBoletosReservados(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.ConsultarBoletosReservados(entradaDTO));
        }
        /// <summary>
        /// Elimina boletos reservados
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("EliminarBoletosReservados")]
        public async Task<ActionResult> EliminarBoletosReservados(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.EliminarBoletosReservados(entradaDTO));
        }
        /// <summary>
        /// Actualiza fecha, hora y usuario boletos reservados
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ActualizarUsuarioFechaHoraReserva")]
        public async Task<ActionResult> ActualizarUsuarioFechaHoraReserva(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.ActualizarUsuarioFechaHoraReserva(entradaDTO));
        }
        /// <summary>
        /// Venta de boletos
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("VentaBoletos")]
        public async Task<ActionResult> VentaBoletos(VentaBoletosTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.VentaBoletos(entradaDTO));
        }
        /// <summary>
        /// Realiza la consulta de última reserva activa
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("ConsultarUltimaReservaActiva")]
        public async Task<ActionResult> ConsultarUltimaReservaActiva(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            return Ok(await _juegos.ConsultarUltimaReservaActiva(entradaDTO));
        }
        /// <summary>
        /// Agrega la forma de pago del premio
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("AgregarOrdenPago")]
        public async Task<ActionResult> AgregarOrdenPago(AgregarOrdenPagoDTO entradaDTO)
        {
            return Ok(await _juegos.AgregarOrdenPago(entradaDTO));
        }
        /// <summary>
        /// Cobrar el premio
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("RetirarOrdenPago")]
        public async Task<ActionResult> RetirarOrdenPago(RetirarOrdenPagoDTO entradaDTO)
        {
            return Ok(await _juegos.RetirarOrdenPago(entradaDTO));
        }
        /// <summary>
        /// Anular orden pago
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("AnularOrdenPago")]
        public async Task<ActionResult> AnularOrdenPago(AnularOrdenPagoDTO entradaDTO)
        {
            return Ok(await _juegos.AnularOrdenPago(entradaDTO));
        }
    }
}
