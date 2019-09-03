
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    // сделать защиту от дудоса

    class OutChatMsg
    {
        private CORE.MessageSender _messageSender;

        public void SendMsgOneUser(ISendingMsg sendingMsg, string id, string msg) { sendingMsg.SendOne(id, msg); }
        public void SendMsgOnlineUser(ISendingMsg sendingMsg, string msg) { sendingMsg.SendOnline(msg); }
        
        public OutChatMsg () {
            _messageSender = new MessageSender();          
        }

        public void sendMsg(string id, string msg) {
            SendMsgOneUser(_messageSender,id,msg);              
        
        }
        

    }
}
