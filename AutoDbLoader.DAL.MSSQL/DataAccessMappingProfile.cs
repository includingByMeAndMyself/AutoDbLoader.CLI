using AutoDbLoader.DAL.MSSQL.Entity;
using AutoDbLoader.DAL.txt.Entity;
using AutoMapper;

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
