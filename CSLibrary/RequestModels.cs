using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaSign
{
	#region Document Models

	public abstract class InviteRequest
	{
		public string From { get; }

		public InviteRequest(string from)
		{
			From = from;
		}
	}

	public class FreeFormInvite : InviteRequest
	{
		public string To { get; }

		public FreeFormInvite(string from, string to)
			: base(from)
		{
			To = to;
		}
	}

	public class Signer
	{
		public string Email { get; }
		public string Role { get; }
		public int Order { get; }
		public string RoleId { get; }
		public string AuthenticationType { get; set; } // use enum?  what values are valid?
		public string Password { get; set; }
		public int? ExpirationDays { get; set; }
		public int? Reminder { get; set; }

		public Signer(string email, string role, int order = 1)
		{
			Email = email;
			Role = role;
			Order = order;
			RoleId = string.Empty;
		}
	}

	public class RoleBasedInvite : InviteRequest
	{
		public List<Signer> To { get; } = new List<Signer>();

		public RoleBasedInvite(string from, params Signer[] to)
			: base(from)
		{
			if (to == null || to.Length == 0)
				throw new ArgumentOutOfRangeException("to", "You must specify at least one signer");
			To.AddRange(to);
		}

		public List<string> Cc { get; } = new List<string>();

		public string Subject { get; set; }

		public string Message { get; set; }
	}

	public class Check
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int PageNumber { get; set; }
	}

	public class Text
	{
		public int Size { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int PageNumber { get; set; }
		public string Font { get; set; }
		public string Data { get; set; }
		public float LineHeight { get; set; }
	}

	public abstract class Field
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int PageNumber { get; set; }
		public string Role { get; set; }
		public bool Required { get; set; }
		public string Type { get; protected set; }
	}

	public class SignatureField : Field
	{
		public SignatureField()
		{
			Type = "signature";
		}
	}

	public class TextField : Field
	{
		public string Label { get; set; }

		public TextField()
		{
			Type = "text";
		}
	}

	public class CheckBoxField : Field
	{
		public CheckBoxField()
		{
			Type = "checkbox";
		}
	}

	public class InitialsField : Field
	{
		public InitialsField()
		{
			Type = "initials";
		}
	}

	public class EnumerationField : Field
	{
		public EnumerationField()
		{
			Type = "enumeration";
		}

		public string Label { get; set; }

		public bool CustomDefinedOption { get; set; }

		public List<string> EnumerationOptions { get; } = new List<string>();
	}

	public class RadioButtonOption
	{
		public int PageNumber { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public string Checked { get; set; } // huh? is it an int or string here?  what is valid?
		public string Value { get; set; }
		public DateTime? Created { get; set; } // not sure why docs show this as part of the request
	}

	public class RadioButtonField : Field
	{
		public RadioButtonField()
		{
			Type = "radiobutton";
		}

		public List<RadioButtonOption> Radio { get; } = new List<RadioButtonOption>();
	}


	public class UpdateRequest
	{
		public List<Text> Texts { get; } = new List<Text>();

		public List<Check> Checks { get; } = new List<Check>();

		public List<Field> Fields { get; } = new List<Field>();
	}

	#endregion

	#region Folder Models

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

	#endregion

	#region User Models

	public class UserInfo
	{
		public string Email { get; }
		public string Password { get; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public UserInfo(string email, string password)
		{
			Email = email;
			Password = password;
		}
	}

	#endregion
}
