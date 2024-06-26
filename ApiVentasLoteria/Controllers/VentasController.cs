﻿using Microsoft.AspNetCore.Mvc;
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
        /// Recupera las opciones de los juegos que vende Lotería Nacional
        /// entre ellos tenemos Pega3, Lotto, etc
        /// </summary>
        /// <param name="entradaDTO"></param>
        /// <returns></returns>
        [HttpPost("VentaProductos")]
        public async Task<ActionResult> VentaProductos(LoginDTO entradaDTO)
        {
            string respuesta = string.Empty;
            switch (entradaDTO.productoVender)
            {
                case "Pega3":
                    {
                        respuesta = await _juegos.ObtieneOpcionesPega3(entradaDTO);
                    }
                    break;
            }
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
            switch (entradaDTO.productoVender)
            {
                case "Pega3":
                    {
                        respuesta = await _juegos.CrearTicketPega3(entradaDTO);
                    }
                    break;
                case "Pega4":
                    {
                        //respuesta = await _juegos.CrearTicketPega3(entradaDTO);
                    }
                    break;
            }
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
                case "Pega3":
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
            switch (entradaDTO.productoVender)
            {
                case "Pega3":
                    {
                        respuesta = await _juegos.ConsultarVentasPega3();
                    }
                    break;
            }
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
    }
}
