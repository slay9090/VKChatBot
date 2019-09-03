using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    interface IGetInfo
    {
        bool CheckIsRegistration(string id);
        bool CheckIsBan(string id);
        bool CheckIsOnline (OTHER.Configuration.ColumnNameTableOnline ColumnNameForReturn, string id, OTHER.Configuration.ColumnNameTableOnline ColumnNameForFind);
        bool CheckIsServiceCmd(string msg);
        bool CheckIsNickExist(string nickname);
        string[] GetIdOnlineList();
        string GetNickName(string id);
        /// <summary>
        /// получаем массив ИД бездействующих юзеров
        /// </summary>
        /// <returns></returns>
        string[] GetIdOnlineForExit();
        
    }
}
