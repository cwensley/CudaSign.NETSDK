using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CudaSign
{
	public class OAuth2Token
	{
		public string AccessToken { get; set; }
		public string TokenType { get; set; }
		//public TimeSpan ExpiresIn { get; set; }
		public string RefreshToken { get; set; }
		public string Id { get; set; }
		public string Scope { get; set; }
	}

    public class OAuth2
    {
		CudaSignClient client;

		internal OAuth2(CudaSignClient client)
		{
			this.client = client;
		}

		/// <summary>
		/// Request an Access Token using the User's Credentials
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <param name="scope">A space delimited list of API URIs e.g. "user%20documents%20user%2Fdocumentsv2"</param>
		/// <returns>New Access Token, Token Type, Expires In, Refresh Token, ID, Scope</returns>
		public OAuth2Token RequestToken(string email, string password, string scope = "*")
        {
			var request = client.CreateRequest(null, "/oauth2/token", Method.POST)
				.AddHeader("Authorization", "Basic " + client.EncodedClientCredentials)
				.AddHeader("Content-Type", "application/x-www-form-urlencoded")
                .AddParameter("username", email)
                .AddParameter("password", password)
                .AddParameter("grant_type", "password")
                .AddParameter("scope", scope);

			var response = client.Execute(request);
			return response.GetResult<OAuth2Token>();
        }

        /// <summary>
        /// Verify a User's Access Token
        /// </summary>
        /// <param name="accessToken">User's Access Token</param>
        /// <param name="ResultFormat">JSON, XML</param>
        /// <returns>Access Token, Token Type, Expires In, Refresh Token, Scope</returns>
        public OAuth2Token Verify(OAuth2Token accessToken)
        {
			var request = client.CreateRequest(accessToken, "/oauth2/token", Method.POST);

            var response = client.Execute(request);

			return response.GetResult<OAuth2Token>();
        }
    }
}
