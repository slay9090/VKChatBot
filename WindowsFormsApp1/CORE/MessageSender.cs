using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    class MessageSender : ISendingMsg
    {
        private API.GetAuth _getAuth;
        private API.SendMsgFromGroup _sendMsgFromGroup;
        private CORE.User _user;
        private API.GetGroupUsersId _getGroupUsersId;
       
        public int amount_all=0;
        public int amount_online = 0; //не реализ.
        public int amount_one = 0; //не реализ.

        public MessageSender() {
            _getAuth = new API.GetAuth();
            _sendMsgFromGroup = new API.SendMsgFromGroup(_getAuth);
            _user = new User();
            _getGroupUsersId=new API.GetGroupUsersId(_getAuth);
           
        }
        public MessageSender(FORM.ControlForm controlForm) {

        }

     
        
      
        public void SendOnline(string msg)
        {
            foreach (string idonline in IdList(_user))
            {
                _sendMsgFromGroup.SendMessage(msg, long.Parse(idonline));              
            }
        }

        public void SendOne(string id, string msg)
        {
            _sendMsgFromGroup.SendMessage(msg, long.Parse(id));
        }

        public string[] IdList(IGetInfo getUserData)
        {
            return getUserData.GetIdOnlineList();
        }

        public void SendAll(string idgroup, int offset, VkNet.Enums.SafetyEnums.GroupsSort sort, string msg)
        {
            new Thread(() => //
            {
                foreach (long id in _getGroupUsersId.ListIdUsersGroup(idgroup, offset, sort))
                {
                    amount_all++;
                    try
                    {
                        Console.WriteLine(id);
                        _sendMsgFromGroup.SendMessage(msg, id);

                        Thread.Sleep(3000);
                        Console.WriteLine(id);
                    }
                    catch (Exception ex) { }

                }

            }).Start();

    
        }
        /// <summary>
        /// получить счётчики
        /// </summary>
        /// <returns></returns>
        /// 
        public int Counters(OTHER.Configuration.Counters counterName) {
            int amount;
            switch (counterName) {
                case OTHER.Configuration.Counters.allSend : amount=amount_all; ; break;
                case OTHER.Configuration.Counters.onlineSend: amount = amount_online; ; break;
                case OTHER.Configuration.Counters.oneSend: amount = amount_one; ; break;
                default: amount = 0; ; break;
            }
            return amount;
        }
     
    }
}
