﻿using Lykke.Job.ChainalysisTxDetector.Settings.JobSettings;
using Lykke.Job.ChainalysisTxDetector.Settings.SlackNotifications;
using Lykke.Job.ChainalysisTxDetector.Settings.TxDetector;
using Lykke.Service.ChainalysisProxy.Client;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.ChainalysisTxDetector.Settings
{
    public class AppSettings
    {
        public ChainalysisTxDetectorSettings ChainalysisTxDetectorJob { get; set; }

        public SlackNotificationsSettings SlackNotifications { get; set; }

        [Optional]
        public MonitoringServiceClientSettings MonitoringServiceClient { get; set; }

        public TxDetectorJobSettings TxDetectorJob { get; set; }

        public ChainalysisProxyServiceClientSettings ChainalysisProxyServiceClient { get; set; } 

    }


}
