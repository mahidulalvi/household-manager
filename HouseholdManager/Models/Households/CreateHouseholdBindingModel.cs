using System.ComponentModel.DataAnnotations;

namespace HouseholdManager.Models.Households
{
    public class CreateHouseholdBindingModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}