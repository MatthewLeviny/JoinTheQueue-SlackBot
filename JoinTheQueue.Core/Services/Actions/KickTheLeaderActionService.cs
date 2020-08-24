using System.Threading.Tasks;
using JoinTheQueue.Core.Dto;

namespace JoinTheQueue.Core.Services.Actions
{
    public class KickTheLeaderActionService :IActionService
    {
        public string Key => "KickAction";
        public Task PerformAction(Root request)
        {
            throw new System.NotImplementedException();
        }
    }
}