using AutoDbLoader.DAL.txt.Entity;
using System.Collections.Generic;

namespace AutoDbLoader.DAL.txt.Interface
{
    public interface IPaymentRepository
    {
        List<AliasData> GetAliasDataFromFile();
        List<Payment> GetPaymentsDataFromFile();
    }
}
