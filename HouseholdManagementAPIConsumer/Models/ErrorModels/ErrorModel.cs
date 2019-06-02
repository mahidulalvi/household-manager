using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementAPIConsumer.Models.ErrorModels
{
    public class ErrorModel
    {
        public string Message { get; set; }
        public Dictionary<string, string[]> ModelState { get; set; }
    }
}