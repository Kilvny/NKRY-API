using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NKRY_API.Domain.Entities
{
    public class Car
    {
        public Guid Id { get; set; }
        public string? Company { get; set; }
        public string? Model { get; set; }
        public int ManfactureYear { get; set; }
        public string? PlateNumber { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}