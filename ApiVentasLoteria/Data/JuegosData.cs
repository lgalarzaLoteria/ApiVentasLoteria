using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using System.Xml.Linq;
using RestSharp;
using static ApiVentasLoteria.Models.JuegosDTO;
using static ApiVentasLoteria.Models.SeguridadDTO;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.IO.Hashing;
using NuGet.Protocol;
using System.Drawing.Drawing2D;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;


namespace ApiVentasLoteria.Data
{
    public class JuegosData
    {
        private readonly IConfigurationSection _urlJuegos;
        private readonly IConfigurationSection _tradicionales;

        public JuegosData(IConfiguration configuration, SeguridadData repositorio)
        {
            _urlJuegos = configuration.GetSection("ScientificGames");
            _tradicionales = configuration.GetSection("Tradicionales");
        }
        

        #region Pega
        public async Task<string> ObtieneOpcionesPega(LoginDTO entradaDTO, string codigoJuego)
        {
            try 
            {
                string urlMetodo = _urlJuegos.GetValue<string>("urlOpcionesPega3") + codigoJuego;
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Get;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("token", entradaDTO.token);
                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                return respuesta.Content;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> CrearTicketPega(CrearTicketPega3RequerimientoDTO entradaDTO)
        {
            string respuestaJson = string.Empty;
            try
            {
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(entradaDTO);
                string urlMetodo = _urlJuegos.GetValue<string>("urlOperacionesTicket");
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("token", entradaDTO.token);
                requerimiento.AddHeader("deviceId", entradaDTO.deviceId);
                requerimiento.AddHeader("customerSessionId", entradaDTO.customerSessionId);
                requerimiento.AddBody(jsonRequest);

                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                
                if(respuesta.ResponseStatus==ResponseStatus.Error)
                {
                    dynamic responseContent = JsonConvert.DeserializeObject(respuesta.Content);
                    respuestaJson = JsonConvert.SerializeObject(responseContent);
                }
                else
                {
                    if(respuesta.ResponseStatus==ResponseStatus.Completed)
                    {
                        dynamic responseContent = JsonConvert.DeserializeObject(respuesta.Content);
                        string numeroTicketsinHash = responseContent.SelectToken("gameTicketNumber");
                        if (numeroTicketsinHash == null || numeroTicketsinHash == string.Empty)
                            throw new Exception("Error al general el ticket");

                        string numeroTicket = AddHash(numeroTicketsinHash);

                        responseContent.gameTicketNumber = numeroTicket;
                        respuestaJson = JsonConvert.SerializeObject(responseContent);

                    }
                }

                //dynamic responseContent = JsonConvert.DeserializeObject(respuesta.Content);
                //string numeroTicketsinHash = responseContent.SelectToken("gameTicketNumber");
                //if (numeroTicketsinHash==null || numeroTicketsinHash==string.Empty)
                //    throw new Exception("Error al general el ticket");

                //string numeroTicket = AddHash(numeroTicketsinHash);

                //responseContent.gameTicketNumber = numeroTicket;
                //var respuestaJson = JsonConvert.SerializeObject(responseContent);

                return respuestaJson;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }
        public async Task<string> ConsultarTicketPega3(ConsultarTicketDTO entradaDTO)
        {
            try
            {
                string numeroticketEnviar = entradaDTO.ticketNumber.Substring(0, 24);
                string urlMetodo = _urlJuegos.GetValue<string>("urlOperacionesTicket") + numeroticketEnviar + "?showParticipations=true";
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Get;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("token", entradaDTO.token);
                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> PagarTicketPega3(PagarTicketRequerimientoDTO entradaDTO)
        {
            try
            {
               var jsonRequest = System.Text.Json.JsonSerializer.Serialize(entradaDTO);
                string urlMetodo = _urlJuegos.GetValue<string>("urlOperacionesTicket") + entradaDTO.ticketNumber + "/claim";
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("token", entradaDTO.token);
                requerimiento.AddHeader("deviceId", entradaDTO.deviceId);
                requerimiento.AddBody(jsonRequest);
                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> CancelarTicketPega3(PagarTicketRequerimientoDTO entradaDTO)
        {
            try
            {
                string numeroticketEnviar = entradaDTO.ticketNumber.Substring(0, 24);
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(entradaDTO);
                string urlMetodo = _urlJuegos.GetValue<string>("urlOperacionesTicket") + numeroticketEnviar + "/cancel";
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("token", entradaDTO.token);
                requerimiento.AddHeader("customerSessionId", entradaDTO.customerSessionId);
                requerimiento.AddHeader("deviceId", entradaDTO.deviceId);
                requerimiento.AddBody(jsonRequest);
                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ObtieneUltimosSorteosxJuego(LoginDTO entradaDTO, string codigoJuego)
        {
            try
            {
                string urlMetodo = string.Concat(_urlJuegos.GetValue<string>("urlOpcionesPega3"),codigoJuego, "/draws/latest");
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Get;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("token", entradaDTO.token);
                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ObtieneSorteosActivoxJuego(LoginDTO entradaDTO, string codigoJuego)
        {
            try
            {
                string urlMetodo = string.Concat(_urlJuegos.GetValue<string>("urlOpcionesPega3"), codigoJuego, "/draws/active");
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Get;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("token", entradaDTO.token);
                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        #endregion
        #region Raspaditas
        public async Task<string> ConsultarTicketRaspaditas(ConsultarTicketDTO entradaDTO)
        {
            try
            {
                ConsultarTicketRaspaditasDTO dtoConsulta = new ConsultarTicketRaspaditasDTO();
                dtoConsulta.TicketBarcode = entradaDTO.ticketNumber;
                dtoConsulta.UserID = int.Parse(entradaDTO.userId.ToString());
                dtoConsulta.SecurityID = entradaDTO.token;
                //Serializo el DTO
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(dtoConsulta);

                string urlMetodo = _urlJuegos.GetValue<string>("urlConsultarTicketRaspaditas");
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddBody(jsonRequest);

                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> PagarTicketRaspaditas(PagarTicketRequerimientoDTO entradaDTO)
        {
            try
            {
                PagarTicketRaspaditasDTO datoEnviar = new PagarTicketRaspaditasDTO();
                datoEnviar.TicketBarcode = entradaDTO.ticketNumber;
                datoEnviar.ValidationDate = entradaDTO.validationDate;
                datoEnviar.reinquire = bool.Parse(entradaDTO.reinquire.ToString());
                datoEnviar.TransactionID = entradaDTO.transactionID;
                datoEnviar.SecurityID = entradaDTO.token;
                datoEnviar.UserID = int.Parse(entradaDTO.userID.ToString());
                //Serializo el DTO
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(datoEnviar);

                string urlMetodo = _urlJuegos.GetValue<string>("urlPagarTicketRaspaditas");
                var apiScientificGames = new RestClient(urlMetodo);
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddBody(jsonRequest);

                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);
                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        #endregion
        #region Tradicionales
        public async Task<string> ConsultarTicketTradicional(ConsultarTicketDTO entradaDTO)
        {
            try
            {
                string lineaBO = string.Empty;
                if (entradaDTO.CB != null)
                    lineaBO = "<BO CB='" + entradaDTO.CB + "' />";
                else
                {
                    if (entradaDTO.CO != null)
                        lineaBO = "<BO CO='" + entradaDTO.CO + "' />";
                }

                string lineaBE = string.Empty;
                if (entradaDTO.CL != null)
                    lineaBE = "<BE CL='" + entradaDTO.CL + "'/>";

                string lineaPP = string.Empty;
                if (lineaBO == string.Empty)
                {
                    if (entradaDTO.nombreGanador == null || entradaDTO.nombreGanador == string.Empty)
                        lineaPP = "<PP TD='" + entradaDTO.tipoDocumento + "' CE='" + entradaDTO.numeroDocumento + "' NO='NA'>";
                    else
                        lineaPP = "<PP TD='" + entradaDTO.tipoDocumento + "' CE='" + entradaDTO.numeroDocumento + "' NO='" + entradaDTO.nombreGanador + "'>";
                }
                else
                {
                    lineaPP = "<PP TD='" + 2 + "' CE='" + string.Empty + "' NO='NA'>";
                }

                XDocument documentoTransaccionXML = new XDocument();

                if (lineaPP != string.Empty)
                {
                    //Se arma la data para generar y enviar el XML
                    documentoTransaccionXML = XDocument.Parse("" +
                    "<mt>" +
                        "<c>" +
                            "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                            "<transaccion>" + _tradicionales.GetValue<string>("transaccion") + "</transaccion>" +
                            "<usuario>" + entradaDTO.userId + "</usuario>" +
                            "<maquina>0.0.0.0</maquina>" +
                            "<codError>0</codError>" +
                            "<msgError />" +
                            "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                            "<token>" + entradaDTO.token + "</token>" +
                            "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                        "</c>" +
                        "<i>" +
                            "<UsuarioId>" + entradaDTO.userId + "</UsuarioId>" +
                            "<ClienteId>" + _tradicionales.GetValue<string>("clienteId") + "</ClienteId>" +
                            "<MedioId>" + _tradicionales.GetValue<string>("medio") + "</MedioId>" +
                            "<ParametroEntrada>" +
                               "<TRN MOR='" + entradaDTO.MOR + "' USU='" + entradaDTO.userId + "' TRN='" + _tradicionales.GetValue<string>("transaccionValidarBoleto") + "'>" +
                                  "<PAR>" +
                                      lineaPP +
                                         lineaBO +
                                         lineaBE +
                                     "</PP>" +
                                  "</PAR>" +
                               "</TRN>" +
                            "</ParametroEntrada>" +
                        "</i>" +
                    "</mt>"
                    );
                }
                else
                {
                    //Se arma la data para generar y enviar el XML
                    documentoTransaccionXML = XDocument.Parse("" +
                    "<mt>" +
                        "<c>" +
                            "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                            "<transaccion>" + _tradicionales.GetValue<string>("transaccion") + "</transaccion>" +
                            "<usuario>" + entradaDTO.userId + "</usuario>" +
                            "<maquina>0.0.0.0</maquina>" +
                            "<codError>0</codError>" +
                            "<msgError />" +
                            "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                            "<token>" + entradaDTO.token + "</token>" +
                            "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                        "</c>" +
                        "<i>" +
                            "<UsuarioId>" + entradaDTO.userId + "</UsuarioId>" +
                            "<ClienteId>" + _tradicionales.GetValue<string>("clienteId") + "</ClienteId>" +
                            "<MedioId>" + _tradicionales.GetValue<string>("medio") + "</MedioId>" +
                            "<ParametroEntrada>" +
                               "<TRN MOR='" + entradaDTO.MOR + "' USU='" + entradaDTO.userId + "' TRN='" + _tradicionales.GetValue<string>("transaccionValidarBoleto") + "'>" +
                                  "<PAR>" +
                                      lineaPP +
                                         lineaBO +
                                         lineaBE +
                                     //"</PP>" +
                                  "</PAR>" +
                               "</TRN>" +
                            "</ParametroEntrada>" +
                        "</i>" +
                    "</mt>"
                    );
                }
                

                //servicioMTDesa.ServicioMTClient servicioMT = new servicioMTDesa.ServicioMTClient();
                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaConsultarTicketTradicionalDTO datoDevolver = new RespuestaConsultarTicketTradicionalDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;

                if(datoDevolver.codError.Equals(0))
                {
                    datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                    datoDevolver.transaccion = int.Parse(((XElement)listaNodos[3]).Value);
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;

                    var nodoO = xmlRespuesta.Descendants("o");
                    var listaNodosO = nodoO.Nodes().ToList();
                    var listaNodosResultado = ((XElement)listaNodosO[0]).Nodes().ToList();
                    string detalleResultado = ((XText)listaNodosResultado[0]).Value;
                    XDocument xmlResultado = XDocument.Parse(detalleResultado);
                    var nodoPPR = xmlResultado.Descendants("PPR");
                    PprDTO ppr = new PprDTO();
                    var strPPR = XElement.Parse(nodoPPR.FirstOrDefault().ToString());
                    var MPI = strPPR.Attribute("MPI").Value;
                    var REF = strPPR.Attribute("REF").Value;
                    ppr.MPI = int.Parse(MPI.ToString());
                    ppr.REF = int.Parse(REF.ToString());
                    var listaNodosPPR = nodoPPR.Nodes().ToList();
                    ppr.BPR = new List<BprDTO>();
                    foreach(var item in listaNodosPPR)
                    {
                        BprDTO datoBPR = new BprDTO();
                        var strBPR = XElement.Parse(item.ToString());
                        if(strBPR.Attribute("COD") != null)
                            datoBPR.COD = strBPR.Attribute("COD").Value;
                        if(strBPR.Attribute("CLA") != null)
                          datoBPR.CLA = strBPR.Attribute("CLA").Value;
                        if(strBPR.Attribute("JID") != null)
                          datoBPR.COD = strBPR.Attribute("JID").Value;
                        if(strBPR.Attribute("JNO") != null)
                          datoBPR.JNO = strBPR.Attribute("JNO").Value;
                        if(strBPR.Attribute("SID") != null)
                          datoBPR.SID = strBPR.Attribute("SID").Value;
                        if (strBPR.Attribute("PRE1") != null)
                            datoBPR.PRE1 = strBPR.Attribute("PRE1").Value;
                        if (strBPR.Attribute("MON1") != null)
                            datoBPR.MON1 = decimal.Parse(strBPR.Attribute("MON1").Value);
                        if (strBPR.Attribute("VDE") != null)
                            datoBPR.VDE = decimal.Parse(strBPR.Attribute("VDE").Value);
                        if (strBPR.Attribute("VAL") != null)
                          datoBPR.VAL = strBPR.Attribute("VAL").Value;
                        if(strBPR.Attribute("MEN") != null)
                          datoBPR.MEN = strBPR.Attribute("MEN").Value;
                        ppr.BPR.Add(datoBPR);
                    }
                    ResultadoDTO resultadoDTO = new ResultadoDTO();
                    resultadoDTO.PPR = ppr;
                    datoDevolver.resultado = resultadoDTO;
                }

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RecuperarJuegosPorMedio(TransaccionesTradicionalesDTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<mt>" +
                    "<c>" +
                        "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                        "<transaccion>" + _tradicionales.GetValue<string>("transaccionRecuperarJuegosPorMedio") + "</transaccion>" +
                        "<usuario>" + entradaDTO.UserName + "</usuario>" +
                        "<maquina>0.0.0.0</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                    "</c>" +
                    "<i>" +
                        "<MedioId>" + entradaDTO.medioId + "</MedioId>" +
                    "</i>" +
                "</mt>"
                );


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                JuegosPorMedioDTO datoDevolver = new JuegosPorMedioDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalle = new List<DetalleJuegosPorMedio>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleJuegosPorMedio detalle = new DetalleJuegosPorMedio();
                            detalle.juegoId = ((XElement)itemNodo).Attribute("JId").Value;
                            detalle.nombreJuego = ((XElement)itemNodo).Attribute("Nomb").Value;
                            detalle.estadoJuego = Convert.ToBoolean(((XElement)itemNodo).Attribute("Act").Value);
                            detalle.juegoVisible = Convert.ToBoolean(((XElement)itemNodo).Attribute("Visible").Value);
                            datoDevolver.listaDetalle.Add(detalle);
                        }
                       
                    }
                }

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RecuperarSorteosDisponibles(TransaccionesTradicionalesDTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<mt>" +
                    "<c>" +
                        "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                        "<transaccion>" + _tradicionales.GetValue<string>("transaccionRecuperarJuegosPorMedio") + "</transaccion>" +
                        "<usuario>" + entradaDTO.UserName + "</usuario>" +
                        "<maquina>0.0.0.0</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                    "</c>" +
                    "<i>" +
                        "<MedioId>" + entradaDTO.medioId + "</MedioId>" +
                        "<JuegoId>" + entradaDTO.juegoId + "</JuegoId>" +
                    "</i>" +
                "</mt>"
                );


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                JuegosPorMedioDTO datoDevolver = new JuegosPorMedioDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalle = new List<DetalleJuegosPorMedio>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleJuegosPorMedio detalle = new DetalleJuegosPorMedio();
                            detalle.juegoId = ((XElement)itemNodo).Attribute("JId").Value;
                            detalle.nombreJuego = ((XElement)itemNodo).Attribute("JNomb").Value;
                            detalle.sorteoId = ((XElement)itemNodo).Attribute("SortId").Value;
                            detalle.nombreSorteo = ((XElement)itemNodo).Attribute("SortNomb").Value;
                            detalle.nombreSalaSorteo = ((XElement)itemNodo).Attribute("SortNombSala").Value;
                            detalle.clase = ((XElement)itemNodo).Attribute("SortNombSala").Value;
                            detalle.pvp = ((XElement)itemNodo).Attribute("PVP").Value;
                            detalle.cantidadFraccion = Convert.ToInt32(((XElement)itemNodo).Attribute("CFrac").Value);
                            detalle.fechaSorteo = ((XElement)itemNodo).Attribute("FSort").Value;
                            detalle.fechaCierreVenta = ((XElement)itemNodo).Attribute("CieVta").Value;
                            detalle.seAcumula = Convert.ToBoolean(((XElement)itemNodo).Attribute("SeAcum").Value);
                            detalle.montoProximoSorteo = ((XElement)itemNodo).Attribute("MProxSort").Value;
                            detalle.valorPremio = ((XElement)itemNodo).Attribute("VPremio").Value;
                            detalle.esSorteoDestacado = Convert.ToBoolean(((XElement)itemNodo).Attribute("Destcdo").Value);
                            detalle.nombreSegundaCombinacion = ((XElement)itemNodo).Attribute("NomComb2").Value;
                            detalle.nombreTerceraCombinacion = ((XElement)itemNodo).Attribute("NomComb3").Value;
                            detalle.nombreCuartaCombinacion = ((XElement)itemNodo).Attribute("NomComb4").Value;
                            detalle.nombreQuintaCombinacion = ((XElement)itemNodo).Attribute("NomComb5").Value;
                            detalle.tienePremioInstantaneo = Convert.ToBoolean(((XElement)itemNodo).Attribute("TieneInst").Value);
                            detalle.tipoPremioPrimeraSuerte = ((XElement)itemNodo).Attribute("TPremPS").Value;
                            detalle.nombrePrimeraSuerte = ((XElement)itemNodo).Attribute("NPremPS").Value;
                            detalle.cantidadDigitosCombinacionPrincipal = Convert.ToInt32(((XElement)itemNodo).Attribute("CDigCombPrin").Value);
                            
                            if(((XElement)itemNodo).Attribute("CDigCombSec").Value!=null)
                              detalle.cantidadDigitosCombinacionSecundaria = Convert.ToInt32(((XElement)itemNodo).Attribute("CDigCombSec").Value);

                            if(((XElement)itemNodo).Attribute("TieneRevancha").Value!=null)
                            {
                                detalle.tieneRevancha = Convert.ToBoolean(((XElement)itemNodo).Attribute("TieneRevancha").Value);
                                detalle.juegoRevanchaId = ((XElement)itemNodo).Attribute("JRelId").Value;
                                detalle.sorteoRevanchaId = ((XElement)itemNodo).Attribute("SortRelId").Value;
                            }
                            datoDevolver.listaDetalle.Add(detalle);
                        }
                    }
                }

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RecuperarFigurasPorJuego(TransaccionesTradicionalesDTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<mt>" +
                    "<c>" +
                        "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                        "<transaccion>" + _tradicionales.GetValue<string>("transaccionRecuperarFigurasPorJuego") + "</transaccion>" +
                        "<usuario>" + entradaDTO.UserName + "</usuario>" +
                        "<maquina>0.0.0.0</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                    "</c>" +
                    "<i>" +
                       "<JuegoId>" + entradaDTO.juegoId + "</JuegoId>" +
                    "</i>" +
                "</mt>"
                );


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                JuegosPorMedioDTO datoDevolver = new JuegosPorMedioDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalle = new List<DetalleJuegosPorMedio>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleJuegosPorMedio detalle = new DetalleJuegosPorMedio();
                            detalle.codigoImagen = ((XElement)itemNodo).Attribute("Cod").Value;
                            detalle.descripcionImagen = ((XElement)itemNodo).Attribute("Desc").Value;
                            detalle.abreviaturaImagen = ((XElement)itemNodo).Attribute("Abrev").Value;
                            datoDevolver.listaDetalle.Add(detalle);
                        }
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RecuperarSorteosJugados(TransaccionesTradicionalesDTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<mt>" +
                    "<c>" +
                        "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                        "<transaccion>" + _tradicionales.GetValue<string>("transaccionRecuperarSorteosJugados") + "</transaccion>" +
                        "<usuario>" + entradaDTO.UserName + "</usuario>" +
                        "<maquina>0.0.0.0</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                    "</c>" +
                    "<i>" +
                       "<MedioId>" + entradaDTO.medioId + "</MedioId>" +
                       "<JuegoId>" + entradaDTO.juegoId + "</JuegoId>" +
                    "</i>" +
                "</mt>"
                );


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                JuegosPorMedioDTO datoDevolver = new JuegosPorMedioDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalle = new List<DetalleJuegosPorMedio>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleJuegosPorMedio detalle = new DetalleJuegosPorMedio();
                            detalle.nombreJuego = ((XElement)itemNodo).Attribute("JNomb").Value;
                            detalle.sorteoId = ((XElement)itemNodo).Attribute("SortId").Value;
                            detalle.nombreSorteo = ((XElement)itemNodo).Attribute("SortNomb").Value;
                            detalle.pvp = ((XElement)itemNodo).Attribute("PVP").Value;
                            detalle.fechaSorteo = ((XElement)itemNodo).Attribute("FSort").Value;
                            detalle.cantidadFraccion = Convert.ToInt32(((XElement)itemNodo).Attribute("CFrac").Value);
                            detalle.seAcumula = Convert.ToBoolean(((XElement)itemNodo).Attribute("SeAcum").Value);
                            detalle.montoProximoSorteo = ((XElement)itemNodo).Attribute("MProxSort").Value;
                            detalle.valorPremio = ((XElement)itemNodo).Attribute("VPremio").Value;
                            detalle.esSorteoDestacado = Convert.ToBoolean(((XElement)itemNodo).Attribute("Destcdo").Value);
                            datoDevolver.listaDetalle.Add(detalle);
                        }
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RecuperarSuertesPrincipales(TransaccionesTradicionalesDTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<mt>" +
                    "<c>" +
                        "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                        "<transaccion>" + _tradicionales.GetValue<string>("transaccionRecuperarSorteosJugados") + "</transaccion>" +
                        "<usuario>" + entradaDTO.UserName + "</usuario>" +
                        "<maquina>0.0.0.0</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                    "</c>" +
                    "<i>" +
                       "<MedioId>" + entradaDTO.medioId + "</MedioId>" +
                       "<Cantidad>" + entradaDTO.cantidad + "</Cantidad>" +
                       "<JuegoId>" + entradaDTO.juegoId + "</JuegoId>" +
                    "</i>" +
                "</mt>"
                );


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                JuegosPorMedioDTO datoDevolver = new JuegosPorMedioDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalle = new List<DetalleJuegosPorMedio>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleJuegosPorMedio detalle = new DetalleJuegosPorMedio();
                            detalle.juegoId = ((XElement)itemNodo).Attribute("JId").Value;
                            detalle.nombreJuego = ((XElement)itemNodo).Attribute("JNomb").Value;
                            detalle.sorteoId = ((XElement)itemNodo).Attribute("SortId").Value;
                            detalle.fechaSorteo = ((XElement)itemNodo).Attribute("FSort").Value;
                            detalle.montoProximoSorteo = ((XElement)itemNodo).Attribute("MProxSort").Value;
                            detalle.seAcumula = Convert.ToBoolean(((XElement)itemNodo).Attribute("Acum").Value);
                            detalle.combinacion = ((XElement)itemNodo).Attribute("Comb").Value;
                            detalle.esPrimeraSuerte = Convert.ToBoolean(((XElement)itemNodo).Attribute("PriSue").Value);
                            detalle.esPremioEspecial = Convert.ToBoolean(((XElement)itemNodo).Attribute("Espec").Value);
                            detalle.codigoImagen = ((XElement)itemNodo).Attribute("Figura").Value;
                            detalle.desprendible = ((XElement)itemNodo).Attribute("DesPre").Value;
                            detalle.valorPremio = ((XElement)itemNodo).Attribute("ValPre").Value;
                            detalle.identificadorPremio = ((XElement)itemNodo).Attribute("Prem").Value;
                            detalle.cantidadFraccion = Convert.ToInt32(((XElement)itemNodo).Attribute("CFrac").Value);
                            datoDevolver.listaDetalle.Add(detalle);
                        }
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RecuperarFormasCobro(TransaccionesTradicionalesDTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<mt>" +
                    "<c>" +
                        "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                        "<transaccion>" + _tradicionales.GetValue<string>("transaccionRecuperarFormasCobro") + "</transaccion>" +
                        "<usuario>" + entradaDTO.UserName + "</usuario>" +
                        "<maquina>0.0.0.0</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                    "</c>" +
                    "<i>" +
                       "<MedioId>" + entradaDTO.medioId + "</MedioId>" +
                       "<Cantidad>" + entradaDTO.cantidad + "</Cantidad>" +
                       "<JuegoId>" + entradaDTO.juegoId + "</JuegoId>" +
                    "</i>" +
                "</mt>"
                );


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                FormasCobroDTO datoDevolver = new FormasCobroDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalle = new List<DetalleFormasCobro>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleFormasCobro detalle = new DetalleFormasCobro();
                            detalle.codigo = ((XElement)itemNodo).Attribute("Codigo").Value;
                            detalle.abreviatura = ((XElement)itemNodo).Attribute("Abreviatura").Value;
                            detalle.descripcion = ((XElement)itemNodo).Attribute("Descripcion").Value;
                            datoDevolver.listaDetalle.Add(detalle);
                        }
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RecuperarNumerosDisponiblesPorCombinacion(TransaccionesTradicionalesDTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<mt>" +
                    "<c>" +
                        "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                        "<transaccion>" + _tradicionales.GetValue<string>("transaccionRecuperarNumerosDisponiblesPorCombinancion") + "</transaccion>" +
                        "<usuario>" + entradaDTO.UserName + "</usuario>" +
                        "<maquina>0.0.0.0</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                    "</c>" +
                    "<i>" +
                       "<JuegoId>" + entradaDTO.juegoId + "</JuegoId>" +
                       "<MedioId>" + entradaDTO.medioId + "</MedioId>" +
                       "<SorteoId>" + entradaDTO.sorteoId + "</SorteoId>" +
                       "<Combinacion>" + entradaDTO.combinacion + "</Combinacion>" +
                       "<Registros>" + entradaDTO.registros + "</Registros>" +
                       "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>" +
                       "<CombFigura>" + entradaDTO.combinacionFigura + "</CombFigura>" +
                       "<Sugerir>" + entradaDTO.sugerir + "</Sugerir>" +
                    "</i>" +
                "</mt>"
                );


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                NumerosDisponiblesPorCombinacionDTO datoDevolver = new NumerosDisponiblesPorCombinacionDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalle = new List<DetalleNumerosDisponiblesPorCombinacion>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleNumerosDisponiblesPorCombinacion detalle = new DetalleNumerosDisponiblesPorCombinacion();
                            if (((XElement)itemNodo).Attribute("Id").Value != null)
                                detalle.Id = ((XElement)itemNodo).Attribute("Id").Value;
                            if (((XElement)itemNodo).Attribute("JId").Value != null)
                                detalle.juegoId = ((XElement)itemNodo).Attribute("JId").Value;
                            if (((XElement)itemNodo).Attribute("Sort").Value != null)
                                detalle.sorteoId = ((XElement)itemNodo).Attribute("Sort").Value;
                            if (((XElement)itemNodo).Attribute("Num").Value!=null)
                               detalle.numero = ((XElement)itemNodo).Attribute("Num").Value;
                            if (((XElement)itemNodo).Attribute("Num2").Value != null)
                                detalle.numero2 = ((XElement)itemNodo).Attribute("Num2").Value;
                            if (((XElement)itemNodo).Attribute("Num3").Value != null)
                                detalle.numero3 = ((XElement)itemNodo).Attribute("Num3").Value;
                            if (((XElement)itemNodo).Attribute("Num4").Value != null)
                                detalle.numero4 = ((XElement)itemNodo).Attribute("Num4").Value;
                            if (((XElement)itemNodo).Attribute("Num5").Value != null)
                                detalle.numero5 = ((XElement)itemNodo).Attribute("Num5").Value;
                            if (((XElement)itemNodo).Attribute("Fra").Value != null)
                                detalle.fracciones = ((XElement)itemNodo).Attribute("Fra").Value;
                            if (((XElement)itemNodo).Attribute("Fig").Value != null)
                                detalle.figura = ((XElement)itemNodo).Attribute("Fig").Value;
                            if (((XElement)itemNodo).Attribute("Cant").Value != null)
                                detalle.cantidad = Convert.ToInt32(((XElement)itemNodo).Attribute("Cant").Value);


                            datoDevolver.listaDetalle.Add(detalle);
                        }
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ReservarBoletos(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionReservarBoletos") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<MedioId>" + entradaDTO.medioId + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "<ReservaId>" + entradaDTO.reservaId + "</ReservaId>";
                dataXml = dataXml + "<Observacion>" + entradaDTO.observacion + "</Observacion>";
                dataXml = dataXml + "<xmlNumeros>";
                dataXml = dataXml + "<RS>";
                if(entradaDTO.listaJuegos.Count>0)
                {
                    foreach(var itemJuego in entradaDTO.listaJuegos)
                    {
                        dataXml = dataXml + "<JG " + itemJuego.juegoId + ">";
                        if(itemJuego.listaSorteos.Count>0)
                        {
                            foreach (var itemSorteo in itemJuego.listaSorteos)
                            {
                                dataXml = dataXml + "<R sorteo=" + itemSorteo.sorteoId + " numero=" + itemSorteo.numeroCombinacionPrincipal + " cantidad=" + itemSorteo.cantidadBoletosReservar + ">";
                                if(itemSorteo.listaFracciones.Count>0)
                                {
                                    foreach (var itemFraccion in itemSorteo.listaFracciones)
                                    {
                                        dataXml = dataXml + "<F id=" + itemFraccion.fraccionId + "/>";
                                    }
                                }
                                dataXml = dataXml + "</R>";
                            }
                        }
                        dataXml = dataXml + "</JG>";
                    }
                }
                dataXml = dataXml + "</RS>";
                dataXml = dataXml + "</xmlNumeros>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaResevaBoletosTradcionalesDTO datoDevolver = new RespuestaResevaBoletosTradcionalesDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalleReserva = new List<DetalleReserva>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleReserva detalle = new DetalleReserva();
                            if (((XElement)itemNodo).Attribute("id").Value != null)
                                detalle.reservaId = ((XElement)itemNodo).Attribute("id").Value;
                            if (((XElement)itemNodo).Attribute("JId").Value != null)
                                detalle.juegoId = ((XElement)itemNodo).Attribute("JId").Value;
                            if (((XElement)itemNodo).Attribute("Sort").Value != null)
                                detalle.sorteoId = ((XElement)itemNodo).Attribute("Sort").Value;
                            if (((XElement)itemNodo).Attribute("Num").Value != null)
                                detalle.numeroCombinacionReservada = ((XElement)itemNodo).Attribute("Num").Value;
                            if (((XElement)itemNodo).Attribute("Cant").Value != null)
                                detalle.cantidadFraccionesReservadas = Convert.ToInt32(((XElement)itemNodo).Attribute("Cant").Value);
                            if (((XElement)itemNodo).Attribute("Fra").Value != null)
                                detalle.fraccionId = ((XElement)itemNodo).Attribute("Fra").Value;

                            datoDevolver.listaDetalleReserva.Add(detalle);
                        }
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ConsultarBoletosReservados(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionConsultaBoletosReservados") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<ReservaId>" + entradaDTO.reservaId + "</ReservaId>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaResevaBoletosTradcionalesDTO datoDevolver = new RespuestaResevaBoletosTradcionalesDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("r");
                    var listaNodosR = nodoR.Nodes().ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.listaDetalleReserva = new List<DetalleReserva>();
                        foreach (var itemNodo in listaNodosR)
                        {
                            DetalleReserva detalle = new DetalleReserva();
                            if (((XElement)itemNodo).Attribute("ResrvId").Value != null)
                                detalle.reservaId = ((XElement)itemNodo).Attribute("ResrvId").Value;
                            if (((XElement)itemNodo).Attribute("MedioId").Value != null)
                                detalle.medioId = ((XElement)itemNodo).Attribute("MedioId").Value;
                            if (((XElement)itemNodo).Attribute("UsrId").Value != null)
                                detalle.usuario = ((XElement)itemNodo).Attribute("UsrId").Value;
                            if (((XElement)itemNodo).Attribute("Vendida").Value != null)
                                detalle.vendida = Convert.ToBoolean(((XElement)itemNodo).Attribute("Vendida").Value);
                            if (((XElement)itemNodo).Attribute("Anulada").Value != null)
                                detalle.anulada = Convert.ToBoolean(((XElement)itemNodo).Attribute("Anulada").Value);
                            if (((XElement)itemNodo).Attribute("FecResrv").Value != null)
                                detalle.fechaReserva = Convert.ToDateTime(((XElement)itemNodo).Attribute("FecResrv").Value);
                            if (((XElement)itemNodo).Attribute("FecAnul").Value != null)
                                detalle.fechaAnulacion = Convert.ToDateTime(((XElement)itemNodo).Attribute("FecAnul").Value);
                            if (((XElement)itemNodo).Attribute("VentaId").Value != null)
                                detalle.ventaId = ((XElement)itemNodo).Attribute("VentaId").Value;
                            if (((XElement)itemNodo).Attribute("Obs").Value != null)
                                detalle.observacion = ((XElement)itemNodo).Attribute("Obs").Value;
                            if (((XElement)itemNodo).Attribute("JId").Value != null)
                                detalle.juegoId = ((XElement)itemNodo).Attribute("JId").Value;
                            if (((XElement)itemNodo).Attribute("Sort").Value != null)
                                detalle.sorteoId = ((XElement)itemNodo).Attribute("Sort").Value;
                            if (((XElement)itemNodo).Attribute("Num").Value != null)
                                detalle.numeroCombinacionReservada = ((XElement)itemNodo).Attribute("Num").Value;
                            if (((XElement)itemNodo).Attribute("Cant").Value != null)
                                detalle.cantidadFraccionesReservadas = Convert.ToInt32(((XElement)itemNodo).Attribute("Cant").Value);
                            if (((XElement)itemNodo).Attribute("Resrv").Value != null)
                                detalle.numeroBoletosReservados = Convert.ToInt32(((XElement)itemNodo).Attribute("Resrv").Value);
                            if (((XElement)itemNodo).Attribute("Fra").Value != null)
                                detalle.fraccionId = ((XElement)itemNodo).Attribute("Fra").Value;
                            if (((XElement)itemNodo).Attribute("Id").Value != null)
                                detalle.id = ((XElement)itemNodo).Attribute("Id").Value;

                            datoDevolver.listaDetalleReserva.Add(detalle);
                        }
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> EliminarBoletosReservados(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionEliminacionBoletosReservados") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<MedioId>" + entradaDTO.medioId + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "<ReservaId>" + entradaDTO.reservaId + "</ReservaId>";
                dataXml = dataXml + "<Observacion>" + entradaDTO.observacion + "</Observacion>";
                dataXml = dataXml + "<xmlNumeros>";
                dataXml = dataXml + "<RS>";
                if (entradaDTO.listaJuegos.Count > 0)
                {
                    foreach (var itemJuego in entradaDTO.listaJuegos)
                    {
                        dataXml = dataXml + "<JG " + itemJuego.juegoId + ">";
                        if (itemJuego.listaSorteos.Count > 0)
                        {
                            foreach (var itemSorteo in itemJuego.listaSorteos)
                            {
                                dataXml = dataXml + "<R sorteo=" + itemSorteo.sorteoId + "numero=" + itemSorteo.numeroCombinacionPrincipal + ">";
                                if (itemSorteo.listaFracciones.Count > 0)
                                {
                                    foreach (var itemFraccion in itemSorteo.listaFracciones)
                                    {
                                        dataXml = dataXml + "<F id=" + itemFraccion.fraccionId + "/>";
                                    }
                                }
                                dataXml = dataXml + "</R>";
                            }
                        }
                        dataXml = dataXml + "</JG>";
                    }
                }
                dataXml = dataXml + "</RS>";
                dataXml = dataXml + "</xmlNumeros>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaEliminarResevaBoletosTradcionalesDTO datoDevolver = new RespuestaEliminarResevaBoletosTradcionalesDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoo = xmlRespuesta.Descendants("o");
                    var listaNodoso = nodoo.Nodes().ToList();
                    if (listaNodoso.Count() > 0)
                    {
                        datoDevolver.returnValue = ((XElement)listaNodoso[0]).Value; 
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ActualizarUsuarioFechaHoraReserva(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionActualizaUsuarioFechaHoraReserva") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<ReservaId>" + entradaDTO.reservaId + "</ReservaId>";
                dataXml = dataXml + "<MedioId>" + entradaDTO.medioId + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaEliminarResevaBoletosTradcionalesDTO datoDevolver = new RespuestaEliminarResevaBoletosTradcionalesDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                //if (datoDevolver.codError == 0)
                //{
                //    var nodoo = xmlRespuesta.Descendants("o");
                //    var listaNodoso = nodoo.Nodes().ToList();
                //    if (listaNodoso.Count() > 0)
                //    {
                //        datoDevolver.returnValue = ((XElement)listaNodoso[0]).Value;
                //    }
                //}
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> VentaBoletos(VentaBoletosTradicionalesDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionActualizaUsuarioFechaHoraReserva") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<ReservaId>" + entradaDTO.reservaId + "</ReservaId>";
                dataXml = dataXml + "<xmlVenta>";
                dataXml = dataXml + "<VT>";
                dataXml = dataXml + "<V total=" + entradaDTO.totalVenta + " tipoIdent=" + entradaDTO.tipoIdentificacion + " numIdent=" + entradaDTO.numeroIdentificacion + " nombComp=" + entradaDTO.nombreComprador + ">";
                dataXml = dataXml + "</V>";
                dataXml = dataXml + "<FP ordComp=" + entradaDTO.codigoOrdenCompra + " forCo=" + entradaDTO.codigoFormaCobro + " tarCre=" + entradaDTO.operadorTarjeta + " codAut=" + entradaDTO.codigoAutorizacion +
                          " Total=" + entradaDTO.totalVentaFormaCobro;
                if(entradaDTO.operadorTarjeta == "DIN")
                {
                    dataXml = dataXml + " valIce=" + entradaDTO.valorICE + " tipCre=" + entradaDTO.tipoCredito + " valInt=" + entradaDTO.valorInteres + " meses=" + entradaDTO.mesesPlazo;
                }
                if (entradaDTO.codigoFormaCobro == "DNE")
                {
                    dataXml = dataXml + " codConf=" + entradaDTO.codigoConfirmacion + " numCed=" + entradaDTO.numeroCedula + " numCel=" + entradaDTO.numeroCelular;
                }
                dataXml = dataXml + ">";
                dataXml = dataXml + "</FP>";
                dataXml = dataXml + "</VT>";
                dataXml = dataXml + "</xmlVenta>";
                dataXml = dataXml + "<xmlNumeros>";
                dataXml = dataXml + "<RS>";
                if (entradaDTO.listaJuegos.Count > 0)
                {
                    foreach (var itemJuego in entradaDTO.listaJuegos)
                    {
                        dataXml = dataXml + "<JG " + itemJuego.juegoId + ">";
                        if (itemJuego.listaSorteos.Count > 0)
                        {
                            foreach (var itemSorteo in itemJuego.listaSorteos)
                            {
                                dataXml = dataXml + "<R sorteo=" + itemSorteo.sorteoId + " numero=" + itemSorteo.numeroCombinacionPrincipal + " cantidad=" + itemSorteo.cantidadBoletosReservar + ">";
                                if (itemSorteo.listaFracciones.Count > 0)
                                {
                                    foreach (var itemFraccion in itemSorteo.listaFracciones)
                                    {
                                        dataXml = dataXml + "<F id=" + itemFraccion.fraccionId + "/>";
                                    }
                                }
                                dataXml = dataXml + "</R>";
                            }
                        }
                        dataXml = dataXml + "</JG>";
                    }
                }
                dataXml = dataXml + "</RS>";
                dataXml = dataXml + "</xmlNumeros>";
                dataXml = dataXml + "<MedioId>" + entradaDTO.medioId + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaVentaDTO datoDevolver = new RespuestaVentaDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoVTA = xmlRespuesta.Descendants("VTA");
                    var listaNodosVTA = nodoVTA.Nodes().ToList();
                    if (listaNodosVTA.Count() > 0)
                    {
                        foreach (var itemNodoVTA in listaNodosVTA)
                        {
                            if (((XElement)itemNodoVTA).Attribute("VId").Value != null)
                                datoDevolver.ventaId = ((XElement)itemNodoVTA).Attribute("VId").Value;
                            if (((XElement)itemNodoVTA).Attribute("tipoIden").Value != null)
                                datoDevolver.tipoIdentificacion = ((XElement)itemNodoVTA).Attribute("tipoIden").Value;
                            if (((XElement)itemNodoVTA).Attribute("numIden").Value != null)
                                datoDevolver.numeroIdentificacion = ((XElement)itemNodoVTA).Attribute("numIden").Value;
                            if (((XElement)itemNodoVTA).Attribute("nombCom").Value != null)
                                datoDevolver.nombreComprador = ((XElement)itemNodoVTA).Attribute("nombCom").Value;

                            var nodoSUE = xmlRespuesta.Descendants("SUE");
                            var listaNodosSUE = nodoSUE.Nodes().ToList();
                            if (listaNodosSUE.Count() > 0)
                            {
                                datoDevolver.listaVentaSuerte = new List<RespuestaVentaSuerte>();
                                foreach (var itemNodoSUE in listaNodosSUE)
                                {
                                    RespuestaVentaSuerte detalleSuerte = new RespuestaVentaSuerte();
                                    if (((XElement)itemNodoSUE).Attribute("COMP").Value != null)
                                        detalleSuerte.numeroComprobanteSuerte = ((XElement)itemNodoSUE).Attribute("COMP").Value;
                                    if (((XElement)itemNodoSUE).Attribute("ANU1").Value != null)
                                        detalleSuerte.anuncio1 = ((XElement)itemNodoSUE).Attribute("ANU1").Value;
                                    if (((XElement)itemNodoSUE).Attribute("ANU2").Value != null)
                                        detalleSuerte.anuncio2 = ((XElement)itemNodoSUE).Attribute("ANU2").Value;
                                    if (((XElement)itemNodoSUE).Attribute("ANU3").Value != null)
                                        detalleSuerte.anuncio3 = ((XElement)itemNodoSUE).Attribute("ANU3").Value;

                                    var nodoSOR = xmlRespuesta.Descendants("SOR");
                                    var listaNodosSOR = nodoSOR.Nodes().ToList();
                                    if (listaNodosSOR.Count() > 0)
                                    {
                                        detalleSuerte.listaVentaSorteos = new List<RespuestaVentaSorteos>();
                                        foreach (var itemNodoSOR in listaNodosSOR)
                                        {
                                            RespuestaVentaSorteos detalleSorteo = new RespuestaVentaSorteos();
                                            if (((XElement)itemNodoSOR).Attribute("JId").Value != null)
                                                detalleSorteo.juegoId = ((XElement)itemNodoSOR).Attribute("JId").Value;
                                            if (((XElement)itemNodoSOR).Attribute("Sort").Value != null)
                                                detalleSorteo.sorteoId = ((XElement)itemNodoSOR).Attribute("Sort").Value;
                                            if (((XElement)itemNodoSOR).Attribute("JNomb").Value != null)
                                                detalleSorteo.nombreJuego = ((XElement)itemNodoSOR).Attribute("JNomb").Value;
                                            if (((XElement)itemNodoSOR).Attribute("SortNomb").Value != null)
                                                detalleSorteo.nombreSorteo = ((XElement)itemNodoSOR).Attribute("SortNomb").Value;
                                            if (((XElement)itemNodoSOR).Attribute("FSort").Value != null)
                                                detalleSorteo.fechaSorteo = Convert.ToDateTime(((XElement)itemNodoSOR).Attribute("FSort").Value);
                                            if (((XElement)itemNodoSOR).Attribute("PVP").Value != null)
                                                detalleSorteo.precioVentaPublico = ((XElement)itemNodoSOR).Attribute("PVP").Value;
                                            if (((XElement)itemNodoSOR).Attribute("Premio").Value != null)
                                                detalleSorteo.premioMayorPrimeraSuerte = ((XElement)itemNodoSOR).Attribute("Premio").Value;
                                            if (((XElement)itemNodoSOR).Attribute("TPremPS").Value != null)
                                                detalleSorteo.tipoPremioPrimeraSuerte = ((XElement)itemNodoSOR).Attribute("TPremPS").Value;
                                            if (((XElement)itemNodoSOR).Attribute("NPremPS").Value != null)
                                                detalleSorteo.nombrePremioPrimeraSuerte = ((XElement)itemNodoSOR).Attribute("NPremPS").Value;

                                            var nodoR = xmlRespuesta.Descendants("R");
                                            var listaNodosR = nodoR.Nodes().ToList();
                                            if (listaNodosR.Count() > 0)
                                            {
                                                detalleSorteo.listaNumerosVendidos = new List<RespuestaVentaR>();
                                                foreach (var itemNodoR in listaNodosR)
                                                {
                                                    RespuestaVentaR detalleR = new RespuestaVentaR();
                                                    if (((XElement)itemNodoR).Attribute("Num").Value != null)
                                                        detalleR.numeroVendido = ((XElement)itemNodoR).Attribute("Num").Value;
                                                    if (((XElement)itemNodoR).Attribute("Cant").Value != null)
                                                        detalleR.cantidadFracciones = Convert.ToInt32(((XElement)itemNodoR).Attribute("Cant").Value);
                                                    if (((XElement)itemNodoR).Attribute("Val").Value != null)
                                                        detalleR.valorTotalNumeroVendido = ((XElement)itemNodoR).Attribute("Val").Value;
                                                    var nodoC = xmlRespuesta.Descendants("C");
                                                    var listaNodosC = nodoC.Nodes().ToList();
                                                    if (listaNodosC.Count() > 0)
                                                    {
                                                        detalleR.listaFracciones = new List<RespuestaVentaC>();
                                                        foreach (var itemNodoC in listaNodosC)
                                                        {
                                                            RespuestaVentaC detalleC = new RespuestaVentaC();
                                                            if (((XElement)itemNodoR).Attribute("Num").Value != null)
                                                                detalleC.numeroFraccion = ((XElement)itemNodoR).Attribute("Num").Value;
                                                            if (((XElement)itemNodoR).Attribute("Nomb").Value != null)
                                                                detalleC.numeroFraccion = ((XElement)itemNodoR).Attribute("Nomb").Value;

                                                            detalleR.listaFracciones.Add(detalleC);
                                                        }
                                                    }

                                                    detalleSorteo.listaNumerosVendidos.Add(detalleR);
                                                }
                                            }
                                            detalleSuerte.listaVentaSorteos.Add(detalleSorteo);

                                        }
                                    }
                                    datoDevolver.listaVentaSuerte.Add(detalleSuerte);


                                }
                            }
                            var nodoINST = xmlRespuesta.Descendants("INST");
                            var listaNodosINST = nodoINST.Nodes().ToList();
                            if (listaNodosINST.Count() > 0)
                            {
                                datoDevolver.listaPremiosInstantaneos = new List<RespuestaVentaPremioInstantaneo>();
                                foreach(var itemNodoINST in listaNodosINST)
                                {
                                    RespuestaVentaPremioInstantaneo detallePremioInstantaneo = new RespuestaVentaPremioInstantaneo();
                                    if (((XElement)itemNodoINST).Attribute("COMP").Value != null)
                                        detallePremioInstantaneo.numeroComprobantePremioInstantaneo = ((XElement)itemNodoINST).Attribute("COMP").Value;
                                    if (((XElement)itemNodoINST).Attribute("MSG").Value != null)
                                        detallePremioInstantaneo.mensaje = ((XElement)itemNodoINST).Attribute("MSG").Value;

                                    var nodoSOR = xmlRespuesta.Descendants("SOR");
                                    var listaNodosSOR = nodoSOR.Nodes().ToList();
                                    if (listaNodosSOR.Count() > 0)
                                    {
                                        detallePremioInstantaneo.listaVentaSorteosPremioInstantaneo = new List<RespuestaVentaSorteosPremioInstantaneo>();
                                        foreach(var itemNodoSOR in listaNodosSOR)
                                        {
                                            RespuestaVentaSorteosPremioInstantaneo detalleInstantaneo = new RespuestaVentaSorteosPremioInstantaneo();
                                            if (((XElement)itemNodoSOR).Attribute("JId").Value != null)
                                                detalleInstantaneo.juegoId= ((XElement)itemNodoSOR).Attribute("JId").Value;
                                            if (((XElement)itemNodoSOR).Attribute("JNomb").Value != null)
                                                detalleInstantaneo.nombreJuego = ((XElement)itemNodoSOR).Attribute("JNomb").Value;
                                            if (((XElement)itemNodoSOR).Attribute("Sort").Value != null)
                                                detalleInstantaneo.sorteoId = ((XElement)itemNodoSOR).Attribute("Sort").Value;
                                            if (((XElement)itemNodoSOR).Attribute("SortNomb").Value != null)
                                                detalleInstantaneo.nombreSorteo = ((XElement)itemNodoSOR).Attribute("SortNomb").Value;

                                            var nodoR = xmlRespuesta.Descendants("R");
                                            var listaNodosR = nodoR.Nodes().ToList();
                                            if (listaNodosR.Count() > 0)
                                            {
                                                detalleInstantaneo.listaNumerosGanadoresPremioInstantaneo = new List<RespuestaVentaRPremioInstantaneo>();
                                                foreach (var itemNodoR in listaNodosR)
                                                {
                                                    RespuestaVentaRPremioInstantaneo detalleNumerosGanadores = new RespuestaVentaRPremioInstantaneo();
                                                    if (((XElement)itemNodoINST).Attribute("Num").Value != null)
                                                        detalleNumerosGanadores.numeroGanador = ((XElement)itemNodoINST).Attribute("Num").Value;
                                                    if (((XElement)itemNodoINST).Attribute("Num2").Value != null)
                                                        detalleNumerosGanadores.numeroSerie = ((XElement)itemNodoINST).Attribute("Num2").Value;
                                                    if (((XElement)itemNodoINST).Attribute("Fra").Value != null)
                                                        detalleNumerosGanadores.fraccionGanadora = ((XElement)itemNodoINST).Attribute("Fra").Value;
                                                    if (((XElement)itemNodoINST).Attribute("Prem").Value != null)
                                                        detalleNumerosGanadores.descripcionPremio = ((XElement)itemNodoINST).Attribute("Prem").Value;
                                                    if (((XElement)itemNodoINST).Attribute("Val").Value != null)
                                                        detalleNumerosGanadores.valorPremio = ((XElement)itemNodoINST).Attribute("Val").Value;
                                                    if (((XElement)itemNodoINST).Attribute("ConDesc").Value != null)
                                                        detalleNumerosGanadores.valorPremioConDescuento = ((XElement)itemNodoINST).Attribute("ConDesc").Value;
                                                    if (((XElement)itemNodoINST).Attribute("TPrem").Value != null)
                                                        detalleNumerosGanadores.tipoPremio = ((XElement)itemNodoINST).Attribute("TPrem").Value;

                                                    detalleInstantaneo.listaNumerosGanadoresPremioInstantaneo.Add(detalleNumerosGanadores);
                                                }
                                            }
                                            detallePremioInstantaneo.listaVentaSorteosPremioInstantaneo.Add(detalleInstantaneo);
                                        }
                                    }

                                    datoDevolver.listaPremiosInstantaneos.Add(detallePremioInstantaneo);
                                }
                            }
                        }


                        //datoDevolver.listaDetalleReserva = new List<DetalleReserva>();
                        //foreach (var itemNodo in listaNodosR)
                        //{
                        //    DetalleReserva detalle = new DetalleReserva();
                        //    if (((XElement)itemNodo).Attribute("id").Value != null)
                        //        detalle.reservaId = ((XElement)itemNodo).Attribute("id").Value;
                        //    if (((XElement)itemNodo).Attribute("JId").Value != null)
                        //        detalle.juegoId = ((XElement)itemNodo).Attribute("JId").Value;
                        //    if (((XElement)itemNodo).Attribute("Sort").Value != null)
                        //        detalle.sorteoId = ((XElement)itemNodo).Attribute("Sort").Value;
                        //    if (((XElement)itemNodo).Attribute("Num").Value != null)
                        //        detalle.numeroCombinacionReservada = ((XElement)itemNodo).Attribute("Num").Value;
                        //    if (((XElement)itemNodo).Attribute("Cant").Value != null)
                        //        detalle.cantidadFraccionesReservadas = Convert.ToInt32(((XElement)itemNodo).Attribute("Cant").Value);
                        //    if (((XElement)itemNodo).Attribute("Fra").Value != null)
                        //        detalle.fraccionId = ((XElement)itemNodo).Attribute("Fra").Value;

                        //    datoDevolver.listaDetalleReserva.Add(detalle);
                        //}
                    }
                }

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ConsultarUltimaReservaActiva(ReservaBoletosTradicionalesDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionConsultaUltimaReservaActiva") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<MedioId>" + entradaDTO.medioId + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "<ReservaId>" + entradaDTO.reservaId + "</ReservaId>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaResevaBoletosTradcionalesDTO datoDevolver = new RespuestaResevaBoletosTradcionalesDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("o");
                    var listaNodosO = nodoO.Nodes().ToList();
                    if (listaNodosO.Count() > 0)
                    {
                        datoDevolver.reservaId = ((XElement)listaNodosO[0]).Value;
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> AgregarOrdenPago(AgregarOrdenPagoDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionAgregaOrdenPago") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + 17 + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<MedioId>" + entradaDTO.medioId + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "<xmlOrdenPago>";
                dataXml = dataXml + "<OPA>";
                if(entradaDTO.juegoId==null || entradaDTO.juegoId==string.Empty || entradaDTO.juegoId=="")
                {
                    dataXml = dataXml + "<R RTest=" + Convert.ToString(0) + " Valor=" + entradaDTO.valorPagar + " TPrem=" + entradaDTO.tipoPremio;
                    dataXml = dataXml + " NuReti=" + entradaDTO.numeroRetiro + " NuTranWeb=" + entradaDTO.numeroTransaccionWeb + " ValDesctdo=" + entradaDTO.valorDescontadoWeb + "/>";
                }
                else
                {
                    dataXml = dataXml + "<R JId=" + entradaDTO.juegoId + " Sort=" + entradaDTO.sorteoId + " BolId=" + entradaDTO.boletoId;
                    dataXml = dataXml + " TPrem=" + entradaDTO.tipoPremio + " PremId=" + entradaDTO.premioId + " VId=" + entradaDTO.identificadorVenta + "/>";
                }
                dataXml = dataXml + "</OPA>";
                dataXml = dataXml + "</xmlOrdenPago>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaAgregarOrdenPagoDTO datoDevolver = new RespuestaAgregarOrdenPagoDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("o");
                    var listaNodosO = nodoO.Nodes().ToList();
                    if (listaNodosO.Count() > 0)
                    {
                        datoDevolver.returnValue = ((XElement)listaNodosO[0]).Value;
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RetirarOrdenPago(RetirarOrdenPagoDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionRetiraOrdenPago") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + 17 + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<MedioId>" + 17 + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "<ClienteId>" + entradaDTO.ClienteId + "</ClienteId>";
                dataXml = dataXml + "<NumeroTransaccion>" + entradaDTO.numeroTransaccion + "</NumeroTransaccion>";
                dataXml = dataXml + "<NumeroRetiro>" + entradaDTO.numeroRetiro + "</NumeroRetiro>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaRetirarOrdenPagoDTO datoDevolver = new RespuestaRetirarOrdenPagoDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("o");
                    var listaNodosO = nodoO.Nodes().ToList();
                    if (listaNodosO.Count() > 0)
                    {
                        datoDevolver.returnValue = ((XElement)listaNodosO[0]).Value;
                        var nodoR = xmlRespuesta.Descendants("r");
                        var listaNodosR = nodoR.Nodes().ToList();
                        foreach (var itemNodo in listaNodosR)
                        {
                            if (((XElement)itemNodo).Attribute("OrdPagId").Value != null)
                                datoDevolver.ordenPagoId = ((XElement)itemNodo).Attribute("OrdPagId").Value;
                            if (((XElement)itemNodo).Attribute("FPago").Value != null)
                                datoDevolver.fechaPago = ((XElement)itemNodo).Attribute("FPago").Value;
                            if (((XElement)itemNodo).Attribute("Ident").Value != null)
                                datoDevolver.numeroIdentificacion = ((XElement)itemNodo).Attribute("Ident").Value;
                            if (((XElement)itemNodo).Attribute("Valor").Value != null)
                                datoDevolver.valorRetirado = ((XElement)itemNodo).Attribute("Valor").Value;
                        }
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> AnularOrdenPago(AnularOrdenPagoDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionAnulaOrdenPago") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + 17 + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<MedioId>" + 17 + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "<ClienteId>" + entradaDTO.ClienteId + "</ClienteId>";
                dataXml = dataXml + "<NumeroTransaccion>" + entradaDTO.numeroTransaccion + "</NumeroTransaccion>";
                dataXml = dataXml + "<Motivo>" + entradaDTO.motivo + "</Motivo>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaAnularOrdenPagoDTO datoDevolver = new RespuestaAnularOrdenPagoDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("o");
                    var listaNodosR = nodoR.Nodes().ToList();
                    foreach (var itemNodo in listaNodosR)
                    {
                        if (((XElement)itemNodo).Attribute("OrdPagId").Value != null)
                            datoDevolver.ordenPagoId = ((XElement)itemNodo).Attribute("OrdPagId").Value;
                        if (((XElement)itemNodo).Attribute("Ident").Value != null)
                            datoDevolver.numeroIdentificacion = ((XElement)itemNodo).Attribute("Ident").Value;
                        if (((XElement)itemNodo).Attribute("Valor").Value != null)
                            datoDevolver.valorRetirado = ((XElement)itemNodo).Attribute("Valor").Value;
                        if (((XElement)itemNodo).Attribute("NuTrans").Value != null)
                            datoDevolver.numeroTransaccion = ((XElement)itemNodo).Attribute("NuTrans").Value;
                        if (((XElement)itemNodo).Attribute("FAnulacion").Value != null)
                            datoDevolver.fechaAnulacion = ((XElement)itemNodo).Attribute("FAnulacion").Value;
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ConsultaVentaRealizada(ConsultaVentaRealizadaDTO entradaDTO)
        {
            try
            {
                string dataXml = string.Empty;
                dataXml = "<mt>";
                dataXml = dataXml + "<c>";
                dataXml = dataXml + "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>";
                dataXml = dataXml + "<transaccion>" + _tradicionales.GetValue<string>("transaccionConsultaVentaRealizada") + "</transaccion>";
                dataXml = dataXml + "<usuario>" + entradaDTO.UserName + "</usuario>";
                dataXml = dataXml + "<maquina>0.0.0.0</maquina>";
                dataXml = dataXml + "<codError>0</codError>";
                dataXml = dataXml + "<msgError />";
                dataXml = dataXml + "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>";
                dataXml = dataXml + "<token>" + entradaDTO.token + "</token>";
                dataXml = dataXml + "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>";
                dataXml = dataXml + "</c>";
                dataXml = dataXml + "<i>";
                dataXml = dataXml + "<ordComp>" + entradaDTO.numeroTransaccionWeb + "</ordComp>";
                dataXml = dataXml + "<MedioId>" + 17 + "</MedioId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "<ClienteId>" + entradaDTO.ClienteId + "</ClienteId>";
                dataXml = dataXml + "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>";
                dataXml = dataXml + "</i>";
                dataXml = dataXml + "</mt>";

                XDocument documentoTransaccionXML = new XDocument();

                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse(dataXml);


                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnEjecutaTransaccionAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaAnularOrdenPagoDTO datoDevolver = new RespuestaAnularOrdenPagoDTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.transaccion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoR = xmlRespuesta.Descendants("o");
                    var listaNodosR = nodoR.Nodes().ToList();
                    foreach (var itemNodo in listaNodosR)
                    {
                        if (((XElement)itemNodo).Attribute("OrdPagId").Value != null)
                            datoDevolver.ordenPagoId = ((XElement)itemNodo).Attribute("OrdPagId").Value;
                        if (((XElement)itemNodo).Attribute("Ident").Value != null)
                            datoDevolver.numeroIdentificacion = ((XElement)itemNodo).Attribute("Ident").Value;
                        if (((XElement)itemNodo).Attribute("Valor").Value != null)
                            datoDevolver.valorRetirado = ((XElement)itemNodo).Attribute("Valor").Value;
                        if (((XElement)itemNodo).Attribute("NuTrans").Value != null)
                            datoDevolver.numeroTransaccion = ((XElement)itemNodo).Attribute("NuTrans").Value;
                        if (((XElement)itemNodo).Attribute("FAnulacion").Value != null)
                            datoDevolver.fechaAnulacion = ((XElement)itemNodo).Attribute("FAnulacion").Value;
                    }
                }
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        #endregion
        #region Bet593
        public async Task<string> RecargarBet593(RecargaBet593DTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();
                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<dat>" +
                    "<c>" +
                        "<usuario>" + entradaDTO.usuario + "</usuario>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>RECARGA593</operacion>" +
                        "<canal>" + entradaDTO.canal + "</canal>" +
                        "<medioid>" + entradaDTO.medioId.ToString() + "</medioid>" +
                        "<puntooperacionid>" + entradaDTO.puntooperacionid.ToString() + "</puntooperacionid>" +
                    "</c>" +
                    "<i>" +
                        "<cuentaweb>" + entradaDTO.cuentaweb + "</cuentaweb>" +
                        "<valor>" + entradaDTO.valor.ToString() + "</valor>" +
                        "<codigotrn>" + entradaDTO.codigotrn + "</codigotrn>" +
                    "</i>" +
                "</dat>"
                );

                servicioBet593.ServicioClient servicio593 = new servicioBet593.ServicioClient();
                var strRespuesta = await servicio593.fnEjecutaTransaccionALTAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaRecargaBet593DTO datoDevolver = new RespuestaRecargaBet593DTO();
                datoDevolver.usuario = ((XElement)listaNodos[0]).Value;
                datoDevolver.token = ((XElement)listaNodos[1]).Value;
                datoDevolver.operacion = ((XElement)listaNodos[2]).Value;
                datoDevolver.codError = int.Parse(((XElement)listaNodos[3]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[4]).Value;

                if (datoDevolver.codError==0)
                {
                    var nodoO = xmlRespuesta.Descendants("o");
                    var listaNodosO = nodoO.Nodes().ToList();
                    datoDevolver.resultado = ((XElement)listaNodosO[0]).Value;
                    datoDevolver.cuentaweb = ((XElement)listaNodosO[1]).Value;
                    datoDevolver.nombre = ((XElement)listaNodosO[2]).Value;
                    datoDevolver.apellido = ((XElement)listaNodosO[3]).Value;
                    datoDevolver.tipoDocumento = ((XElement)listaNodosO[4]).Value;
                    datoDevolver.valor = ((XElement)listaNodosO[5]).Value;
                    datoDevolver.fecharecarga = Convert.ToDateTime(((XElement)listaNodosO[6]).Value);
                    datoDevolver.recargaid = ((XElement)listaNodosO[7]).Value;
                    datoDevolver.serialnumber = ((XElement)listaNodosO[8]).Value;
                }
                

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ConfirmarBet593(RecargaBet593DTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();
                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<dat>" +
                    "<c>" +
                        "<usuario>" + entradaDTO.usuario + "</usuario>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>CONFIRMA593</operacion>" +
                        "<canal>" + entradaDTO.canal + "</canal>" +
                        "<medioid>" + entradaDTO.medioId.ToString() + "</medioid>" +
                        "<puntooperacionid>" + entradaDTO.puntooperacionid.ToString() + "</puntooperacionid>" +
                    "</c>" +
                    "<i>" +
                        "<cuentaweb>" + entradaDTO.cuentaweb + "</cuentaweb>" +
                        "<recargaid>" + entradaDTO.recargaid + "</recargaid>" +
                        "<serialnumber>" + entradaDTO.serialnumber + "</serialnumber>" +
                    "</i>" +
                "</dat>"
                );

                servicioBet593.ServicioClient servicio593 = new servicioBet593.ServicioClient();
                var strRespuesta = await servicio593.fnEjecutaTransaccionALTAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaRecargaBet593DTO datoDevolver = new RespuestaRecargaBet593DTO();
                datoDevolver.usuario = ((XElement)listaNodos[0]).Value;
                datoDevolver.token = ((XElement)listaNodos[1]).Value;
                datoDevolver.operacion = ((XElement)listaNodos[2]).Value;
                datoDevolver.codError = int.Parse(((XElement)listaNodos[3]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[4]).Value;

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("o");
                    var listaNodosO = nodoO.Nodes().ToList();
                    datoDevolver.resultado = ((XElement)listaNodosO[0]).Value;
                    datoDevolver.cuentaweb = ((XElement)listaNodosO[1]).Value;
                }
               
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ValidarBet593(RecargaBet593DTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();
                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<dat>" +
                    "<c>" +
                        "<usuario>" + entradaDTO.usuario + "</usuario>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>VALIDA593</operacion>" +
                        "<canal>" + entradaDTO.canal + "</canal>" +
                        "<medioid>" + entradaDTO.medioId.ToString() + "</medioid>" +
                        "<puntooperacionid>" + entradaDTO.puntooperacionid.ToString() + "</puntooperacionid>" +
                    "</c>" +
                    "<i>" +
                        "<cuentaweb>" + entradaDTO.cuentaweb + "</cuentaweb>" +
                        "<recargaid>" + entradaDTO.recargaid + "</recargaid>" +
                        "<serialnumber>" + entradaDTO.serialnumber + "</serialnumber>" +
                    "</i>" +
                "</dat>"
                );

                servicioBet593.ServicioClient servicio593 = new servicioBet593.ServicioClient();
                var strRespuesta = await servicio593.fnEjecutaTransaccionALTAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaRecargaBet593DTO datoDevolver = new RespuestaRecargaBet593DTO();
                datoDevolver.usuario = ((XElement)listaNodos[0]).Value;
                datoDevolver.token = ((XElement)listaNodos[1]).Value;
                datoDevolver.operacion = ((XElement)listaNodos[2]).Value;
                datoDevolver.codError = int.Parse(((XElement)listaNodos[3]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[4]).Value;

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("o");
                    var listaNodosO = nodoO.Nodes().ToList();
                    datoDevolver.resultado = ((XElement)listaNodosO[0]).Value;
                    datoDevolver.cuentaweb = ((XElement)listaNodosO[1]).Value;
                    datoDevolver.estado = ((XElement)listaNodosO[2]).Value;
                }
                
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ReversarBet593(RecargaBet593DTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();
                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<dat>" +
                    "<c>" +
                        "<usuario>" + entradaDTO.usuario + "</usuario>" +
                        "<token>" + entradaDTO.token + "</token>" +
                        "<operacion>REVRETIROOL</operacion>" +
                        "<canal>" + entradaDTO.canal + "</canal>" +
                        "<medioid>" + entradaDTO.medioId.ToString() + "</medioid>" +
                        "<puntooperacionid>" + entradaDTO.puntooperacionid.ToString() + "</puntooperacionid>" +
                    "</c>" +
                    "<i>" +
                        "<cuentaweb>" + entradaDTO.cuentaweb + "</cuentaweb>" +
                        "<recargaid>" + entradaDTO.recargaid + "</recargaid>" +
                        "<serialnumber>" + entradaDTO.serialnumber + "</serialnumber>" +
                        "<motivo>" + entradaDTO.motivo + "</motivo>" +
                    "</i>" +
                "</dat>"
                );

                servicioBet593.ServicioClient servicio593 = new servicioBet593.ServicioClient();
                var strRespuesta = await servicio593.fnEjecutaTransaccionALTAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaRecargaBet593DTO datoDevolver = new RespuestaRecargaBet593DTO();
                datoDevolver.usuario = ((XElement)listaNodos[0]).Value;
                datoDevolver.token = ((XElement)listaNodos[1]).Value;
                if(listaNodos.Count==5)
                {
                    datoDevolver.operacion = ((XElement)listaNodos[2]).Value;
                    datoDevolver.codError = int.Parse(((XElement)listaNodos[3]).Value);
                    datoDevolver.msgError = ((XElement)listaNodos[4]).Value;
                }
                else
                {
                    datoDevolver.operacion = "REVRETIROOL";
                    datoDevolver.codError = int.Parse(((XElement)listaNodos[2]).Value);
                    datoDevolver.msgError = ((XElement)listaNodos[3]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("o");
                    var listaNodosO = nodoO.Nodes().ToList();
                    datoDevolver.resultado = ((XElement)listaNodosO[0]).Value;
                    datoDevolver.cuentaweb = ((XElement)listaNodosO[1]).Value;
                }
                
                
                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> RetirarBet593(RetiroBet593DTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();
                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<dat>" +
                    "<c>" +
                        "<usuario>" + entradaDTO.usuario + "</usuario>" +
                        "<maquina>" + entradaDTO.maquina + "</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<operacion>RETIROOL</operacion>" +
                        "<token>" + entradaDTO.token + "</token>" +
                    "</c>" +
                    "<i>" +
                        "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>" +
                        "<ClienteId>" + entradaDTO.ClienteId.ToString() + "</ClienteId>" +
                        "<MedioId>" + entradaDTO.MedioId.ToString() + "</MedioId>" +
                        "<NumeroTransaccion>" + entradaDTO.NumeroTransaccion + "</NumeroTransaccion>" +
                        "<Identificacion>" + entradaDTO.Identificacion + "</Identificacion>" +
                        "<NumeroRetiro>" + entradaDTO.NumeroRetiro + "</NumeroRetiro>" +
                    "</i>" +
                "</dat>"
                );

                servicioBet593.ServicioClient servicio593 = new servicioBet593.ServicioClient();
                var strRespuesta = await servicio593.fnEjecutaTransaccionALTAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaRetiroBet593DTO datoDevolver = new RespuestaRetiroBet593DTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.operacion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }
                else
                {
                    datoDevolver.operacion = "RETIROOL";
                    datoDevolver.token = ((XElement)listaNodos[3]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("xmlRetiroOutput");
                    var listaNodosO = nodoO.ElementAt(0).Value;
                    XDocument doc = XDocument.Parse(listaNodosO);
                    var nodoR = doc.Descendants("R");
                    var listaNodosR = nodoR.ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.ordenPagoId = Convert.ToInt32(listaNodosR[0].Attribute("OrdPagId").Value.ToString());
                        datoDevolver.identificacion = listaNodosR[0].Attribute("Ident").Value;
                        datoDevolver.valor = listaNodosR[0].Attribute("Valor").Value;
                        datoDevolver.numeroTransaccion = listaNodosR[0].Attribute("NuTrans").Value;
                        datoDevolver.fecha = Convert.ToDateTime(listaNodosR[0].Attribute("FPago").Value.ToString());
                    }
                }

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ReversarRetiroBet593(RetiroBet593DTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();
                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<dat>" +
                    "<c>" +
                        "<usuario>" + entradaDTO.usuario + "</usuario>" +
                        "<maquina>" + entradaDTO.maquina + "</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<operacion>REVRETIROOL</operacion>" +
                        "<token>" + entradaDTO.token + "</token>" +
                    "</c>" +
                    "<i>" +
                        "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>" +
                        "<ClienteId>" + entradaDTO.ClienteId.ToString() + "</ClienteId>" +
                        "<MedioId>" + entradaDTO.MedioId.ToString() + "</MedioId>" +
                        "<NumeroTransaccion>" + entradaDTO.NumeroTransaccion + "</NumeroTransaccion>" +
                        "<Identificacion>" + entradaDTO.Identificacion + "</Identificacion>" +
                        "<Motivo>" + entradaDTO.Motivo + "</Motivo>" +
                    "</i>" +
                "</dat>"
                );

                servicioBet593.ServicioClient servicio593 = new servicioBet593.ServicioClient();
                var strRespuesta = await servicio593.fnEjecutaTransaccionALTAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaRetiroBet593DTO datoDevolver = new RespuestaRetiroBet593DTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.operacion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }
                else
                {
                    datoDevolver.operacion = "RETIREVERSOOL";
                    datoDevolver.token = ((XElement)listaNodos[3]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("xmlRetiroOutput");
                    var listaNodosO = nodoO.ElementAt(0).Value;
                    XDocument doc = XDocument.Parse(listaNodosO);
                    var nodoR = doc.Descendants("R");
                    var listaNodosR = nodoR.ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.ordenPagoId = Convert.ToInt32(listaNodosR[0].Attribute("OrdPagId").Value.ToString());
                        datoDevolver.identificacion = listaNodosR[0].Attribute("Ident").Value;
                        datoDevolver.valor = listaNodosR[0].Attribute("Valor").Value;
                        datoDevolver.numeroTransaccion = listaNodosR[0].Attribute("NuTrans").Value;
                        datoDevolver.fecha = Convert.ToDateTime(listaNodosR[0].Attribute("FReverso").Value.ToString());
                    }
                }

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> ConsultarRetiroBet593(RetiroBet593DTO entradaDTO)
        {
            try
            {
                XDocument documentoTransaccionXML = new XDocument();
                //Se arma la data para generar y enviar el XML
                documentoTransaccionXML = XDocument.Parse("" +
                "<dat>" +
                    "<c>" +
                        "<usuario>" + entradaDTO.usuario + "</usuario>" +
                        "<maquina>" + entradaDTO.maquina + "</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<operacion>CONRETIROOL</operacion>" +
                        "<token>" + entradaDTO.token + "</token>" +
                    "</c>" +
                    "<i>" +
                        "<UsuarioId>" + entradaDTO.UsuarioId + "</UsuarioId>" +
                        "<ClienteId>" + entradaDTO.ClienteId.ToString() + "</ClienteId>" +
                        "<MedioId>" + entradaDTO.MedioId.ToString() + "</MedioId>" +
                        "<NumeroTransaccion>" + entradaDTO.NumeroTransaccion + "</NumeroTransaccion>" +
                        "<Identificacion>" + entradaDTO.Identificacion + "</Identificacion>" +
                        "<NumeroRetiro>" + entradaDTO.NumeroRetiro + "</NumeroRetiro>" +
                    "</i>" +
                "</dat>"
                );

                servicioBet593.ServicioClient servicio593 = new servicioBet593.ServicioClient();
                var strRespuesta = await servicio593.fnEjecutaTransaccionALTAsync(documentoTransaccionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                RespuestaRetiroBet593DTO datoDevolver = new RespuestaRetiroBet593DTO();
                datoDevolver.codError = int.Parse(((XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((XElement)listaNodos[1]).Value;
                datoDevolver.usuario = ((XElement)listaNodos[2]).Value;
                if (listaNodos.Count == 5)
                {
                    datoDevolver.operacion = ((XElement)listaNodos[3]).Value;
                    datoDevolver.token = ((XElement)listaNodos[4]).Value;
                }
                else
                {
                    datoDevolver.operacion = "CONRETIROOL";
                    datoDevolver.token = ((XElement)listaNodos[3]).Value;
                }

                if (datoDevolver.codError == 0)
                {
                    var nodoO = xmlRespuesta.Descendants("xmlRetiroOutput");
                    var listaNodosO = nodoO.ElementAt(0).Value;
                    XDocument doc = XDocument.Parse(listaNodosO);
                    var nodoR = doc.Descendants("R");
                    var listaNodosR = nodoR.ToList();
                    if (listaNodosR.Count() > 0)
                    {
                        datoDevolver.identificacion = listaNodosR[0].Attribute("Ident").Value;
                        datoDevolver.valor = listaNodosR[0].Attribute("Valor").Value;
                        datoDevolver.nombre = listaNodosR[0].Attribute("Nombre").Value;
                        datoDevolver.fecha = Convert.ToDateTime(listaNodosR[0].Attribute("FOrdenPago").Value.ToString());
                    }
                }

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        #endregion
        #region Métodos y Funciones Privadas
        string AddHash(string ticketNumber)
        {
            string letras = ticketNumber.Substring(0,2);
            string value = ticketNumber.Substring(2);
            var crc32 = new Crc32();
            var bytes = Encoding.UTF8.GetBytes(value);
            crc32.Append(bytes);
            var checkSum = crc32.GetCurrentHash();
            Array.Reverse(checkSum);
            var stringCrc = BitConverter.ToInt32(checkSum);
            string ticketNumberRetorno = string.Concat(value, stringCrc.ToString().Replace("-", "").Substring(0, 3));
            ticketNumberRetorno = string.Concat(letras, ticketNumberRetorno);
            return ticketNumberRetorno;
        }
        #endregion
    }
}
