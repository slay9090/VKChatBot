using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.OTHER
{
    class PollyHttpClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new PolicyHandler
            {
                InnerHandler = new HttpClientHandler {
                    DefaultProxyCredentials = CredentialCache.DefaultCredentials
                }        

            };

        }

    


    }


}
