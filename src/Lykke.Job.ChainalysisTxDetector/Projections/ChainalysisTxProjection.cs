using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Job.ChainalysisTxDetector.Core.Services;
using Lykke.Job.ChainalysisTxDetector.Modules;
using Lykke.Job.ChainalysisTxDetector.Utils;

namespace Lykke.Job.ChainalysisTxDetector.Projections
{
    public class ChainalysisTxProjection
    {
        private readonly ILog _log;
        private readonly IChainalysisTxService _chainalysisTxService;

        public ChainalysisTxProjection(
            [NotNull] ILog log,
            [NotNull] IChainalysisTxService chainalysisTxService)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _chainalysisTxService = chainalysisTxService ?? throw new ArgumentNullException(nameof(chainalysisTxService));
        }

        public async Task Handle(ConfirmationSavedEvent evt)
        {

            await _chainalysisTxService.ProccedAsync(evt);
            ChaosKitty.Meow();
           
        }
    }
}
