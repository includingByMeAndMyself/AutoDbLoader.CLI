
namespace AutoDbLoader.DAL.txt.Infrastructure
{
    public class TxtSettings
    {
        public TxtSettings(string paymentsDataPath, string aliasDataPath)
        {
            PaymentsDataPath = paymentsDataPath;
            AliasDataPath = aliasDataPath;
        }

        public string PaymentsDataPath { get; }
        public string AliasDataPath { get; }
    }
}
