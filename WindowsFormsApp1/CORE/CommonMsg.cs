using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{


    //всё плохо!
    /// <summary>
    /// Обработка обычных сообщений чата
    /// </summary>
    class CommonMsg
    {
        private CORE.User _user;
        // private API.SendMsgFromGroup _sendMsgFromGroup;
        // private API.GetAuth _getAuth;
        private CORE.MessageSender _messageSender;
        private CORE.LastMessageChat _lastMessage;
        string msgSegment = "";
        string bufferSegment = "";

        public string NickName(IGetInfo igetInfo, string id) {
            return igetInfo.GetNickName(id);
        }
        public void SendMsgOneUser(ISendingMsg sendingMsg, string id, string msg) { sendingMsg.SendOne(id, msg); }
        public void SendMsgOnlineUser(ISendingMsg sendingMsg, string msg) { sendingMsg.SendOnline(msg); }

        public CommonMsg() {
            _user = new User();
            _messageSender = new MessageSender();
            _lastMessage = new LastMessageChat();
          
            SendingMessages();
        }


        //сделать отправлние блоком а не по 1му
        public void SendingMessages()
        {
            new Thread(() => //
            {           
                    while (true)
                    {
                        if (bufferSegment != "")
                        {
                            Console.WriteLine("bufferSegment " + bufferSegment);
                            msgSegment += bufferSegment;
                        bufferSegment = "";

                    }               
                }
            }).Start();



            new Thread(() => //
            {
                string buffquene = "";
                    while (true)
                    {
                        Thread.Sleep(5000);
 
                    if (msgSegment != "")
                    {
                        buffquene = msgSegment;
                        msgSegment = "";                     
                     
                            SendMsgOnlineUser(_messageSender, buffquene ); //рассылаем юзерам в 
                        _lastMessage.FileWriter(buffquene); //записываем сообщения в файл

                        buffquene = "";                    
                    }
                }
            }).Start();

        }

     
        public void msgSegmentStorage(string id,string messages) {

            bufferSegment="["+NickName(_user,id)+"]: "+messages+Environment.NewLine;

        }

    }
}
