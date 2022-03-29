using AutoDbLoader.DAL.MSSQL.Interface;
using AutoDbLoader.DAL.txt.Entity;
using AutoDbLoader.DAL.txt.Interface;
using AutoDbLoader.CLI.Infrastructure;
using AutoDbLoader.CLI.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AutoDbLoader.CLI.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly string _paymentsJsonDataPath;
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

        public PaymentService(IPaymentRepository paymentRepository,
                              ITerritoryPaymentsRepository territoryPaymentRepository,
                              ILogger<PaymentService> logger,
                              JsonSettings jsonSettings)
        {
            _territoryPaymentRepository = territoryPaymentRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
            _paymentsJsonDataPath = jsonSettings.PaymentsJsonDataPath;
        }


        public void LoadDataToDb()
        {
            var paymentsData = GetDataWithAlias();

            var paymentsCountInDb = _territoryPaymentRepository.GetCountLS();

            _logger.LogWarning($" Колличество строк в БД territory: {paymentsCountInDb}");

            //TODO не забыть про IsValid - поменять знак на >=
            var paymentsCountFromFileMinusTwoPercent = (paymentsData.Count() - (paymentsData.Count() / 100 * 2));
            var IsValid = paymentsCountFromFileMinusTwoPercent >= paymentsCountInDb;

            if (paymentsData != null && IsValid)
            {
                try
                {
                    _logger.LogInformation(" Обновление Бд territory данными поставщика");

                    _territoryPaymentRepository.Add(paymentsData);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    throw;
                }
            }
            else
            {
                _logger.LogError($" Колличество строк от поставщика меньше, чем в базе данных");
            }
        }


        private List<Payment> GetDataWithAlias()
        {
            _logger.LogInformation(" Загрузка данных из файла для получения Alias");

            var aliasData = _paymentRepository.GetAliasDataFromFile();

            _logger.LogInformation(" Загрузка данных из файла поставщика");

            var paymentsData = _paymentRepository.GetPaymentsDataFromFile();

            _logger.LogInformation($" Загруженно {paymentsData.Count()} строк из файла поставщика ");

            var response = GetPaymentsWithAlias(aliasData, paymentsData);

            return response;
        }


        private List<Payment> GetPaymentsWithAlias(List<AliasData> aliasData, List<Payment> paymentsData)
        {
            if (paymentsData != null)
            {
                foreach (var payment in paymentsData)
                {
                    var paymentAlias = GetAlias(aliasData, payment);

                    if (string.IsNullOrEmpty(paymentAlias))
                    {
                        WriteJSON(payment, "payment_without_alias.json");
                    }
                    else
                    {
                        payment.Alias = paymentAlias;
                    }
                }
            }
            return paymentsData;
        }


        public string GetAlias(List<AliasData> aliasData, Payment payment)
        {
            var key = GetValidKey(payment);

            if (key == "БЕЗ КЛЮЧА")
            {
                WriteJSON(payment, "payment_without_key.json");
            }

            var checkaddress = Validator.GetAddressForCheck(payment.Address);

            payment.Address += " " + key;

            return aliasData
                .Where(p =>
                       payment.INN == p.INN &&
                       payment.PaymentAccount == p.PaymentAccount &&
                       key == p.Key &&
                       checkaddress == p.Address)
                .Select(x => x.Alias)
                .FirstOrDefault();
        }


        private string GetValidKey(Payment payment)
        {
            if (Validator.IsJKU(payment))
            {
                return "ЖКУ";
            }
            else if (Validator.IsParking(payment))
            {
                return "паркинг";
            }
            else if (Validator.IsSecurity(payment))
            {
                return "охрана";
            }
            else if (Validator.IsOverhaul(payment))
            {
                return "капремонт";
            }
            return "БЕЗ КЛЮЧА";
        }


        private void WriteJSON(Payment payment, string fileName)
        {
            var date = DateTime.Now.ToShortDateString();
            //var path = @"D:\!Test_proj\FrisbeeDigital.Loader\FrisbeeDigital.Parser.Territoria\bin\Debug\!warning\";
            var path = _paymentsJsonDataPath;

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            _logger.LogWarning("Не присвоился Alias:\r\n");
            _logger.LogWarning(JsonSerializer.Serialize(payment, options));

            using (FileStream fs = new FileStream(path + $"{date} {fileName}", FileMode.Append))
            {
                JsonSerializer.Serialize(fs, payment, options);
            }
        }
    }
}