using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    /// <summary>
    /// Обработка команд чата 
    /// </summary>
    class ServiceMsg
    {
      //  private API.GetAuth _getAuth;
       // private API.SendMsgFromGroup _sendMsgFromGroup;
       // private DB.TableData _tableData;
        private CORE.User _user;
        private CORE.MessageSender _messageSender;
        private LastMessageChat _lastMessage;
        
        public ServiceMsg() {
         //   _getAuth = new API.GetAuth();
         //   _sendMsgFromGroup = new API.SendMsgFromGroup(_getAuth);
          //  _tableData = new DB.TableData();
            _user = new User();
            _messageSender = new MessageSender();
            _lastMessage = new LastMessageChat();
        }

        public bool CheckIsNickExist(IGetInfo getUserData, string nickname)
        {
            return getUserData.CheckIsNickExist(nickname);
        }
        public string SetLimitLenghtNickname(ISetInfo setUserData, string inputString, int maxChars, string postfix = "...")
        {
            return setUserData.SetLenghtLimitNick(inputString, maxChars, postfix);
        }
        public void AddReg(ISetInfo setUserData, string id, string nickname)
        {
            setUserData.AddDB(id,nickname); //ВОТ ОНО!!! ВОТ ЧЕМ ЗБС ИНТЕРФЕЙС! ВСЕГО 2 СВОЙСТВА когда их 6 нах
        }
        public bool CheckIsRegistrationOnExist(IGetInfo getUserData, string id) {
            return getUserData.CheckIsRegistration(id);
        }
        public void SendMsgOneUser(ISendingMsg sendingMsg, string id, string msg)  { sendingMsg.SendOne(id, msg); }
        public void SendMsgOnlineUser(ISendingMsg sendingMsg, string msg) {  sendingMsg.SendOnline(msg);  }
        public void AddOnline(ISetInfo setInfo ,string id) {
            setInfo.AddOnline(id);
        }
        public void ChangeNick(ISetInfo setInfo, string id, string nick) {
            setInfo.ChangeNickName(id, nick);
        }
        public void LeaveOnlineUser(ISetInfo setInfo,string id) {
            setInfo.RemoveLeavingOnlineUser(id);
        }
              
        /// <summary>
        /// Ловим какая команда, отправляем по соотвествующим методам
        /// </summary>
        /// <param name="servicemsg"></param>
        /// <param name="id"></param>
        public void SendTypeCmd(string servicemsg, string id) {                      
            string pattern = @"^\S+";
            Regex regex = new Regex(pattern, RegexOptions.Singleline);
            Match match = regex.Match(servicemsg);
            Debug.WriteLine("GETCMD "+match);
            switch (match.ToString())
            {
                case "!рег": RegCmd(servicemsg, id); ; break;
                case "!помощь": HelpCmd(servicemsg, id); ; break;
                case "!ник": ChangeNickCmd(servicemsg, id); ; break;
                case "!бан": BanUserCmd(servicemsg, id); ; break;
                case "!начать": StartChatingCmd(id); ; break;
                case "!выход": ExitChatingCmd(id); ; break;
                case "!ктотут": WhoHereCmd(id); ; break;
                default: SendMsgOneUser(_messageSender,id, OTHER.Configuration.textCmdUnknown); ; break;
            }
        }

        public void RegCmd(string servicemsg, string id) {
            try {
                // str = "слово1^слово2...";
                if (CheckIsRegistrationOnExist(_user, id) == false)
                {
                    string nickname = servicemsg.Substring(servicemsg.IndexOf(' ') + 1, servicemsg.Length - 5);
                    nickname = SetLimitLenghtNickname(_user, nickname, 15, ""); //обрезать до кол-выа сиволов
                    Debug.WriteLine("RegCmd");
                    if (CheckIsNickExist(_user, nickname) == false) //если ник не повторяется, регаем
                    {
                        Debug.WriteLine("nickname NOT aleardy " + nickname);
                        AddReg(_user, id, nickname);
                        SendMsgOneUser(_messageSender,id, OTHER.Configuration.textRegistrationSuccesfull);

                    }
                    else
                    {
                        SendMsgOneUser(_messageSender,id, OTHER.Configuration.nicknameAleardy);
                    }
                }
                else {
                    SendMsgOneUser(_messageSender,id, OTHER.Configuration.textUserIsExistRegistration);
                }
            }
            catch (Exception ex) { }
        }

        public void StartChatingCmd(string id) {
            if (_user.CheckIdAleardyInOnline(id) == null)
            {
                AddOnline(_user, id);
                SendMsgOneUser(_messageSender,id,"Последние 5 сообщений чата:\n"+_lastMessage.FileReader());
                SendMsgOnlineUser(_messageSender, " * [" + _user.GetNickName(id) + "] вошёл в чат &#128515;"); //рассылаем юзерам в чате              
              
            }
            else
            {
                SendMsgOneUser(_messageSender, id, " * Вы уже в чате &#128527;");
            }
        }
        public void ExitChatingCmd(string id) {
            LeaveOnlineUser(_user,id);
            SendMsgOneUser(_messageSender,id, OTHER.Configuration.textExitUser);
            SendMsgOnlineUser(_messageSender, " * [" + _user.GetNickName(id) + "] Вышел из чата");
        }
        public void HelpCmd(string servicemsg, string id) {
            SendMsgOneUser(_messageSender, id, OTHER.Configuration.textHelpCmd);
        }

        public void ChangeNickCmd(string servicemsg, string id) {

            string[] words = servicemsg.Split(' ');
            if (words[1] != "") {
                SendMsgOnlineUser(_messageSender, " * [" + _user.GetNickName(id) + "] "+ OTHER.Configuration.textChangeNicknameIsComplited+" [" + words[1] + "]" ); //рассылаем юзерам в чате              
                ChangeNick(_user, id, words[1]);
            }
           
        }
        public void BanUserCmd (string servicemsg, string id) { }

        public void WhoHereCmd(string id) {
            string usersOnlineList="";
            int number = 1;
            foreach (string userOnlineId in _user.GetIdOnlineList()) {
                usersOnlineList += number++.ToString()+" - "+"[" + _user.GetNickName(userOnlineId) + "]\n";
            }
            SendMsgOneUser(_messageSender, id, usersOnlineList);
        }
             


    }
}
