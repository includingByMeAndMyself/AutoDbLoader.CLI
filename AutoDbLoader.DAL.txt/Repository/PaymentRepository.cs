using AutoDbLoader.DAL.txt.Entity;
using AutoDbLoader.DAL.txt.Infrastructure;
using AutoDbLoader.DAL.txt.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoDbLoader.DAL.txt.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _paymentsDataPath;
        private readonly string _aliasDataPath;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(TxtSettings txtSettings,
                                 ILogger<PaymentRepository> logger)
        {
            _paymentsDataPath = txtSettings.PaymentsDataPath;
            _aliasDataPath = txtSettings.AliasDataPath;
            _logger = logger;
        }
        
        public List<AliasData> GetAliasDataFromFile()
        {
            var aliasData = GetDataFromFile(_aliasDataPath);
            var response = Parser.ParseToListAlias(aliasData);
            return response;
        }

        public List<Payment> GetPaymentsDataFromFile()
        {
            var paymentsData = GetDataFromFile(_paymentsDataPath, eEncoding.Win1251);
            var response = Parser.ParseToListPayments(paymentsData);
            return response;
        }

        private string GetDataFromFile(string path, eEncoding encoding = eEncoding.UTF8, string extension = "*.txt")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var files = Directory.GetFiles(path, extension);
            var data = "";

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    try
                    {
                        _logger.LogInformation($" Загрузка из файла {file}");

                        if(encoding == eEncoding.Win1251)
                            data += File.ReadAllText(file, Encoding.GetEncoding(1251));
                        else
                            data += File.ReadAllText(file, Encoding.UTF8);
                    }
                    catch (System.Exception e)
                    {
                        _logger.LogError(e.Message);
                        throw;
                    }
                    
                }
            }
            return data;
        }
    }
}
