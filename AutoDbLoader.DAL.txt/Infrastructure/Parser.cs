using AutoDbLoader.DAL.txt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoDbLoader.DAL.txt.Infrastructure
{
    public class Parser
    {
        public static List<AliasData> ParseToListAlias(string data)
        {
            var response = new List<AliasData>();

            var filteredList = data.Split("\r\n", StringSplitOptions.None).ToList();

            foreach (var row in filteredList)
            {
                if (!string.IsNullOrWhiteSpace(row))
                {
                    var aliasInfo = row.Split(";");

                    var alias = new AliasData()
                    {
                        INN = aliasInfo[Constant.AL_INN],
                        PaymentAccount = aliasInfo[Constant.AL_PAYMENT_ACCOUNT],
                        Alias = aliasInfo[Constant.AL_ALIAS],
                        Key = aliasInfo[Constant.AL_KEY]
                    };

                    if (aliasInfo.Length > 0)
                    {
                        response.Add(alias);
                    }
                }
            }
            return response;
        }

        public static List<Payment> ParseToListPayments(string data)
        {
            var response = new List<Payment>();

            var filteredList = data.Split(":[!];;1\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var row in filteredList)
            {
                if (!string.IsNullOrWhiteSpace(row))
                {
                    var paymentsInfo = row.Split(";");

                    var payments = new Payment()
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

                    if (paymentsInfo.Length > 0)
                    {
                        response.Add(payments);
                    }
                }
            }
            return response;
        }
    }
}
