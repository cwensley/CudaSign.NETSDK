using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CudaSign
{
	public class UserIdentity
	{
		public string Identified { get; set; }
		public string Status { get; set; }
		public bool OkToRetry { get; set; }
	}

	public class UserDetail
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Active { get; set; }
		public int Type { get; set; }
		public int Pro { get; set; }
		public DateTime Created { get; set; }
		public string[] Emails { get; set; }
		public UserIdentity Identity { get; set; }
		public int Credits { get; set; }
		public bool HasAtticusAccess { get; set; }
		public bool IsLoggedIn { get; set; }
		//public TeamInfo[] Teams { get; set; } ?? what is the return type here, or is it strings?  the api docs don't give an example.
	}

	public class CreateUserResult
	{
		public string Id { get; set; }
		public int Verified { get; set; }
		public string Email { get; set; }
	}

    public class User
    {
		CudaSignClient client;
		internal User(CudaSignClient client)
		{
			this.client = client;
		}

		/// <summary>
		/// Creates a New User CudaSign Account
		/// </summary>
		/// <param name="user">User Information</param>
		/// <param name="Password">New User's Password</param>
		/// <param name="FirstName">New User's First Name</param>
		/// <param name="LastName">New User's Last Name</param>
		/// <returns>The ID of the new user account and verification status.</returns>
		public CreateUserResult Create(UserInfo user, string clientId = null, string clientSecret = null)
        {
            var clientCredentials = client.EncodedClientCredentials;

            if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret))
            {
                clientCredentials = client.EncodeClientCredentials(clientId, clientSecret);
            }

            var request = new RestRequest("/user", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("Authorization", "Basic " + client.EncodedClientCredentials);

			request.AddJsonNetBody(user);

            var response = client.Execute(request);

			return response.GetResult<CreateUserResult>();
        }

		/// <summary>
		/// Retrieves a User Account
		/// </summary>
		/// <param name="accessToken">User's Access Token</param>
		/// <returns>User Account Information</returns>
		public UserDetail Get(OAuth2Token accessToken)
        {
			var request = client.CreateRequest(accessToken, "/user", Method.GET);

            var response = client.Execute(request);

			return response.GetResult<UserDetail>();
        }
    }
}
