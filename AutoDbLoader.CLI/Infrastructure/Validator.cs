using AutoDbLoader.DAL.txt.Entity;
using System.Collections.Generic;

namespace AutoDbLoader.CLI.Infrastructure
{
    public static class Validator
    {
        private static List<string> parking = new List<string>() { "паркинг", "п/м", "стр.паркинг", "Паркинг" };
        private static List<string> office = new List<string>() { "офис", "оф", "Офис", "офис1" };
        private static string[] seporator = new string[] {" г, ", " п, ", " кв", " пом", " офис", " стр.паркинг", " Офис", 
            " тренажерный зал", " п/м", " соор", " офис1", " п / м", " стр", " выставочный"};

        public static bool IsParking(Payment payment) =>
           IsConteinsParcing(payment) && IsConteinsJKURecipient(payment);


        public static bool IsSecurity(Payment payment) =>
            IsConteinsSecurityPaymentAccount(payment) && (!IsHaveIndicationsOfMeteringDevices(payment));


        public static bool IsOverhaul(Payment payment) =>
            IsConteinsParcing(payment) && (!IsConteinsJKURecipient(payment)) ||
            IsConteinsOffice(payment) && (!IsConteinsJKURecipient(payment)) && (!IsHaveIndicationsOfMeteringDevices(payment)) ||
            IsConteinsOverhaulRecipient(payment) && (!IsHaveIndicationsOfMeteringDevices(payment));
       

        public static bool IsJKU(Payment payment) =>
            (!IsConteinsParcing(payment)) &&
            ((IsConteinsOffice(payment) && IsConteinsJKURecipient(payment)) ||
            (IsConteinsJKURecipient(payment) && (!IsHaveIndicationsOfMeteringDevices(payment)) && (!IsConteinsSecurityPaymentAccount(payment))) ||
            (IsConteinsJKURecipient(payment) && IsHaveIndicationsOfMeteringDevices(payment)) ||
            (payment.Recipient.Contains("ТСЖ") && IsHaveIndicationsOfMeteringDevices(payment)));


        public static string GetAddressForCheck(string address)
        {
            var street = address.Split(seporator,System.StringSplitOptions.TrimEntries);
            return street[1];
        }


        private static bool IsConteinsParcing(Payment payment) 
        {
            foreach (var item in parking)
            {
                if(payment.Address.Contains(item))
                    return true;
            }
            return false;
        } 
            
       
        private static bool IsConteinsOffice(Payment payment)
        {
            foreach (var item in office)
            {
                if (payment.Address.Contains(item))
                    return true;
            }
            return false;
        }

        private static bool IsConteinsOverhaulRecipient(Payment payment) =>
            payment.Recipient.Contains("ТСЖ") ||
            payment.Recipient.Contains("УЖК") ||
            payment.Recipient.Contains("ЖЭК") ||
            payment.Recipient.Contains("Региональный Фонд капитального");

        private static bool IsConteinsJKURecipient(Payment payment) =>
            payment.Recipient.Contains("Центр расчетов") ||
            payment.Recipient.Contains("ЖЭК");

       
        private static bool IsHaveIndicationsOfMeteringDevices(Payment payment) => 
            payment.IndicationsOfMeteringDevices.Length > 1;
       
        private static bool IsConteinsSecurityPaymentAccount(Payment payment) =>
            payment.PaymentAccount.Contains("40702810616540062876") ||
            payment.PaymentAccount.Contains("40702810516540062879") ||
            payment.PaymentAccount.Contains("40821810116540000361") ||
            payment.PaymentAccount.Contains("40821810116540000099");
    }
}
