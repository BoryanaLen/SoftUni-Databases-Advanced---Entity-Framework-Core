using AutoMapper;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierImportDto, Supplier>();

            this.CreateMap<PartImportDto, Part>();

            this.CreateMap<CarImportDto, Car>();

            this.CreateMap<CustomerImportDto, Customer>();
        }
    }
}
