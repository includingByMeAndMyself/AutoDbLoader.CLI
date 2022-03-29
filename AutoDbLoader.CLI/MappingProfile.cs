using AutoMapper;
using AutoDbLoader.DAL.MSSQL.Entity;
using AutoDbLoader.DAL.txt.Entity;

namespace AutoDbLoader.CLI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Payment, TerritoryPayments>();
        }
    }
}
