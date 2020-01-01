using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core;
using Discord;

namespace Bichebot.Modules.Base
{
    public class StatefulBaseModule<T> : BaseModule // where T : class
    {
        private readonly IRepository<ulong, T> repository;
        
        private readonly Dictionary<ulong, T> states;

        private readonly Func<T> createDefault;

        protected StatefulBaseModule(IBotCore core, Func<T> createDefault) : base(core)
        {
            repository = new FakeRepository<ulong, T>();
            states = new Dictionary<ulong, T>();
            this.createDefault = createDefault;

            var data = repository.GetAllAsync().Result;
            foreach (var (key, value) in data) 
                states[key] = value;

            Task.Run(SaveStates);
        }

        internal T GetState(ulong id)
        {
            if (states.TryGetValue(id, out var result))
                return result;
        
            var newState = createDefault();

            states[id] = newState;

            return newState;
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