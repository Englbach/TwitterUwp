using LinqToTwitter;
using Microsoft.Toolkit.Uwp.Services.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TwitterUwp.Interfaces;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace TwitterUwp.Interfaces
{

    public interface Itools
    {
        bool CheckInternetConnectionAsync();
        void TwitterAuthorizer();

    }

    public class Tools : Itools
    {
        /// <summary>
        /// 
        /// </summary>
        const string ComsumerKey = "52EtgzgVOIDsr4vY9DCb7aus8";
        const string ConsumerSecret = "zjLfzObKJ7Q6nV3stvRyKHWTYAR5664xMnFQf7WHUnWXynsz5O";
        const string callBackUri = "http://myapp.com/auth/twitter/callback/";
        const string request_token_url = "https://api.twitter.com/oauth/request_token";
        const string request_accesstoken_url = "https://api.twitter.com/oauth/access_token";
        const string request_oauth_url = "https://api.twitter.com/oauth/authorize";


        /// <summary>
        /// Check internet
        /// </summary>
        /// <returns></returns>
        public bool CheckInternetConnectionAsync()
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            return isInternetConnected;
        }

        public async void TwitterAuthorizer()
        {
            var authorizer = new UniversalAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = ComsumerKey,
                    ConsumerSecret = ConsumerSecret,
                },
                Callback = callBackUri
            };
            await authorizer.AuthorizeAsync();

            SharedState.Authorizer = authorizer;

            //var ctx = new TwitterContext(authorizer);
            //return ctx;
        }

        
    }
}
