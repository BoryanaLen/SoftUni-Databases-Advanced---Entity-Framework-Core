using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Export
{
    public class UserDto
    {
        [JsonProperty(propertyName: "firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }


        [JsonProperty(propertyName: "lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "age", NullValueHandling = NullValueHandling.Ignore)]
        public int? Age { get; set; }

        [JsonProperty(propertyName: "soldProducts")]
        public ProductSoldDto SoldProducts { get; set; }
    }
}
