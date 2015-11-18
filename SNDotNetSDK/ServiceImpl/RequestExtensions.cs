using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SNDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SNDotNetSDK.ServiceImpl
{
	/// <summary>
	/// Helper extensions to make api calls and get results.
	/// </summary>
	static class RequestExtensions
	{
		public static T GetResult<T>(this IRestResponse restResponse)
		{
			return GetResult(restResponse).ToObject<T>();
		}

		public static JToken GetResult(this IRestResponse restResponse)
		{
			JToken result = null;
			if (
				restResponse.Content != null
				&& (
					restResponse.ContentType.StartsWith("text/json")
					|| restResponse.ContentType.StartsWith("application/json")
				)
			)
			{
				result = JToken.Parse(restResponse.Content);
				var jobj = result as JObject;
				if (jobj != null)
				{
					var errorsObject = result["errors"];
					if (errorsObject != null)
					{
						var errors = errorsObject.ToObject<ErrorInfo[]>();
						if (errors.Length > 1)
							throw new AggregateException(errors.Select(error => new CudaSignException(error)));
						else
							throw new CudaSignException(errors.FirstOrDefault() ?? new ErrorInfo { Message = "Unknown error" });
					}
				}
			}
			if (restResponse.StatusCode != HttpStatusCode.OK)
			{
				throw new CudaSignException(new ErrorInfo { Code = (int)restResponse.StatusCode, Message = restResponse.StatusDescription });
			}
			return result;
		}

		public static IRestRequest CreateRequest(this Oauth2Token token, string url, Method method, object body = null, string contentType = "application/json", string accept = "application/json")
		{
			var request = new RestRequest(url, method)
					.AddHeader("Authorization", "Bearer " + token.AccessToken);

			if (accept != null)
				request.AddHeader("Accept", "application/json");

			if (contentType != null)
				request.AddHeader("Content-Type", contentType);

			if (body != null)
			{
				string requestBody = JsonConvert.SerializeObject(body, Formatting.Indented);
				request.AddParameter("text/json", requestBody, ParameterType.RequestBody);
			}
			return request;
		}

		public static IRestResponse GetResponse(this IRestClient client, Oauth2Token token, string url, Method method, object body = null, string contentType = "application/json", string accept = "application/json")
		{
			var request = CreateRequest(token, url, method, body, contentType, accept);

			return client.Execute(request);
		}
	}
}
