using System.ComponentModel.DataAnnotations;

namespace HouseholdManager.Models.HouseholdMemberInvites
{
    public class InviteHouseholdMemberBindingModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}