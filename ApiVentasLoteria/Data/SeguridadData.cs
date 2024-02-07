using System;
using static ApiVentasLoteria.Models.SeguridadDTO;
using System.Text.Json;
using RestSharp;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using System.Drawing;
using NuGet.Common;
using System.Xml.Linq;
using System.Security.AccessControl;
using System.Xml;

namespace ApiVentasLoteria.Data
{
    public class SeguridadData
    {
        private readonly IConfigurationSection _urlLogin;
        private readonly IConfigurationSection _tradicionales;

        public SeguridadData(IConfiguration configuration)
        {
            _urlLogin = configuration.GetSection("ScientificGames");
            _tradicionales = configuration.GetSection("Tradicionales");
        }
        public async Task<string> LoginPega3(LoginDTO usuario)
        {
            try 
            {
                LoginPega3DTO dataEnvio = new LoginPega3DTO();
                dataEnvio.username = usuario.UserName;
                string claveEncriptada = Encryptar(usuario.Password);
                dataEnvio.encryptedPassword = claveEncriptada;

                //serializo la clase LoginDTO
                var jsonLogin = System.Text.Json.JsonSerializer.Serialize(dataEnvio);

                //llamo al API
                var apiScientificGames = new RestClient(_urlLogin.GetValue<string>("urlLoginPega3"));
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("deviceId", usuario.DeviceId);
                requerimiento.AddBody(jsonLogin);

                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);

                //if (respuesta.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                //{
                //    Object token =  JsonConvert.DeserializeObject(respuesta.Content);
                //    //dataRespuesta.token = ((Newtonsoft.Json.Linq.JObject)token).GetValue("token").ToString();   
                //}
                //else
                //{
                //    dataRespuesta.errorServicio = respuesta.Content.Trim();
                //}
                
                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> LogoutPega3(LoginDTO usuario)
        {
            try
            {
                //llamo al API
                var apiScientificGames = new RestClient(_urlLogin.GetValue<string>("urlLogoutPega3"));
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddHeader("token", usuario.token);
                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);

                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }


        }
        public async Task<string> LoginRaspaditas(LoginDTO usuario)
        {
            try
            {
                //string claveEncriptada = Encryptar(usuario.Password);
                //usuario.Password = claveEncriptada;

                LoginRaspaditasDTO dataEnvio = new LoginRaspaditasDTO();
                dataEnvio.UserName = usuario.UserName;
                dataEnvio.Password = usuario.Password;
                dataEnvio.DeviceID = usuario.DeviceId;
                

                //serializo la clase LoginRaspaditasDTO
                var jsonLogin = System.Text.Json.JsonSerializer.Serialize(dataEnvio);

                //llamo al API
                var apiScientificGames = new RestClient(_urlLogin.GetValue<string>("urlLoginRaspaditas"));
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddBody(jsonLogin);

                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);

                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public async Task<string> LogoutRaspaditas(LoginDTO usuario)
        {
            try
            {
                LogoutRaspaditasDTO datoEnviar = new LogoutRaspaditasDTO();
                datoEnviar.userID = int.Parse(usuario.DeviceId);
                datoEnviar.securityID = usuario.token;

                //serializo la clase LogoutRaspaditasDTO
                var jsonLogin = System.Text.Json.JsonSerializer.Serialize(datoEnviar);

                //llamo al API
                var apiScientificGames = new RestClient(_urlLogin.GetValue<string>("urlLogoutRaspaditas"));
                var requerimiento = new RestRequest();
                requerimiento.Method = Method.Post;
                requerimiento.AddHeader("Content-Type", "application/json");
                requerimiento.AddHeader("Accept", "application/json");
                requerimiento.AddBody(jsonLogin);

                var respuesta = await apiScientificGames.ExecuteAsync(requerimiento);

                return respuesta.Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }


        }
        public async Task<string> LoginTradicionales(LoginDTO usuario)
        {
            try
            {
                //Se arma la data para generar y enviar el XML
                XDocument documentoAutenticacionXML = XDocument.Parse("" +
                "<mt>" +
                    "<c>" +
                        "<aplicacion>" + _tradicionales.GetValue<string>("aplicacion") + "</aplicacion>" +
                        "<usuario>" + usuario.UserName + "</usuario>" +
                        "<clave>" + usuario.Password + "</clave>" +
                        "<maquina>0.0.0.0</maquina>" +
                        "<codError>0</codError>" +
                        "<msgError />" +
                        "<medio>" + _tradicionales.GetValue<string>("medio") + "</medio>" +
                        "<operacion>" + _tradicionales.GetValue<string>("operacion") + "</operacion>" +
                    "</c>" +
                "</mt>"
                );

                servicioMTPrep.ServicioMTClient servicioMT = new servicioMTPrep.ServicioMTClient();
                var strRespuesta = await servicioMT.fnAutenticacionAsync(documentoAutenticacionXML.ToString());
                XDocument xmlRespuesta = XDocument.Parse(strRespuesta);
                
                //Para prueba en caso de que se caiga el servicio de MT
                //XDocument xmlRespuesta1 = XDocument.Parse("" +
                //"<mt>" +
                //    "<c>" +
                //        "<codError>0</codError>" +
                //        "<msgError />" +
                //        "<token>31333124032012210903313331240321</token>" +
                //    "</c>" +
                //"</mt>"
                //);

                var nodo = xmlRespuesta.Descendants("c");
                var listaNodos = nodo.Nodes().ToList();

                LoginTradicionalRespuestaDTO datoDevolver = new LoginTradicionalRespuestaDTO();
                datoDevolver.codError = int.Parse(((System.Xml.Linq.XElement)listaNodos[0]).Value);
                datoDevolver.msgError = ((System.Xml.Linq.XElement)listaNodos[1]).Value;
                datoDevolver.token = string.Empty;
                if (listaNodos.Count == 3)
                    datoDevolver.token = ((System.Xml.Linq.XElement)listaNodos[2]).Value;

                var jsonRespuesta = System.Text.Json.JsonSerializer.Serialize(datoDevolver);

                return jsonRespuesta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        #region Métodos Privados
        public static string Encryptar(string clave)
        {
            var publicKey = "SGFlexSys1234567";
            var key = Encoding.UTF8.GetBytes(publicKey);
            var iv = Encoding.UTF8.GetBytes(publicKey);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encryptedBytes;

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(clave);
                        }

                        encryptedBytes = ms.ToArray();
                    }
                }

                string encriptado = Convert.ToBase64String(encryptedBytes);
                return encriptado;
                
            }
        }

        #endregion
    }
}
