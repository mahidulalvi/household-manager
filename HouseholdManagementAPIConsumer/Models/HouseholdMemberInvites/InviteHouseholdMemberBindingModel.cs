using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseholdManagementAPIConsumer.Models.HouseholdMemberInvites
{
    public class InviteHouseholdMemberBindingModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}