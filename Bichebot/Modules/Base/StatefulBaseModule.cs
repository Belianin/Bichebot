using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bichebot.Core;
using Discord;

namespace Bichebot.Modules.Base
{
    [Obsolete("Уже не очень полезная абстракция")]
    public class StatefulBaseModule<TState> : BaseModule where TState : class
    {
        private readonly IRepository<ulong, TState> repository;

        private readonly Func<TState> createDefault;

        protected StatefulBaseModule(IBotCore core, Func<TState> createDefault) : base(core)
        {
            repository = new CachingRepository<ulong, TState>(new FakeRepository<ulong, TState>());
            this.createDefault = createDefault;
        }

        internal TState GetState(ulong id)
        {
            return repository.GetOrCreate(id, createDefault()).Result;
        }

        internal TState SetState(ulong id, TState state)
        {
            repository.CreateOrUpdateAsync(id, state).GetAwaiter().GetResult();
            return state;
        }
    }
}