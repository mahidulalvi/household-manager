using HouseholdManagementAPIConsumer.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdManagementAPIConsumer.Models.ErrorModels
{
    public class CreateCategoryErrorModel
    {
        public string Message { get; set; }
        public CreateCategoryViewModel ModelState {get;set;}
    }
}