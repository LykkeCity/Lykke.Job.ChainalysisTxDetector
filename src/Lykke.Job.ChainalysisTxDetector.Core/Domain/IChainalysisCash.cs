using System;
namespace Lykke.Job.ChainalysisTxDetector.Core.Domain
{
    public interface IChainalysisCash
    {
        string LwClientId { get; set; }
        string BtcTransactionHash { get; set; }
        string WalletAddress { get; set; }
        int OutputNumber { get; set; }
        double Amount { get; set; }
    }
}
