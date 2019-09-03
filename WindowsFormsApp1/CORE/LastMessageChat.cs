using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    class LastMessageChat
    {
       // private CORE.MessageSender _messageSender;
        public LastMessageChat()
        {
          //  _messageSender = new MessageSender();
        }
       // public void SendMsgOneUser(ISendingMsg sendingMsg, string id, string msg) { sendingMsg.SendOne(id, msg); }

       // public void SendLastMessage() { }

        public string FileReader()
        {
            // var list = new List<string>();
            string lastmsg=null;
            string input = null;
            //Читаем из файла информацию
            if (File.Exists(Directory.GetCurrentDirectory() + OTHER.Configuration.nameFileChatLastMsg))
            {
                FileRemoveExcess(5); //удалить лишние строки из файла перед считыванием
                StreamReader ReadFile = File.OpenText(Directory.GetCurrentDirectory() + OTHER.Configuration.nameFileChatLastMsg);
                while ((input = ReadFile.ReadLine()) != null)
                {
                    lastmsg += input+Environment.NewLine;
                }
                return lastmsg;
            }
            else
            {
                return null;
            }

        }
        public void FileWriter(string message)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + OTHER.Configuration.nameFileChatLastMsg))
            {
                try
                {
                    File.AppendAllText(Directory.GetCurrentDirectory() + OTHER.Configuration.nameFileChatLastMsg, message);
                }
                catch (Exception ex) { }
            }
            
        }

        public void FileRemoveExcess(int maxrows) {
            int num = 0; 
            try
            {
                int length = File.ReadAllLines(Directory.GetCurrentDirectory() + OTHER.Configuration.nameFileChatLastMsg).Where(x => x != "").Count(); //get int all row
              
                if (length > maxrows)
                {
                    //(5) 15 -5 = 10 / 30 - 5 = 25 / 3 - 5 = -2
                    string[] strorki = File.ReadAllLines(Directory.GetCurrentDirectory() + OTHER.Configuration.nameFileChatLastMsg, Encoding.UTF8);
                    string[] wr = new string[strorki.Length - (length-maxrows)];
                    Array.Copy(strorki, (length - maxrows), wr, 0, wr.Length);
                    File.WriteAllLines(Directory.GetCurrentDirectory() + OTHER.Configuration.nameFileChatLastMsg, wr);
                }


                


            }
            catch (Exception ex) {
                Debug.WriteLine("Exeption FileRemoveExcess");
            }

        }
    }
}
