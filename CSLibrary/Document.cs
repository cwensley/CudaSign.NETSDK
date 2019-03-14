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
		/// <param name="bytes">The bytes of the file, or null if <paramref name="fileName"/> refers to a file on disk</param>
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
		/// <param name="accessToken"></param>
		/// <param name="documentID">Document Id to update</param>
		/// <param name="data">Data for the updated fields</param>
		/// <returns>Document ID</returns>
		public string Update(OAuth2Token accessToken, string documentId, UpdateRequest data)
        {
			var request = client.CreateRequest(accessToken, "/document/" + documentId, Method.PUT);

            request.AddJsonNetBody(data);

            var response = client.Execute(request);

			var result = response.GetResult();
            return result["id"].Value<string>();
        }

        /// <summary>
        /// Retrieve a Document Resource
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="documentId">Document Id</param>
		/// <param name="withAnnotations">Include annotations in the result</param>
        /// <returns>Document Information, Status, Fields...</returns>
        public DocumentInfo Get(OAuth2Token accessToken, string documentId, bool withAnnotations = false)
        {
			var qsWithAnnotations = (withAnnotations) ? "?with_annotation=true" : "";

			var request = client.CreateRequest(accessToken, "/document/" + documentId, Method.GET);

			var response = client.Execute(request);

			return response.GetResult<DocumentInfo>();
        }

        /// <summary>
        /// Deletes an Existing Document
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="documentId">Document Id</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise</returns>
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
		/// <returns>Collapsed document data in PDF format.</returns>
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
		/// <param name="inviteData">Details for the invite data, either a <see cref="FreeFormInvite"/> or <see cref="RoleBasedInvite"/> object.</param>
		/// <returns><c>true</c> if successful, <c>false</c> otherwise</returns>
		public bool Invite(OAuth2Token accessToken, string documentId, InviteRequest inviteData, bool disableEmail = false)
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
        /// <param name="accessToken"></param>
        /// <param name="documentId"></param>
        /// <returns>URL to download the document as a PDF</returns>
        public string Share(OAuth2Token accessToken, string documentId)
        {
			var request = client.CreateRequest(accessToken, "/document/" + documentId + "/download/link", Method.POST);

            var response = client.Execute(request);

			return response.GetResult()["link"].Value<string>();
        }

		/// <summary>
		/// Merges Existing Documents
		/// </summary>
		/// <param name="accessToken"></param>
		/// <param name="name">The name of the newly merged document</param>
		/// <param name="documentIds">An array of document IDs to merge</param>
		/// <returns>The merged document data in PDF.</returns>
		public byte[] Merge(OAuth2Token accessToken, string name, params string[] documentIds)
        {
			var request = client.CreateRequest(accessToken, "/document/merge", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { name = name, document_ids = documentIds });

            var response = client.Execute(request);

			response.GetResult(); // ensure there's no errors

			return response.RawBytes;
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
