using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SNDotNetSDK.Configuration;
using SNDotNetSDK.Models;
using SNDotNetSDK.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace SNDotNetSDK.ServiceImpl
{

	[Serializable]
	public class CudaSignException : Exception
	{
		public int Code { get; private set; }

		internal CudaSignException(ErrorInfo error) : base(error.Message)
		{
			Code = error.Code;
		}
        internal CudaSignException(ErrorInfo error, string message, Exception inner)
			: base(error.Message, inner)
		{
			Code = error.Code;
		}

		protected CudaSignException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{ }
	}

	class ErrorInfo
	{
		public int Code { get; set; }
		public string Message { get; set; }
	}


	/**
     * Created by Deepak on 5/14/2015
     * 
     * This class is used to perform operations on the Documents. This class is provides the guidle lines on how to call the SignNow API
     * for several operations like Create (POST), GetDocuemnt (GET), Update Document (PUT), Get Document History, etc.,
     */
	public class DocumentService : IDocumentService
	{
		Config config;
		RestClient client;

		public DocumentService(Config config)
		{
			this.config = config;

			client = new RestClient();
			client.BaseUrl = config.GetApiBase();
		}
		/*
         * This method is used to create  or POST the document for a given user in the SignNow Application
         */
		public Document Create(Oauth2Token token, string filePath, bool checkFields = false)
		{
			if (token == null)
				throw new ArgumentNullException("token");
			if (filePath == null)
				throw new ArgumentNullException("filePath");
			return CreateInternal(token, filePath, null, "/document", request =>
			{
				if (checkFields)
					request.AddParameter("check_fields", "true");
			});
		}

		public Document Create(Oauth2Token token, string fileName, Stream stream, bool checkFields = false)
		{
			if (token == null)
				throw new ArgumentNullException("token");
			if (stream == null)
				throw new ArgumentNullException("stream");
			if (fileName == null)
				throw new ArgumentNullException("filePath");
			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				return Create(token, fileName, ms.ToArray(), checkFields);
			}
		}

		public Document Create(Oauth2Token token, string fileName, byte[] data, bool checkFields = false)
		{
			if (token == null)
				throw new ArgumentNullException("token");
			if (data == null)
				throw new ArgumentNullException("data");
			if (fileName == null)
				throw new ArgumentNullException("filePath");

			return CreateInternal(token, fileName, data, "/document", request =>
			{
				if (checkFields)
					request.AddParameter("check_fields", "true");
			});
		}

		Document CreateInternal(Oauth2Token token, string filePath, byte[] data, string url, Action<IRestRequest> updateRequest = null)
		{
			var request = token.CreateRequest(url, Method.POST, contentType: "multipart/form-data");
			if (updateRequest != null)
				updateRequest(request);

			if (data == null)
				request.AddFile("file", filePath);
			else
				request.AddFile("file", data, filePath);

			var response = client.Execute(request);

			return response.GetResult<Document>();
		}

		/*
         * This method retrieves all the uploaded documents for the specified user.
         */
		public IEnumerable<Document> GetDocuments(Oauth2Token token)
		{
			var response = client.GetResponse(token, "/user/documentsv2", Method.GET);
            return response.GetResult<Document[]>();
		}

		/*
            This method is used to GET the document for a given user from the SignNow Application
        */
		public Document GetDocumentbyId(Oauth2Token token, string id)
		{
			var response = client.GetResponse(token, "/document" + "/" + id, Method.GET);
			return response.GetResult<Document>();
		}

		/*
            This method is used to update [PUT] the document for a given user from the SignNow Application
        */
		public string UpdateDocument(Oauth2Token token, DocumentFieldMap fieldsMap, string id)
		{
			var response = client.GetResponse(token, "/document" + "/" + id, Method.PUT, fieldsMap);
			return response.GetResult()["id"].ToString();
		}

		/*
         * This method is used to (POST) invite the signers to sign on  the document in the SignNow Application
         */
		public string Invite(Oauth2Token token, Invitation invitation, string id, bool sendEmail = false)
		{
			var url = "/document/" + id + "/invite";
			if (!sendEmail)
				url += "?email=disable";

			var response = client.GetResponse(token, url, Method.POST, invitation);
			var result = response.GetResult();

			return result["result"].ToString();
		}

		/*
        This method is used to (POST)perform rolebased  to invite the signers to sign on  the document in the SignNow Application
        */
		public string RoleBasedInvite(Oauth2Token token, EmailSignature emailSignature, string id, bool sendEmail = false)
		{
			var url = "/document/" + id + "/invite";
			if (!sendEmail)
				url += "?email=disable";

			var response = client.GetResponse(token, url, Method.POST, emailSignature);
			var result = response.GetResult();
			return result["status"].ToString();
		}

		/*
         * This method Cancels an invite to a document.
         */
		public string CancelInvite(Oauth2Token token, string id)
		{
			var response = client.GetResponse(token, "/document/" + id + "/fieldinvitecancel", Method.PUT);
			var result = response.GetResult();
			return result["status"].ToString();
		}

		/*
         * This method is used to download (POST) the document as PDF for a given user from the SignNow Application
         */
		public Document ShareDocument(Oauth2Token token, string id)
		{
			var response = client.GetResponse(token, "/document" + "/" + id + "/download/link", Method.POST);
			var result = response.GetResult();
			return result.ToObject<Document>();
		}

		/*
        This method is used to (GET) get the Document History for a given Document and for a given user from the SignNow Application
        */
		public IEnumerable<DocumentHistory> GetDocumentHistory(Oauth2Token token, string id)
		{
			var response = client.GetResponse(token, "/document" + "/" + id + "/history", Method.GET);
			var result = response.GetResult();
			return result.ToObject<DocumentHistory[]>();
		}

		/*
         * This method is used to (POST)  create the template from a document in the SignNow Application
         */
		public Template CreateTemplate(Oauth2Token token, Template template)
		{
			var response = client.GetResponse(token, "/template", Method.POST, template);
			return response.GetResult<Template>();
		}

		/*
        This method is used to (POST) create a new document from the given template id in the SignNow Application
        */
		public Template CreateNewDocumentFromTemplate(Oauth2Token token, Template template)
		{
			var response = client.GetResponse(token, "/template" + "/" + template.Id + "/copy", Method.POST, template);
			return response.GetResult<Template>();
		}

		/*
        This method is used to Download a collapsed document(Response Content = application/pdf)
        */
		public byte[] DownloadCollapsedDocument(Oauth2Token token, string id)
		{
			var response = client.GetResponse(token, "/document" + "/" + id + "/download?type=collapsed", Method.GET, accept: null, contentType: null);
			response.GetResult(); // check for errors
			return response.RawBytes;
		}

		/*
         This method is used to Deletes a previously uploaded document
         */
		public string DeleteDocument(Oauth2Token token, string id)
		{
			var request = client.GetResponse(token, "/document" + "/" + id, Method.DELETE);
			return request.GetResult()["status"].ToString();
		}

		/*
         This method is used to (POST) merge the new document from the given template id in the SignNow Application
         */
		public byte[] MergeDocuments(Oauth2Token token, Hashtable myMergeMap)
		{
			byte[] arr = null;
			try
			{
				string requestBody = JsonConvert.SerializeObject(myMergeMap, Formatting.Indented);
				var client = new RestClient();
				client.BaseUrl = config.GetApiBase();

				var request = new RestRequest("/document/merge", Method.POST)
						.AddHeader("Accept", "application/json")
						.AddHeader("Content-Type", "application/pdf")
						.AddHeader("Authorization", "Bearer " + token.AccessToken);
				request.AddParameter("text/json", requestBody, ParameterType.RequestBody);

				var httpResponse = client.Execute(request);
				arr = httpResponse.RawBytes;
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Exception: {0}", ex.Message));
				throw;
			}
			return arr;
		}

		/*
         This method is Used for creating webhooks that will be triggered when the specified event takes place.
         */
		public EventSubscription CreateEventSubscription(Oauth2Token token, EventSubscription events)
		{
			EventSubscription result = null;
			try
			{
				string requestBody = JsonConvert.SerializeObject(events, Formatting.Indented);
				var client = new RestClient();
				client.BaseUrl = config.GetApiBase();

				var request = new RestRequest("/event_subscription", Method.POST)
						.AddHeader("Content-Type", "application/json")
						.AddHeader("Authorization", "Bearer " + token.AccessToken);
				request.AddParameter("text/json", requestBody, ParameterType.RequestBody);

				var httpResponse = client.Execute(request);
				string json = httpResponse.Content.ToString();
				result = JsonConvert.DeserializeObject<EventSubscription>(json);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Exception: {0}", ex.Message));
				throw;
			}
			return result;
		}

		/*
         This method is used to Delete an event subscription.
         */
		public EventSubscription DeleteEventSubscription(Oauth2Token token, string id)
		{
			EventSubscription result = null;
			try
			{
				string requestBody = JsonConvert.SerializeObject(token, Formatting.Indented);
				var client = new RestClient();
				client.BaseUrl = config.GetApiBase();

				var request = new RestRequest("/event_subscription" + "/" + id, Method.DELETE)
						.AddHeader("Accept", "application/json")
						.AddHeader("Authorization", "Bearer " + token.AccessToken);

				var httpResponse = client.Execute(request);
				string json = httpResponse.Content.ToString();
				result = JsonConvert.DeserializeObject<EventSubscription>(json);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Exception: {0}", ex.Message));
				throw;
			}
			return result;
		}

		/*
         This method Uploads a file that contains SignNow Document Field Tags (Simple Field tags only)
         */
		public Document CreateSimpleFieldTag(Oauth2Token token, Document documentPath)
		{
			Document document = null;
			try
			{
				string requestBody = JsonConvert.SerializeObject(documentPath.FilePath, Formatting.Indented);
				var client = new RestClient();
				client.BaseUrl = config.GetApiBase();

				var request = new RestRequest("/document/fieldextract", Method.POST)
						.AddHeader("Authorization", "Bearer " + token.AccessToken)
						.AddHeader("Content-Type", "multipart/form-data");
				request.AddFile("file", documentPath.FilePath);

				var httpResponse = client.Execute(request);

				string json = httpResponse.Content.ToString();
				document = JsonConvert.DeserializeObject<Document>(json);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Exception: {0}", ex.Message));
				throw;
			}
			return document;
		}

		/*
         * This method is used to create  or POST the document that contains SignNow Document Field Tags.
         */
		public Document CreateDocumentFieldExtract(Oauth2Token token, string filePath)
		{
			return CreateInternal(token, filePath, null, "/document/fieldextract");
		}
		public Document CreateDocumentFieldExtract(Oauth2Token token, string fileName, byte[] data)
		{
			return CreateInternal(token, fileName, data, "/document/fieldextract");
		}
		public Document CreateDocumentFieldExtract(Oauth2Token token, string fileName, Stream stream)
		{
			if (token == null)
				throw new ArgumentNullException("token");
			if (stream == null)
				throw new ArgumentNullException("stream");
			if (fileName == null)
				throw new ArgumentNullException("filePath");
			using (var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				return CreateInternal(token, fileName, ms.ToArray(), "/document/fieldextract");
			}
		}
	}
}