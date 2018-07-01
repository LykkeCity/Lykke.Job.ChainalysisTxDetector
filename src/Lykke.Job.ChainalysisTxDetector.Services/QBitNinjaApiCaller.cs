using System;
using System.Threading.Tasks;
using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;

namespace Lykke.Job.ChainalysisTxDetector.Services
{
    public class QBitNinjaApiCaller
    {
        private readonly Func<QBitNinjaClient> _clientFactory;

        public QBitNinjaApiCaller(Func<QBitNinjaClient> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        
        public Task<GetTransactionResponse> GetTransaction(string hash)
        {
            var client = _clientFactory();
            client.Colored = true;
            return client.GetTransaction(uint256.Parse(hash));
        }

        public Task<GetBlockResponse> GetBlock(int blockHeight)
        {
            var client = _clientFactory();

            return client.GetBlock(new BlockFeature(blockHeight));
        }

        public async Task<int> GetCurrentBlockNumber()
        {
            var client = _clientFactory();

            return (await client.GetBlock(new BlockFeature(SpecialFeature.Last), true)).AdditionalInformation.Height;
        }
    }
}
