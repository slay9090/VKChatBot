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
                AccessToken = "*your secret group key*" //ключ группы
            });
            Console.WriteLine(api.Token);
        }



    }
}
