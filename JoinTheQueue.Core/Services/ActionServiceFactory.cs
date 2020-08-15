using System;
using System.Collections.Generic;
using System.Linq;
using JoinTheQueue.Core.Services.Actions;

namespace JoinTheQueue.Core.Services
{
    public interface IActionServiceFactory
    {
        IActionService GetActionService(string action);
    }

    public class ActionServiceFactory : IActionServiceFactory
    {
        private readonly IEnumerable<IActionService> _actions;

        public ActionServiceFactory(IEnumerable<IActionService> actions)
        {
            _actions = actions;
        }

        public IActionService GetActionService(string action)
        {
            return _actions.FirstOrDefault(x => x.Key == action);
        }
    }
}