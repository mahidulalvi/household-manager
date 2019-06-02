using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementAPIConsumer.Models.HouseholdMemberInvites
{
    public class JoinHouseholdBindingModel
    {
        public string InviteId { get; set; }
        public string HouseholdName { get; set; }
    }
}