using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.Job.ChainalysisTxDetector.Core.Services;
using Lykke.Job.ChainalysisTxDetector.IncomingMessages;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;

namespace Lykke.Job.ChainalysisTxDetector.RabbitSubscribers
{
    public class ChainalysisTxSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly string _connectionString;
        private readonly string _exchangeName;
        private RabbitMqSubscriber<ChainalisysCashMessage> _subscriber;
        private IChainalysisTxService _chainalysisTxService;

        public ChainalysisTxSubscriber(
            ILog log,
            string connectionString,
            string exchangeName,
            IChainalysisTxService chainalysisTxService   
        )
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _exchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
            _chainalysisTxService = chainalysisTxService ?? throw new ArgumentNullException(nameof(chainalysisTxService));

        }

        public void Start()
        {
            // NOTE: Read https://github.com/LykkeCity/Lykke.RabbitMqDotNetBroker/blob/master/README.md to learn
            // about RabbitMq subscriber configuration

            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_connectionString, _exchangeName, "chainalysistxdetector");
            // TODO: Make additional configuration, using fluent API here:
            // ex: .MakeDurable()

            _subscriber = new RabbitMqSubscriber<ChainalisysCashMessage>(settings,
                    new ResilientErrorHandlingStrategy(_log, settings,
                        retryTimeout: TimeSpan.FromSeconds(10),
                        next: new DeadQueueErrorHandlingStrategy(_log, settings)))
                .SetMessageDeserializer(new JsonMessageDeserializer<ChainalisysCashMessage>())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetLogger(_log)
                .SetConsole(new LogToConsole())
                .Start();
        }

        private async Task ProcessMessageAsync(ChainalisysCashMessage arg)
        {
            await _chainalysisTxService.StoreRecord(arg);
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }
    }
}
