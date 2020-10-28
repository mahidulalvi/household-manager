using System;
using System.Collections.Generic;

namespace HouseholdManager.Models.Households
{
    public class HouseholdViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public HouseholdMember HouseholdOwner { get; set; }

        public List<HouseholdMember> HouseholdMembers { get; set; }

        public HouseholdViewModel()
        {
            HouseholdMembers = new List<HouseholdMember>();
        }
    }
}