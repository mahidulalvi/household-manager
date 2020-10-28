using HouseholdManager.Models.BankAccounts;
using HouseholdManager.Models.Categories;
using HouseholdManager.Models.Transactions;
using System.Collections.Generic;

namespace HouseholdManager.Models.Households
{
    public class StatModel
    {
        public List<BankAccountViewModel> BankAccounts { get; set; }
        public List<CategoryViewModel> Categories { get; set; }        
        public List<TransactionViewModel> Transactions { get; set; }
    }
}