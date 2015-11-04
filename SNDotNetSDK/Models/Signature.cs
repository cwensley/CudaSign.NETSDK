using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNDotNetSDK.Models
{
	public class Signature : Field
	{
		/// <summary>
		/// Base 64 encoded signature data
		/// </summary>
		public string Data { get; set; }
	}
}
