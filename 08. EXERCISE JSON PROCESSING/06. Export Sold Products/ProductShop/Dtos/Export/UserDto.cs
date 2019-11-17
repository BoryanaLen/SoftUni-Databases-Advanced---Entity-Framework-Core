using Newtonsoft.Json;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Export
{
    public class UserDto
    {
        [JsonProperty(propertyName: "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "lastName")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "soldProducts")]
        public ICollection<ProductSoldDto> SoldProducts { get; set; }
    }
}
