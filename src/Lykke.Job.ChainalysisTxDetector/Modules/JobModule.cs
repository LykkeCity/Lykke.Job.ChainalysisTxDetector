using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.Job.ChainalysisTxDetector.Core.Services;
using Lykke.Job.ChainalysisTxDetector.Settings.JobSettings;
using Lykke.Job.ChainalysisTxDetector.Services;
using Lykke.SettingsReader;
using Lykke.Job.ChainalysisTxDetector.AzureRepositories;
using AzureStorage.Tables;
using Lykke.Job.ChainalysisTxDetector.Core.Domain;
using Lykke.Job.ChainalysisTxDetector.Settings;

namespace Lykke.Job.ChainalysisTxDetector.Modules
{
    public class JobModule : Module
    {
        private readonly AppSettings _settings;
        private readonly IReloadingManager<ChainalysisTxDetectorSettings> _settingsManager;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;


        public JobModule(IReloadingManager<AppSettings> settingsManager, ILog log)
        {
            _settings = settingsManager.CurrentValue;
            _log = log;

            _settingsManager = settingsManager.Nested(x => x.ChainalysisTxDetectorJob);

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            // builder.RegisterType<QuotesPublisher>()
            //  .As<IQuotesPublisher>()
            //  .WithParameter(TypedParameter.From(_settings.Rabbit.ConnectionString))

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<ChainalysisRepository>()
                .As<IChainalysisRepository>()
                .WithParameter(TypedParameter.From(AzureTableStorage<ChainalysisCash>.Create(_settingsManager.ConnectionString(x => x.Db.DataConnString), "ChainalyisTxCach", _log)))
                .SingleInstance();

            builder.RegisterType<ChainalysisTxService>()
                   .WithParameter("ninjaUrl", _settings.TxDetectorJob.Ninja.Url)
                   .WithParameter("isMainNetwork", _settings.TxDetectorJob.Ninja.IsMainNet)
                .As<IChainalysisTxService>()
                .SingleInstance();


            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();


            // TODO: Add your dependencies here

            builder.Populate(_services);
        }


    }
}
