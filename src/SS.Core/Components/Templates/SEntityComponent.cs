using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Entities;
using System;

namespace StardustSandbox.Core.Components.Templates
{
    public abstract class SEntityComponent(ISGame gameInstance, SEntity entityInstance) : SComponent(gameInstance)
    {
        protected SEntity SEntityInstance => entityInstance;

        internal object[] Serialize()
        {
            return OnSerialized();
        }
        internal void Deserialize(ReadOnlySpan<object> data)
        {
            OnDeserialized(data);
        }

        protected virtual object[] OnSerialized() { return []; }
        protected virtual void OnDeserialized(ReadOnlySpan<object> data) { return; }
    }
}
