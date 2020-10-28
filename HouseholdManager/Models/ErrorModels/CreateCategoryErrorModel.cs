using HouseholdManager.Models.Categories;

namespace HouseholdManager.Models.ErrorModels
{
    public class CreateCategoryErrorModel
    {
        public string Message { get; set; }
        public CreateCategoryViewModel ModelState {get;set;}
    }
}