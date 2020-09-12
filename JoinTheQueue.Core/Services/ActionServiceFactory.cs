using System.Collections.Generic;
using System.Linq;
using JoinTheQueue.Core.Dto;
using JoinTheQueue.Core.Services.Actions;

namespace JoinTheQueue.Core.Services
{
    public interface IActionServiceFactory
    {
        IActionService GetActionService(Action action);
    }

    public class ActionServiceFactory : IActionServiceFactory
    {
        private readonly IEnumerable<IActionService> _actions;

        public ActionServiceFactory(IEnumerable<IActionService> actions)
        {
            _actions = actions;
        }

        public IActionService GetActionService(Action action)
        {
            return action.value == null
                ? _actions.FirstOrDefault(x => x.Key == action.selected_option.Value)
                : _actions.FirstOrDefault(x => x.Key == action.value);
        }
    }
}