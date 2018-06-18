using System;
using System.Threading.Tasks;

namespace Lykke.Job.ChainalysisTxDetector.Core.Domain
{
    public interface IChainalysisRepository
    {
        Task<bool> SaveAsync(IChainalysisCash row);
    }
}
