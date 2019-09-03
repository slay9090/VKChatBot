using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    interface ISetInfo
    {
        /// <summary>
        /// обрезать ник до кол-ва символов
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="maxChars">максимальное кол-во</param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        string SetLenghtLimitNick(string inputString, int maxChars, string postfix = "...");
        /// <summary>
        /// Добавить новую запись в БД
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nickname"></param>
        void AddDB(string id, string nickname);
        /// <summary>
        /// удаляем бездействующего пользователя
        /// </summary>
        /// <param name="id"></param>
        void RemoveNoActiveUser(string id);

        void AddOnline(string id);

        void ChangeOnlineActivity(string id);

        void ChangeNickName(string id, string nickname);

        void RemoveLeavingOnlineUser(string id);
    }
}
