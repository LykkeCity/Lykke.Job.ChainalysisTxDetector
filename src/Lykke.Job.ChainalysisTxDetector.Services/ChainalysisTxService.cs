﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.ChainalysisTxDetector.Core.Domain;
using Lykke.Job.ChainalysisTxDetector.Core.Services;
using NBitcoin;
using QBitNinja.Client;

namespace Lykke.Job.ChainalysisTxDetector.Services
{
    public class ChainalysisTxService : IChainalysisTxService
    {
        private readonly IChainalysisRepository _chainalysisRepository;
        private readonly ILog _log;
        private readonly QBitNinjaApiCaller _ninjaClient;
        private readonly Network _ninjaNetwork;
        public ChainalysisTxService(IChainalysisRepository chainalysisRepository, string ninjaUrl, bool isMainNetwork, ILog log)
        {
            _chainalysisRepository = chainalysisRepository ?? throw new ArgumentNullException(nameof(chainalysisRepository));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _ninjaNetwork = isMainNetwork ? Network.Main : Network.TestNet;
            _ninjaClient = new QBitNinjaApiCaller(() => new QBitNinjaClient(ninjaUrl, _ninjaNetwork));
           
        }

        public async Task ProccedAsync(IChainalysisTxEvent txEvent)
        {
            _log.WriteInfo(nameof(ChainalysisTxService), nameof(ProccedAsync), txEvent.ToJson());

            try
            {
                List<Task> tasks = new List<Task>();
                foreach(var cashIn in await GetDataToSave(txEvent))
                {
                    tasks.Add(_chainalysisRepository.SaveAsync(cashIn));
                }
                await Task.WhenAll(tasks.ToArray());

            }
            catch (Exception e)
            {
                _log.WriteError(nameof(ChainalysisTxService), nameof(ProccedAsync), e);
            }
        }

        private async Task<List<IChainalysisCash>> GetDataToSave(IChainalysisTxEvent txEvent)
        {
            
            var result = new List<IChainalysisCash>();
            try
            {
                var transaction = await _ninjaClient.GetTransaction(txEvent.TransactionHash);
                if (transaction != null)
                {
                    foreach (var rec in transaction.ReceivedCoins)
                    {
                        if (txEvent.Multisig.Equals(rec.TxOut.ScriptPubKey.GetDestinationAddress(_ninjaNetwork)?.ToString()))
                        {
                            result.Add(new ChainalisysCashMessage
                            {
                                LwClientId = txEvent.ClientId,
                                BtcTransactionHash = txEvent.TransactionHash,
                                WalletAddress = txEvent.Multisig,
                                OutputNumber = (int)rec.Outpoint.N,
                                Amount = rec.TxOut.Value.ToDecimal(MoneyUnit.BTC)
                            });
                        }
                    }
                }


            }
            catch (Exception e)
            {

                _log.WriteError(nameof(ChainalysisTxService), nameof(GetDataToSave), e);
            }

            return result;

        }
    }
}
