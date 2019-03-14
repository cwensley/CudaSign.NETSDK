using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaSign
{
    public class CudaSignClient
    {
        internal string EncodedClientCredentials = "";
        internal string ApiHost = "";

		RestClient rest;

		/// <summary>
		/// CudaSign Initialization
		/// </summary>
		/// <param name="client">API Credentials - Client</param>
		/// <param name="secret">API Credentials - Secret</param>
		/// <param name="production">For Public API set True for Eval API set False</param>
		/// <param name="apiServer">API Server Path. Defaults to CudaSign EVALUATION if left blank.</param>
		public CudaSignClient(string client, string secret, string apiServer = null)
        {
			ApiHost = !string.IsNullOrEmpty(apiServer) ? apiServer : "https://api-eval.cudasign.com/";

			EncodedClientCredentials = EncodeClientCredentials(client, secret);
			rest = new RestClient(ApiHost);
			Link = new Link(this);
			Document = new Document(this);
			DocumentGroup = new DocumentGroup(this);
			Folder = new Folder(this);
			Template = new Template(this);
			User = new User(this);
			Webhook = new Webhook(this);
			OAuth2 = new OAuth2(this);
        }

        public string EncodeClientCredentials(string client, string secret)
        {
            string idAndSecret = client + ":" + secret;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(idAndSecret);
            return System.Convert.ToBase64String(plainTextBytes);
        }

		public Link Link { get; }
		public Document Document { get; }
		public DocumentGroup DocumentGroup { get; }
		public Folder Folder { get; }
		public Template Template { get; }
		public User User { get; }
		public Webhook Webhook { get; }
		public OAuth2 OAuth2 { get; }

		internal IRestResponse Execute(IRestRequest request)
		{
			return rest.Execute(request);
		}

		internal Task<IRestResponse> ExecuteAsync(IRestRequest request)
		{
			return rest.ExecuteTaskAsync(request);
		}

		internal byte[] DownloadData(IRestRequest request)
		{
			return rest.DownloadData(request);
		}

		internal IRestRequest CreateRequest(OAuth2Token token, string path, Method method, string accept = "application/json")
		{
			var request = new RestRequest(path, method);
			if (token != null)
				request.AddHeader("Authorization", "Bearer " + token.AccessToken);

			if (accept != null)
				request.AddHeader("Accept", accept);

			return request;
		}

	}
}
