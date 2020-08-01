using System.Threading.Tasks;

namespace JoinTheQueue.Core.Services
{
    public interface IWebHookService
    {
        Task<bool> TriggerWebHook(string url, object payload);
    }
}