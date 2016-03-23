using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaSign
{
	public class FieldAttributes
	{
		public int PageNumber { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public bool Required { get; set; }
		public string Label { get; set; }
		public string PrefilledText { get; set; }
	}

	public class FieldInfo
	{
		public string Id { get; set; }
		public string Type { get; set; }  // use enum?  what are the valid types?
		public string RoleId { get; set; }
		[JsonProperty("json_attributes")]
		public FieldAttributes Attributes { get; set; }
		public string Role { get; set; }
		public string Originator { get; set; }
		public string Fulfiller { get; set; }
		public string FieldRequestId { get; set; }
		public string ElementId { get; set; }
		//public bool? FieldRequestCanceled { get; set; }
		public string TemplateFieldId { get; set; }
		public string FieldId { get; set; }
	}

	public class RoleInfo
	{
		public string UniqueId { get; set; }
		public int SigningOrder { get; set; }
		public string Name { get; set; }
	}

	public class FolderInfo
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public DateTime Created { get; set; }
		public string Name { get; set; }
		public string ParentId { get; set; }
		public bool SystemFolder { get; set; }
		public bool Shared { get; set; }
	}

	public class FolderList : FolderInfo
	{
		public List<FolderInfo> Folders { get; } = new List<FolderInfo>();
	}

	public class FieldInviteInfo
	{
		public string Id { get; set; }
		public string Status { get; set; } // use enum?  what are the valid statuses?
		public DateTime Updated { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
	}

	public class SignatureInfo
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public string Email { get; set; }
		public int PageNumber { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public DateTime Created { get; set; }
		public byte[] Data { get; set; }
	}

	public class DocumentInfo
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
		public string DocumentName { get; set; }
		public string OriginalFilename { get; set; }
		public string OriginDocumentId { get; set; }
		public string Owner { get; set; }
		public string OriginUserId { get; set; }
		public int PageCount { get; set; }
		public bool Template { get; set; }
		public string ParentId { get; set; }
		public List<FieldInfo> Fields { get; } = new List<FieldInfo>();
		public List<RoleInfo> Roles { get; } = new List<RoleInfo>();
		public List<FieldInviteInfo> FieldInvites { get; } = new List<FieldInviteInfo>();
		public List<SignatureInfo> Signatures { get; } = new List<SignatureInfo>();
	}

	public class FolderDetail : FolderList
	{
		public int TotalDocuments { get; set; }
		public List<DocumentInfo> Documents { get; } = new List<DocumentInfo>();
	}

	public class LinkInfo
	{
		public string Url { get; set; }

		public string UrlNoSignup { get; set; }
	}
}
