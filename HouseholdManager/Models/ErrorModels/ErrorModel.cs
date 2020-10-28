using Newtonsoft.Json;

namespace HouseholdManager.Models.ErrorModels
{
    public class ErrorModel
    {
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}