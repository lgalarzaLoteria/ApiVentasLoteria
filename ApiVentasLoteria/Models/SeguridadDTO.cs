namespace ApiVentasLoteria.Models
{
    public class SeguridadDTO
    {
        /// <summary>
        /// Datos de entrada para validar acceso y proceder a vender este DTO Se enviará al servicio de SG
        /// </summary>
        public class LoginDTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Clave del comercio para acceder al sistema (No debe ser encriptada)
            /// </summary>
            public string? Password { get; set; }
            /// <summary>
            /// Nueva Clave del comercio para acceder al sistema (No debe ser encriptada) para cambio de clave
            /// </summary>
            public string? NewPassword { get; set; }
            /// <summary>
            /// Id del dispositvo asignado al comercio
            /// </summary>
            public string? DeviceId { get; set; }
            /// <summary>
            /// Token de seguridad que devuelve el inicio de sesión
            /// </summary>
            public string? token { get; set; }
            /// <summary>
            /// Producto a vender, posibles valores: Pega3, Raspaditas, Tradicionales
            /// </summary>
            public string? productoVender { get; set; }
            
        }
        public class LoginPega3DTO
        {
            /// <summary>
            /// Usuario del comercio para acceder al sistema
            /// </summary>
            public string? username { get; set; }
            /// <summary>
            /// Clave del comercio para acceder al sistema (No debe ser encriptada)
            /// </summary>
            public string? encryptedPassword { get; set; }
           
        }
        public class LoginRaspaditasDTO
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public int Language { get; set; } = 0;
            public string DeviceID { get; set; } 
        }
        public class LogoutRaspaditasDTO
        {
            public string securityID { get; set; }
            public int userID { get; set; }
            public int TC { get; set; } = 0;
            public string SC { get; set; } = "";
        }
        public class LoginTradicionalRespuestaDTO
        {
            public int codError { get; set; }
            public string? msgError { get; set; }
            public string token { get; set; }
            public string? usuario { get; set; }
            public string? operacion { get; set; }

        }
        public class LoginBet593RespuestaDTO
        {
            public string usuario { get; set; }
            public string token { get; set; }
            public int codError { get; set; }
            public string? msgError { get; set; }

        }
        
    }
}
