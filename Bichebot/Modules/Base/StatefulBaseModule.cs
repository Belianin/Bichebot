using System;
using System.Collections.Generic;
using Bichebot.Core;
using Discord;

namespace Bichebot.Modules.Base
{
    public class StatefulBaseModule<T> : BaseModule
    {
        private readonly Dictionary<ulong, T> states;

        private readonly Func<T> createDefault;
        
        public StatefulBaseModule(IBotCore core, Func<T> createDefault) : base(core)
        {
            states = new Dictionary<ulong, T>();
            this.createDefault = createDefault;
        }

        internal T GetState(ulong id)
        {
            if (states.TryGetValue(id, out var result))
                return result;
        
            var newState = createDefault();

            states[id] = newState;

            return newState;
        }
    }
}