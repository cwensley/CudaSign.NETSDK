using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace CudaSign
{
    public class Link
    {
		CudaSignClient client;
		internal Link(CudaSignClient client)
		{
			this.client = client;
		}

        public LinkInfo Create(OAuth2Token token, string documentId)
        {
			var request = client.CreateRequest(token, "/link", Method.Post);

            request.AddJsonBody(new { document_id = documentId });

            var response = client.Execute(request);

			return response.GetResult<LinkInfo>();
        }
    }
}
