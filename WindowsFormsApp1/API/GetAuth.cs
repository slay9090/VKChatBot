using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace ServerChatBalakovo.API
{
    public class GetAuth
    {
        public VkApi api = new VkApi();
        public GetAuth()
        {
          //  FlurlHttp.Configure(settings => { settings.HttpClientFactory = new OTHER.WindowsAuthClientFactory(); });
          // FlurlHttp.Configure(settings => settings.HttpClientFactory = new OTHER.PollyHttpClientFactory());
            api.Authorize(new ApiAuthParams()
            {
                AccessToken = "5c9fb58b8bd85a05dd8017f24b8ab6ec0b689a6c359e74ae6051980272d932ce9882bc719e0c4cc924797"
            });
            Console.WriteLine(api.Token);
        }



    }
}
