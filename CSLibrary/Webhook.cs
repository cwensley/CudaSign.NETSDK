﻿using Newtonsoft.Json;
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
using System.Dynamic;
using System.Xml;

namespace CudaSign
{
    public class Webhook
    {
		CudaSignClient client;
		internal Webhook(CudaSignClient client)
		{
			this.client = client;
		}

        /// <summary>
        /// Get a List of Current Webhook Subscriptions
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="ResultFormat">JSON, XML</param>
        /// <returns>List of Subscriptions</returns>
        public dynamic List(string AccessToken, string ResultFormat = "JSON")
        {
            var request = new RestRequest("/event_subscription", Method.GET)
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
        /// Create Webhook that will be Triggered when the Specified Event Takes Place
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="EventType">document.create, document.update, document.delete, invite.create, invite.update</param>
        /// <param name="CallbackUrl">The URL called when the even is triggered.</param>
        /// <param name="ResultFormat">JSON, XML</param>
        /// <returns>ID, Created, Updated</returns>
        public dynamic Create(string AccessToken, string EventType, string CallbackUrl, string ResultFormat = "JSON")
        {
            var request = new RestRequest("/event_subscription", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", "Bearer " + AccessToken);

            request.RequestFormat = DataFormat.Json;

            dynamic jsonObj = new ExpandoObject();
            jsonObj.@event = EventType;
            jsonObj.callback_url = CallbackUrl;

            request.AddBody(jsonObj);

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
        /// Deletes a Webhook
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="SubscriptionId"></param>
        /// <param name="ResultFormat">JSON, XML</param>
        /// <returns>{"status":"success"}</returns>
        public dynamic Delete(string AccessToken, string SubscriptionId, string ResultFormat = "JSON")
        {
            var request = new RestRequest("/event_subscription/" + SubscriptionId, Method.DELETE)
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

    }
}
