﻿using Newtonsoft.Json;
using RestSharp;
using SNDotNetSDK.Configuration;
using SNDotNetSDK.Models;
using SNDotNetSDK.Service;
using System;

namespace SNDotNetSDK.ServiceImpl
{
    /**
     * Created by Deepak on 5/14/2015
     * 
     * This class is used to perform the OAuth2 token specific operations to access SignNow Application.
     */
    public class OAuth2TokenService : IAuthenticationService
    {
        private Config config;
        public OAuth2TokenService(Config config)
        {
            this.config = config;
        }

        /**
        * This method is used to request (POST)the OAuth2 token for a specific user to access SignNow Application.
        */
        public Oauth2Token RequestToken(User user)
        {
            
            Oauth2Token requestedToken = null;
            try
            {
                var client = new RestClient();
                client.BaseUrl = config.GetApiBase();

                var request = new RestRequest("/oauth2/token", Method.POST)
                        .AddHeader("Accept", "application/json")
                        .AddHeader("Authorization", "Basic " + config.GetBase64EncodedClientCredentials())
                        .AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("username", user.Email)
                        .AddParameter("password", user.Password)
                        .AddParameter("grant_type", "password");

                var httpResponse = client.Execute(request);

                string json = httpResponse.Content.ToString();
                requestedToken = JsonConvert.DeserializeObject<Oauth2Token>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception: {0}", ex.Message));
                throw;
            }
        return requestedToken;
        }

        /**
        * This method is used to verify (GET) the OAuth2 token for a specific user to access SignNow Application.
        */
        public Oauth2Token Verify(string AccessToken)
        {
        Oauth2Token verifyToken = null;
        try {
            var client = new RestClient();
            client.BaseUrl = config.GetApiBase();

            var request = new RestRequest("/oauth2/token", Method.GET)
                    .AddHeader("Authorization", "Bearer " + AccessToken)
                    .AddHeader("Accept", "application/json");
            
            var httpResponse = client.Execute(request);
       
            string json = httpResponse.Content.ToString();
            verifyToken = JsonConvert.DeserializeObject<Oauth2Token>(json);
        } 
        catch (Exception ex) 
        {
            Console.WriteLine(string.Format("Exception: {0}", ex.Message));
            throw;
        }
        return verifyToken;
        }
    }
}
