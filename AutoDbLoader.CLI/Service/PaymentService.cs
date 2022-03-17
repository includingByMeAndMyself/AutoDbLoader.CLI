using AutoDbLoader.CLI.Interface;
using AutoDbLoader.DAL.MSSQL.Interface;
using AutoDbLoader.DAL.txt.Entity;
using AutoDbLoader.DAL.txt.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace AutoDbLoader.CLI.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITerritoryPaymentsRepository _territoryPaymentRepository;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository paymentRepository,
                              ITerritoryPaymentsRepository territoryPaymentRepository,
                              ILogger<PaymentService> logger)
        {
            _territoryPaymentRepository = territoryPaymentRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public void LoadDataToDb()
        {
            var paymentsData = FillAlias();

            var paymentsCountInDb = _territoryPaymentRepository.GetCountLS();

            _logger.LogWarning($" Колличество строк в БД territory: {paymentsCountInDb}");

            var IsValid = true;

            if ((paymentsData.Count() - (paymentsData.Count() / 100 * 2)) < paymentsCountInDb)
            {
                IsValid = false;
                _logger.LogError($" Колличество строк от поставщика меньше, чем в базе данных");
            }

            if (paymentsData != null && IsValid)
            {
                try
                {
                    _logger.LogInformation(" Обновление Бд territory данными поставщика");

                    _territoryPaymentRepository.Add(paymentsData);
                }
                catch (System.Exception e)
                {
                    _logger.LogError(e.Message);
                    throw;
                }
            }
        }

        private List<Payment> FillAlias()
        {
            _logger.LogInformation(" Загрузка данных из файла для получения Alias");

            var aliasData = _paymentRepository.GetAliasDataFromFile();

            _logger.LogInformation(" Загрузка данных из файла поставщика");

            var paymentsData = _paymentRepository.GetPaymentsDataFromFile();

            _logger.LogInformation($" Загруженно {paymentsData.Count()} строк из файла поставщика");

            if (paymentsData != null)
            {
                foreach (var payment in paymentsData)
                {
                    var paymentAlias = aliasData
                        .Where(p =>
                               p.PaymentAccount == payment.PaymentAccount &&
                               p.INN == payment.INN)
                        .Select(x => x.Alias);

                    if (paymentAlias.Count() != 0)
                    {
                        foreach (var alias in paymentAlias)
                        {
                            if (paymentAlias.Count() > 1 &&
                               payment.Address.Contains(alias))
                                payment.Alias = alias;
                            else
                                payment.Alias = alias;
                        }
                    }
                }
            }
            return paymentsData;
        }
    }
}
