using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNDotNetSDK.Models
{
	public class DocumentFieldMap
	{
		List<Text> texts = new List<Text>();
		List<Field> fields = new List<Field>();
		List<Checkbox> checks = new List<Checkbox>();

		[JsonProperty("texts")]
		public IList<Text> Texts { get { return texts; } }

		[JsonProperty("fields")]
		public IList<Field> Fields { get { return fields; } }

		[JsonProperty("checks")]
		public IList<Checkbox> Checks { get { return checks; } }
	}
}
