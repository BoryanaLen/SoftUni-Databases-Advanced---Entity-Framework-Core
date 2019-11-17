using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Export
{
    public class CategoryDto
    {
        [JsonProperty(propertyName: "category")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty(propertyName: "averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty(propertyName: "totalRevenue")]
        public string TotalRevenue { get; set; }
    }
}
