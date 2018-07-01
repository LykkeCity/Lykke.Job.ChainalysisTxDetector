using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.ChainalysisTxDetector.Settings.TxDetector
{
    public class TxDetectorJobSettings
    {

        public long RetryDelayInMilliseconds { get; set; }
        [Optional]
        public ChaosSettings ChaosKitty { get; set; }
        public string RabbitMQConnectionString { get; set; }
        public string Environment { get; set; }
        public NinjaSettings Ninja { get; set; }
    }


}
