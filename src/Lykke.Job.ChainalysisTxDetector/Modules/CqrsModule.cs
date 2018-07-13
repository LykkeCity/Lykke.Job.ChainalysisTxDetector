using System.Collections.Generic;
using Autofac;
using Common.Log;
using Lykke.Common.Chaos;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Job.ChainalysisTxDetector.Projections;
using Lykke.Job.ChainalysisTxDetector.Settings;
using Lykke.Job.ChainalysisTxDetector.Utils;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.SettingsReader;

namespace Lykke.Job.ChainalysisTxDetector.Modules
{
    public class CqrsModule : Module
    {
        private readonly AppSettings _settings;
        private readonly ILog _log;

        public CqrsModule(IReloadingManager<AppSettings> settingsManager, ILog log)
        {
            _settings = settingsManager.CurrentValue;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_settings.TxDetectorJob.ChaosKitty != null)
            {
                Utils.ChaosKitty.StateOfChaos = _settings.TxDetectorJob.ChaosKitty.StateOfChaos;
            }
            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>().SingleInstance();

            var rabbitMqSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _settings.TxDetectorJob.RabbitMQConnectionString };
            var messagingEngine = new MessagingEngine(_log,
                new TransportResolver(new Dictionary<string, TransportInfo>
                {
                    {"RabbitMq", new TransportInfo(rabbitMqSettings.Endpoint.ToString(), rabbitMqSettings.UserName, rabbitMqSettings.Password, "None", "RabbitMq")}
                }),
                new RabbitMqTransportFactory());

            builder.RegisterType<ChainalysisTxProjection>();

            var defaultRetryDelay = _settings.TxDetectorJob.RetryDelayInMilliseconds;
            builder.Register(ctx =>
            {
                var projection = ctx.Resolve<ChainalysisTxProjection>();

                return new CqrsEngine(
                    _log,
                    ctx.Resolve<IDependencyResolver>(),
                    messagingEngine,
                    new DefaultEndpointProvider(),
                    true,
                    Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver("RabbitMq", "protobuf", environment: _settings.TxDetectorJob.Environment)),


                    Register.BoundedContext("chainalysis-test")
                        .ListeningEvents(typeof(ConfirmationSavedEvent))
                        .From("transactions").On("transactions-events")
                        .WithProjection(projection, "transactions")
                );
            }).As<ICqrsEngine>().SingleInstance().AutoActivate();
            
        }
    }
}
