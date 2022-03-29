using AutoMapper;
using AutoDbLoader.DAL.MSSQL.Context;
using AutoDbLoader.DAL.MSSQL.Entity;
using AutoDbLoader.DAL.MSSQL.Interface;
using AutoDbLoader.DAL.txt.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
                var listAlias = new HashSet<string>();
                try
                {
                    ExecUpdateTable();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw;
                }

                try
                {
                    ExecDeleteTable();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw;
                }

                foreach (var payment in payments)
                {
                    var newTerritoryPayment = _mapper.Map<TerritoryPayments>(payment);
                    try
                    {
                        _logger.LogInformation($" Добавление в таблицу territory:" +
                            $" \r\n Alias: {payment.Alias}, " +
                            $" \r\n PaymentAccount: {payment.PaymentAccount}");

                        listAlias.Add(payment.Alias);

                        _context.Payments.Add(newTerritoryPayment);
                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        throw;
                    }
                }

                try
                {
                    if (string.IsNullOrEmpty(listAlias.ToString()))
                    {
                        var output = string.Join(";", listAlias);
                        output += ";territory_all;";
                        ExecBalancePayments(output);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw;
                }
            }
        }

        private void ExecUpdateTable()
        {
            _context.Database.ExecuteSqlRaw(@"USE Ekassir_kart;" +
                @"EXEC sp_rename 'dbo.territory', 'territory_11';" +
                @"EXEC sp_rename 'dbo.territory_old', 'territory';" +
                @"EXEC sp_rename 'dbo.territory_11', 'territory_old';");
        }


        private void ExecDeleteTable()
        {
            _context.Database.ExecuteSqlRaw(@"USE Ekassir_kart;" +
                @"DELETE FROM dbo.territory");
        }

        private void ExecBalancePayments(string listAlias)
        {
            _context.Database.ExecuteSqlRaw($"execute[Ekassir_kart].[dbo].[Del_Balance_payments] \'{listAlias}\'");
        }

        public int GetCountLS()
        {
            return _context.Payments.Count();
        }
    }
}
