using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Export
{
    public class ProductSoldDto
    {
        [JsonProperty(propertyName: "count")]
        public int Count { get; set; }

        [JsonProperty(propertyName: "products")]
        public ICollection<ProductDto> Products { get; set; }
    }
}
