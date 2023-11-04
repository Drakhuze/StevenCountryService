using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StevenCountryService.Models
{
    public class Country
    {
        [Required]
        [JsonProperty("countryId")]
        public string CountryId { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("callingCode")]
        public string CallingCode { get; set; }
    }
}
