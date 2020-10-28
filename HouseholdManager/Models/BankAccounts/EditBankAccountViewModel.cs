using System.ComponentModel.DataAnnotations;

namespace HouseholdManager.Models.BankAccounts
{
    public class EditBankAccountViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}