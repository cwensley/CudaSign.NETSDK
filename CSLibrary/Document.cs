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
	public abstract class InviteBase
	{
	}

	public class FreeFormInvite : InviteBase
	{
		public string From { get; }
		public string To { get; }

		public FreeFormInvite(string from, string to)
		{
			From = from;
			To = to;
		}
	}

	public class Signer
	{
		public string Email { get; }
		public string Role { get; }
		public int Order { get; }
		public string RoleId { get; }
		public string AuthenticationType { get; set; }
		public string Password { get; set; }
		public int? ExpirationDays { get; set; }
		public int? Reminder { get; set; }

		public Signer(string email, string role, int order = 1)
		{
			Email = email;
			Role = role;
			Order = order;
			RoleId = string.Empty;
			//AuthenticationType = "password";
		}
	}

	public class RoleBasedInvite : InviteBase
	{
		public Signer[] To { get; }
		public string From { get; }

		public RoleBasedInvite(string from, params Signer[] to)
		{
			if (to == null || to.Length == 0)
				throw new ArgumentOutOfRangeException("to", "You must specify at least one signer");
			From = from;
			To = to;
		}

		public string[] Cc { get; set; }

		public string Subject { get; set; }

		public string Message { get; set; }
	}

	public class Document
    {
		CudaSignClient client;
		internal Document(CudaSignClient client)
		{
			this.client = client;
		}

		/// <summary>
		/// Uploads a File and Creates a Document
		/// Accepted File Types: .doc, .docx, .pdf, .png
		/// </summary>
		/// <param name="accessToken"></param>
		/// <param name="fileName">Local Path to the File, or name of file if <paramref name="bytes"/> is specified</param>
		/// <param name="extractFields">If set TRUE the document will be checked for special field tags. If any exist they will be converted to fields.</param>
		/// <returns>ID of the document that was created</returns>
		public string Create(OAuth2Token accessToken, string fileName, bool extractFields = false, byte[] bytes = null)
		{
			var path = (extractFields) ? "/document/fieldextract" : "/document";

			var request = client.CreateRequest(accessToken, path, Method.POST);

			if (bytes != null)
				request.AddFile("file", bytes, fileName);
			else
				request.AddFile("file", Path.GetFullPath(fileName));

			var response = client.Execute(request);

			var result = response.GetResult();

			return Convert.ToString(result["id"]);
		}

		/// <summary>
		/// Updates an Existing Document
		/// </summary>
		/// <param name="AccessToken"></param>
		/// <param name="DocumentID">Document Id</param>
		/// <param name="DataObj">Data Object (ex. dynamic new { fields = new[] { new { x = 10, y = 10, width = 122... } } }</param>
		/// <returns>Document ID</returns>
		public string Update(string AccessToken, string DocumentId, dynamic DataObj, string ResultFormat = "JSON")
        {
            var request = new RestRequest("/document/" + DocumentId, Method.PUT)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", "Bearer " + AccessToken);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(DataObj);

            var response = client.Execute(request);


			var result = response.GetResult();
            return result["id"].Value<string>();
        }

        /// <summary>
        /// Retrieve a Document Resource
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="documentId">Document Id</param>
        /// <returns>Document Information, Status, Fields...</returns>
        public DocumentInfo Get(OAuth2Token accessToken, string documentId)
        {
			var request = client.CreateRequest(accessToken, "/document/" + documentId, Method.GET);

			var response = client.Execute(request);

			return response.GetResult<DocumentInfo>();
        }

        /// <summary>
        /// Deletes an Existing Document
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="documentId">Document Id</param>
        /// <returns>{"status":"success"}</returns>
        public bool Delete(OAuth2Token accessToken, string documentId)
        {
			var request = client.CreateRequest(accessToken, "/document/" + documentId, Method.DELETE);

            var response = client.Execute(request);

			var result = response.GetResult();
            return result.IsCudaSuccess();
        }

		/// <summary>
		/// Downloads a Collapsed Document
		/// </summary>
		/// <param name="accessToken"></param>
		/// <param name="documentId">Document Id</param>
		/// <param name="SaveFilePath">Local Path to Save File</param>
		/// <param name="SaveFileName">File Name without Extension</param>
		/// <returns>Collapsed document in PDF format saved to a the location provided.</returns>
		public byte[] Download(OAuth2Token accessToken, string documentId)
        {
			var request = client.CreateRequest(accessToken, "/document/" + documentId + "/download?type=collapsed", Method.GET);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
				return client.DownloadData(request);
            }
            else
            {
				response.GetResult();
				throw new CudaSignException((int)response.StatusCode, response.StatusDescription);
            }
        }

        /// <summary>
        /// Send a Role-based or Free Form Document Invite
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="documentId"></param>
        /// <param name="dataObj">Data Object (ex. dynamic new { to = new[] { new { email = "name@domain.com", role_id = ... } } }</param>
        /// <returns>{"result":"success"}</returns>
        public bool Invite(OAuth2Token accessToken, string documentId, InviteBase inviteData, bool disableEmail = false)
        {
			var disableEmailParam = (disableEmail) ? "?email=disable" : "";

			var request = client.CreateRequest(accessToken, "/document/" + documentId + "/invite" + disableEmailParam, Method.POST);
			request.AddJsonNetBody(inviteData);


            var response = client.Execute(request);

			var result = response.GetResult();
			return result.IsCudaSuccess();
        }

        /// <summary>
        /// Cancel Invite
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="documentId"></param>
        /// <returns>{"status":"success"}</returns>
        public bool CancelInvite(OAuth2Token accessToken, string documentId)
        {
			var request = client.CreateRequest(accessToken, "/document/" + documentId + "/fieldinvitecancel", Method.PUT);

            var response = client.Execute(request);

			var result = response.GetResult();

			return result.IsCudaSuccess();
     }

        /// <summary>
        /// Create a One-time Use Download URL
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="DocumentId"></param>
        /// <param name="ResultFormat">JSON, XML</param>
        /// <returns>URL to download the document as a PDF</returns>
        public dynamic Share(string AccessToken, string DocumentId, string ResultFormat = "JSON")
        {
            var request = new RestRequest("/document/" + DocumentId + "/download/link", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", "Bearer " + AccessToken);

            var response = client.Execute(request);

            dynamic results = "";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Content;
            }
            else
            {
                Console.WriteLine(response.Content.ToString());
                results = response.Content.ToString();
            }

            if (ResultFormat == "JSON")
            {
                results = JsonConvert.DeserializeObject(results);
            }
            else if (ResultFormat == "XML")
            {
                results = (XmlDocument)JsonConvert.DeserializeXmlNode(results, "root");
            }

            return results;
        }

        /// <summary>
        /// Merges Existing Documents
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="DataObj">Data Object (ex. dynamic new { to = new[] { new { name = "My New Merged Doc", document_ids = ... } } }</param>
        /// <param name="ResultFormat">JSON, XML</param>
        /// <returns>Location the PDF file was saved to.</returns>
        public dynamic Merge(string AccessToken, dynamic DataObj, string SaveFilePath = "", string SaveFileName = "my-merged-document", string ResultFormat = "JSON")
        {
            var request = new RestRequest("/document/merge", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", "Bearer " + AccessToken);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(DataObj);

            var response = client.Execute(request);

            dynamic results = "";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var path = (SaveFilePath != "") ? Path.GetDirectoryName(SaveFilePath) + "\\" + SaveFileName + ".pdf" : Directory.GetCurrentDirectory() + "\\" + SaveFileName + ".pdf";
                client.DownloadData(request).SaveAs(path);

                dynamic jsonObject = new JObject();
                jsonObject.file = path;

                results = JsonConvert.SerializeObject(jsonObject);
            }
            else
            {
                Console.WriteLine(response.Content.ToString());
                results = response.Content.ToString();
            }

            if (ResultFormat == "JSON")
            {
                results = JsonConvert.DeserializeObject(results);
            }
            else if (ResultFormat == "XML")
            {
                results = (XmlDocument)JsonConvert.DeserializeXmlNode(results, "root");
            }

            return results;
        }

        /// <summary>
        /// Get Document History
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="DocumentId"></param>
        /// <param name="ResultFormat">JSON, XML</param>
        /// <returns>Array of history for the document.</returns>
        public dynamic History(string AccessToken, string DocumentId, string ResultFormat = "JSON")
        {
            var request = new RestRequest("/document/" + DocumentId + "/history", Method.GET)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", "Bearer " + AccessToken);

            var response = client.Execute(request);

            dynamic results = "";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Content;
            }
            else
            {
                Console.WriteLine(response.Content.ToString());
                results = response.Content.ToString();
            }

            if (ResultFormat == "JSON")
            {
                results = JsonConvert.DeserializeObject(results);
            }
            else if (ResultFormat == "XML")
            {
                results = (XmlDocument)JsonConvert.DeserializeXmlNode(results, "root");
            }

            return results;
        }

		public bool Move(OAuth2Token accessToken, string documentId, string newFolderId)
		{
			var request = client.CreateRequest(accessToken, "/document/" + documentId + "/move", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(new { folder_id = newFolderId });

			var response = client.Execute(request);

			var result = response.GetResult();

			return result.IsCudaSuccess();
		}
	}
}
