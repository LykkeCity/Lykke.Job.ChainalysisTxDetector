using System.Threading.Tasks;

namespace Lykke.Job.ChainalysisTxDetector.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
