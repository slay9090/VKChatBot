using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    /// <summary>
    /// Приёмник и сортировщик входящих сообщений группы. Патерн конвеер
    /// </summary>
    class Receiver
    {
        private API.ReaderAllEvents _readerAllEvents;
        private API.GetAuth _getAuth;    
        private FORM.ControlForm _controlForm;
        private FORM.MainForm _mainForm;
        private CORE.OutChatMsg _outChatMsg;
        private CORE.ServiceMsg _serviceMsg;
        private CORE.CommonMsg _commonMsg;
        private CORE.User _user;
        

        private string _userMessage = ""; //зона буфера, что бы не проёбывались сообщ
        private long? _userID = 0; //зона буфера, что бы не проёбывались сообщ

        public static string userMessage = "";
        public static long? userID = 0;
      
        

        public Receiver(FORM.MainForm mainForm) {
       
            this._mainForm = mainForm;
            _getAuth   = new API.GetAuth();       
            _controlForm = new FORM.ControlForm(_mainForm);
            _readerAllEvents = new API.ReaderAllEvents(_getAuth, _controlForm);   

            _outChatMsg = new OutChatMsg();
             _serviceMsg = new ServiceMsg();
            _commonMsg = new CommonMsg();
            _user = new User();
           


        }

        public bool CheckIsRegistration(IGetInfo getUserData, string id) {
           return getUserData.CheckIsRegistration(id);
        }
        public void ConfirmActivityUser(ISetInfo setInfo, string id) {
            setInfo.ChangeOnlineActivity(id);
        }
        public bool CheckIsServiceCmd(IGetInfo getUserData,string msg) {
            return getUserData.CheckIsServiceCmd(msg);
        }
        public bool CheckIsBan(IGetInfo getUserData, string id)
        {
            return getUserData.CheckIsBan(id);
        }
        public bool CheckIsOnline(IGetInfo getUserData, OTHER.Configuration.ColumnNameTableOnline ColumnNameForReturn, string id, OTHER.Configuration.ColumnNameTableOnline ColumnNameForFind) {
return getUserData.CheckIsOnline(ColumnNameForReturn,id, ColumnNameForFind);
                    }

        /// <summary>
        /// Принимаем все сообщения от группы
        /// </summary>
        public void GetMsgFromFroup() {

            CORE.AutoExit _autoExit = CORE.AutoExit.GetInstance(); //синглтончик
            _controlForm.SetLabelStateProgramm("Сервер включен");
            new Thread(() => _readerAllEvents.readMsgFromGroup()).Start(); //в этом потоке нонстоп читаем данные с сервера

            new Thread(() => //в этом потоке ждём, получаем и формируем блок сообщений, отправляем сортировщику
            {
                while (true)
                {
                    Thread.Sleep(7000);

                    if (userMessage != "" && userID != 0)
                    {
                        _userMessage = userMessage; _userID = userID;
                        _controlForm.AddMsgRichTextBox(_userID.ToString(), _userMessage);
                        userMessage = "";
                        userID = 0;
                        SortMsgFromGroup(_userMessage);
                      //  _sendMsgFromGroup.SendMessage(_userMessage, _userID); // !!! тут сортировщик вставить
                        _userMessage = ""; _userID = 0;

                    }
                    if (FORM.MainForm.stopRun == true)
                    {
                        Debug.WriteLine("Stop Sender");
                        break;
                    }
                }
            }).Start();
        }

        /// <summary>
        /// Сортируем сообщения, отправляем разным классам на дальнейшую обработку
        /// </summary>
        public void SortMsgFromGroup(string msg) {
            try
            {
                Console.WriteLine("DO "+ msg);


                string[] sentences = Regex.Split(msg, @"%start%(.*)%final%" + Environment.NewLine + ""); //разбиваем блок в массив строк
                string [] tt = sentences.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                // string[] sentences= Regex.Match(msg, Regex.Escape("&begin[") + "(.*?)" + Regex.Escape("]")).Groups[1].Value.ToString();
                foreach (string s in tt)
                {
                   
                    if (s != "")
                    {

                        string id = Regex.Match(s, Regex.Escape("&begin[") + "(.*?)" + Regex.Escape("]")).Groups[1].Value.ToString();
                        string textmsg = Regex.Match(s, Regex.Escape("]") + "(.*?)" + Regex.Escape("&end"), RegexOptions.Singleline).Groups[1].Value.ToString();


                    Debug.WriteLine("POSLE PARSE (" + id + ") " + textmsg);

                        //проверяем есть ли айди в БД
                        if (CheckIsRegistration(_user, (id)) == true) //юзер зареган
                        {
                            Debug.WriteLine("user is REG");

                            //проверяем есть ли бан  -> проверяем есть ли айди  в таблице онлайн ->  проверяем служебное ли сообщение -> значит это обычное сообщение в чата
                            if (CheckIsBan(_user, id) != true) //есть ли бан
                            {
                                Debug.WriteLine("Не Забанен ");
                                if (CheckIsOnline(_user, OTHER.Configuration.ColumnNameTableOnline.Idvk, id, OTHER.Configuration.ColumnNameTableOnline.Idvk)) //онлайн?
                                { //online
                                    Debug.WriteLine("в чате");
                                    //проверям команда или нет
                                    
                                    if (CheckIsServiceCmd(_user, textmsg) == true)
                                    {
                                        //если команда, оправляем сервис классу
                                        _serviceMsg.SendTypeCmd(textmsg, id);
                                    }
                                    else
                                    {
                                        // обычное сообщение отправляем в хаб                                       
                                        _commonMsg.msgSegmentStorage(id, textmsg); 

                                    }
                                    ConfirmActivityUser(_user,id);
                                }
                                else //offline
                                {
                                    if (textmsg == "!начать")
                                    {
                                        _serviceMsg.SendTypeCmd(textmsg, id);
                                    }
                                    else { _outChatMsg.sendMsg(id, OTHER.Configuration.textUserIsOffline); }
                                }
                            }
                            else
                            {
                                Debug.WriteLine("Забанен ");
                                // 
                                _outChatMsg.sendMsg(id, OTHER.Configuration.textUserIsBanned + " " + _user.GetBanDateTimeValue(id).ToString());
                            }

                        }
                        else //юзер не зареган
                        {
                            Debug.WriteLine("user not REG");

                            if (CheckIsServiceCmd(_user, textmsg) == true)
                            {
                                //если команда, оправляем сервис классу
                                _serviceMsg.SendTypeCmd(textmsg, id);

                            }
                            else
                            {
                                _outChatMsg.sendMsg(id, OTHER.Configuration.textIdUnknown); //просим шоб регнулся
                            }

                        }
                    }
                }

            }
            catch (Exception ex){
                //тут логер ебануть

            }
            
        }

     

    

    }
}
