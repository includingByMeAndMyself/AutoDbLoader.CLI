using AutoDbLoader.DAL.txt.Entity;
using System.Collections.Generic;

namespace AutoDbLoader.DAL.MSSQL.Interface
{
    public interface ITerritoryPaymentsRepository
    {
        void Add(List<Payment> payments);
        int GetCountLS();
    }
}
