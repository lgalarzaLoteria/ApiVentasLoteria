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


namespace ApiVentasLoteria.Data
{
    public class JuegosData
    {
        private readonly IConfigurationSection _urlJuegos;
        private readonly SeguridadData _seguridad;
        private readonly IConfigurationSection _tradicionales;

        public JuegosData(IConfiguration configuration, SeguridadData repositorio)
        {
            _urlJuegos = configuration.GetSection("ScientificGames");
            _tradicionales = configuration.GetSection("Tradicionales");
            _seguridad = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        #region Pega3
        public async Task<string> ObtieneOpcionesPega3(LoginDTO entradaDTO)
        {
            try 
            {
                string urlMetodo = _urlJuegos.GetValue<string>("urlOpcionesPega3") + "1001";
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
        public async Task<string> CrearTicketPega3(CrearTicketPega3RequerimientoDTO entradaDTO)
        {
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
                //return respuesta.Content;

                dynamic responseContent = JsonConvert.DeserializeObject(respuesta.Content);
                string numeroTicketsinHash = responseContent.SelectToken("gameTicketNumber");
                if (numeroTicketsinHash==null || numeroTicketsinHash==string.Empty)
                    throw new Exception("Error al general el ticket");

                string numeroTicket = AddHash(numeroTicketsinHash);

                responseContent.gameTicketNumber = numeroTicket;
                var respuestaJson = JsonConvert.SerializeObject(responseContent);

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
                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(entradaDTO);
                string urlMetodo = _urlJuegos.GetValue<string>("urlOperacionesTicket") + entradaDTO.ticketNumber + "/cancel";
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
        public async Task<string> ConsultarVentasPega3()
        {
            return string.Empty;
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

                var nodoO = xmlRespuesta.Descendants("o");
                var listaNodosO = nodoO.Nodes().ToList();
                datoDevolver.resultado = ((XElement)listaNodosO[0]).Value;
                datoDevolver.cuentaweb = ((XElement)listaNodosO[1]).Value;
               
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

                var nodoO = xmlRespuesta.Descendants("o");
                var listaNodosO = nodoO.Nodes().ToList();
                datoDevolver.resultado = ((XElement)listaNodosO[0]).Value;
                datoDevolver.cuentaweb = ((XElement)listaNodosO[1]).Value;
                datoDevolver.estado = ((XElement)listaNodosO[2]).Value;

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
                        "<operacion>REVERSA593</operacion>" +
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
                    datoDevolver.operacion = "REVERSA593";
                    datoDevolver.codError = int.Parse(((XElement)listaNodos[2]).Value);
                    datoDevolver.msgError = ((XElement)listaNodos[3]).Value;
                }
                var nodoO = xmlRespuesta.Descendants("o");
                var listaNodosO = nodoO.Nodes().ToList();
                datoDevolver.resultado = ((XElement)listaNodosO[0]).Value;
                datoDevolver.cuentaweb = ((XElement)listaNodosO[1]).Value;
                
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
                

                var nodoO = xmlRespuesta.Descendants("R");
                var listaNodosO = nodoO.Nodes().ToList();
                datoDevolver.ordenPagoId = Convert.ToInt32(((XElement)listaNodosO[0]).Value.ToString());
                datoDevolver.identificacion = ((XElement)listaNodosO[1]).Value;
                datoDevolver.valor = ((XElement)listaNodosO[2]).Value;
                datoDevolver.numeroTransaccion = ((XElement)listaNodosO[3]).Value;
                datoDevolver.fecha = Convert.ToDateTime(((XElement)listaNodosO[4]).Value.ToString());

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
                        "<operacion>RETIROOL</operacion>" +
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


                var nodoO = xmlRespuesta.Descendants("R");
                var listaNodosO = nodoO.Nodes().ToList();
                datoDevolver.ordenPagoId = Convert.ToInt32(((XElement)listaNodosO[0]).Value.ToString());
                datoDevolver.identificacion = ((XElement)listaNodosO[1]).Value;
                datoDevolver.valor = ((XElement)listaNodosO[2]).Value;
                datoDevolver.numeroTransaccion = ((XElement)listaNodosO[3]).Value;
                datoDevolver.fecha = Convert.ToDateTime(((XElement)listaNodosO[4]).Value.ToString());

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


                var nodoO = xmlRespuesta.Descendants("R");
                var listaNodosO = nodoO.Nodes().ToList();
                datoDevolver.identificacion = ((XElement)listaNodosO[0]).Value;
                datoDevolver.valor = ((XElement)listaNodosO[1]).Value;
                datoDevolver.nombre = ((XElement)listaNodosO[2]).Value;
                datoDevolver.fecha = Convert.ToDateTime(((XElement)listaNodosO[3]).Value.ToString());

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
            string value = ticketNumber.Substring(2);
            var crc32 = new Crc32();
            var bytes = Encoding.UTF8.GetBytes(value);
            crc32.Append(bytes);
            var checkSum = crc32.GetCurrentHash();
            Array.Reverse(checkSum);
            var stringCrc = BitConverter.ToInt32(checkSum);
            string ticketNumberRetorno = string.Concat(value, stringCrc.ToString().Replace("-", "").Substring(0, 3));
            return ticketNumberRetorno;
        }
        #endregion
    }
}
