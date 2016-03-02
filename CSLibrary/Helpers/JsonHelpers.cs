using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CudaSign
{
	static class JsonHelpers
	{
		static JsonSerializer serializer;
		static JsonSerializerSettings settings;

		public static JsonSerializer Serializer
		{
			get { return serializer ?? (serializer = JsonSerializer.Create(Settings)); }
		}

		public static JsonSerializerSettings Settings
		{
			get
			{
				if (settings != null)
					return settings;
				settings = new JsonSerializerSettings
				{
					ContractResolver = new UnderscoreMappingResolver(),
					NullValueHandling = NullValueHandling.Ignore,
					Converters = {
						new CustomDateTimeConverter(),
						new StringEnumConverter
						{
							CamelCaseText = false
						}
					}
				};
				return settings;
			}
		}

		class UnderscoreMappingResolver : DefaultContractResolver
		{
			protected override string ResolvePropertyName(string propertyName)
			{
				return NameHelpers.Underscore(propertyName);
			}
		}

		class CustomDateTimeConverter : JsonConverter
		{
			public override bool CanConvert(Type objectType)
			{
				return objectType == typeof(DateTime);
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				var t = long.Parse((string)reader.Value);
				return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(t);
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				throw new NotImplementedException();
			}
		}
	}
}
