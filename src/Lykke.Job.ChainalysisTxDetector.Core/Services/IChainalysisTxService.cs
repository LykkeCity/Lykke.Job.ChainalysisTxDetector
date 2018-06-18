using System;
using System.Threading.Tasks;
using Lykke.Job.ChainalysisTxDetector.Core.Domain;

namespace Lykke.Job.ChainalysisTxDetector.Core.Services
{
    public interface IChainalysisTxService
    {
        Task<bool> StoreRecord(IChainalysisCash row);
    }
}
