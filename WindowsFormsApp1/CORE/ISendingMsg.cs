using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatBalakovo.CORE
{
    public interface ISendingMsg
    {
        /// <summary>
        /// отправка одному пользователю
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        void SendOne(string id, string msg);
        /// <summary>
        /// отправка всем кто онлайн в чате
        /// </summary>
        /// <param name="msg"></param>
        void SendOnline(string msg);
        /// <summary>
        /// отправка всем кто состоит в группе
        /// </summary>
        /// <param name="idgroup"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="msg"></param>
        void SendAll(string idgroup, int offset, VkNet.Enums.SafetyEnums.GroupsSort sort, string msg);
    }
}
