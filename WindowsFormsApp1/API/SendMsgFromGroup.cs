using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.RequestParams;

namespace ServerChatBalakovo.API
{
    public class SendMsgFromGroup
    {
        private API.GetAuth _GetAuth;
        public SendMsgFromGroup(API.GetAuth GetAuth)
        {
            this._GetAuth = GetAuth;
        }
      

        public void SendMessage(string message, long? userID)
        {
            try
            {
                Random rnd = new Random();
                _GetAuth.api.Messages.Send(new MessagesSendParams
                {
                    RandomId = rnd.Next(),
                    UserId = userID,
                    Message = message
                });
            }
            catch (Exception ex) { }

        }

    }
}
