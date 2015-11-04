using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SNDotNetSDK.Models
{
	/**
     * Created by Deepak on 5/14/2015
     * 
     * This class is used to create the document model object
     */
	public class Document
	{
		public string Id { get; set; }

		[JsonProperty("oauth2_token")]
		public Oauth2Token OAuth2Token { get; set; }

		public string Link { get; set; }

		[JsonProperty("file_path")]
		public string FilePath { get; set; }

		public Field[] Fields { get; set; }

		public Signature[] Signatures { get; set; }

        public string Error { get; set; }

        public int Code { get; set; }
    }
}