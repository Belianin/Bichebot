using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core;
using Discord;

namespace Bichebot.Modules.Base
{
    public class StatefulBaseModule<TState> : BaseModule // where T : class
    {
        private readonly IRepository<ulong, TState> repository;
        
        private readonly Dictionary<ulong, TState> states;

        private readonly Func<TState> createDefault;

        protected StatefulBaseModule(IBotCore core, Func<TState> createDefault) : base(core)
        {
            repository = new FakeRepository<ulong, TState>();
            states = new Dictionary<ulong, TState>();
            this.createDefault = createDefault;

            var data = repository.GetAllAsync().Result;
            foreach (var (key, value) in data) 
                states[key] = value;

            Task.Run(SaveStates);
        }

        internal TState GetState(ulong id)
        {
            if (states.TryGetValue(id, out var result))
                return result;
        
            var newState = createDefault();

            states[id] = newState;

            return newState;
        }

        internal TState SetState(ulong id, TState state)
        {
            states[id] = state;
            return state;
        }

        private async Task SaveStates()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromHours(1));
                foreach (var (key, value) in states)
                    await repository.CreateOrUpdateAsync(key, value).ConfigureAwait(false);
            }
        }
    }
}