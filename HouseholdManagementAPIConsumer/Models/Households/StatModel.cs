using HouseholdManagementAPIConsumer.Models.BankAccounts;
using HouseholdManagementAPIConsumer.Models.Categories;
using HouseholdManagementAPIConsumer.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementAPIConsumer.Models.Households
{
    public class StatModel
    {
        public List<BankAccountViewModel> BankAccounts { get; set; }
        public List<CategoryViewModel> Categories { get; set; }        
        public List<TransactionViewModel> Transactions { get; set; }
    }
}