using System;
using System.Collections.Generic;
using Bichebot.Core.Repositories;

namespace Bichebot.Core.Modules.Base
{
    [Obsolete("Уже не очень полезная абстракция")]
    public class StatefulBaseModule<TState> : BaseModule where TState : class
    {
        private readonly IDictionary<ulong, TState> repository;

        private readonly Func<TState> createDefault;

        protected StatefulBaseModule(IBotCore core, Func<TState> createDefault) : base(core)
        {
            repository = new Dictionary<ulong, TState>();
            this.createDefault = createDefault;
        }

        protected TState GetState(ulong id)
        {
            if (repository.TryGetValue(id, out var state))
                return state;

            var newState = createDefault();
            repository[id] = createDefault();

            return newState;
        }

        protected TState SetState(ulong id, TState state)
        {
            repository[id] = state;
            return state;
        }
    }
}