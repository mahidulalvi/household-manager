﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementAPIConsumer.Models.Transactions
{
    public class TransactionViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsTransactionVoid { get; set; }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string BankAccountId { get; set; }
        public string BankAccountName { get; set; }

        public string TransactionOwnerUserName { get; set; }
    }
}