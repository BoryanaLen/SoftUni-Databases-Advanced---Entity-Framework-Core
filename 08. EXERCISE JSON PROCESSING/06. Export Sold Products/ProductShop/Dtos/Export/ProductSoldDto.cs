using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Export
{
    public class ProductSoldDto
    {
        [JsonProperty(propertyName: "name")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "price")]
        public decimal Price { get; set; }

        [JsonProperty(propertyName: "buyerFirstName")]
        public string BuyerFirstName { get; set; }

        [JsonProperty(propertyName: "buyerLastName")]
        public string BuyerLastName { get; set; }
    }
}
