using System;
using Lykke.Job.ChainalysisTxDetector.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.ChainalysisTxDetector.AzureRepositories
{
    public class ChainalysisCash : TableEntity, IChainalysisCash
    {
        public ChainalysisCash()
        {
            ETag = "*";
        }

        private string _btcTransactionHash;
        private int _outputNumber;

        public ChainalysisCash(IChainalysisCash item)
        {
            LwClientId = item.LwClientId;
            BtcTransactionHash = item.BtcTransactionHash;
            WalletAddress = item.WalletAddress;
            OutputNumber = item.OutputNumber;
            Amount = item.Amount;
        }

        private void UpdateRowKey()
        {
            RowKey = $"{_btcTransactionHash}:{_outputNumber}";
        }

        public string LwClientId { get; set; }
        public string BtcTransactionHash { get=>_btcTransactionHash; set { _btcTransactionHash = value; UpdateRowKey(); } }
        public string WalletAddress { get=>PartitionKey; set=>PartitionKey = value; }
        public int OutputNumber { get => _outputNumber; set { _outputNumber = value; UpdateRowKey(); } }
        public decimal Amount { get; set; }
    }
}
