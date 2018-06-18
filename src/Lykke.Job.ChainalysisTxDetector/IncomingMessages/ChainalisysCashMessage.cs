using Lykke.Job.ChainalysisTxDetector.Core.Domain;

namespace Lykke.Job.ChainalysisTxDetector.IncomingMessages
{
    public class ChainalisysCashMessage : IChainalysisCash
    {
        public string LwClientId { get; set; }
        public string BtcTransactionHash { get; set; }
        public string WalletAddress { get; set; }
        public int OutputNumber { get; set; }
        public double Amount { get; set; }
    }
}