using System;
namespace Lykke.Job.ChainalysisTxDetector.Core.Domain
{
    public interface IChainalysisTxEvent
    {   
        string TransactionHash { get; set; }
        string ClientId { get; set; }
        string Multisig { get; set; }
    }
}
