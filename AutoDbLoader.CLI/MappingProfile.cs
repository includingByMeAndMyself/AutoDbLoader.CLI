using AutoDbLoader.DAL.MSSQL.Entity;
using AutoDbLoader.DAL.txt.Entity;
using AutoMapper;

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
