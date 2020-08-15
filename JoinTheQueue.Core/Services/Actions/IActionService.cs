using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;

namespace JoinTheQueue.Core.Services.Actions
{
    public interface IActionService
    {
        string Key { get; }
        Task PerformAction(Root request);
    }
}