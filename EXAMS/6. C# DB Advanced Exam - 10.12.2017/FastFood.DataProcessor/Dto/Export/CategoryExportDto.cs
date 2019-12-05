using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Export
{
    [XmlType("Category")]
    public class CategoryExportDto
    {
        public string Name { get; set; }

        public ItemExportDto MostPopularItem { get; set; }
    }
}
