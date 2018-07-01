using System;
using Lykke.Job.ChainalysisTxDetector.Core.Domain;
using ProtoBuf;

namespace Lykke.Job.ChainalysisTxDetector.Modules
{
    [ProtoContract]
    public class ConfirmationSavedEvent : IChainalysisTxEvent
    {
        [ProtoMember(1)]
        public string TransactionHash { get; set; }
        [ProtoMember(2)]
        public string ClientId { get; set; }
        [ProtoMember(3)]
        public string Multisig { get; set; }
    }
}
