using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    class AutoExit
    {
        private CORE.User _user;
        private CORE.MessageSender _messageSender;

 

        public string[] ListIdNoActiveUsers(IGetInfo getInfo) {
            return getInfo.GetIdOnlineForExit();
        }
        public void RemoveUser(ISetInfo setInfo, string id) {
            setInfo.RemoveNoActiveUser(id);
        }
        public void SendMsgOneUser(ISendingMsg sendingMsg, string id, string msg) { sendingMsg.SendOne(id, msg); }
        public void SendMsgOnlineUser(ISendingMsg sendingMsg, string msg) { sendingMsg.SendOnline(msg); }
        public static System.Timers.Timer timer = new System.Timers.Timer(OTHER.Configuration.timeWaitCheckIsNoActiveUser * 60000);
        private AutoExit(){
            _user = new User();
            _messageSender = new MessageSender();

            
            
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;

            timer.Start();
        


        }


        // Конструктор Одиночки всегда должен быть скрытым, чтобы предотвратить
        // создание объекта через оператор new.


        // Объект одиночки храниться в статичном поле класса. Существует
        // несколько способов инициализировать это поле, и все они имеют разные
        // достоинства и недостатки. В этом примере мы рассмотрим простейший из
        // них, недостатком которого является полная неспособность правильно
        // работать в многопоточной среде.
        private static AutoExit _instance;

        // Это статический метод, управляющий доступом к экземпляру одиночки.
        // При первом запуске, он создаёт экземпляр одиночки и помещает его в
        // статическое поле. При последующих запусках, он возвращает клиенту
        // объект, хранящийся в статическом поле.
        public static AutoExit GetInstance()
        {
            if (_instance == null)
            {

                _instance = new AutoExit();
            }
            timer.Start();
            return _instance;
            
        }


        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            (sender as System.Timers.Timer).Stop();
            try
            {
               // if (TimerRun == false) { timer.Stop(); }
                //processing
                
                foreach (string id in ListIdNoActiveUsers(_user))
                {
                    Console.WriteLine("id " + id);
                    if (id != "" && id != null)
                    {
                        SendMsgOneUser(_messageSender, id, OTHER.Configuration.textUserIsNoActivity);
                        RemoveUser(_user, id);
                    }

                }

            }
            finally
            {
                (sender as System.Timers.Timer).Start();
            }
        }


        public static void TimerStop() {
            timer.Stop();

        }


    }
}
