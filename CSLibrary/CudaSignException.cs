using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CudaSign
{
	[Serializable]
	public class CudaSignException : Exception
	{
		public int Code { get; private set; }

		internal CudaSignException(int code, string message) : base(message)
		{
			Code = code;
		}
		internal CudaSignException(int code, string message, Exception inner)
			: base(message, inner)
		{
			Code = code;
		}

		protected CudaSignException(SerializationInfo info, StreamingContext context) : base(info, context)
		{ }

		public override string ToString()
		{
			return $"Error Code {Code}: " + base.ToString();
		}
	}
}
