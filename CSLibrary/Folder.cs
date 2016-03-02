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
	public enum SigningStatus
	{
		WaitingForMe,
		WaitinfForOthers,
		Signed,
		Pending
	}

	public enum FolderSort
	{
		DocumentName,
		Updated,
		Created
	}

	public enum SortOrder
	{
		Ascending,
		Descending
	}

	public abstract class FolderFilter
	{

	}

	public class SigningStatusFolderFilter : FolderFilter
	{
		public SigningStatus Status { get; }

		public SigningStatusFolderFilter(SigningStatus status)
		{
			Status = status;
		}

		public override string ToString()
		{
			return "filter=signing-status&filter-value=" + NameHelpers.Dash(Status.ToString());
		}
	}

	public class DocumentUpdatedFolderFilter : FolderFilter
	{
		public DateTime Updated { get; }

		public DocumentUpdatedFolderFilter(DateTime updated)
		{
			Updated = updated;
		}

		public override string ToString()
		{
			return "filter=document-updated&filter-value=" + Updated.ToString();
		}
	}

	public class DocumentCreatedFolderFilter : FolderFilter
	{
		public DateTime Created { get; }

		public DocumentCreatedFolderFilter(DateTime created)
		{
			Created = created;
		}

		public override string ToString()
		{
			return "filter=document-created&filter-value=" + Created.ToString();
		}
	}

	public class Folder
    {
		CudaSignClient client;
		internal Folder(CudaSignClient client)
		{
			this.client = client;
		}

		/// <summary>
		/// Gets a List of Folders
		/// </summary>
		/// <param name="token"></param>
		/// <returns>Folders, Document & Template Counts</returns>
		public FolderList List(OAuth2Token token)
        {
			var request = client.CreateRequest(token, "/folder", Method.GET);

            var response = client.Execute(request);

			return response.GetResult<FolderList>();
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="token"></param>
		/// <param name="folderId">ID of the Folder to Get</param>
		/// <param name="params">Option Filter and Sort By Params</param>
		/// <returns>List of documents in the folder.</returns>
		public FolderDetails Get(OAuth2Token token, string folderId, FolderFilter filter = null, FolderSort? sortBy = null, SortOrder order = SortOrder.Ascending)
        {
			var sb = new StringBuilder();
			if (filter != null)
				sb.Append(filter.ToString());
			if (sortBy != null)
			{
				if (sb.Length > 0) sb.Append("&");
				sb.Append(NameHelpers.Dash(sortBy.Value.ToString()));
				sb.Append("&order=");
				sb.Append(order == SortOrder.Ascending ? "asc" : "desc");
			}
			if (sb.Length > 0)
				sb.Insert(0, "?");

			var request = client.CreateRequest(token, "/folder/" + folderId + sb.ToString(), Method.GET);

            var response = client.Execute(request);

			return response.GetResult<FolderDetails>();
        }
    }
}
