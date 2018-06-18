using System;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.ChainalysisTxDetector.Core.Domain;

namespace Lykke.Job.ChainalysisTxDetector.AzureRepositories
{
    public class ChainalysisRepository : IChainalysisRepository
    {
        private readonly INoSQLTableStorage<ChainalysisCash> _repository;
        public ChainalysisRepository(INoSQLTableStorage<ChainalysisCash> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        }

        public async Task<bool> SaveAsync(IChainalysisCash row)
        {
            await _repository.InsertOrMergeAsync(new ChainalysisCash(row));
            return true;
        }
    }
}
