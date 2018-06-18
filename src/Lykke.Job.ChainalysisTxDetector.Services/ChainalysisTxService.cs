using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.ChainalysisTxDetector.Core.Domain;
using Lykke.Job.ChainalysisTxDetector.Core.Services;

namespace Lykke.Job.ChainalysisTxDetector.Services
{
    public class ChainalysisTxService : IChainalysisTxService
    {
        private readonly IChainalysisRepository _chainalysisRepository;
        private readonly ILog _log;
        public ChainalysisTxService(IChainalysisRepository chainalysisRepository, ILog log)
        {
            _chainalysisRepository = chainalysisRepository ?? throw new ArgumentNullException(nameof(chainalysisRepository));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<bool> StoreRecord(IChainalysisCash row)
        {
            try
            {
                await _log.WriteInfoAsync(nameof(ChainalysisTxService), nameof(StoreRecord), row.ToJson());
                return await _chainalysisRepository.SaveAsync(row);

            }
            catch(Exception e)
            {
                await _log.WriteErrorAsync(nameof(ChainalysisTxService), nameof(StoreRecord), e);
                return false;
            }

        }
    }
}
