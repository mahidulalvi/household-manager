using System.ComponentModel.DataAnnotations;

namespace HouseholdManager.Models.Categories
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}