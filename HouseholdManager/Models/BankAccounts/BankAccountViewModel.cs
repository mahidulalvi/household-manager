using System;

namespace HouseholdManager.Models.BankAccounts
{
    public class BankAccountViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public decimal Balance { get; set; }

        public string HouseholdId { get; set; }
        public string HouseholdName { get; set; }
    }
}