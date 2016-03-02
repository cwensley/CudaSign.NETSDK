using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CudaSign
{
	static class NameHelpers
	{
		public static string Underscore(string name)
		{
			return Regex.Replace(name, @"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4").ToLower();
		}

		public static string Dash(string name)
		{
			return Regex.Replace(name, @"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3-$2$4").ToLower();
		}
	}

	class ErrorInfo
	{
		public int Code { get; set; }
		public string Message { get; set; }
	}


	/// <summary>
	/// Helper extensions to make api calls and get results.
	/// </summary>
	static class RequestHelpers
	{
		public static T GetResult<T>(this IRestResponse restResponse)
		{
			return GetResult(restResponse).ToObject<T>(JsonHelpers.Serializer);
		}

		public static bool IsCudaSuccess(this JToken token)
		{
			var statusToken = token["status"] ?? token["result"];
			if (statusToken == null)
				return false;
			return Equals(statusToken.Value<string>(), "success");
		}

		public static JToken GetResult(this IRestResponse restResponse)
		{
			JToken result = null;
			if (restResponse.Content != null)
			{
				result = JToken.Parse(restResponse.Content);
				var jobj = result as JObject;
				if (jobj != null)
				{
					var errorsObject = result["errors"];
					if (errorsObject != null)
					{
						var errors = errorsObject.ToObject<ErrorInfo[]>(JsonHelpers.Serializer);
						if (errors.Length > 1)
							throw new AggregateException(errors.Select(error => new CudaSignException(error.Code, error.Message)));
						else
						{
							var error = errors.FirstOrDefault() ?? new ErrorInfo { Message = "Unknown error" };
							throw new CudaSignException(error.Code, error.Message);
						}
					}
				}
				if (restResponse.StatusCode != HttpStatusCode.OK)
				{
					throw new CudaSignException((int)restResponse.StatusCode, restResponse.StatusDescription);
				}
			}
			return result;
		}

		public static IRestRequest AddJsonNetBody(this IRestRequest request, object data)
		{
			var json = JsonConvert.SerializeObject(data, JsonHelpers.Settings);
			request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
			return request;
		}
	}
}
