using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.ChainalysisTxDetector.Settings.TxDetector
{
    public class NinjaSettings
    {
        public bool IsMainNet { get; set; }

        [HttpCheck("/")]
        public string Url { get; set; }
    }


}
