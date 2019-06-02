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
                }
            };
        }
    }
}