using System.Collections.Generic;

namespace HouseholdManager.Models.ErrorModels
{
    public class ApiError
    {
        public string Message { get; set; }
        public Dictionary<string, string[]> ModelState { get; set; }
    }
}