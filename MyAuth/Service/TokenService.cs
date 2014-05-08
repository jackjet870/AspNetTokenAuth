using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAuth.Service
{
    /// <summary>
    /// This is the Token service 
    /// to Create Token ,and get the expire
    /// </summary>
    public class TokenService
    {
        /// <summary>
        /// secret key for Encrypt
        /// </summary>
        private const string SecretKey = "wuweiwei1989";

        /// <summary>
        /// the  Encrypt information
        /// </summary>
        public IDictionary<string, object> TokenDictionary { get; set; }

        /// <summary>
        /// Constuctor
        /// set the Encryt information Dictionary
        /// </summary>
        /// <param name="token"></param>
        public TokenService(string token)
        {
            try
            {
                TokenDictionary = JWT.JsonWebToken.DecodeToObject(token, SecretKey) as IDictionary<string, object>;
            }
            catch (JWT.SignatureVerificationException)
            {
                
                throw;
            }
        }

        /// <summary>
        /// create token form registerUserId
        /// </summary>
        /// <param name="registerUserId">registerUserId</param>
        /// <returns>token</returns>
        public static string CreateToken(Guid registerUserId)
        {
            var playload = new Dictionary<string, object>()
            {
                {"registerUserId", registerUserId},
                {"expiredTime", DateTime.UtcNow.AddHours(1)}
            };
            var token = JWT.JsonWebToken.Encode(playload, SecretKey, JWT.JwtHashAlgorithm.HS256);
            return token;
        }

        /// <summary>
        /// check the Expired Time
        /// </summary>
        /// <returns></returns>
        public bool IsExpired()
        {
            DateTime dt;
            if (DateTime.TryParse(TokenDictionary["expiredTime"].ToString(),out dt))
                return dt < DateTime.Now;
            return false;
        }

        /// <summary>
        /// get the token from the header,
        /// </summary>
        /// <returns>TokenService contains token information filed</returns>
        public static TokenService GetTokenFromHeader()
        {
            if (HttpContext.Current.Request.Headers.AllKeys.Contains("Authorization"))
            {
                var authHeader = HttpContext.Current.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authHeader))
                {
                    return new TokenService(authHeader);
                }
            }

            return null;
        }
    }
}