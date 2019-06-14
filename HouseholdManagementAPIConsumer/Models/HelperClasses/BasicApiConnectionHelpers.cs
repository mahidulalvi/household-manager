using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementAPIConsumer.Models.HelperClasses
{
    public class BasicApiConnectionHelpers
    {
        public string Domain { get; set; }
        public List<HttpUrl> AllUrls { get; set; }
        
        public BasicApiConnectionHelpers()
        {
            Domain = "http://localhost:51074";
            AllUrls = new List<HttpUrl>
            {
                new HttpUrl
                {
                    Name = "RegisterApiConsumer",
                    Method = "Post",
                    Url = $"{Domain}/api/Account/Register"
                },

                new HttpUrl
                {
                    Name = "LoginApiConsumer",
                    Method = "Post",
                    Url = $"{Domain}/Token"
                },

                new HttpUrl
                {
                    Name = "ForgotPassword",
                    Method = "Post",
                    Url = $"{Domain}/api/Account/ForgotPassword"
                },

                new HttpUrl
                {
                    Name = "ConfirmEmail",
                    Method = "Post",
                    Url = $"{Domain}/api/Account/ConfirmEmail"
                },

                new HttpUrl
                {
                    Name = "ForgotPasswordReset",
                    Method = "Post",
                    Url = $"{Domain}/api/Account/ResetPassword"
                },

                new HttpUrl
                {
                    Name = "ViewHouseholds",
                    Method = "Get",
                    Url = $"{Domain}/api/Households/GetHouseholds"
                },

                new HttpUrl
                {
                    Name = "CreateHousehold",
                    Method = "Post",
                    Url = $"{Domain}/api/Households"
                },

                new HttpUrl
                {
                    Name = "ViewHousehold",
                    Method = "Get",
                    Url = $"{Domain}/api/Households/"
                },

                new HttpUrl
                {
                    Name = "EditHousehold",
                    Method = "Put",
                    Url = $"{Domain}/api/Households/"
                },

                new HttpUrl
                {
                    Name = "_InviteHouseholdMember",
                    Method = "Post",
                    Url = $"{Domain}/api/HouseholdMembers/"
                },

                new HttpUrl
                {
                    Name = "JoinHousehold",
                    Method = "Put",
                    Url = $"{Domain}/api/HouseholdMembers/JoinHousehold/"
                },

                new HttpUrl
                {
                    Name = "LeaveHousehold",
                    Method = "Put",
                    Url = $"{Domain}/api/HouseholdMembers/LeaveHousehold/"
                },

                new HttpUrl
                {
                    Name = "_ViewHouseholdMembers",
                    Method = "Get",
                    Url = $"{Domain}/api/HouseholdMembers/"
                },

                new HttpUrl
                {
                    Name = "ChangePassword",
                    Method = "Post",
                    Url = $"{Domain}/api/Account/ChangePassword"
                },

                new HttpUrl
                {
                    Name = "EditHousehold",
                    Method = "Get",
                    Url = $"{Domain}/api/Households/EditHousehold/"
                },


                new HttpUrl
                {
                    Name = "DeleteHousehold",
                    Method = "Delete",
                    Url = $"{Domain}/api/Households/"
                },

                new HttpUrl
                {
                    Name = "ViewCategories",
                    Method = "Get",
                    Url = $"{Domain}/api/Categories/"
                },

                new HttpUrl
                {
                    Name = "ViewCategory",
                    Method = "Get",
                    Url = $"{Domain}/api/Categories/"
                },

                new HttpUrl
                {
                    Name = "CreateCategory",
                    Method = "Post",
                    Url = $"{Domain}/api/Categories/CreateCategory/"
                },

                new HttpUrl
                {
                    Name = "EditCategory",
                    Method = "Get",
                    Url = $"{Domain}/api/Categories/Categories/"
                },

                new HttpUrl
                {
                    Name = "EditCategory",
                    Method = "Put",
                    Url = $"{Domain}/api/Categories/EditCategory/"
                },

                new HttpUrl
                {
                    Name = "DeleteCategory",
                    Method = "Delete",
                    Url = $"{Domain}/api/Categories/DeleteCategory/"
                },

                new HttpUrl
                {
                    Name = "ViewBankAccounts",
                    Method = "Get",
                    Url = $"{Domain}/api/BankAccounts/"
                },

                new HttpUrl
                {
                    Name = "ViewBankAccount",
                    Method = "Get",
                    Url = $"{Domain}/api/BankAccounts/"
                },

                new HttpUrl
                {
                    Name = "CreateBankAccount",
                    Method = "Post",
                    Url = $"{Domain}/api/BankAccounts/"
                },

                new HttpUrl
                {
                    Name = "EditBankAccount",
                    Method = "Get",
                    Url = $"{Domain}/api/BankAccounts/Edit/"
                },

                new HttpUrl
                {
                    Name = "EditBankAccount",
                    Method = "Put",
                    Url = $"{Domain}/api/BankAccounts/"
                },

                new HttpUrl
                {
                    Name = "DeleteBankAccount",
                    Method = "Delete",
                    Url = $"{Domain}/api/BankAccounts/"
                },

                new HttpUrl
                {
                    Name = "UpdateBankAccountBalance",
                    Method = "Put",
                    Url = $"{Domain}/api/BankAccounts/"
                },

                new HttpUrl
                {
                    Name = "ViewTransactions",
                    Method = "Get",
                    Url = $"{Domain}/api/Transactions/"
                },

                new HttpUrl
                {
                    Name = "ViewTransaction",
                    Method = "Get",
                    Url = $"{Domain}/api/Transactions/"
                },

                new HttpUrl
                {
                    Name = "CreateTransaction",
                    Method = "Post",
                    Url = $"{Domain}/api/Transactions/"
                },

                new HttpUrl
                {
                    Name = "CreateTransactionFromHousehold",
                    Method = "Post",
                    Url = $"{Domain}/api/Transactions/"
                },

                new HttpUrl
                {
                    Name = "EditTransaction",
                    Method = "Get",
                    Url = $"{Domain}/api/Transactions/Edit/"
                },

                new HttpUrl
                {
                    Name = "EditTransaction",
                    Method = "Put",
                    Url = $"{Domain}/api/Transactions/"
                },

                new HttpUrl
                {
                    Name = "DeleteTransaction",
                    Method = "Delete",
                    Url = $"{Domain}/api/Transactions/"
                },

                new HttpUrl
                {
                    Name = "ToggleVoidTransaction",
                    Method = "Put",
                    Url = $"{Domain}/api/Transactions/ToggleVoidTransaction/"
                },

                new HttpUrl
                {
                    Name = "GetBankAccountsForStats",
                    Method = "Get",
                    Url = $"{Domain}/api/BankAccounts/"
                },

                new HttpUrl
                {
                    Name = "GetCategoriesForStats",
                    Method = "Get",
                    Url = $"{Domain}/api/Categories/"
                },

                new HttpUrl
                {
                    Name = "GetTransactions",
                    Method = "Get",
                    Url = $"{Domain}/api/Transactions/"
                },
            };
        }
    }
}