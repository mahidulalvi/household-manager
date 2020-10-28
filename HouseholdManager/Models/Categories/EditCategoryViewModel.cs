using System.ComponentModel.DataAnnotations;

namespace HouseholdManager.Models.Categories
{
    public class EditCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}