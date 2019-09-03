using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace ServerChatBalakovo.API
{
    class GetGroupUsersId
    {

        private API.GetAuth _GetAuth;
        public GetGroupUsersId(API.GetAuth GetAuth)
        {
            this._GetAuth = GetAuth;
        }

        public long [] ListIdUsersGroup(string groupId, int offset, VkNet.Enums.SafetyEnums.GroupsSort sort) {
            var ids = _GetAuth.api.Groups.GetMembers(new GroupsGetMembersParams() { GroupId = groupId, Count = 1000, Offset = offset, Sort = sort });
            return ids.Select(user => user.Id).ToArray();
        }
    
    }
}
