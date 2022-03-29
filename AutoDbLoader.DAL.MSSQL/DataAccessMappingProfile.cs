using AutoMapper;
using AutoDbLoader.DAL.MSSQL.Entity;
using AutoDbLoader.DAL.txt.Entity;

namespace AutoDbLoader.DAL.MSSQL
{
    public class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<Payment, TerritoryPayments>()
                .ReverseMap();
        }
    }
}
