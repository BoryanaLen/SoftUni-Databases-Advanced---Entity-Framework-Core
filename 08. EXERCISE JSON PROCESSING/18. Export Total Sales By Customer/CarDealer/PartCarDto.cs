using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class PartCarDto
    {
        [JsonProperty(propertyName: "car")]
        public CarDto Car { get; set; }

        [JsonProperty(propertyName: "parts")]
        public ICollection<PartDto> Parts { get; set; }
    }
}
