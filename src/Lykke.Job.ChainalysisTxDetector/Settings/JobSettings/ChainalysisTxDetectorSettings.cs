namespace Lykke.Job.ChainalysisTxDetector.Settings.JobSettings
{
    public class ChainalysisTxDetectorSettings
    {
        public DbSettings Db { get; set; }
        public RabbitMqSettings Rabbit { get; set; }
    }
}
