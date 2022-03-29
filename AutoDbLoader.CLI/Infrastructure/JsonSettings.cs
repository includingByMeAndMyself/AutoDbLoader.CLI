
namespace AutoDbLoader.CLI.Infrastructure
{
    public class JsonSettings
    {
        public JsonSettings(string paymentsJsonDataPath)
        {
            PaymentsJsonDataPath = paymentsJsonDataPath;
        }
        public string PaymentsJsonDataPath { get; }
    }
}
