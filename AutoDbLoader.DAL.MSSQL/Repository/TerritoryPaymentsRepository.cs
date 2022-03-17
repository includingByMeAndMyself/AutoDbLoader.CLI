using AutoDbLoader.DAL.MSSQL.Context;
using AutoDbLoader.DAL.MSSQL.Entity;
using AutoDbLoader.DAL.MSSQL.Interface;
using AutoDbLoader.DAL.txt.Entity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;


namespace AutoDbLoader.DAL.MSSQL.Repository
{
    public class TerritoryPaymentsRepository : ITerritoryPaymentsRepository
    {
        private readonly TerritoryPaymentContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TerritoryPaymentsRepository> _logger;

        public TerritoryPaymentsRepository(TerritoryPaymentContext context,
                                           ILogger<TerritoryPaymentsRepository> logger,
                                           IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public void Add(List<Payment> payments)
        {
            if (payments != null)
            {
                ExecUpdateTable();

                foreach (var payment in payments)
                {
                    var newTerritoryPayment = _mapper.Map<TerritoryPayments>(payment);
                    try
                    {
                        _logger.LogInformation($" Добавление в таблицу territory:" +
                            $" \r\n Alias: {payment.Alias}, " +
                            $" \r\n PaymentAccount: {payment.PaymentAccount}");

                        _context.Payments.Add(newTerritoryPayment);
                        _context.SaveChanges();
                    }
                    catch (System.Exception e)
                    {
                        _logger.LogError(e.Message);
                        throw;
                    }

                }
            }
        }

        private void ExecUpdateTable()
        {
            _context.Database.ExecuteSqlRaw(@"USE Ekassir_kart;" +
                @"EXEC sp_rename ' ', ' ';" +
                @"EXEC sp_rename ' ', ' ';" +
                @"EXEC sp_rename ' ', ' ';");
        }

        public int GetCountLS()
        {
            return _context.Payments.Count();
        }
    }
}
