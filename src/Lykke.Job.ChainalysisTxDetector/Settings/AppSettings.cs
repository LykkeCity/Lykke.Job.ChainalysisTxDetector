using Lykke.Job.ChainalysisTxDetector.Settings.JobSettings;
using Lykke.Job.ChainalysisTxDetector.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.ChainalysisTxDetector.Settings
{
    public class AppSettings
    {
        public ChainalysisTxDetectorSettings ChainalysisTxDetectorJob { get; set; }

        public SlackNotificationsSettings SlackNotifications { get; set; }

        [Optional]
        public MonitoringServiceClientSettings MonitoringServiceClient { get; set; }
    }
}
