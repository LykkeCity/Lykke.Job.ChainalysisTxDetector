using Lykke.Job.ChainalysisTxDetector.Core.Domain;

namespace Lykke.Job.ChainalysisTxDetector.Services
{
    public class ChainalisysCashMessage : IChainalysisCash
    {
        public string LwClientId { get; set; }
        public string BtcTransactionHash { get; set; }
        public string WalletAddress { get; set; }
        public int OutputNumber { get; set; }
        public string Amount { get; set; }
        public string ChainalysisScore { get; set; }
        public string ChainalysisName { get; set; }
        public string ChainalysisCategory { get; set; }
    }
}
