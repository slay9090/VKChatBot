
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace ServerChatBalakovo.API
{
    public class ReaderAllEvents
    {
        private API.GetAuth _GetAuth;
        private FORM.ControlForm _controlForm; //возможно дичь
       // private FORM.MainForm _mainForm;
       


        public string SrvLongPoll;
        private string _ts;
        

        public ReaderAllEvents(API.GetAuth GetAuth, FORM.ControlForm controlForm) {
            this._GetAuth = GetAuth;
            this._controlForm = controlForm;
        }
      

        public void readMsgFromGroup()
        {
          
                var s = _GetAuth.api.Groups.GetLongPollServer(75514519); //обновлять в экзепшн
                SrvLongPoll = s.Server;
                _ts = s.Ts;

                while (true) // Бесконечный цикл, получение обновлений
                {
             
                try
                {
                    s = _GetAuth.api.Groups.GetLongPollServer(75514519); //обновлять в экзепшн
                    SrvLongPoll = s.Server;
                    var poll = _GetAuth.api.Groups.GetBotsLongPollHistory(
                        new BotsLongPollHistoryParams { Server = SrvLongPoll, Ts = _ts, Key = s.Key, Wait = 10 });

                    _ts = poll.Ts; // <-- нужно обновлять Ts
                    if (FORM.MainForm.stopRun == true) //стоп если есть флаг
                    {
                        Debug.WriteLine("Stop Reader");
                        _controlForm.SetLabelStateProgramm("Сервер выключен");
                        break;

                    }

                    if (poll.Updates != null)
                    {
                        foreach (var a in poll.Updates)
                        {

                            if (a.Type == GroupUpdateType.MessageNew)
                            {

                                CORE.Receiver.userMessage += "%start%&begin[" + a.Message.PeerId + "]" + a.Message.Text + "&end%final%" + Environment.NewLine;
                                CORE.Receiver.userID = a.Message.PeerId;

                                //  Debug.WriteLine($"[{CORE.Receiver.userID}] {CORE.Receiver.userMessage}");


                            }
                            else //тут если нет новый сообщений
                            {

                            }


                        }
                    }
                }
                catch (Exception ex) {

                    //s = _GetAuth.api.Groups.GetLongPollServer(75514519); //обновлять в экзепшн
                    //SrvLongPoll = s.Server;
                    //_ts = s.Ts;
                }

                }
            }
           
        }


    }

