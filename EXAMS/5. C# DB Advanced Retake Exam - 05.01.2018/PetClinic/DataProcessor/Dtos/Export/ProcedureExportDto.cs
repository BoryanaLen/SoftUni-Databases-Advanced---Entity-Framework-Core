using System;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dtos.Export
{
    [XmlType("Procedure")]
    public class ProcedureExportDto
    {
        public string Passport { get; set; }

        public string OwnerNumber { get; set; }

        public string DateTime { get; set; }

        public AnimalAidExportDto[] AnimalAids { get; set; }

        public decimal TotalPrice { get; set; }

    }
}
