using AutoDbLoader.CLI.Service;
using AutoDbLoader.DAL.MSSQL.Interface;
using AutoDbLoader.DAL.txt.Entity;
using AutoDbLoader.DAL.txt.Infrastructure;
using AutoDbLoader.DAL.txt.Interface;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoDbLoader.Test
{
    public class PaymentServiceTest
    {
        private PaymentService _service;
        private Mock<IPaymentRepository> _paymentRepositoryMock;
        private Mock<ITerritoryPaymentsRepository> _territoryPaymentsRepositoryMock;
        private Mock<ILogger<PaymentService>> _loggerMock;



        [SetUp]
        public void SetUp()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _territoryPaymentsRepositoryMock = new Mock<ITerritoryPaymentsRepository>();
            _loggerMock = new Mock<ILogger<PaymentService>>();

            _service = new PaymentService(_paymentRepositoryMock.Object, _territoryPaymentsRepositoryMock.Object, _loggerMock.Object);
        }


        private Payment GetDataForTest(string data)
        {
            var paymentsInfo = data.Split(";");

            return new Payment
            {
                Payer = paymentsInfo[Constant.PAY_PAYER],
                Address = paymentsInfo[Constant.PAY_ADDRESS],
                PersonalAccount = paymentsInfo[Constant.PAY_PERSONAL_ACCOUNT],
                Debt = paymentsInfo[Constant.PAY_DEBT].Replace(',', '.'),
                BIK = paymentsInfo[Constant.PAY_BIK],
                PaymentAccount = paymentsInfo[Constant.PAY_PAYMENT_ACCOUNT],
                INN = paymentsInfo[Constant.PAY_INN],
                Recipient = paymentsInfo[Constant.PAY_RECIPIENT],
                IndicationPeriod = paymentsInfo[Constant.PAY_INDICATION_PERIOD],
                IndicationsOfMeteringDevices = paymentsInfo[Constant.PAY_INDICATION_OF_METERING_DEVICES],
                Alias = String.Empty
            };
        }

        private List<AliasData> GetAliasDataFromFile()
        {
            var aliasData = GetDataFromFile(@"путь до файла сравнения");
            var response = Parser.ParseToListAlias(aliasData);
            return response;
        }

        private string GetDataFromFile(string path, eEncoding encoding = eEncoding.UTF8)
        {
            var files = Directory.GetFiles(path, "*.txt");
            var data = "";

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    data = File.ReadAllText(file, Encoding.UTF8);
                }
            }
            return data;
        }

        [Test]
        [TestCase("плательщик;Екатеринбург г, Чайковского ул, д.80 к.2 стр.паркинг п/м 5-6;11111111;1111;1111111;40702810916540062880;6679065175;ООО \"Центр расчетов\";022022;показания счетчиков", "ter_chai_80/2_par")]
        [TestCase("плательщик;Екатеринбург г, Николая Островского ул, д.1 Офис;111111;1111;111111;40702810916540062877;6679065175;ООО \"Центр расчетов\";022022;", "ter102")]
        public void GetAlias_ShouldNoEmpty(string data, string expectedResult)
        {
            // arrage

            var payment = GetDataForTest(data);
            var aliasData = GetAliasDataFromFile();

            //act

            var result = _service.GetAlias(aliasData, payment);

            //assert

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
