using System;
using Bichebot.Core.Repositories;

namespace Bichebot.Core.Modules.Base
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

        protected TState GetState(ulong id)
        {
            return repository.GetOrCreateAsync(id, createDefault()).Result;
        }

        protected TState SetState(ulong id, TState state)
        {
            repository.CreateOrUpdateAsync(id, state).GetAwaiter().GetResult();
            return state;
        }
    }
}