using System;
namespace Lykke.Job.ChainalysisTxDetector.Core.Domain
{
    public interface IChainalysisCash
    {
        string LwClientId { get; set; }
        string BtcTransactionHash { get; set; }
        string WalletAddress { get; set; }
        int OutputNumber { get; set; }
        string Amount { get; set; }
        string ChainalysisScore { get; set; }
        string ChainalysisName { get; set; }
        string ChainalysisCategory { get; set; }
    }
}
